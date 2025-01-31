using Microsoft.EntityFrameworkCore;
using noted_api.Contexts;
using noted_api.Entities;
using noted_api.Interface;
using noted_api.Request;
using noted_api.Response;
using noted_api.Wrappers;

namespace noted_api.Service;

public class NoteService : INoteService
{
    private readonly AppDbContext _context;

    public NoteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> CreateNoteAsync(NoteRequest request, CancellationToken cancellationToken)
    {
        var existing = await _context.Notes
            .AsNoTracking()
            .Where(x => x.Title == request.Title && x.Content == request.Content)
            .AnyAsync(cancellationToken);
        if (existing)
            return await Result<string>.FailAsync("Note already exists");

        var note = new Note()
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.Now
        };

        await _context.Notes.AddAsync(note, cancellationToken);
        var response = await _context.SaveChangesAsync(cancellationToken);
        return response > 0
            ? await Result<string>.SuccessAsync(note.Id.ToString(), "Note created successfully")
            : await Result<string>.FailAsync("Failed to create note");
    }

    public async Task<Result<string>> UpdateNoteAsync(int id, NoteRequest request, CancellationToken cancellationToken)
    {
        var requestUpdate = await _context.Notes
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (requestUpdate == null)
            return await Result<string>.FailAsync("Note not found");

        requestUpdate.Title = request.Title;
        requestUpdate.Content = request.Content;
        requestUpdate.UpdatedAt = DateTime.Now;

        _context.Notes.Update(requestUpdate);
        var response = await _context.SaveChangesAsync(cancellationToken);
        return response > 0
            ? await Result<string>.SuccessAsync(requestUpdate.Id.ToString(), "Note updated successfully")
            : await Result<string>.FailAsync("Failed to update note");
    }

    public async Task<List<NoteResponse>> GetAllNoteAsync(CancellationToken cancellationToken)
    {
        var data = await _context.Notes
            .AsNoTracking()
            .OrderByDescending(x => x.Id)
            .Select(x => new NoteResponse()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToListAsync(cancellationToken);
        return data;
    }

    public async Task<Result<NoteResponse>> GetNoteByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _context.Notes
            .Where(x => x.Id == id)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new NoteResponse()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).FirstOrDefaultAsync(cancellationToken);
        var result = data == null
            ? await Result<NoteResponse>.FailAsync("Note not found")
            : await Result<NoteResponse>.SuccessAsync(data: data);
        return result;
    }

    public async Task<Result<string>> DeleteNoteAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _context.Notes
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (data == null)
            return await Result<string>.FailAsync("Note not found");

        _context.Notes.Remove(data);
        var response = await _context.SaveChangesAsync(cancellationToken);
        return response > 0
            ? await Result<string>.SuccessAsync("Note deleted successfully")
            : await Result<string>.FailAsync("Failed to delete note");
    }
}