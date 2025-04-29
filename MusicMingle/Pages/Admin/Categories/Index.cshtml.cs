using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicMingle.Pages.Admin.Categories
{
    [Authorize(Roles = "Admin")] // Restrict access to Admins
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync();
        }
    }
}