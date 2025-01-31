using Microsoft.AspNetCore.Mvc;
using noted_api.Interface;
using noted_api.Request;

namespace noted_api.Controllers;

[ApiController]
[Route("api")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet("notes")]
    public async Task<IActionResult> GetAllNoteAsync(CancellationToken cancellationToken)
    {
        var notes = await _noteService.GetAllNoteAsync(cancellationToken);
        return Ok(notes);
    }

    [HttpGet("note/{id}")]
    public async Task<IActionResult> GetNoteByIdAsync(int id, CancellationToken cancellationToken)
    {
        var note = await _noteService.GetNoteByIdAsync(id, cancellationToken);
        if (note.Succeeded)
            return Ok(note);
        return BadRequest(note);
    }

    [HttpPost("note")]
    public async Task<IActionResult> CreateNoteAsync(NoteRequest noteRequest, CancellationToken cancellationToken)
    {
        var note = await _noteService.CreateNoteAsync(noteRequest, cancellationToken);
        if (note.Succeeded)
            return Ok(await _noteService.GetNoteByIdAsync(Convert.ToInt16(note.Data), cancellationToken));
        return BadRequest(note);
    }

    [HttpPut("note/{id}")]
    public async Task<IActionResult> UpdateNoteAsync(int id, NoteRequest noteRequest,
        CancellationToken cancellationToken)
    {
        var note = await _noteService.UpdateNoteAsync(id, noteRequest, cancellationToken);
        if (note.Succeeded)
            return Ok(await _noteService.GetNoteByIdAsync(Convert.ToInt16(note.Data), cancellationToken));
        return BadRequest(note);
    }

    [HttpDelete("note/{id}")]
    public async Task<IActionResult> DeleteNoteAsync(int id, CancellationToken cancellationToken)
    {
        var note = await _noteService.DeleteNoteAsync(id, cancellationToken);
        if (note.Succeeded)
            return Ok(note);
        return BadRequest(note);
    }
}