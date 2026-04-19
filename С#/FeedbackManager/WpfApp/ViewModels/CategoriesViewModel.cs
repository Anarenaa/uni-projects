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
    public partial class CategoriesViewModel : ObservableObject
    {
        private readonly CategoryService _categoryService;
        private readonly LogService _logService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsAnyCategory))]
        private ObservableCollection<Category> _categories;

        public bool IsAnyCategory => Categories?.Any() ?? false;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotEditMode))]
        private bool _isEditMode;
        public bool IsNotEditMode => !IsEditMode;

        [ObservableProperty]
        private Feedback _currentFeedback;

        [ObservableProperty]
        private string _newCategoryName = string.Empty;
        [ObservableProperty]
        private Category _selectedCategory;
        [ObservableProperty]
        private int _selectedCategoryId;
        public CategoriesViewModel(CategoryService categoryService,
            Feedback? feedback = null)
        {
            _categoryService = categoryService;
            _logService = App.ServiceProvider.GetRequiredService<LogService>();
            _userSession = App.ServiceProvider.GetRequiredService<IUserSession>();

            if (feedback != null)
                CurrentFeedback = feedback;
            loadCategories();
        }

        private void loadCategories()
        {
            Categories = new ObservableCollection<Category>(_categoryService.GetAllCategories());
        }

        [RelayCommand]
        private void OpenNewCategoryWindow()
        {
            var addCategoryWindow = App.ServiceProvider.GetRequiredService<AddNewCategoryWindow>();
            addCategoryWindow.DataContext = this;
            addCategoryWindow.ShowDialog();
        }
        [RelayCommand]
        private void AddNewCategory(object window)
        {
            var category = new Category { Name = NewCategoryName };
            try
            {
                _categoryService.AddCategory(category);
                _logService.AddLog(new Log
                {
                    Action = Core.Models.Action.Created,
                    EntityType = EntityType.Category,
                    EntityId = category.Id,
                    UserId = _userSession.CurrentUser.Id,
                });
                NewCategoryName = string.Empty;
                loadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (window is Window popupWindow)
            {
                popupWindow.Close();
            }
        }

        [RelayCommand]
        private void EditCategory()
        {
            var editCategoryWindow = App.ServiceProvider.GetRequiredService<AddNewCategoryWindow>();

            //Get the ViewModel of this particular window
            if (editCategoryWindow.DataContext is CategoriesViewModel vm)
            {
                vm.IsEditMode = true;
                vm.SelectedCategoryId = SelectedCategory.Id;
                vm.NewCategoryName = SelectedCategory.Name;
            }
            editCategoryWindow.ShowDialog();
            loadCategories();
        }
        [RelayCommand]
        private void SaveCategoryChanges(object window)
        {
            try
            {
                _categoryService.EditCategory(SelectedCategoryId, NewCategoryName);
                _logService.AddLog(new Log
                {
                    Action = Core.Models.Action.Updated,
                    EntityType = EntityType.Category,
                    EntityId = SelectedCategoryId,
                    UserId = _userSession.CurrentUser.Id
                });
                SelectedCategoryId = 0;
                NewCategoryName = string.Empty;
                IsEditMode = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (window is Window popupWindow)
            {
                popupWindow.Close();
            }
        }
        [RelayCommand]
        private void DeleteCategory()
        {
            if (SelectedCategory != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the category '{SelectedCategory.Name}'?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _categoryService.DeleteCategory(SelectedCategory.Id);
                        _logService.AddLog(new Log
                        {
                            Action = Core.Models.Action.Deleted,
                            EntityType = EntityType.Category,
                            EntityId = SelectedCategory.Id,
                            UserId = _userSession.CurrentUser.Id
                        });
                        loadCategories();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        [RelayCommand]
        private void AddTag(object window)
        {
            if (SelectedCategory == null)
            {
                MessageBox.Show("Оберіть категорію", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentFeedback == null)
            {
                MessageBox.Show("Відгук не знайдено", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CurrentFeedback.TempTags.Any(t => t.Id == SelectedCategory.Id))
            {
                MessageBox.Show("Цей тег вже додано", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CurrentFeedback.TempTags.Add(SelectedCategory);

            if (window is Window popupWindow)
            {
                popupWindow.DialogResult = true;
            }
        }
        [RelayCommand]
        private void RemoveTag(Category category)
        {
            if (CurrentFeedback == null)
                MessageBox.Show("Current Feedback error");

            if (CurrentFeedback.TempTags.Contains(category))
                CurrentFeedback.TempTags.Remove(category);
        }
    }
}
