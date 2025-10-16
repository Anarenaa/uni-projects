using System.IO;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using Core;
using Transaction = Core.Transaction;
using Infrastructure;
using Microsoft.Win32;
using UI.Windows;
using static UI.Helper;

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

        void updateMenuItemsState()
        {            
            bool hasFile = !string.IsNullOrEmpty(path);
            
            MenuItemSaveAs.IsEnabled = hasFile;
            MenuItemAdd.IsEnabled = hasFile;

            MenuItemOperations.IsEnabled = hasFile;
            MenuItemReports.IsEnabled = hasFile;
            MenuItemCharts.IsEnabled = hasFile;
        }

        void updateRecentFiles()
        {
            MenuItemRecentFiles.Items.Clear();
            path = StatusBarPath.Content?.ToString();
            foreach (var path in _recentFiles)
            {
                var menuItem = new MenuItem { Header = path, Tag = path };
                menuItem.Click += RecentFile_Click;
                MenuItemRecentFiles.Items.Add(menuItem);
            }
        }

        void fileExit()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Бажаєте зберегти файл?",
                "Підтвердження",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            try
            {
                if (result == MessageBoxResult.Yes && !string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    string extension = Path.GetExtension(path).ToLower().TrimStart('.');
                    _manager = Helper.GetManager(extension);
                    _manager.Write(path, transactions);
                    MessageBox.Show("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}");
            }
            finally
            {
                path = null;
                StatusBarPath.Content = null;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        //Window
        public MainWindow()
        {
            InitializeComponent();
            
            updateMenuItemsState();
            
            MenuItemEdit.IsEnabled = DataGridTransactions.SelectedItem != null;
            MenuItemRemove.IsEnabled = DataGridTransactions.SelectedItem != null;

            updateRecentFiles();

        }
        private void Window_Closing(object sender, EventArgs e)
        {
            if (StatusBarPath.Content != null)
            {
                fileExit();
            }
        }
        //----------------------------------------------------------------

        //File 
        private void MenuItemNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (StatusBarPath.Content != null)
            {
                fileExit();
            }

            var dialog = new SaveFileDialog();
            dialog.Filter = $"CSV files (*.csv)|*.csv";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;

                if (filePath != null)
                {
                    StatusBarPath.Content = filePath;
                    path = filePath;
                    string extension = Path.GetExtension(path).ToLower().TrimStart('.');
                    _manager = Helper.GetManager(extension);
                    transactions.Clear();
                    operationRecords.Clear();
                    updateTransactionsList();
                    updateMenuItemsState();
                }
            }
        }
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (StatusBarPath.Content != null)
            {
                fileExit();
            }

            var dialog = new OpenFileDialog();
            dialog.Filter = $"CSV files (*.csv)|*.csv|JSON files (*.json)|*.json|XML files (*.xml)|*.xml|XSLX files (*.xlsx)|*.xlsx";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                string filePath = dialog.FileName;
                path = filePath;
                StatusBarPath.Content = filePath;
                
                _recentFiles.Remove(filePath);
                if (_recentFiles.Count > 5)
                {
                    _recentFiles.RemoveAt(5);
                }
                _recentFiles.Insert(0, filePath);

                string extension = Path.GetExtension(path).ToLower().TrimStart('.');

                _manager = Helper.GetManager(extension);

                loadData(path);

                MessageBox.Show($"File loaded: {extension.ToUpper()}");
                operationRecords.Clear();
                updateMenuItemsState();
            }
        }
        //Save as
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var extension = (sender as MenuItem).Tag.ToString();

            var dialog = new SaveFileDialog();
            dialog.Filter = $"{char.ToUpper(extension[0]) + extension.Substring(1).ToLower()} files (*.{extension})|*.{extension}";

            var result = dialog.ShowDialog();

            if (result == true)
            {
                path = dialog.FileName;
                _manager = Helper.GetManager(extension);
                _manager.Write(path, transactions);
                updateLastOperationTime();

                MessageBox.Show($"File saved: {extension.ToUpper()}");
            }
        }
        private void RecentFile_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var filePath = menuItem.Tag.ToString();

            _recentFiles.Remove(filePath);
            _recentFiles.Insert(0, filePath);

            StatusBarPath.Content = filePath;
            path = filePath;
            string extension = Path.GetExtension(path).ToLower().TrimStart('.');
            _manager = Helper.GetManager(extension);
            loadData(path);
        }
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            if(StatusBarPath.Content == null)
            {
                Application.Current.Shutdown();
                return;
            }
            fileExit();
            DataGridTransactions.ItemsSource = null;
            StatusBarPath.Content = StatusBarQuantity.Content = StatusBarLastOperationTime.Content = null;
            operationRecords.Clear();

            MenuItemSaveAs.IsEnabled = path != null;
            MenuItemAdd.IsEnabled = path != null;
            updateMenuItemsState();
        }

        //----------------------------------------------------------------

        // Reports
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
        //----------------------------------------------------------------

        // Charts
        TransactionChartManager chartManager = new TransactionChartManager();
        private void MenuItemTransactionDate_Click(object sender, RoutedEventArgs e)
        {
            Diagram diagram = new Diagram(transactions, chartManager.PlotTransactionsOverTime);
            diagram.Show();
        }
        private void MenuItemTransactionAmount_Click(object sender, RoutedEventArgs e)
        {
            Diagram diagram = new Diagram(transactions, chartManager.PlotTransactionsAmount);
            diagram.Show();
        }

        private void MenuItemTransactionType_Click(object sender, RoutedEventArgs e)
        {
            Diagram diagram = new Diagram(transactions, chartManager.PlotTransactionTypes);
            diagram.Show();
        }
        private void MenuItemTransactionChannel_Click(object sender, RoutedEventArgs e)
        {
            Diagram diagram = new Diagram(transactions, chartManager.PlotTransactionChannel);
            diagram.Show();
        }
        //----------------------------------------------------------------

        // CRUD Operations
        List<OperationRecord> operationRecords = new List<OperationRecord>();
        private void DataGridTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MenuItemEdit.IsEnabled = DataGridTransactions.SelectedIndex != -1;
            MenuItemRemove.IsEnabled = DataGridTransactions.SelectedItem != null;
        }

        private OperationRecord addOperationRecord(string operationName, string tranId)
        {
            return new OperationRecord
            {
                RecordID = operationRecords.Any<OperationRecord>() ? operationRecords.Max(r => r.RecordID) + 1 : 1,
                TransactionID = tranId,
                OperationName = operationName,
                OperationDateTime = DateTime.Now
            };
        }
        Random random = new Random();
        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            int channelIndex = random.Next(Helper.transactionChannels.Length);
            int typeIndex = random.Next(Helper.transactionTypes.Length);
            int cityIndex= random.Next(Helper.cities.Length);

            decimal calculatedAmount = Math.Round((decimal)random.NextDouble() * 300, 2);

            Transaction newTransaction = new Transaction
            {
                TransactionID = "TX" + (transactions.Any<Transaction>()
                    ? transactions.Max(t => int.Parse(t.TransactionID.Substring(2))) + 1
                    : 1
                ).ToString("D6"),
                Customer = new Customer
                {
                    CustomerId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                    Age = random.Next(18, 70),
                    Occupation = "Student"
                },
                TransactionAmount = calculatedAmount,
                Account = new Account
                {
                    AccountId = "AC" + Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper(),
                    AccountBalance = random.Next(1000) + calculatedAmount
                },
                TransactionDate = DateTime.Now,
                TransactionType = Helper.transactionTypes[typeIndex],
                Device = new Device
                {
                    DeviceId = "D" + (transactions.Any<Transaction>()
                        ? transactions.Max(t => int.Parse(t.Device.DeviceId.Substring(1))) + 1
                        : 1
                    ).ToString("D6"),
                    IPAddress = $"{random.Next(1, 224)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}"
                },
                Location = Helper.cities[cityIndex],
                MerchantID = "M" + (transactions.Any<Transaction>()
                    ? transactions.Max(t => int.Parse(t.MerchantID.Substring(1))) + 1
                    : 1
                ).ToString("D3"),
                Channel = Helper.transactionChannels[channelIndex],
                TransactionDuration = random.Next(301),
                PreviousTransactionDate = DateTime.Now.AddHours(-random.Next(1, 24))
            };

            transactions.Add(newTransaction);
            operationRecords.Add(addOperationRecord("Added", newTransaction.TransactionID));
            updateTransactionsList();
            MessageBox.Show($"Transaction {newTransaction.TransactionID} was added.");

        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            int transactionIndex = DataGridTransactions.SelectedIndex;
            Transaction transaction = transactions[transactionIndex];

            if (transaction == null) return;

            EditTransaction editTransactionWindow = new EditTransaction(transactions, transaction);

            var result = editTransactionWindow.ShowDialog();
            if(result == true)
            {
                operationRecords.Add(addOperationRecord("Updated", transaction.TransactionID));
                updateTransactionsList();
                MessageBox.Show($"Transaction {transaction.TransactionID} was updated.");
            }
        }
        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            int transactionIndex = DataGridTransactions.SelectedIndex;
            Transaction transaction = transactions[transactionIndex];


            MessageBoxResult result = MessageBox.Show(
                $"Ви впевнені, що хочете видалити транзакцію {transaction.TransactionID}?",
                "Підтвердження видалення", 
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if(result == MessageBoxResult.Yes)
            {
                transactions.Remove(transaction);
                updateTransactionsList();
                MessageBox.Show($"Transaction {transaction.TransactionID} was deleted.");
                operationRecords.Add(addOperationRecord("Deleted", transaction.TransactionID));
            }
        }
        private void MenuItemTransactionHistory_Click(object sender, RoutedEventArgs e)
        {
            OperationsHistoryWindow OperationsHistoryWindow = new OperationsHistoryWindow(operationRecords);
            OperationsHistoryWindow.ShowDialog();
        }
        //----------------------------------------------------------------
    }
}