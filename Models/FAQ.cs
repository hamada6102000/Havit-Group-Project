using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing a frequently asked question
    /// </summary>
    public class FAQ
    {
        /// <summary>
        /// Unique identifier for the FAQ
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The question text
        /// </summary>
        [Required(ErrorMessage = "Question is required")]
        [StringLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        [Display(Name = "Question")]
        public string Question { get; set; } = string.Empty;

        /// <summary>
        /// The answer text
        /// </summary>
        [Required(ErrorMessage = "Answer is required")]
        [StringLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters")]
        [Display(Name = "Answer")]
        public string Answer { get; set; } = string.Empty;

        /// <summary>
        /// Display order for sorting FAQs
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Indicates whether the FAQ is active and visible
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date when the FAQ was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when the FAQ was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}

