using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicMingle.Data;
using MusicMingle.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace MusicMingle.Pages.Admin.Events
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public Event Event { get; set; } = default!;

        [BindProperty]
        public IFormFile? ThumbnailImage { get; set; }

        public SelectList CategorySelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (eventItem == null)
            {
                return NotFound();
            }
            Event = eventItem;

            // Populate the category dropdown
            CategorySelectList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", Event.CategoryId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Repopulate SelectList in case of validation errors
            CategorySelectList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", Event.CategoryId);

            // Remove navigation properties from ModelState validation
            ModelState.Remove("Event.Category");
            ModelState.Remove("Event.Bookings");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch the existing event from the database to update it
            var eventToUpdate = await _context.Events.FindAsync(Event.Id);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            // Handle file upload if a new file is provided
            if (ThumbnailImage != null)
            {
                // Optional: Delete the old image file if it exists
                if (!string.IsNullOrEmpty(eventToUpdate.ThumbnailImagePath))
                {
                    string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, eventToUpdate.ThumbnailImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save the new file
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "thumbnails");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ThumbnailImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ThumbnailImage.CopyToAsync(fileStream);
                }
                // Update the path in the model being saved
                eventToUpdate.ThumbnailImagePath = Path.Combine("/uploads", "thumbnails", uniqueFileName).Replace("\\", "/");
            }
            else
            {
                // If no new image, keep the existing path (already loaded in eventToUpdate)
                // We need to ensure EF doesn't try to nullify it if ThumbnailImagePath was part of the bound Event property
                 _context.Entry(eventToUpdate).Property(x => x.ThumbnailImagePath).IsModified = false;
            }

            // Update other properties
            // Use TryUpdateModelAsync for security and efficiency
            if (await TryUpdateModelAsync<Event>(
                eventToUpdate,
                "Event", // Prefix used in the form
                 e => e.Title, e => e.Description, e => e.Location, e => e.StartDateTime,
                 e => e.EndDateTime, e => e.MaxAttendees, e => e.Price, e => e.CategoryId))
            {
                 // ThumbnailImagePath is handled separately above
                 eventToUpdate.ThumbnailImagePath = Event.ThumbnailImagePath; // Ensure the path from the hidden field or new upload is assigned

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(Event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("./Index");
            }

            // If TryUpdateModelAsync fails, repopulate and return page
            CategorySelectList = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", eventToUpdate.CategoryId);
            return Page();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}