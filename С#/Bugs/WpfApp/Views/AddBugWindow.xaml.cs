using System.Windows;
using WpfApp.ViewModels;

namespace WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for AddBugWindow.xaml
    /// </summary>
    public partial class AddBugWindow : Window
    {
        public AddBugWindow()
        {
            InitializeComponent();
            DataContext = new AddBugViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
