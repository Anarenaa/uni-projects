using System.Collections.ObjectModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Services;
using WpfApp.Messages;
using WpfApp.Models;
using WpfApp.Views;
using WpfApp.Windows;

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

        private readonly RecordService _recordService;

        public ObservableCollection<FilterItem> Tags { get; set; } = new();
        public ObservableCollection<BugItem> Bugs { get; } = new();

        public int BugsCount => Bugs.Count;

        public MainViewModel()
        {
            _recordService = new RecordService();

            Bugs.CollectionChanged += (s, e) => OnPropertyChanged(nameof(BugsCount));

            for (int i = 0; i < names.Count; i++)
            {
                Tags.Add(new FilterItem
                {
                    TagItem = new TagItem
                    {
                        Name = names[i],
                        AccentColor = colors[i % colors.Length]
                    },
                    IsSelected = false
                });
            }
            foreach (var record in _recordService.GetAllRecords())
            {
                Bugs.Add(
                    new BugItem
                    {
                        Error = record.Error,
                        Context = record.Context,
                        Solution = record.Solution
                    }
                );
            }
            //get message about new bug added
            WeakReferenceMessenger.Default.Register<BugAddedMessage>(this, (r, m) =>
            {
                Bugs.Add(m.NewBug);
            });
        }

        [RelayCommand]
        private void AddNewBug()
        {
            var addBugViewModel = new AddBugViewModel();
            var addBugWindow = new AddBugWindow();
            var res = addBugWindow.ShowDialog();
            if (res == true)
            {
                addBugWindow.Close();
            }
        }
    }

}
