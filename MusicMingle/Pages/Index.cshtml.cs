using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicMingle.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;

    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IList<Event> FeaturedEvents { get; set; } = new List<Event>();

    public async Task OnGetAsync()
    {
        // Fetch, for example, the 3 most recent upcoming events as featured
        FeaturedEvents = await _context.Events
                                .Where(e => e.StartDateTime >= System.DateTime.Today) // Filter for upcoming or current events
                                .OrderBy(e => e.StartDateTime) // Order by date
                                .Take(3) // Take the top 3
                                .Include(e => e.Category) // Include category if needed for display
                                .ToListAsync();
    }
}
