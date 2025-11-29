using HavitGroup.Data;
using HavitGroup.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.ViewComponents
{
    /// <summary>
    /// ViewComponent for loading site settings for the footer
    /// </summary>
    public class FooterViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FooterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var settings = await _context.SiteSettings.FindAsync(1);
                return View(settings);
            }
            catch
            {
                // Return null if settings don't exist or database error
                return View((SiteSettings?)null);
            }
        }
    }
}

