using HavitGroup.Models;
using Microsoft.EntityFrameworkCore;

namespace HavitGroup.Data
{
    /// <summary>
    /// Database context for the application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the ApplicationDbContext
        /// </summary>
        /// <param name="options">Database context options</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Contact messages submitted through the contact form
        /// </summary>
        public DbSet<ContactMessage> ContactMessages { get; set; }

        /// <summary>
        /// Services offered by the freelance company
        /// </summary>
        public DbSet<Service> Services { get; set; }

        /// <summary>
        /// Site-wide settings and configuration (singleton pattern)
        /// </summary>
        public DbSet<SiteSettings> SiteSettings { get; set; }

        /// <summary>
        /// Home page carousel images
        /// </summary>
        public DbSet<HomeImage> HomeImages { get; set; }

        /// <summary>
        /// Configures the model relationships and constraints
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ContactMessage entity
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(5000);
                entity.Property(e => e.AttachmentPath).HasMaxLength(500);
                entity.Property(e => e.OriginalFileName).HasMaxLength(255);
                entity.Property(e => e.SubmittedAt).IsRequired();
                entity.HasIndex(e => e.Email);
                entity.HasIndex(e => e.SubmittedAt);
            });

            // Configure Service entity
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.IconClass).HasMaxLength(100);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasIndex(e => e.IsActive);
            });

            // Configure SiteSettings entity (singleton pattern)
            modelBuilder.Entity<SiteSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever(); // Prevent auto-increment
                entity.Property(e => e.Email).HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Mobile).HasMaxLength(50);
                entity.Property(e => e.AddressLine1).HasMaxLength(200);
                entity.Property(e => e.AddressLine2).HasMaxLength(200);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.State).HasMaxLength(100);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.CompanyName).HasMaxLength(200);
                entity.Property(e => e.Tagline).HasMaxLength(500);
                entity.Property(e => e.Mission).HasMaxLength(2000);
                entity.Property(e => e.Values).HasMaxLength(2000);
                entity.Property(e => e.WhyChooseUs).HasMaxLength(2000);
                entity.Property(e => e.FacebookUrl).HasMaxLength(500);
                entity.Property(e => e.TwitterUrl).HasMaxLength(500);
                entity.Property(e => e.LinkedInUrl).HasMaxLength(500);
                entity.Property(e => e.InstagramUrl).HasMaxLength(500);
                entity.Property(e => e.YouTubeUrl).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Configure HomeImage entity
            modelBuilder.Entity<HomeImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImagePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OriginalFileName).HasMaxLength(255);
                entity.Property(e => e.AltText).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.DisplayOrder);
            });
        }
    }
}

