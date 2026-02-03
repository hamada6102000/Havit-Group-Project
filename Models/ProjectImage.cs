using System.ComponentModel.DataAnnotations;

namespace HavitGroup.Models
{
 /// <summary>
 /// Additional image related to a Project
 /// </summary>
 public class ProjectImage
 {
 public int Id { get; set; }

 [Required]
 public int ProjectId { get; set; }

 [Required]
 [StringLength(500)]
 public string ImagePath { get; set; } = string.Empty;

 [StringLength(255)]
 public string? OriginalFileName { get; set; }

 [StringLength(200)]
 public string? AltText { get; set; }

 public int DisplayOrder { get; set; } =0;

 public bool IsActive { get; set; } = true;

 public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

 public virtual Project? Project { get; set; }
 }
}
