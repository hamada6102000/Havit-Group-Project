using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing a contact message submitted through the contact form
    /// </summary>
    public class ContactMessage
    {
        /// <summary>
        /// Unique identifier for the contact message
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the person sending the message
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the sender
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Company name (optional)
        /// </summary>
        [StringLength(100, ErrorMessage = "Company name cannot exceed 100 characters")]
        public string? Company { get; set; }

        /// <summary>
        /// Phone number (optional)
        /// </summary>
        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        public string? Phone { get; set; }

        /// <summary>
        /// Subject of the message
        /// </summary>
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Message content
        /// </summary>
        [Required(ErrorMessage = "Message is required")]
        [StringLength(5000, ErrorMessage = "Message cannot exceed 5000 characters")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Optional file attachment path
        /// </summary>
        [StringLength(500)]
        public string? AttachmentPath { get; set; }

        /// <summary>
        /// Original filename of the uploaded file
        /// </summary>
        [StringLength(255)]
        public string? OriginalFileName { get; set; }

        /// <summary>
        /// Date and time when the message was submitted
        /// </summary>
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the message has been read by admin
        /// </summary>
        public bool IsRead { get; set; } = false;
    }
}

