using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MusicMingle.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                                     .Include(c => c.Events) // Include events for cascading delete check/warning
                                     .FirstOrDefaultAsync(m => m.Id == id);

            if (category != null)
            {
                // Optional: Add logic here if you need to handle associated events differently
                // before deleting the category (e.g., reassign them, prevent deletion if events exist).
                // By default, if the foreign key constraint is set up correctly (e.g., Cascade), 
                // EF Core might handle deleting associated events automatically.
                // However, explicitly loading them as above is good practice if you need to check.

                // Note: Ensure your database schema's foreign key constraint on Events.CategoryId
                // is configured for ON DELETE CASCADE or ON DELETE SET NULL, or handle deletion manually.
                // If not configured for cascade, you might need to delete associated events first:
                // _context.Events.RemoveRange(category.Events);

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}