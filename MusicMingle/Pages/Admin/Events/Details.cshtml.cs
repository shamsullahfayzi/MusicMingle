using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Threading.Tasks;

namespace MusicMingle.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Event Event { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
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
    }
}