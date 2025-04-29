using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;

namespace MusicMingle.Pages.Admin.Bookings
{
    [Authorize(Roles = "Admin")] // Restrict access to Admins
    public class IndexModel : PageModel
    {
        public readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            this._db = db;
        }
        [BindProperty]
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public void OnGet()
        {
            Bookings = _db.Bookings.Include(a=>a.Event).Include(a => a.User).ToList();
        }

    }
}
