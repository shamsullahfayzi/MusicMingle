using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // Required for IWebHostEnvironment
using System.IO; // Required for Path operations
using Microsoft.AspNetCore.Http; // Required for IFormFile
using System;

namespace MusicMingle.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Inject IWebHostEnvironment

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        // Property to bind the uploaded file
        [BindProperty]
        public IFormFile? ThumbnailImage { get; set; }

        // SelectList for Categories
        public SelectList CategorySelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // Populate the category dropdown
            CategorySelectList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Repopulate SelectList in case of validation errors
            CategorySelectList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", Event.CategoryId);

            // Remove Category from ModelState validation if it's null but CategoryId is valid
            ModelState.Remove("Event.Category"); 
            ModelState.Remove("Event.Bookings"); // Also remove Bookings if present

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Handle file upload
            if (ThumbnailImage != null)
            {
                // Ensure the wwwroot/uploads directory exists
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "thumbnails");
                Directory.CreateDirectory(uploadsFolder); // Create directory if it doesn't exist

                // Create a unique file name
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ThumbnailImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ThumbnailImage.CopyToAsync(fileStream);
                }

                // Store the relative path in the Event model
                Event.ThumbnailImagePath = Path.Combine("/uploads", "thumbnails", uniqueFileName).Replace("\\", "/"); // Use forward slashes for web paths
            }

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}