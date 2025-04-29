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
    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityUser AppUser { get; set; }

        public IList<string> UserRoles { get; set; } = new List<string>();
        public List<IdentityRole> AllRoles { get; set; } = new List<IdentityRole>();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await _userManager.FindByIdAsync(id);

            if (AppUser == null)
            {
                return NotFound();
            }

            UserRoles = await _userManager.GetRolesAsync(AppUser);
            AllRoles = await _roleManager.Roles.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string[] selectedRoles)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser = await _userManager.FindByIdAsync(id);

            if (AppUser == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(AppUser);
            var rolesToAdd = selectedRoles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(selectedRoles);

            var addResult = await _userManager.AddToRolesAsync(AppUser, rolesToAdd);
            if (!addResult.Succeeded)
            {
                // Handle error - perhaps add model state errors
                ModelState.AddModelError(string.Empty, "Failed to add roles.");
                // Reload roles for display
                UserRoles = await _userManager.GetRolesAsync(AppUser);
                AllRoles = await _roleManager.Roles.ToListAsync();
                return Page();
            }

            var removeResult = await _userManager.RemoveFromRolesAsync(AppUser, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                // Handle error
                ModelState.AddModelError(string.Empty, "Failed to remove roles.");
                UserRoles = await _userManager.GetRolesAsync(AppUser);
                AllRoles = await _roleManager.Roles.ToListAsync();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}