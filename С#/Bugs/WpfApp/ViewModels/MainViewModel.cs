using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WpfApp.ViewModels
{
    partial class MainViewModel : ObservableObject
    {
        private readonly List<string> names = new List<string>
        {
            "C#",
            "types",
            "design",
            "logic",
            "database",
            "backend",
            "frontend",
            "API",
            "testing",
            "security",
            "cloud",
            "DevOps",
            "mobile",
            "async",
            "patterns",
            "debugging",
            "architecture",
            "UI/UX",
            "performance",
            "deployment"
        };
        private readonly Brush[] colors = new[]
        {
            Brushes.Yellow,
            Brushes.Plum,
            Brushes.Coral,
            Brushes.SkyBlue,
            Brushes.Bisque,
            Brushes.Cyan,
            Brushes.GreenYellow,
            Brushes.PeachPuff,
            Brushes.HotPink,
            Brushes.Wheat,
            Brushes.Orange,
            Brushes.LightYellow,
            Brushes.Aquamarine,
            Brushes.Gold,
            Brushes.LightSalmon,
            Brushes.NavajoWhite,
            Brushes.Chartreuse,
            Brushes.LightPink
        };

        public ObservableCollection<FilterItem> Tags { get; set; } = new();
        public ObservableCollection<BugItem> Bugs { get; } = new();

        public int BugsCount => Bugs.Count;

        public MainViewModel()
        {
            Bugs.CollectionChanged += (s, e) => OnPropertyChanged(nameof(BugsCount));

            for (int i = 0; i < names.Count; i++)
            {
                Tags.Add(new FilterItem
                {
                    Name = names[i],
                    AccentColor = colors[i % colors.Length]
                });
            }
            Bugs.Add(
                new BugItem
                {
                    Error = "System.InvalidOperationException: The seed entity for entity " +
                    "type 'OfficersRoles (Dictionary<string, object>)' cannot be added " +
                    "because the value provided for the property 'OfficersId' is not compatible " +
                    "with the property type 'long'.",
                    Solution = "instead of id like \"1\", \"2\" we write \"1L\", \"2L\" // L = long",
                    Tags = new(){Tags[0], Tags[1]}
                }
            );
            Bugs.Add(
                new BugItem
                {
                    Error = "System.InvalidOperationException: The seed entity for entity " +
                    "type 'OfficersRoles (Dictionary<string, object>)' cannot be added " +
                    "because the value provided for the property 'OfficersId' is not compatible " +
                    "with the property type 'long'.",
                    Context = "During migration",
                    Solution = "instead of id like \"1\", \"2\" we write \"1L\", \"2L\" // L = long",
                    Tags = new() { Tags[1], Tags[3] }
                }
            );
        }
    }

}
