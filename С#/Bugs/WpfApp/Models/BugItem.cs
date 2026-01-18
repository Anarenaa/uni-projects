using System.Windows;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public class BugItem
    {
        public required string Error { get; set; }
        public string? Context { get; set; }
        public Visibility ContextVisibility => string.IsNullOrWhiteSpace(Context)
            ? Visibility.Collapsed
            : Visibility.Visible;
        public required string Solution { get; set; }
        public List<TagItem> Tags { get; set; }
    }
}
