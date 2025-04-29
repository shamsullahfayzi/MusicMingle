using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MusicMingle.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Category) // Include category for display
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventToDelete = await _context.Events.FindAsync(id);

            if (eventToDelete != null)
            {
                // Delete the associated thumbnail image file if it exists
                if (!string.IsNullOrEmpty(eventToDelete.ThumbnailImagePath))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, eventToDelete.ThumbnailImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Remove the event entity
                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}