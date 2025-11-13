using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing site-wide settings and configuration data
    /// </summary>
    public class SiteSettings
    {
        /// <summary>
        /// Unique identifier for settings (singleton pattern - only one record)
        /// </summary>
        [Key]
        public int Id { get; set; } = 1; // Always 1 for singleton pattern

        // Contact Information
        /// <summary>
        /// Company email address
        /// </summary>
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        /// <summary>
        /// Company phone number
        /// </summary>
        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
        [Display(Name = "Phone Number")]
        public string? Phone { get; set; }

        /// <summary>
        /// Company mobile/alternate phone number
        /// </summary>
        [StringLength(50, ErrorMessage = "Mobile cannot exceed 50 characters")]
        [Display(Name = "Mobile Number")]
        public string? Mobile { get; set; }

        /// <summary>
        /// Company address line 1
        /// </summary>
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        [Display(Name = "Address Line 1")]
        public string? AddressLine1 { get; set; }

        /// <summary>
        /// Company address line 2
        /// </summary>
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        [Display(Name = "Address Line 2")]
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        [Display(Name = "City")]
        public string? City { get; set; }

        /// <summary>
        /// State/Province
        /// </summary>
        [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
        [Display(Name = "State/Province")]
        public string? State { get; set; }

        /// <summary>
        /// Postal/ZIP code
        /// </summary>
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        [Display(Name = "Postal/ZIP Code")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        [Display(Name = "Country")]
        public string? Country { get; set; }

        // Company Information
        /// <summary>
        /// Company name
        /// </summary>
        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        /// <summary>
        /// Company tagline or slogan
        /// </summary>
        [StringLength(500, ErrorMessage = "Tagline cannot exceed 500 characters")]
        [Display(Name = "Tagline")]
        public string? Tagline { get; set; }

        // About Page Content
        /// <summary>
        /// Mission statement for About page
        /// </summary>
        [StringLength(2000, ErrorMessage = "Mission cannot exceed 2000 characters")]
        [Display(Name = "Mission Statement")]
        public string? Mission { get; set; }

        /// <summary>
        /// Values description for About page
        /// </summary>
        [StringLength(2000, ErrorMessage = "Values cannot exceed 2000 characters")]
        [Display(Name = "Company Values")]
        public string? Values { get; set; }

        /// <summary>
        /// Why choose us description for About page
        /// </summary>
        [StringLength(2000, ErrorMessage = "Why choose us cannot exceed 2000 characters")]
        [Display(Name = "Why Choose Us")]
        public string? WhyChooseUs { get; set; }

        // Social Media Links
        /// <summary>
        /// Facebook URL
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        [Display(Name = "Facebook URL")]
        public string? FacebookUrl { get; set; }

        /// <summary>
        /// Twitter URL
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        [Display(Name = "Twitter URL")]
        public string? TwitterUrl { get; set; }

        /// <summary>
        /// LinkedIn URL
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        [Display(Name = "LinkedIn URL")]
        public string? LinkedInUrl { get; set; }

        /// <summary>
        /// Instagram URL
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        [Display(Name = "Instagram URL")]
        public string? InstagramUrl { get; set; }

        /// <summary>
        /// YouTube URL
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format")]
        [StringLength(500, ErrorMessage = "URL cannot exceed 500 characters")]
        [Display(Name = "YouTube URL")]
        public string? YouTubeUrl { get; set; }

        // Timestamps
        /// <summary>
        /// Date when settings were created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when settings were last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}

