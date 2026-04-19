using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Models
{
    public enum Action
    {
        Created = 1,
        Updated,
        Analyzed,
        Deleted,
        Deactivated
    }
    public enum EntityType
    {
        Feedback = 1,
        User,
        Category,
        Report
    }
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Action Action { get; set; }
        [Required]
        public EntityType EntityType { get; set; }
        public int? EntityId { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public string? AuthorEmail { get; set; }

        public string UserString => User != null ? $"{User.Id}" : AuthorEmail ?? "Unknown";

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
