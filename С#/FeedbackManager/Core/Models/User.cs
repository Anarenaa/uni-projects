using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public enum Role
    {
        Admin = 1,
        Analyst,
        Boss
    }
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        [MaxLength(50)]
        public required string FullName { get; set; }
        [Required]
        [MaxLength(15)]
        public required string PhoneNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Login { get; set; }
        [Required]
        [MaxLength(30)]
        public required string Password { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
