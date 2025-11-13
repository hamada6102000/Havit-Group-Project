using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing services in admin area
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServicesController> _logger;

        /// <summary>
        /// Initializes a new instance of the ServicesController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public ServicesController(ApplicationDbContext context, ILogger<ServicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all services
        /// </summary>
        /// <returns>List of services view</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var services = await _context.Services
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(cancellationToken);

            return View(services);
        }

        /// <summary>
        /// Displays details of a specific service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Service details view</returns>
        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        /// <summary>
        /// Displays form to create a new service
        /// </summary>
        /// <returns>Create service view</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles creation of a new service
        /// </summary>
        /// <param name="service">Service data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to services list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,IconClass,IsActive")] Service service, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                service.CreatedAt = DateTime.UtcNow;
                _context.Add(service);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["SuccessMessage"] = "Service created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        /// <summary>
        /// Displays form to edit an existing service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Edit service view</returns>
        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(new object[] { id }, cancellationToken);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        /// <summary>
        /// Handles update of an existing service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <param name="service">Updated service data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to services list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IconClass,IsActive,CreatedAt")] Service service, CancellationToken cancellationToken)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    service.UpdatedAt = DateTime.UtcNow;
                    _context.Update(service);
                    await _context.SaveChangesAsync(cancellationToken);
                    TempData["SuccessMessage"] = "Service updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
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
            return View(service);
        }

        /// <summary>
        /// Displays confirmation page to delete a service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>Delete confirmation view</returns>
        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        /// <summary>
        /// Handles deletion of a service
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to services list</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var service = await _context.Services.FindAsync(new object[] { id }, cancellationToken);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync(cancellationToken);
                TempData["SuccessMessage"] = "Service deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a service exists
        /// </summary>
        /// <param name="id">Service ID</param>
        /// <returns>True if service exists, false otherwise</returns>
        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}

