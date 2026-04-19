using System.Windows;
using System.Windows.Controls;
using Core.Models;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.ViewModels;
using WpfApp.Views.ClientViews;
using WpfApp.Views.InnerViews;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FeedbackViewModel _feedbackViewModel;
        private readonly LoginViewModel _loginViewModel;
        public MainWindow(FeedbackViewModel feedbackViewModel, LoginViewModel loginViewModel)
        {
            InitializeComponent();
            _feedbackViewModel = feedbackViewModel;
            _loginViewModel = loginViewModel;
            this.DataContext = feedbackViewModel;
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(RateTextBox.Text))
            {
                MessageBox.Show("Будь ласка, введіть вашу оцінку.", "Rating Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(!int.TryParse(RateTextBox.Text, out int rating) || rating < 0 || rating > 10)
            {
                MessageBox.Show("Введіть оцінку від 0 до 10.", "Invalid Rating", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(string.IsNullOrWhiteSpace(FeedbackTextBox.Text))
            {
                MessageBox.Show("Будь ласка, напишіть ваш відгук.", "Feedback Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var clientInfoDialog = new ClientInfoDialog(_feedbackViewModel);
            clientInfoDialog.Owner = this;
            clientInfoDialog.ShowDialog();
            clientInfoDialog.Close();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = App.ServiceProvider.GetRequiredService<LogInWindow>();
            loginWindow.Owner = this;
            var result = loginWindow.ShowDialog();
            if (result == true)
            {
                var role = (loginWindow.DataContext as LoginViewModel)?.CurrentRole;

                Window? nextWindow = role switch
                {
                    Role.Admin => App.ServiceProvider.GetRequiredService<AdminView>(),
                    Role.Analyst or Role.Boss => App.ServiceProvider.GetRequiredService<AnalystsView>(),
                    _ => null
                };

                if (nextWindow != null)
                {
                    var allWindows = Application.Current.Windows.Cast<Window>()
                                .Where(w => w != nextWindow)
                                .ToList();
                    allWindows.ForEach(w => w.Close());

                    nextWindow.Show();
                }
            }
        }
    }
}