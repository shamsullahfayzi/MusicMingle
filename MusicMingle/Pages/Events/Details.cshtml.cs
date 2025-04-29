using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data; // Assuming DbContext is in the Data namespace
using MusicMingle.Models;
using System.Security.Claims;

namespace MusicMingle.Pages.Events;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager; // Inject UserManager

    public DetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public Event? Event { get; set; }

    [BindProperty]
    public int EventId { get; set; } // To receive EventId from the form

    [TempData]
    public string? BookingMessage { get; set; }
    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Events == null)
        {
            return NotFound();
        }

        var eventItem = await _context.Events
            .Include(e => e.Category) // Include category details
            .FirstOrDefaultAsync(m => m.Id == id);

        if (eventItem == null)
        {
            return NotFound();
        }
        else
        {
            Event = eventItem;
        }
        return Page();
    }

    // Basic Post handler for booking (needs more validation/logic)
    public async Task<IActionResult> OnPostAsync()
    {
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return Challenge(); // Or redirect to login
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get current user ID
        if (userId == null)
        {
             ErrorMessage = "Could not find user information.";
             return RedirectToPage(new { id = EventId });
        }

        // Check if event exists
        var eventToBook = await _context.Events.FindAsync(EventId);
        if (eventToBook == null)
        {
            ErrorMessage = "Event not found.";
            return RedirectToPage(new { id = EventId });
        }

        // Check if already booked
        var existingBooking = await _context.Bookings
                                    .FirstOrDefaultAsync(b => b.EventId == EventId && b.UserId == userId);

        if (existingBooking != null)
        {
            ErrorMessage = "You have already booked this event.";
            return RedirectToPage(new { id = EventId });
        }

        // Check attendee limit (simplified)
        var currentBookings = await _context.Bookings.CountAsync(b => b.EventId == EventId);
        if (currentBookings >= eventToBook.MaxAttendees)
        {
            ErrorMessage = "Sorry, this event is fully booked.";
            return RedirectToPage(new { id = EventId });
        }

        var booking = new Booking
        {
            EventId = EventId,
            UserId = userId,
            BookingDate = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        BookingMessage = "Booking successful!";
        return RedirectToPage(new { id = EventId });
    }
}