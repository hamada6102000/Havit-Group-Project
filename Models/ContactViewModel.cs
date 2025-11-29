using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// ViewModel for the contact form
    /// </summary>
    public class ContactViewModel
    {
        /// <summary>
        /// Name of the person sending the message
        /// </summary>
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the sender
        /// </summary>
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Company name (optional)
        /// </summary>
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        [Display(Name = "Company")]
        public string? Company { get; set; }

        /// <summary>
        /// Phone number (optional)
        /// </summary>
        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        [Display(Name = "Phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Subject of the message
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
        [Display(Name = "Subject")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Message content
        /// </summary>
        [Required(ErrorMessage = "Message is required")]
        [StringLength(5000, ErrorMessage = "Message cannot exceed 5000 characters")]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Optional file attachment
        /// </summary>
        [Display(Name = "Attachment (Optional)")]
        public IFormFile? Attachment { get; set; }
    }
}

