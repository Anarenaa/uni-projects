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
    public partial class UsersViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly LogService _logService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private StatisticsViewModel _statsVM;

        [ObservableProperty]
        private List<Role> _roles;
        [ObservableProperty]
        private ObservableCollection<User>? _users;

        [ObservableProperty]
        private Role _selectedRole;
        [ObservableProperty]
        private string _newFullName = string.Empty;
        [ObservableProperty]
        private string _newPhoneNumber = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSelectedUser))]
        private User? selectedUser;
        private bool IsSelectedUser => SelectedUser != null;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotEditMode))]
        private bool _isEditMode; 
        [ObservableProperty]
        private int _currentUserId;

        public bool IsNotEditMode => !IsEditMode;

        public UsersViewModel(UserService userService, LogService logService, StatisticsViewModel statsVM)
        {
            _userService = userService;
            _logService = logService;
            _userSession = App.ServiceProvider.GetRequiredService<IUserSession>();

            _statsVM = statsVM;

            Roles = _userService.GetAllRoles();

            loadUsers();
        }
        private void loadUsers()
        {
            var users = _userService.GetActiveUsers();
            Users = new ObservableCollection<User>(users);
        }
        [RelayCommand]
        private void OpenAddUserPopup()
        {
            var addUserWindow = App.ServiceProvider.GetRequiredService<AddUserWindow>();
            {
                addUserWindow.DataContext = this;
            }
            ;
            addUserWindow.ShowDialog();
        }
        [RelayCommand]
        private void AddNewUser(object window)
        {
            try
            {
                int newUserId = _userService.AddUser(SelectedRole, NewFullName, NewPhoneNumber);
                _logService.AddLog(new Log
                {
                    Action = Core.Models.Action.Created,
                    EntityType = EntityType.User,
                    EntityId = newUserId,
                    UserId = _userSession.CurrentUser.Id
                });
                NewFullName = string.Empty;
                NewPhoneNumber = string.Empty;
                loadUsers();
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
        private void EditUser()
        {
            var editUserWindow = App.ServiceProvider.GetRequiredService<AddUserWindow>();

            //Get the ViewModel of this particular window
            if (editUserWindow.DataContext is UsersViewModel vm)
            {
                vm.IsEditMode = true;
                vm.CurrentUserId = SelectedUser.Id;
                vm.NewFullName = SelectedUser.FullName;
                vm.NewPhoneNumber = SelectedUser.PhoneNumber;
            };
            editUserWindow.ShowDialog();
            loadUsers(); //AdminView responsibility
        }

        [RelayCommand]
        private void SaveUserChanges(object window)
        {
            try
            {
                _userService.UpdateUser(CurrentUserId, NewFullName, NewPhoneNumber);
                _logService.AddLog(new Log
                {
                    Action = Core.Models.Action.Updated,
                    EntityType = EntityType.User,
                    EntityId = CurrentUserId,
                    UserId = _userSession.CurrentUser.Id
                });

                CurrentUserId = 0;
                NewFullName = string.Empty;
                NewPhoneNumber = string.Empty;
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
        private void DeactivateUser()
        {
            try
            {
                _userService.DeactivateUser(SelectedUser.Id);
                _logService.AddLog(new Log
                {
                    Action = Core.Models.Action.Deactivated,
                    EntityType = EntityType.User,
                    EntityId = SelectedUser.Id,
                    UserId = _userSession.CurrentUser.Id
                });
                loadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
