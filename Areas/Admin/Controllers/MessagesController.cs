using FreeLance.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreeLance.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing contact messages in admin area
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MessagesController> _logger;

        /// <summary>
        /// Initializes a new instance of the MessagesController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        public MessagesController(ApplicationDbContext context, ILogger<MessagesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Displays list of all contact messages
        /// </summary>
        /// <returns>List of messages view</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var messages = await _context.ContactMessages
                .OrderByDescending(m => m.SubmittedAt)
                .ToListAsync(cancellationToken);

            return View(messages);
        }

        /// <summary>
        /// Displays details of a specific message
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <returns>Message details view</returns>
        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.ContactMessages
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (message == null)
            {
                return NotFound();
            }

            // Mark as read
            if (!message.IsRead)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync(cancellationToken);
            }

            return View(message);
        }

        /// <summary>
        /// Marks a message as read/unread
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to messages list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRead(int id, CancellationToken cancellationToken)
        {
            var message = await _context.ContactMessages.FindAsync(new object[] { id }, cancellationToken);
            if (message == null)
            {
                return NotFound();
            }

            message.IsRead = !message.IsRead;
            await _context.SaveChangesAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Deletes a message
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to messages list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var message = await _context.ContactMessages.FindAsync(new object[] { id }, cancellationToken);
            if (message == null)
            {
                return NotFound();
            }

            // Delete associated file if exists
            if (!string.IsNullOrEmpty(message.AttachmentPath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", message.AttachmentPath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.ContactMessages.Remove(message);
            await _context.SaveChangesAsync(cancellationToken);

            TempData["SuccessMessage"] = "Message deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}

