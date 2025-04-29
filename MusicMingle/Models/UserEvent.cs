using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MusicMingle.Models;

public class UserEvent
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; } = null!;

    [Required]
    public int EventId { get; set; }

    [ForeignKey("EventId")]
    public virtual Event Event { get; set; } = null!;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
}