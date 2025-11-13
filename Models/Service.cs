using System.ComponentModel.DataAnnotations;

namespace FreeLance.Models
{
    /// <summary>
    /// Model representing a service offered by the freelance company
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Unique identifier for the service
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the service
        /// </summary>
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the service
        /// </summary>
        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Icon class name (e.g., Font Awesome or Bootstrap icon)
        /// </summary>
        [StringLength(100)]
        public string? IconClass { get; set; }

        /// <summary>
        /// Date when the service was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the service was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indicates whether the service is active and visible
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}

