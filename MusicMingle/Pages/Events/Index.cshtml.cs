using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data; // Assuming DbContext is in the Data namespace
using MusicMingle.Models;

namespace MusicMingle.Pages.Events;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Event> Events { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SelectedCategoryId { get; set; }

    public SelectList? Categories { get; set; }

    public async Task OnGetAsync()
    {
        // Fetch categories for the dropdown
        var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        Categories = new SelectList(categories, "Id", "Name", SelectedCategoryId);

        // Start with all events
        var eventsQuery = _context.Events
                                .Include(e => e.Category)
                                .OrderBy(e => e.StartDateTime)
                                .AsQueryable(); // Use AsQueryable for deferred execution

        // Apply search filter
        if (!string.IsNullOrEmpty(SearchString))
        {
            eventsQuery = eventsQuery.Where(e => e.Title.Contains(SearchString) ||
                                                 (e.Description != null && e.Description.Contains(SearchString)));
        }

        // Apply category filter
        if (SelectedCategoryId.HasValue)
        {
            eventsQuery = eventsQuery.Where(e => e.CategoryId == SelectedCategoryId.Value);
        }

        Events = await eventsQuery.ToListAsync();
    }
}