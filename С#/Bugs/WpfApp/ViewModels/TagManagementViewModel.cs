using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Services;
using WpfApp.Models;

namespace WpfApp.ViewModels
{
    public partial class TagManagementViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Tag> _tags = new();
        
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

        [ObservableProperty]
        private string _tagName;


        private readonly TagService _tagService;
        private ObservableCollection<TagItem> _tagsInBug;

        public TagManagementViewModel(ObservableCollection<TagItem> tagItems)
        {
            _tagService = new TagService();
            _tagsInBug = tagItems;
            foreach (var tag in _tagService.GetAllTags())
            {
                _tags.Add(tag);
            }
        }

        public void AddNewTag(Tag selectedTag)
        {
            if (string.IsNullOrWhiteSpace(selectedTag.Name))
                return;

            try
            {
                _tagService.AddTag(selectedTag);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error adding tag: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        [RelayCommand]
        private void AddTagToBug(Tag selectedTag)
        {
            var tagItem = new TagItem
            {
                Id = selectedTag.Id,
                Name = selectedTag.Name,
                AccentColor = colors[selectedTag.Id.Value % colors.Length]
            };
            _tagsInBug.Add(tagItem);
        }
    }
}
