using FreeLance.Data;
using FreeLance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreeLance.Areas.Admin.Controllers
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
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to settings page or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteSettings settings, CancellationToken cancellationToken)
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
                    _context.SiteSettings.Add(settings);
                }
                else
                {
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
    }
}

