using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Threading.Tasks;

namespace MusicMingle.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public IActionResult OnGet()
        {
            // No data needed for GET, just display the form
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remove navigation property from validation if needed
            ModelState.Remove("Category.Events");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}