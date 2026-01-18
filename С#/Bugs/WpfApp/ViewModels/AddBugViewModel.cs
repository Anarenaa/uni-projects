using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Services;
using WpfApp.Messages;
using WpfApp.Models;
using WpfApp.Views;

namespace WpfApp.ViewModels
{
    public partial class AddBugViewModel : ObservableObject
    {
        private readonly RecordService _recordService;

        [ObservableProperty]
        private string _error = "";
        [ObservableProperty]
        private string _context = "";
        [ObservableProperty]
        private string _solution = "";

        [ObservableProperty]
        private ObservableCollection<TagItem> _tagsInBug = new();

        public AddBugViewModel()
        {
            _recordService = new RecordService();
        }

        [RelayCommand]
        private void SaveBug()
        {
            var newRecord = new Record
            {
                Error = Error,
                Context = Context,
                Solution = Solution
            };
            try
            {
                _recordService.AddRecord(newRecord);

                if (TagsInBug.Count > 0)
                    foreach (var tag in TagsInBug)
                        _recordService.AddTagToRecord(newRecord.Id.Value, tag.Id.Value);
                
                var newBug = new BugItem {
                    Error = newRecord.Error,
                    Context = newRecord.Context,
                    Solution = newRecord.Solution
                };
                //send message to MainViewModel about new bug added
                WeakReferenceMessenger.Default.Send(new BugAddedMessage(newBug));

            }
            catch (DbUpdateException ex)
            {
                // Отримуємо саму глибоку помилку
                var message = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show(message); // Тепер ви побачите реальну причину від бази
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
        }
        [RelayCommand]
        private void AddTagToBug()
        {
            var tagManagementWindow = new TagManagementWindow(TagsInBug);
            var res = tagManagementWindow.ShowDialog();
            if (res == true)
            {
                tagManagementWindow.Close();
            }
        }

        [RelayCommand]
        private void RemoveTagFromBug(TagItem tag)
        {
            TagsInBug.Remove(tag);
        }
    }
}
