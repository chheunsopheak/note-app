using noted_api.Request;
using noted_api.Response;
using noted_api.Wrappers;

namespace noted_api.Interface;

public interface INoteService
{
    Task<Result<string>> CreateNoteAsync(NoteRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateNoteAsync(int id, NoteRequest request, CancellationToken cancellationToken);
    Task<List<NoteResponse>> GetAllNoteAsync(CancellationToken cancellationToken);
    Task<Result<NoteResponse>> GetNoteByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<string>> DeleteNoteAsync(int id, CancellationToken cancellationToken);
}