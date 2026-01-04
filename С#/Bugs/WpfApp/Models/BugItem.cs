using System.ComponentModel;
using System.Windows;

namespace WpfApp.ViewModels
{
    class BugItem
    {
        public required string Error { get; set; }
        public string? Context { get; set; }
        public Visibility ContextVisibility => string.IsNullOrWhiteSpace(Context)
            ? Visibility.Collapsed
            : Visibility.Visible;
        public required string Solution { get; set; }
        public List<FilterItem> Tags { get; set; }
    }
}
