using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Extensions.DependencyInjection;
using WpfApp.ViewModels;

namespace WpfApp.Views.InnerViews
{
    /// <summary>
    /// Interaction logic for AnalystsView.xaml
    /// </summary>
    public partial class AnalystsView : Window
    {
        public AnalystsView(MainViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if(TagFilterPanel.Visibility == Visibility.Visible)
            {
                TagFilterPanel.Visibility = Visibility.Collapsed;
                FilterIcon.Foreground = (Brush)FindResource("MaterialDesignBody");
            }
            else
            {
                TagFilterPanel.Visibility = Visibility.Visible;
                FilterIcon.Foreground = Brushes.Black;
            }
        }

        private void OpenStatsWindow_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = App.ServiceProvider.GetRequiredService<StatsWindow>();
            statsWindow.Show();
        }
    }
}
