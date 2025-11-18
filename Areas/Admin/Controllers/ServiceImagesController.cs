using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing services page carousel images in admin area
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class ServiceImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceImagesController> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the ServiceImagesController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="environment">Web host environment</param>
        public ServiceImagesController(
            ApplicationDbContext context,
            ILogger<ServiceImagesController> logger,
            IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Displays list of all service images
        /// </summary>
        /// <returns>List of service images view</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                // Ensure database is migrated
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Migration check failed, continuing anyway");
            }

            try
            {
                var images = await _context.ServiceImages
                    .OrderBy(i => i.DisplayOrder)
                    .ThenByDescending(i => i.CreatedAt)
                    .ToListAsync(cancellationToken);

                return View(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading service images");
                TempData["ErrorMessage"] = $"Error loading images: {ex.Message}";
                return View(new List<ServiceImage>());
            }
        }

        /// <summary>
        /// Displays form to create a new service image
        /// </summary>
        /// <returns>Create service image view</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles creation of a new service image
        /// </summary>
        /// <param name="image">Image file</param>
        /// <param name="altText">Alt text for the image</param>
        /// <param name="displayOrder">Display order</param>
        /// <param name="isActive">Whether the image is active</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to images list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            IFormFile image,
            string? altText,
            int displayOrder = 0,
            CancellationToken cancellationToken = default)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("image", "Please select an image file.");
                _logger.LogWarning("Image upload attempted without selecting a file");
                return View();
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("image", "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                _logger.LogWarning("Invalid file type attempted: {FileName}", image.FileName);
                return View();
            }

            // Validate file size (max 10MB)
            if (image.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("image", "Image size cannot exceed 10MB.");
                _logger.LogWarning("File size too large: {Size} bytes", image.Length);
                return View();
            }

            // Remove the image validation from ModelState since we're handling it manually
            ModelState.Remove("image");

            // Handle checkbox binding - checkboxes send "on" or "true" when checked, nothing when unchecked
            bool isActive = false;
            if (Request.Form.ContainsKey("isActive"))
            {
                var isActiveValue = Request.Form["isActive"].ToString();
                isActive = isActiveValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                          isActiveValue.Equals("on", StringComparison.OrdinalIgnoreCase);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure database is migrated
                    try
                    {
                        await _context.Database.MigrateAsync(cancellationToken);
                    }
                    catch (Exception migrateEx)
                    {
                        _logger.LogWarning(migrateEx, "Migration check failed, continuing anyway");
                    }

                    // Create uploads directory if it doesn't exist
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images", "Services");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                        _logger.LogInformation("Created uploads directory: {Folder}", uploadsFolder);
                    }

                    // Generate unique filename
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream, cancellationToken);
                    }

                    // Create database record
                    var serviceImage = new ServiceImage
                    {
                        ImagePath = $"/Images/Services/{uniqueFileName}",
                        OriginalFileName = image.FileName,
                        AltText = altText ?? "Havit Group Services",
                        DisplayOrder = displayOrder,
                        IsActive = isActive,
                        CreatedAt = DateTime.UtcNow
                    };

                    _logger.LogInformation("Adding service image to database: {ImagePath}", serviceImage.ImagePath);
                    _context.ServiceImages.Add(serviceImage);
                    
                    var savedCount = await _context.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Saved {Count} changes to database. Image ID: {Id}", savedCount, serviceImage.Id);

                    TempData["SuccessMessage"] = "Image uploaded successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading service image: {Message}", ex.Message);
                    ModelState.AddModelError("", $"An error occurred while uploading the image: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                }
            }
            else
            {
                _logger.LogWarning("ModelState is invalid. Errors: {Errors}", 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            return View();
        }

        /// <summary>
        /// Handles deletion of a service image
        /// </summary>
        /// <param name="id">Image ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to images list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var serviceImage = await _context.ServiceImages.FindAsync(new object[] { id }, cancellationToken);
            if (serviceImage == null)
            {
                return NotFound();
            }

            try
            {
                // Delete physical file
                var filePath = Path.Combine(_environment.WebRootPath, serviceImage.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                // Delete database record
                _context.ServiceImages.Remove(serviceImage);
                await _context.SaveChangesAsync(cancellationToken);

                TempData["SuccessMessage"] = "Image deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting service image");
                TempData["ErrorMessage"] = "An error occurred while deleting the image.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Updates the display order of images
        /// </summary>
        /// <param name="id">Image ID</param>
        /// <param name="displayOrder">New display order</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JSON result</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateDisplayOrder(int id, int displayOrder, CancellationToken cancellationToken)
        {
            var serviceImage = await _context.ServiceImages.FindAsync(new object[] { id }, cancellationToken);
            if (serviceImage == null)
            {
                return Json(new { success = false, message = "Image not found." });
            }

            serviceImage.DisplayOrder = displayOrder;
            await _context.SaveChangesAsync(cancellationToken);

            return Json(new { success = true });
        }

        /// <summary>
        /// Toggles the active status of an image
        /// </summary>
        /// <param name="id">Image ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JSON result</returns>
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, CancellationToken cancellationToken)
        {
            var serviceImage = await _context.ServiceImages.FindAsync(new object[] { id }, cancellationToken);
            if (serviceImage == null)
            {
                return Json(new { success = false, message = "Image not found." });
            }

            serviceImage.IsActive = !serviceImage.IsActive;
            await _context.SaveChangesAsync(cancellationToken);

            TempData["SuccessMessage"] = $"Image {(serviceImage.IsActive ? "activated" : "deactivated")} successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

