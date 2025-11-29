using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing a client testimonial
    /// </summary>
    public class Testimonial
    {
        /// <summary>
        /// Unique identifier for the testimonial
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Client's testimonial text/quote
        /// </summary>
        [Required(ErrorMessage = "Testimonial text is required")]
        [StringLength(1000, ErrorMessage = "Testimonial text cannot exceed 1000 characters")]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Client's full name
        /// </summary>
        [Required(ErrorMessage = "Client name is required")]
        [StringLength(200, ErrorMessage = "Client name cannot exceed 200 characters")]
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Client's professional title or role
        /// </summary>
        [StringLength(200, ErrorMessage = "Client title cannot exceed 200 characters")]
        public string? ClientTitle { get; set; }

        /// <summary>
        /// Client's company name
        /// </summary>
        [Required(ErrorMessage = "Company name is required")]
        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>
        /// Rating (1-5 stars)
        /// </summary>
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; } = 5;

        /// <summary>
        /// Display order for sorting testimonials
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Indicates whether the testimonial is active and visible
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date when the testimonial was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

