using System.IO;
using System.Windows;
using Core;
using Microsoft.Win32;
using ScottPlot.WPF;

namespace UI.Windows
{
    /// <summary>
    /// Interaction logic for Diagram.xaml
    /// </summary>
    public partial class Diagram : Window
    {
        public Diagram(
            List<Transaction> transactions,
            Action<WpfPlot, List<Transaction>> plotAction)
        {
            InitializeComponent();

            wpfPlot.Plot.Clear();
            plotAction(wpfPlot, transactions);
            wpfPlot.Refresh();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string downloadsPath = Registry.GetValue(
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders",
                "{374DE290-123F-4565-9164-39C4925E467B}", // GUID для "Downloads"
                null
            )?.ToString() ?? string.Empty;

            string timestamp = DateTime.Now.Ticks.ToString();
            string fileName = $"chart_{timestamp}.png";

            string filePath = Path.Combine(downloadsPath, fileName);

            wpfPlot.Plot.SavePng(filePath, 1000, 500);

            MessageBox.Show($"Збережено у: {filePath}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
