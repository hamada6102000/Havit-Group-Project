using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
    /// <summary>
    /// Model representing site statistics (singleton pattern)
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Unique identifier for statistics (singleton pattern - only one record)
        /// </summary>
        [Key]
        public int Id { get; set; } = 1; // Always 1 for singleton pattern

        /// <summary>
        /// Number of completed projects
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Completed projects must be a positive number")]
        [Display(Name = "Completed Projects")]
        public int CompletedProjects { get; set; } = 500;

        /// <summary>
        /// Number of countries served
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Countries served must be a positive number")]
        [Display(Name = "Countries Served")]
        public int CountriesServed { get; set; } = 45;

        /// <summary>
        /// Number of happy clients
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Happy clients must be a positive number")]
        [Display(Name = "Happy Clients")]
        public int HappyClients { get; set; } = 1000;

        /// <summary>
        /// Satisfaction rate percentage (0-100)
        /// </summary>
        [Range(0, 100, ErrorMessage = "Satisfaction rate must be between 0 and 100")]
        [Display(Name = "Satisfaction Rate (%)")]
        public int SatisfactionRate { get; set; } = 98;

        /// <summary>
        /// Date when statistics were created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date when statistics were last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}

