using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class RecordTag
    {
        [Required]
        public int RecordId { get; set; }
        public Record Record { get; set; }
        [Required]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
