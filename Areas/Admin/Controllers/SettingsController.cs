using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing site-wide settings and configuration
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SettingsController> _logger;

        /// <summary>
        /// Initializes a new instance of the SettingsController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public SettingsController(ApplicationDbContext context, ILogger<SettingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Displays the site settings edit form
        /// </summary>
        /// <returns>Settings edit view</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                // Ensure database is migrated (in case migration wasn't applied on startup)
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Migration check failed, but continuing...");
            }

            try
            {
                // Get settings or create default if none exist (singleton pattern)
                var settings = await _context.SiteSettings.FindAsync(new object[] { 1 }, cancellationToken);
                
                if (settings == null)
                {
                    // Create default settings if none exist
                    settings = new SiteSettings
                    {
                        Id = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.SiteSettings.Add(settings);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return View(settings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading site settings");
                TempData["ErrorMessage"] = "SiteSettings table does not exist. Please apply database migrations: dotnet ef database update";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        /// <summary>
        /// Handles the update of site settings
        /// </summary>
        /// <param name="settings">Updated settings data</param>
        /// <param name="portfolioPdf">Portfolio PDF file</param>
        /// <param name="newsletterPdf">Newsletter PDF file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to settings page or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteSettings settings, IFormFile? portfolioPdf, IFormFile? newsletterPdf, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(settings);
            }

            try
            {
                // Ensure ID is always 1 (singleton pattern)
                settings.Id = 1;
                settings.UpdatedAt = DateTime.UtcNow;

                var existingSettings = await _context.SiteSettings.FindAsync(new object[] { 1 }, cancellationToken);
                
                if (existingSettings == null)
                {
                    // Create new settings if none exist
                    settings.CreatedAt = DateTime.UtcNow;
                    
                    // Handle Portfolio PDF upload
                    if (portfolioPdf != null && portfolioPdf.Length > 0)
                    {
                        var portfolioPath = await SavePdfFile(portfolioPdf, "portfolio");
                        settings.PortfolioPdfPath = portfolioPath;
                        settings.PortfolioPdfOriginalName = portfolioPdf.FileName;
                    }

                    // Handle Newsletter PDF upload
                    if (newsletterPdf != null && newsletterPdf.Length > 0)
                    {
                        var newsletterPath = await SavePdfFile(newsletterPdf, "newsletter");
                        settings.NewsletterPdfPath = newsletterPath;
                        settings.NewsletterPdfOriginalName = newsletterPdf.FileName;
                    }

                    _context.SiteSettings.Add(settings);
                }
                else
                {
                    // Handle Portfolio PDF upload
                    if (portfolioPdf != null && portfolioPdf.Length > 0)
                    {
                        // Delete old file if exists
                        if (!string.IsNullOrEmpty(existingSettings.PortfolioPdfPath))
                        {
                            DeletePdfFile(existingSettings.PortfolioPdfPath);
                        }

                        var portfolioPath = await SavePdfFile(portfolioPdf, "portfolio");
                        settings.PortfolioPdfPath = portfolioPath;
                        settings.PortfolioPdfOriginalName = portfolioPdf.FileName;
                    }
                    else
                    {
                        // Keep existing PDF if no new one uploaded
                        settings.PortfolioPdfPath = existingSettings.PortfolioPdfPath;
                        settings.PortfolioPdfOriginalName = existingSettings.PortfolioPdfOriginalName;
                    }

                    // Handle Newsletter PDF upload
                    if (newsletterPdf != null && newsletterPdf.Length > 0)
                    {
                        // Delete old file if exists
                        if (!string.IsNullOrEmpty(existingSettings.NewsletterPdfPath))
                        {
                            DeletePdfFile(existingSettings.NewsletterPdfPath);
                        }

                        var newsletterPath = await SavePdfFile(newsletterPdf, "newsletter");
                        settings.NewsletterPdfPath = newsletterPath;
                        settings.NewsletterPdfOriginalName = newsletterPdf.FileName;
                    }
                    else
                    {
                        // Keep existing PDF if no new one uploaded
                        settings.NewsletterPdfPath = existingSettings.NewsletterPdfPath;
                        settings.NewsletterPdfOriginalName = existingSettings.NewsletterPdfOriginalName;
                    }

                    // Update existing settings
                    _context.Entry(existingSettings).CurrentValues.SetValues(settings);
                    existingSettings.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
                TempData["SuccessMessage"] = "Settings updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating site settings");
                ModelState.AddModelError("", "An error occurred while updating settings. Please try again.");
                return View(settings);
            }
        }

        /// <summary>
        /// Saves a PDF file to the server
        /// </summary>
        /// <param name="file">PDF file to save</param>
        /// <param name="prefix">Prefix for the filename (portfolio or newsletter)</param>
        /// <returns>Relative path to the saved file</returns>
        private async Task<string> SavePdfFile(IFormFile file, string prefix)
        {
            // Validate file type
            var allowedExtensions = new[] { ".pdf" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Only PDF files are allowed.");
            }

            // Create unique filename
            var fileName = $"{prefix}_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
            
            // Create directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path
            return $"/pdfs/{fileName}";
        }

        /// <summary>
        /// Deletes a PDF file from the server
        /// </summary>
        /// <param name="relativePath">Relative path to the file</param>
        private void DeletePdfFile(string relativePath)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Failed to delete PDF file: {relativePath}");
            }
        }
    }
}

