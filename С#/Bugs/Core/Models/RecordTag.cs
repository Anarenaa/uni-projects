using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class RecordTag
    {
        [Required]
        public uint RecordId { get; set; }
        public Record Record { get; set; }
        [Required]
        public uint TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
