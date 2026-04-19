using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class FeedbackCategory
    {
        [Required]
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
