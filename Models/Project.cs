using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing a featured project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Unique identifier for the project
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Brand name/tag for the project
        /// </summary>
        [Required(ErrorMessage = "Brand is required")]
        [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// Project title/name
        /// </summary>
        [Required(ErrorMessage = "Project title is required")]
        [StringLength(200, ErrorMessage = "Project title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Project location (city)
        /// </summary>
        [Required(ErrorMessage = "Location is required")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Project location (country)
        /// </summary>
        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Project completion year
        /// </summary>
        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100")]
        public int Year { get; set; }

        /// <summary>
        /// Project category (e.g., Hospitality, Corporate, Residential)
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Project description
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Path to the project image file (relative to wwwroot)
        /// </summary>
        [Required(ErrorMessage = "Image path is required")]
        [StringLength(500, ErrorMessage = "Image path cannot exceed 500 characters")]
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Original filename of the uploaded image
        /// </summary>
        [StringLength(255)]
        public string? OriginalFileName { get; set; }

        /// <summary>
        /// Display order for sorting projects
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Indicates whether the project is active and visible
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date when the project was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// New optional fields for richer project information
        /// </summary>
        [StringLength(200)]
        public string? Client { get; set; }

        [StringLength(100)]
        public string? Area { get; set; }

        [StringLength(2000)]
        public string? ScopeOfWork { get; set; }

        /// <summary>
        /// Related images for the project
        /// </summary>
        public virtual ICollection<ProjectImage>? RelatedImages { get; set; }
    }
}

