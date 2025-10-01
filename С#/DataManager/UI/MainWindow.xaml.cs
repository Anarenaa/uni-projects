using System.Windows;
using System.Windows.Controls;
using Infrastructure;
using Microsoft.Win32;
using System.IO;
using static UI.Helper;
using Core;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = null;
        ITransactionManager _manager = new TransactionCsvManager();
        List<Transaction> transactions = new List<Transaction>();
        private readonly List<string> _recentFiles = new List<string>(5);


        public MainWindow()
        {
            InitializeComponent();
            //var reader = new TransactionCsvManager();
            //DataGridTransactions.ItemsSource = reader.Read(@"../../../../Resources/bank_transactions_data_2.csv").ToArray();
        }
        void updateRecentFiles(){
            MenuItemRecentFiles.Items.Clear();
            foreach (var path in _recentFiles)
            {
                var menuItem = new MenuItem { Header = path, Tag = path };
                menuItem.Click += RecentFile_Click;
                MenuItemRecentFiles.Items.Add(menuItem);
            }
        }

        private void RecentFile_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var filePath = menuItem.Tag.ToString();

            // Move the clicked file to the top of the recent list
            _recentFiles.Remove(filePath);
            _recentFiles.Insert(0, filePath);
            updateRecentFiles();

            // Load the data
            StatusBarPath.Content = filePath;
            path = filePath;
            string extension = Path.GetExtension(path).ToLower().TrimStart('.');
            _manager = Helper.GetManager(extension);
            loadData(path);
        }
        
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = $"CSV files (*.csv)|*.csv|JSON files (*.json)|*.json|XML files (*.xml)|*.xml|XSLX files (*.xlsx)|*.xlsx";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                StatusBarPath.Content = dialog.FileName;
                string path = dialog.FileName;

                // Update recent files list
                _recentFiles.Remove(path); // Remove if it already exists to move it to the top
                _recentFiles.Insert(0, path); // Add to the top of the list
                if (_recentFiles.Count > 5) // Keep only the last 5 files
                {
                    _recentFiles.RemoveAt(5);
                }

                // TODO: Update the UI to show the recent files menu

                string extension = Path.GetExtension(path).ToLower().TrimStart('.');

                _manager = Helper.GetManager(extension);

                loadData(path);

                MessageBox.Show($"File loaded: {extension.ToUpper()}");
                updateRecentFiles();
            }
        }

        void updateTransactionsList()
        {
            DataGridTransactions.ItemsSource = null;
            var flatData = transactions.Select(t => new
            {
                t.TransactionID,
                t.Account?.AccountId,
                t.Customer?.CustomerId,
                t.TransactionAmount,
                t.TransactionDate,
                t.TransactionType,
                t.Device?.DeviceId,
                t.Location,
                t.MerchantID,
                t.Channel,
                t.TransactionDuration,
                t.PreviousTransactionDate
            });
            DataGridTransactions.ItemsSource = flatData;
            StatusBarQuantity.Content = $"{transactions.Count} записів";
            updateLastOperationTime();
        }
        void updateLastOperationTime(){
            StatusBarLastOperationTime.Content = DateTime.Now.ToString("HH:mm");
        }
        void loadData(string dataPath)
        {
            try
            {
                transactions = _manager.Read(dataPath);
                updateTransactionsList();
                MenuItemDiagrams.ItemsSource = typeof(Transaction).GetProperties().Select(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var extension = (sender as MenuItem).Tag.ToString();

            var dialog = new SaveFileDialog();
            dialog.Filter = $"{char.ToUpper(extension[0]) + extension.Substring(1).ToLower()} files (*.{extension})|*.{extension}";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                //path = toolStripStatusLabelPath.Text = dialog.FileName;
                path = dialog.FileName;
                _manager = Helper.GetManager(extension);
                _manager.Write(path, transactions);
                updateLastOperationTime();

                MessageBox.Show($"File saved: {extension.ToUpper()}");
            }
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            DataGridTransactions.ItemsSource = null;
            StatusBarPath.Content = StatusBarQuantity.Content = StatusBarLastOperationTime.Content = null;
        }

        private void xlsxReport_Click(object sender, RoutedEventArgs e)
        {
            var generator = new TransactionsXlsxReport();

            var dialog = new SaveFileDialog();
            dialog.Filter = $"XLSX files|*.xlsx";

            var result = dialog.ShowDialog();

            if(result == true)
            {
                string filePath = dialog.FileName;
                generator.GenerateTransactionsReport(transactions, filePath);
            }
        }

        private void docxReport_Click(object sender, RoutedEventArgs e)
        {
            var generator = new TransactionsDocxReport();

            var dialog = new SaveFileDialog();
            dialog.Filter = $"DOCX files|*.docx";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;

                generator.GenerateTransactionsReport(transactions, filePath);
            }
        }
    }
}