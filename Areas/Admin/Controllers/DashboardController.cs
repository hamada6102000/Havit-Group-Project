using HavitGroup.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Admin dashboard controller
    /// NOTE: Admin area is only accessible via direct URL (/Admin/Dashboard) - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DashboardController> _logger;

        /// <summary>
        /// Initializes a new instance of the DashboardController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public DashboardController(ApplicationDbContext context, ILogger<DashboardController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Displays the admin dashboard with statistics
        /// </summary>
        /// <returns>Dashboard view</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var totalMessages = await _context.ContactMessages.CountAsync(cancellationToken);
            var unreadMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead, cancellationToken);
            var totalServices = await _context.Services.CountAsync(cancellationToken);
            var activeServices = await _context.Services.CountAsync(s => s.IsActive, cancellationToken);

            ViewBag.TotalMessages = totalMessages;
            ViewBag.UnreadMessages = unreadMessages;
            ViewBag.TotalServices = totalServices;
            ViewBag.ActiveServices = activeServices;

            return View();
        }
    }
}

