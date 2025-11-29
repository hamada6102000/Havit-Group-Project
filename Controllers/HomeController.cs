using System.Diagnostics;
using HavitGroup.Data;
using HavitGroup.Models;
using HavitGroup.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Controllers
{
    /// <summary>
    /// Controller for public-facing pages
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the HomeController
        /// </summary>
        /// <param name="logger">Logger instance</param>
        /// <param name="context">Database context</param>
        /// <param name="emailService">Email service</param>
        /// <param name="environment">Web host environment</param>
        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            IEmailService emailService,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
            _environment = environment;
        }

        /// <summary>
        /// Displays the home page
        /// </summary>
        /// <returns>Home page view</returns>
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var homeImages = await _context.HomeImages
                .Where(i => i.IsActive)
                .OrderBy(i => i.DisplayOrder)
                .ThenByDescending(i => i.CreatedAt)
                .ToListAsync(cancellationToken);

            ViewBag.HomeImages = homeImages;
            
            return View();
        }

        /// <summary>
        /// Displays the About Us page
        /// </summary>
        /// <returns>About Us page view</returns>
        public async Task<IActionResult> About(CancellationToken cancellationToken)
        {
            try
            {
                // Try to ensure database is migrated
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch
            {
                // Migration failed, continue without settings
            }

            try
            {
                // Load site settings for dynamic content
                var settings = await _context.SiteSettings.FindAsync(new object[] { 1 }, cancellationToken);
                ViewBag.Settings = settings;
            }
            catch
            {
                // Table doesn't exist yet, continue without settings
                ViewBag.Settings = null;
            }

            try
            {
                // Load about images for carousel
                var aboutImages = await _context.AboutImages
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.DisplayOrder)
                    .ThenByDescending(i => i.CreatedAt)
                    .ToListAsync(cancellationToken);
                ViewBag.AboutImages = aboutImages;
            }
            catch
            {
                // Table doesn't exist yet, continue without images
                ViewBag.AboutImages = new List<AboutImage>();
            }
            
            return View();
        }

        /// <summary>
        /// Displays the Services page (static content)
        /// </summary>
        /// <returns>Services page view</returns>
        public async Task<IActionResult> Services(CancellationToken cancellationToken)
        {
            var serviceImages = await _context.ServiceImages
                .Where(i => i.IsActive)
                .OrderBy(i => i.DisplayOrder)
                .ThenByDescending(i => i.CreatedAt)
                .ToListAsync(cancellationToken);

            ViewBag.ServiceImages = serviceImages;
            return View();
        }

        /// <summary>
        /// Displays the References page
        /// </summary>
        /// <returns>References page view</returns>
        public async Task<IActionResult> References(CancellationToken cancellationToken)
        {
            try
            {
                // Try to ensure database is migrated
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch
            {
                // Migration failed, continue without settings
            }

            try
            {
                // Load references images for carousel
                var referencesImages = await _context.ReferencesImages
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.DisplayOrder)
                    .ThenByDescending(i => i.CreatedAt)
                    .ToListAsync(cancellationToken);
                ViewBag.ReferencesImages = referencesImages;
            }
            catch
            {
                // Table doesn't exist yet, continue without images
                ViewBag.ReferencesImages = new List<ReferencesImage>();
            }

            try
            {
                // Load statistics
                var statistics = await _context.Statistics.FindAsync(new object[] { 1 }, cancellationToken);
                ViewBag.Statistics = statistics;
            }
            catch
            {
                // Table doesn't exist yet, use default values
                ViewBag.Statistics = new Statistics
                {
                    CompletedProjects = 500,
                    CountriesServed = 45,
                    HappyClients = 1000,
                    SatisfactionRate = 98
                };
            }

            try
            {
                // Load featured projects
                var projects = await _context.Projects
                    .Where(p => p.IsActive)
                    .OrderBy(p => p.DisplayOrder)
                    .ThenByDescending(p => p.CreatedAt)
                    .ToListAsync(cancellationToken);
                ViewBag.Projects = projects;
            }
            catch
            {
                // Table doesn't exist yet, continue without projects
                ViewBag.Projects = new List<Project>();
            }

            try
            {
                // Load testimonials
                var testimonials = await _context.Testimonials
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.DisplayOrder)
                    .ThenByDescending(t => t.CreatedAt)
                    .ToListAsync(cancellationToken);
                ViewBag.Testimonials = testimonials;
            }
            catch
            {
                // Table doesn't exist yet, continue without testimonials
                ViewBag.Testimonials = new List<Testimonial>();
            }
            
            return View();
        }

        /// <summary>
        /// Displays the Contact Us page
        /// </summary>
        /// <returns>Contact Us page view</returns>
        [HttpGet]
        public async Task<IActionResult> Contact(CancellationToken cancellationToken)
        {
            try
            {
                // Try to ensure database is migrated
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch
            {
                // Migration failed, continue without settings
            }

            try
            {
                // Load site settings for dynamic contact information
                var settings = await _context.SiteSettings.FindAsync(new object[] { 1 }, cancellationToken);
                ViewBag.Settings = settings;
            }
            catch
            {
                // Table doesn't exist yet, continue without settings
                ViewBag.Settings = null;
            }

            try
            {
                // Load contact images for carousel
                var contactImages = await _context.ContactImages
                    .Where(i => i.IsActive)
                    .OrderBy(i => i.DisplayOrder)
                    .ThenByDescending(i => i.CreatedAt)
                    .ToListAsync(cancellationToken);
                ViewBag.ContactImages = contactImages;
            }
            catch
            {
                // Table doesn't exist yet, continue without images
                ViewBag.ContactImages = new List<ContactImage>();
            }

            try
            {
                // Load FAQs
                var faqs = await _context.FAQs
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.DisplayOrder)
                    .ThenByDescending(f => f.CreatedAt)
                    .ToListAsync(cancellationToken);
                ViewBag.FAQs = faqs;
            }
            catch
            {
                // Table doesn't exist yet, continue without FAQs
                ViewBag.FAQs = new List<FAQ>();
            }
            
            return View(new ContactViewModel());
        }

        /// <summary>
        /// Handles the contact form submission
        /// </summary>
        /// <param name="model">Contact form data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success message or returns to form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // Reload settings and images for the view
                try
                {
                    var settings = await _context.SiteSettings.FindAsync(new object[] { 1 }, cancellationToken);
                    ViewBag.Settings = settings;
                }
                catch
                {
                    ViewBag.Settings = null;
                }

                try
                {
                    var contactImages = await _context.ContactImages
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.DisplayOrder)
                        .ThenByDescending(i => i.CreatedAt)
                        .ToListAsync(cancellationToken);
                    ViewBag.ContactImages = contactImages;
                }
                catch
                {
                    ViewBag.ContactImages = new List<ContactImage>();
                }

                try
                {
                    var faqs = await _context.FAQs
                        .Where(f => f.IsActive)
                        .OrderBy(f => f.DisplayOrder)
                        .ThenByDescending(f => f.CreatedAt)
                        .ToListAsync(cancellationToken);
                    ViewBag.FAQs = faqs;
                }
                catch
                {
                    ViewBag.FAQs = new List<FAQ>();
                }

                return View(model);
            }

            try
            {
                string? attachmentPath = null;
                string? originalFileName = null;

                // Handle file upload if provided
                if (model.Attachment != null && model.Attachment.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = $"{Guid.NewGuid()}_{model.Attachment.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Attachment.CopyToAsync(fileStream, cancellationToken);
                    }

                    attachmentPath = $"/uploads/{uniqueFileName}";
                    originalFileName = model.Attachment.FileName;
                }

                // Save to database
                var contactMessage = new ContactMessage
                {
                    Name = model.Name,
                    Email = model.Email,
                    Company = model.Company,
                    Phone = model.Phone,
                    Subject = model.Subject,
                    Message = model.Message,
                    AttachmentPath = attachmentPath,
                    OriginalFileName = originalFileName,
                    SubmittedAt = DateTime.UtcNow
                };

                _context.ContactMessages.Add(contactMessage);
                await _context.SaveChangesAsync(cancellationToken);

                // Send email notification
                var emailBody = $@"
                    <h2>New Contact Form Submission</h2>
                    <p><strong>Name:</strong> {model.Name}</p>
                    <p><strong>Email:</strong> {model.Email}</p>
                    {(string.IsNullOrEmpty(model.Company) ? "" : $"<p><strong>Company:</strong> {model.Company}</p>")}
                    {(string.IsNullOrEmpty(model.Phone) ? "" : $"<p><strong>Phone:</strong> {model.Phone}</p>")}
                    <p><strong>Subject:</strong> {model.Subject}</p>
                    <p><strong>Message:</strong></p>
                    <p>{model.Message.Replace("\n", "<br>")}</p>
                    {(attachmentPath != null ? $"<p><strong>Attachment:</strong> {originalFileName}</p>" : "")}
                ";

                await _emailService.SendEmailAsync(
                    model.Email,
                    $"Contact Form: {model.Subject}",
                    emailBody,
                    cancellationToken);

                TempData["SuccessMessage"] = "Thank you for contacting us! We will get back to you soon.";
                return RedirectToAction(nameof(Contact));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contact form submission");
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                
                // Reload settings and images for the view
                try
                {
                    var settings = await _context.SiteSettings.FindAsync(new object[] { 1 }, cancellationToken);
                    ViewBag.Settings = settings;
                }
                catch
                {
                    ViewBag.Settings = null;
                }

                try
                {
                    var contactImages = await _context.ContactImages
                        .Where(i => i.IsActive)
                        .OrderBy(i => i.DisplayOrder)
                        .ThenByDescending(i => i.CreatedAt)
                        .ToListAsync(cancellationToken);
                    ViewBag.ContactImages = contactImages;
                }
                catch
                {
                    ViewBag.ContactImages = new List<ContactImage>();
                }

                try
                {
                    var faqs = await _context.FAQs
                        .Where(f => f.IsActive)
                        .OrderBy(f => f.DisplayOrder)
                        .ThenByDescending(f => f.CreatedAt)
                        .ToListAsync(cancellationToken);
                    ViewBag.FAQs = faqs;
                }
                catch
                {
                    ViewBag.FAQs = new List<FAQ>();
                }

                return View(model);
            }
        }

        /// <summary>
        /// Displays error page
        /// </summary>
        /// <returns>Error page view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
