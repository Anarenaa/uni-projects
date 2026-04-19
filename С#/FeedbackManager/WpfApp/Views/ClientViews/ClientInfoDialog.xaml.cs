using System.Windows;
using WpfApp.ViewModels;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ClientInfoDialog.xaml
    /// </summary>
    public partial class ClientInfoDialog : Window
    {
        public ClientInfoDialog(FeedbackViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
