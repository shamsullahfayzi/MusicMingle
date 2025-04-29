using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicMingle.Models;

public class Booking
{
    public int Id { get; set; }

    // Foreign Key for Event
    [Required]
    public int EventId { get; set; }

    // Navigation property for Event
    [ForeignKey("EventId")]
    public Event? Event { get; set; }

    // Foreign Key for User
    [Required]
    public string UserId { get; set; } = string.Empty;

    // Navigation property for User
    [ForeignKey("UserId")]
    public IdentityUser? User { get; set; } // Change to ApplicationUser if you create a custom user class

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime BookingDate { get; set; }
}