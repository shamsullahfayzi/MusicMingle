using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Required for authorization

namespace MusicMingle.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")] // Only allow users in the "Admin" role
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Event> Events { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Events = await _context.Events
                .Include(e => e.Category) // Include Category data
                .ToListAsync();
        }
    }
}