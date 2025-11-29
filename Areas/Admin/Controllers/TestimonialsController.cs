using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing testimonials in admin area
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TestimonialsController> _logger;

        /// <summary>
        /// Initializes a new instance of the TestimonialsController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public TestimonialsController(
            ApplicationDbContext context,
            ILogger<TestimonialsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all testimonials
        /// </summary>
        /// <returns>List of testimonials view</returns>
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
                var testimonials = await _context.Testimonials
                    .OrderBy(t => t.DisplayOrder)
                    .ThenByDescending(t => t.CreatedAt)
                    .ToListAsync(cancellationToken);

                return View(testimonials);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading testimonials");
                TempData["ErrorMessage"] = $"Error loading testimonials: {ex.Message}";
                return View(new List<Testimonial>());
            }
        }

        /// <summary>
        /// Displays details of a specific testimonial
        /// </summary>
        /// <param name="id">Testimonial ID</param>
        /// <returns>Testimonial details view</returns>
        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        /// <summary>
        /// Displays form to create a new testimonial
        /// </summary>
        /// <returns>Create testimonial view</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles creation of a new testimonial
        /// </summary>
        /// <param name="testimonial">Testimonial data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to testimonials list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,ClientName,ClientTitle,CompanyName,Rating,DisplayOrder,IsActive")] Testimonial testimonial, CancellationToken cancellationToken)
        {
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

                    testimonial.CreatedAt = DateTime.UtcNow;
                    _context.Add(testimonial);
                    await _context.SaveChangesAsync(cancellationToken);
                    TempData["SuccessMessage"] = "Testimonial created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating testimonial: {Message}", ex.Message);
                    ModelState.AddModelError("", $"An error occurred while creating the testimonial: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                }
            }
            return View(testimonial);
        }

        /// <summary>
        /// Displays form to edit an existing testimonial
        /// </summary>
        /// <param name="id">Testimonial ID</param>
        /// <returns>Edit testimonial view</returns>
        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(new object[] { id }, cancellationToken);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }

        /// <summary>
        /// Handles update of an existing testimonial
        /// </summary>
        /// <param name="id">Testimonial ID</param>
        /// <param name="testimonial">Updated testimonial data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to testimonials list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,ClientName,ClientTitle,CompanyName,Rating,DisplayOrder,IsActive,CreatedAt")] Testimonial testimonial, CancellationToken cancellationToken)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync(cancellationToken);
                    TempData["SuccessMessage"] = "Testimonial updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testimonial);
        }

        /// <summary>
        /// Displays confirmation page to delete a testimonial
        /// </summary>
        /// <param name="id">Testimonial ID</param>
        /// <returns>Delete confirmation view</returns>
        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        /// <summary>
        /// Handles deletion of a testimonial
        /// </summary>
        /// <param name="id">Testimonial ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to testimonials list</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var testimonial = await _context.Testimonials.FindAsync(new object[] { id }, cancellationToken);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["SuccessMessage"] = "Testimonial deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a testimonial exists
        /// </summary>
        /// <param name="id">Testimonial ID</param>
        /// <returns>True if testimonial exists, false otherwise</returns>
        private bool TestimonialExists(int id)
        {
            return _context.Testimonials.Any(e => e.Id == id);
        }
    }
}

