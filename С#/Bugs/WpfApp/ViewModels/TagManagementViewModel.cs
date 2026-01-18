using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Microsoft.EntityFrameworkCore;
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
        [ObservableProperty]
        private bool _isModified = false;


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

            Tags.CollectionChanged += Tags_CollectionChanged;
        }
        private void Tags_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IsModified = true;

            if (e.NewItems != null)
            {
                foreach (var tag in e.NewItems)
                {
                    if (tag is INotifyPropertyChanged npc)
                        npc.PropertyChanged += Tag_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var tag in e.OldItems)
                {
                    if (tag is INotifyPropertyChanged npc)
                        npc.PropertyChanged -= Tag_PropertyChanged;
                }
            }
        }
        private void Tag_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            IsModified = true;
        }

        [RelayCommand]
        private void AddTagToBug(object parameter)
        {
            if (parameter is not Tag selectedTag)
                return;

            try
            {
                if (IsModified)
                {
                    _tagService.ReplaceAllTags(Tags);
                    IsModified = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to save tags: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var tagItem = new TagItem
            {
                Id = selectedTag.Id,
                Name = selectedTag.Name,
                AccentColor = colors[selectedTag.Id.GetValueOrDefault() % colors.Length]
            };

            if(_tagsInBug.Any(t => t.Id == tagItem.Id))
            {
                MessageBox.Show("Tag is already added", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _tagsInBug.Add(tagItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add tag: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void CheckTagName(string tagName)
        {
            if (Tags.Any(t => t.Name == tagName))
            {
                MessageBox.Show("Tag name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
