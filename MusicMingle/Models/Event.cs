using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicMingle.Models;

public class Event
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDateTime { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDateTime { get; set; }

    [Range(0, int.MaxValue)]
    public int MaxAttendees { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")] // Specify SQL Server column type for precision
    public decimal Price { get; set; }

    // Store image paths or URLs
    [StringLength(255)]
    public string? ThumbnailImagePath { get; set; }

    // Could store multiple larger image paths/URLs, perhaps as a JSON string or separate related table
    public string? LargeImagePathsJson { get; set; } // Simple approach for now

    // Foreign Key for Category
    [Required]
    public int CategoryId { get; set; }

    // Navigation property for Category
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    // Navigation property for Bookings (attendees)
    public ICollection<Booking>? Bookings { get; set; }

    // Consider adding OrganizerId (string) linking to ApplicationUser later
    // public string? OrganizerId { get; set; }
    // public ApplicationUser? Organizer { get; set; }
}