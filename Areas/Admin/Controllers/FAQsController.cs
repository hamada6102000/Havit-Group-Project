using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing FAQs in admin area
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class FAQsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FAQsController> _logger;

        public FAQsController(ApplicationDbContext context, ILogger<FAQsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Migration check failed, continuing anyway");
            }

            try
            {
                var faqs = await _context.FAQs
                    .OrderBy(f => f.DisplayOrder)
                    .ThenByDescending(f => f.CreatedAt)
                    .ToListAsync(cancellationToken);

                return View(faqs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading FAQs");
                TempData["ErrorMessage"] = $"Error loading FAQs: {ex.Message}";
                return View(new List<FAQ>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FAQ faq, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(faq);
            }

            try
            {
                faq.CreatedAt = DateTime.UtcNow;
                _context.FAQs.Add(faq);
                await _context.SaveChangesAsync(cancellationToken);

                TempData["SuccessMessage"] = "FAQ created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating FAQ");
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(faq);
            }
        }

        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faq = await _context.FAQs.FindAsync(new object[] { id }, cancellationToken);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FAQ faq, CancellationToken cancellationToken)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(faq);
            }

            try
            {
                var existingFaq = await _context.FAQs.FindAsync(new object[] { id }, cancellationToken);
                if (existingFaq == null)
                {
                    return NotFound();
                }

                existingFaq.Question = faq.Question;
                existingFaq.Answer = faq.Answer;
                existingFaq.DisplayOrder = faq.DisplayOrder;
                existingFaq.IsActive = faq.IsActive;
                existingFaq.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                TempData["SuccessMessage"] = "FAQ updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating FAQ");
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                return View(faq);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var faq = await _context.FAQs.FindAsync(new object[] { id }, cancellationToken);
            if (faq == null)
            {
                return NotFound();
            }

            try
            {
                _context.FAQs.Remove(faq);
                await _context.SaveChangesAsync(cancellationToken);

                TempData["SuccessMessage"] = "FAQ deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting FAQ");
                TempData["ErrorMessage"] = "An error occurred while deleting the FAQ.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id, CancellationToken cancellationToken)
        {
            var faq = await _context.FAQs.FindAsync(new object[] { id }, cancellationToken);
            if (faq == null)
            {
                return Json(new { success = false, message = "FAQ not found." });
            }

            faq.IsActive = !faq.IsActive;
            await _context.SaveChangesAsync(cancellationToken);

            TempData["SuccessMessage"] = $"FAQ {(faq.IsActive ? "activated" : "deactivated")} successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

