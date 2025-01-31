using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace noted_api.Entities;

public class Note
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "int")]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(255)")] public required string Title { get; set; }

    [Column(TypeName = "nvarchar(max)")] public string? Content { get; set; }

    [Column(TypeName = "datetime")] public DateTime CreatedAt { get; set; }

    [Column(TypeName = "datetime")] public DateTime? UpdatedAt { get; set; }
}