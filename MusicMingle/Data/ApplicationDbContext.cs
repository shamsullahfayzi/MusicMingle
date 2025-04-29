using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Models;

namespace MusicMingle.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<Booking> Bookings { get; set; } = default!;
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; } // Add this for the attending list


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Categories
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Concert" },
            new Category { Id = 2, Name = "Festival" },
            new Category { Id = 3, Name = "Workshop" },
            new Category { Id = 4, Name = "Open Mic" }
        );

        // Seed Events
        builder.Entity<Event>().HasData(
            new Event
            {
                Id = 1,
                Title = "Indie Rock Night",
                Description = "Join us for a night of electrifying indie rock performances.",
                Location = "The Underground Venue",
                StartDateTime = DateTime.UtcNow.AddDays(10).Date.AddHours(19), // Example: 10 days from now at 7 PM UTC
                EndDateTime = DateTime.UtcNow.AddDays(10).Date.AddHours(23),   // Example: 10 days from now at 11 PM UTC
                MaxAttendees = 150,
                CategoryId = 1, // Concert
                ThumbnailImagePath = "/images/events/thumbnails/indie_rock.jpg" // Placeholder path
            },
            new Event
            {
                Id = 2,
                Title = "Summer Music Fest",
                Description = "Three days of non-stop music across multiple stages.",
                Location = "Greenfield Park",
                StartDateTime = DateTime.UtcNow.AddMonths(2).Date.AddHours(12), // Example: 2 months from now at 12 PM UTC
                EndDateTime = DateTime.UtcNow.AddMonths(2).AddDays(2).Date.AddHours(23), // Example: 2 months + 2 days from now at 11 PM UTC
                MaxAttendees = 5000,
                CategoryId = 2, // Festival
                ThumbnailImagePath = "/images/events/thumbnails/summer_fest.jpg" // Placeholder path
            },
             new Event
            {
                Id = 3,
                Title = "Songwriting Workshop",
                Description = "Learn the art of songwriting from industry professionals.",
                Location = "Creative Hub Studios",
                StartDateTime = DateTime.UtcNow.AddDays(20).Date.AddHours(10), // Example: 20 days from now at 10 AM UTC
                EndDateTime = DateTime.UtcNow.AddDays(20).Date.AddHours(16),   // Example: 20 days from now at 4 PM UTC
                MaxAttendees = 30,
                CategoryId = 3, // Workshop
                ThumbnailImagePath = "/images/events/thumbnails/workshop.jpg" // Placeholder path
            }
        );

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}