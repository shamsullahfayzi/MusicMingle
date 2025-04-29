using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Security.Claims;

namespace MusicMingle.Pages.Bookings;

[Authorize] // Only logged-in users can see their bookings
public class MyBookingsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public MyBookingsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<Booking> Bookings { get; set; } = new List<Booking>();

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            // This shouldn't happen if [Authorize] is working, but good practice to check
            return Challenge(); 
        }

        Bookings = await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Event) // Include Event details
                .ThenInclude(e => e!.Category) // Include Category details within Event
            .OrderBy(b => b.Event!.StartDateTime) // Order by event start date
            .ToListAsync();

        return Page();
    }
}