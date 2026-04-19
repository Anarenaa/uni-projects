using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Range(0,10, ErrorMessage = "Rate must be between 0 and 10.")]
        public int Rate { get; set; }
        [Required]
        [MaxLength(4000)]
        public required string Comment { get; set; }
        [Required]
        [MaxLength(50)]
        public required string AuthorName { get; set; }
        [Required]
        [MaxLength(100)]
        public required string AuthorEmail { get; set; }
        [Required]
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        [Required]
        public bool IsAnalyzed { get; set; } = false;
        [Required]
        public bool IsImportant { get; set; } = false;
        [Required]
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;
        
        public int? AnalystId { get; set; }
        public User? Analyst { get; set; }
        public DateTime? AnalyzedAt { get; set; }

        public ICollection<FeedbackCategory> FeedbackCategories { get; set; } = new List<FeedbackCategory>();
        
        [NotMapped]
        public ObservableCollection<Category> TempTags { get; set; } = new();
    }
}
