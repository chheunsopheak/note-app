using System.ComponentModel.DataAnnotations;

namespace noted_api.Request;

public class NoteRequest
{
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; set; }

    public string? Content { get; set; }
}