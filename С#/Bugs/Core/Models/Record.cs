using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [Required]
        public required string Error { get; set; }
        public string? Context { get; set; }
        [Required]
        public required string Solution { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
