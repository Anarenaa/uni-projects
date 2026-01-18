using System.Windows.Media;

namespace WpfApp.Models
{
    public class TagItem
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required Brush AccentColor { get; set; }
    }
}
