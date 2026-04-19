using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.ViewModels;

namespace WpfApp.Views.InnerViews
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        public AdminView(UsersViewModel viewModel, MainViewModel mainViewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            ExitButton.DataContext = mainViewModel;
        }

        private void ShowLogButton_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = App.ServiceProvider.GetRequiredService<LogWindow>();
            logWindow.ShowDialog();
        }
    }
}
