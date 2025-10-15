using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Transaction = Core.Transaction;
using Customer = Core.Customer;

namespace UI.Windows
{
    /// <summary>
    /// Interaction logic for EditTransaction.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        private Transaction transaction;
        public EditTransaction(List<Transaction> transactions, Transaction tr)
        {
            InitializeComponent();
            transaction = tr;
            DataContext = transaction;
            ComboBoxCustomerId.ItemsSource = transactions.OrderBy(t => t.Customer.CustomerId).Select(t => t.Customer).Distinct().ToList();
            ComboBoxCustomerId.DisplayMemberPath = "CustomerId";
            DatePickerTransactionDate.Maximum = DateTime.Now;

            ComboBoxCustomerId.SelectedValue = transaction.Customer;
            TextBoxAmount.Text = transaction.TransactionAmount.ToString();
            DatePickerTransactionDate.Value = transaction.TransactionDate;
        }
        private void TextBoxAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^\d*(,?\d*)?$"))
            {
                e.Handled = true;
            }
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string customerId = ComboBoxCustomerId.Text;

            if (string.IsNullOrWhiteSpace(customerId))
            {
                MessageBox.Show("Customer ID не може бути пустим.", "Помилка валідації", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var allCustomers = ((List<Customer>)ComboBoxCustomerId.ItemsSource).Distinct().ToList();

            var existingCustomer = allCustomers.FirstOrDefault(c => c.CustomerId.Equals(customerId, StringComparison.OrdinalIgnoreCase));

            if (existingCustomer == null)
            {
                MessageBox.Show($"Клієнта з ID '{customerId}' не знайдено. Будь ласка, введіть або оберіть існуючий ID.", "Клієнта не знайдено", MessageBoxButton.OK, MessageBoxImage.Error);
                return; 
            }
            transaction.Customer = existingCustomer;

            decimal newAmount = decimal.Parse(TextBoxAmount.Text);
            if (newAmount <= 0)
            {
                MessageBox.Show("Transaction amount must be greater than zero.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            transaction.TransactionAmount = newAmount;
            transaction.Account.AccountBalance += (newAmount - transaction.TransactionAmount);
            transaction.TransactionDate = DatePickerTransactionDate.Value ?? DateTime.Now;

            this.DialogResult = true;
        }

    }
}
