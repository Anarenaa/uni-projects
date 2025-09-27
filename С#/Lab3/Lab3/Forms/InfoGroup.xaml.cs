using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab3.Models;
namespace Lab3.Forms
{
    /// <summary>
    /// Interaction logic for InfoGroup.xaml
    /// </summary>
    public partial class InfoGroup : Window
    {
        public InfoGroup(Group group)
        {
            InitializeComponent();

            ListViewStudentsInGroup.ItemsSource = group.Students;
        }
    }
}
