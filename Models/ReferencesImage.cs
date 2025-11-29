using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing a references page carousel image
    /// </summary>
    public class ReferencesImage
    {
        /// <summary>
        /// Unique identifier for the image
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Path to the image file (relative to wwwroot)
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
        /// Alt text for the image (for accessibility)
        /// </summary>
        [StringLength(200)]
        public string? AltText { get; set; }

        /// <summary>
        /// Display order for sorting images in the carousel
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Indicates whether the image is active and visible
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date when the image was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

