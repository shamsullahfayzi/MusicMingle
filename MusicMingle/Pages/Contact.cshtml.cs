using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Threading.Tasks;

namespace MusicMingle.Pages
{
    public class ContactModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ContactModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ContactMessage ContactMessage { get; set; } = new ContactMessage();

        [TempData]
        public bool MessageSent { get; set; }

        public void OnGet()
        {
            // Just display the page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ContactMessage.SubmittedDate = System.DateTime.UtcNow;
            _context.ContactMessages.Add(ContactMessage);
            await _context.SaveChangesAsync();

            MessageSent = true; 
            // Clear the model state so the form is empty after successful submission
            ModelState.Clear();
            ContactMessage = new ContactMessage(); // Re-initialize the model

            // Redirect back to the same page to show the success message
            // Using RedirectToPage avoids issues with form resubmission on refresh
            return RedirectToPage();
        }
    }
}