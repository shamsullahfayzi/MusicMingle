using System.ComponentModel.DataAnnotations;

namespace MusicMingle.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    // Navigation property for related Events
    public ICollection<Event>? Events { get; set; }
}