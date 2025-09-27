using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab3.Models;

namespace Lab3.Forms
{
    /// <summary>
    /// Interaction logic for AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Window
    {
        private DanceSchool danceSchool;
        public AddGroup(DanceSchool danceSchool)
        {
            InitializeComponent();
            this.danceSchool = danceSchool;

            comboBoxTeachersCreateGroup.ItemsSource = danceSchool.GetTeachers();
            comboBoxTeachersCreateGroup.SelectedIndex = 0;
        }
        
        public Group GetGroup()
        {
            return new Group
            {
                Name = nameOfTheGroup.txtInput.Text,
                Teacher = comboBoxTeachersCreateGroup.SelectedItem as Teacher
            };
        }

        private void btnNewGroupSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameOfTheGroup.txtInput.Text))
            {
                MessageBox.Show("Будь ласка, введіть назву групи",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
        }
    }
}
