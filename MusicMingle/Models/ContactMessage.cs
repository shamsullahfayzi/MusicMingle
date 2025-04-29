using System.ComponentModel.DataAnnotations;

namespace MusicMingle.Models;

public class ContactMessage
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;
}