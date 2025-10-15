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
using Core;

namespace UI.Windows
{
    /// <summary>
    /// Interaction logic for OperationsHistoryWindow.xaml
    /// </summary>
    public partial class OperationsHistoryWindow : Window
    {
        public OperationsHistoryWindow(List<OperationRecord> list)
        {
            InitializeComponent();
            OperationsHistoryListView.ItemsSource = list.OrderByDescending(tr => tr.OperationDateTime).ToList();
        }
    }
}
