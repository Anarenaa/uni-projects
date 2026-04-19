using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Services;
using WpfApp.Views.InnerViews;

namespace WpfApp.ViewModels
{
    public class RemoveTagInfo
    {
        public Feedback? Feedback { get; set; }
        public Category? Tag { get; set; }
    }
    public partial class MainViewModel : ObservableObject
    {
        private readonly IUserSession _userSession;
        private readonly FeedbackService _feedbackService;
        private readonly CategoryService _categoryService;
        private readonly LogService _logService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNoFeedback))]
        private ObservableCollection<Feedback> _feedbacks;
        public bool IsNoFeedback => Feedbacks != null && !Feedbacks.Any();

        [ObservableProperty]
        private ObservableCollection<Category> _filterCategories;
        [ObservableProperty]
        private ObservableCollection<int> _selectedCategoryIds = new();
        [ObservableProperty]
        private string _searchQuery = string.Empty;
        partial void OnSearchQueryChanged(string value)
        {
            loadFeedbacks();
        }

        [ObservableProperty]
        private bool _isAnalyzed;
        [ObservableProperty]
        private bool _isImportant;
        public MainViewModel(FeedbackService feedbackService, CategoryService categoryService, IUserSession userSession)
        {
            _feedbackService = feedbackService;
            _categoryService = categoryService;
            _userSession = userSession;
            _logService = App.ServiceProvider.GetRequiredService<LogService>();

            loadFilterCategories();
            loadFeedbacks();
        }
        public bool IsAnalyst => _userSession.CurrentUser?.Role == Role.Analyst;
        public bool IsBoss => _userSession.CurrentUser?.Role == Role.Boss;

        private void loadFeedbacks()
        {
            IEnumerable<Feedback> feedbacks;

            if (IsAnalyst)
            {
                feedbacks = _feedbackService.GetFilteredFeedbacks(isAnalyzed: false, searchText: SearchQuery)
                    .OrderByDescending(x => x.SubmittedAt);
                Feedbacks = new ObservableCollection<Feedback>(feedbacks);
            }
            else
            {
                feedbacks = _feedbackService.GetFilteredFeedbacks(isAnalyzed: true, SelectedCategoryIds, searchText: SearchQuery)
                    .OrderByDescending(x => x.SubmittedAt).OrderByDescending(x => x.IsImportant);
                Feedbacks = new ObservableCollection<Feedback>(feedbacks);
            }
        }
        private void loadFilterCategories()
        {
            var categories = _categoryService.GetAllCategories();
            FilterCategories = new ObservableCollection<Category>(categories);
        }

        [RelayCommand]
        private void RefreshFeedbacks()
        {
            loadFeedbacks();
        }
        [RelayCommand]
        private void Exit()
        {
            var session = App.ServiceProvider.GetRequiredService<IUserSession>();
            session.Logout();

            var loginWindow = App.ServiceProvider.GetRequiredService<MainWindow>();

            var currentWindows = App.Current.Windows.Cast<Window>()
                .Where(w => w != loginWindow)
                .ToList();

            currentWindows.ForEach(w => w.Close());
            loginWindow.Show();
        }
        [RelayCommand]
        private void ToggleCategoryFilter(int categoryId)
        {
            if (SelectedCategoryIds.Contains(categoryId))
            {
                SelectedCategoryIds.Remove(categoryId);
            }
            else
            {
                SelectedCategoryIds.Add(categoryId);
            }

            loadFeedbacks();
        }
        //Tags management
        [RelayCommand]
        private void AddTag(Feedback feedback)
        {
            if (feedback == null) return;

            var catService = App.ServiceProvider.GetRequiredService<CategoryService>();
            var viewModel = new CategoriesViewModel(catService, feedback);

            var addTagWindow = new AddTagWindow(viewModel);
            var res = addTagWindow.ShowDialog();
            if (res == true)
            {
                addTagWindow.Close();
            }
        }

        [RelayCommand]
        private void RemoveTag(RemoveTagInfo info)
        {
            if (info.Feedback != null && info.Tag != null)
                info.Feedback.TempTags.Remove(info.Tag);
        }

        //Analysis
        [RelayCommand]
        private void AnalyzeFeedback(Feedback feedback)
        {
            if (feedback == null) return;

            try
            {
                _feedbackService.AnalyzeFeedback(feedback, IsImportant, _userSession.CurrentUser!);
                _logService.AddLog(new Log
                {
                    Action = Core.Models.Action.Analyzed,
                    EntityType = EntityType.Feedback,
                    EntityId = feedback.Id,
                    UserId = _userSession.CurrentUser.Id
                });

                loadFeedbacks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error analyzing feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
