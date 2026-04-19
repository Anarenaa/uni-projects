using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Models;
using Services;

namespace WpfApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthenticationService _authService;
        private readonly IUserSession _userSession;

        [ObservableProperty]
        private string _login = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public Role? CurrentRole { get; private set; }

        public LoginViewModel(AuthenticationService authService, IUserSession userSession)
        {
            _authService = authService;
            _userSession = userSession;
        }

        [RelayCommand]
        private void LoginUser(object window)
        {
            try
            {
                _userSession.CurrentUser = _authService.Authenticate(Login, Password);
                CurrentRole = _userSession.CurrentUser?.Role;

                if (CurrentRole == null)
                    throw new Exception("Authentication failed.");

                if (window is Window loginWindow)
                {
                    loginWindow.DialogResult = true;
                    loginWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
