using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Models; // Assuming your ApplicationUser is here
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicMingle.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public readonly UserManager<IdentityUser> UserManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        public IList<IdentityUser> Users { get; set; } = new List<IdentityUser>();

        public async Task OnGetAsync()
        {
            Users = await UserManager.Users.ToListAsync();
        }
    }
}