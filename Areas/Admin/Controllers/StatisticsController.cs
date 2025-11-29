using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing site statistics
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StatisticsController> _logger;

        /// <summary>
        /// Initializes a new instance of the StatisticsController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public StatisticsController(ApplicationDbContext context, ILogger<StatisticsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Displays the statistics edit form
        /// </summary>
        /// <returns>Statistics edit view</returns>
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
                // Get statistics or create default if none exist (singleton pattern)
                var statistics = await _context.Statistics.FindAsync(new object[] { 1 }, cancellationToken);
                
                if (statistics == null)
                {
                    // Create default statistics if none exist
                    statistics = new Statistics
                    {
                        Id = 1,
                        CompletedProjects = 500,
                        CountriesServed = 45,
                        HappyClients = 1000,
                        SatisfactionRate = 98,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.Statistics.Add(statistics);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                return View(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading statistics");
                TempData["ErrorMessage"] = "Statistics table does not exist. Please apply database migrations: dotnet ef database update";
                return RedirectToAction("Index", "Dashboard");
            }
        }

        /// <summary>
        /// Handles the update of statistics
        /// </summary>
        /// <param name="statistics">Updated statistics data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to statistics page or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Statistics statistics, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(statistics);
            }

            try
            {
                // Ensure ID is always 1 (singleton pattern)
                statistics.Id = 1;
                statistics.UpdatedAt = DateTime.UtcNow;

                var existingStatistics = await _context.Statistics.FindAsync(new object[] { 1 }, cancellationToken);
                
                if (existingStatistics == null)
                {
                    // Create new statistics if none exist
                    statistics.CreatedAt = DateTime.UtcNow;
                    _context.Statistics.Add(statistics);
                }
                else
                {
                    // Update existing statistics
                    _context.Entry(existingStatistics).CurrentValues.SetValues(statistics);
                    existingStatistics.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
                TempData["SuccessMessage"] = "Statistics updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating statistics");
                ModelState.AddModelError("", "An error occurred while updating statistics. Please try again.");
                return View(statistics);
            }
        }
    }
}

