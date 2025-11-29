using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing projects in admin area
    /// NOTE: Admin area is only accessible via direct URL - no navigation links are displayed.
    /// For production, uncomment the [Authorize] attribute below to require authentication.
    /// </summary>
    [Area("Admin")]
    // [Authorize] // TODO: Uncomment this line when authentication is implemented to secure the admin area
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectsController> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the ProjectsController
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="environment">Web host environment</param>
        public ProjectsController(
            ApplicationDbContext context,
            ILogger<ProjectsController> logger,
            IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Displays list of all projects
        /// </summary>
        /// <returns>List of projects view</returns>
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
                var projects = await _context.Projects
                    .OrderBy(p => p.DisplayOrder)
                    .ThenByDescending(p => p.CreatedAt)
                    .ToListAsync(cancellationToken);

                return View(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects");
                TempData["ErrorMessage"] = $"Error loading projects: {ex.Message}";
                return View(new List<Project>());
            }
        }

        /// <summary>
        /// Displays details of a specific project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Project details view</returns>
        public async Task<IActionResult> Details(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        /// <summary>
        /// Displays form to create a new project
        /// </summary>
        /// <returns>Create project view</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles creation of a new project
        /// </summary>
        /// <param name="project">Project data</param>
        /// <param name="image">Project image file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to projects list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Brand,Title,Location,Country,Year,Category,Description,DisplayOrder,IsActive")] Project project,
            IFormFile image,
            CancellationToken cancellationToken)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("image", "Please select an image file.");
                _logger.LogWarning("Project creation attempted without selecting an image file");
                return View(project);
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("image", "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                _logger.LogWarning("Invalid file type attempted: {FileName}", image.FileName);
                return View(project);
            }

            // Validate file size (max 10MB)
            if (image.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("image", "Image size cannot exceed 10MB.");
                _logger.LogWarning("File size too large: {Size} bytes", image.Length);
                return View(project);
            }

            // Remove the image validation from ModelState since we're handling it manually
            ModelState.Remove("image");
            ModelState.Remove("ImagePath");
            ModelState.Remove("OriginalFileName");

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
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images", "Projects");
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

                    // Set image path
                    project.ImagePath = $"/Images/Projects/{uniqueFileName}";
                    project.OriginalFileName = image.FileName;
                    project.CreatedAt = DateTime.UtcNow;

                    _context.Add(project);
                    await _context.SaveChangesAsync(cancellationToken);
                    TempData["SuccessMessage"] = "Project created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating project: {Message}", ex.Message);
                    ModelState.AddModelError("", $"An error occurred while creating the project: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                }
            }
            return View(project);
        }

        /// <summary>
        /// Displays form to edit an existing project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Edit project view</returns>
        public async Task<IActionResult> Edit(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        /// <summary>
        /// Handles update of an existing project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="project">Updated project data</param>
        /// <param name="image">New project image file (optional)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to projects list or returns form with errors</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Brand,Title,Location,Country,Year,Category,Description,ImagePath,OriginalFileName,DisplayOrder,IsActive,CreatedAt")] Project project,
            IFormFile? image,
            CancellationToken cancellationToken)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            // Remove image validation if no new image is uploaded
            if (image == null || image.Length == 0)
            {
                ModelState.Remove("image");
            }
            else
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("image", "Only image files (jpg, jpeg, png, gif, webp) are allowed.");
                    return View(project);
                }

                // Validate file size (max 10MB)
                if (image.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("image", "Image size cannot exceed 10MB.");
                    return View(project);
                }
            }

            ModelState.Remove("ImagePath");
            ModelState.Remove("OriginalFileName");

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle new image upload if provided
                    if (image != null && image.Length > 0)
                    {
                        // Delete old image
                        if (!string.IsNullOrEmpty(project.ImagePath))
                        {
                            var oldFilePath = Path.Combine(_environment.WebRootPath, project.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Create uploads directory if it doesn't exist
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images", "Projects");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate unique filename
                        var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
                        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream, cancellationToken);
                        }

                        project.ImagePath = $"/Images/Projects/{uniqueFileName}";
                        project.OriginalFileName = image.FileName;
                    }

                    _context.Update(project);
                    await _context.SaveChangesAsync(cancellationToken);
                    TempData["SuccessMessage"] = "Project updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
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
            return View(project);
        }

        /// <summary>
        /// Displays confirmation page to delete a project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>Delete confirmation view</returns>
        public async Task<IActionResult> Delete(int? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        /// <summary>
        /// Handles deletion of a project
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Redirects to projects list</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
            if (project != null)
            {
                try
                {
                    // Delete physical file
                    if (!string.IsNullOrEmpty(project.ImagePath))
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, project.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    _context.Projects.Remove(project);
                    await _context.SaveChangesAsync(cancellationToken);
                    TempData["SuccessMessage"] = "Project deleted successfully.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting project");
                    TempData["ErrorMessage"] = "An error occurred while deleting the project.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Checks if a project exists
        /// </summary>
        /// <param name="id">Project ID</param>
        /// <returns>True if project exists, false otherwise</returns>
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}

