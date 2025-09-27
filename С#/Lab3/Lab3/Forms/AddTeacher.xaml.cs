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
    /// Interaction logic for AddTeacher.xaml
    /// </summary>
    public partial class AddTeacher : Window
    {
        private bool isEditTeacher;
        public AddTeacher(bool isEditTeacher)
        {
            InitializeComponent();
            this.isEditTeacher = isEditTeacher;
        }

        private void numericNewTeacherExperience_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _); //дозволяє вводити тільки цифри
        }

        private void btnNewTeacherSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputFieldNewTeacherName.txtInput.Text))
            {
                MessageBox.Show("Введіть ім'я",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(inputFieldNewTeacherSurname.txtInput.Text))
            {
                MessageBox.Show("Введіть прізвище",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(inputFieldNewTeacherPatronymic.txtInput.Text))
            {
                MessageBox.Show("Введіть по батькові",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            this.DialogResult = true;
        }

        private void btnAddPhotoTeacher_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.Title = "Select a file";
            openFileDialog.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png;*.jpeg";
            openFileDialog.FilterIndex = 1;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFileName = openFileDialog.FileName;

                if(!isEditTeacher)
                    btnAddPhotoTeacher.Visibility = Visibility.Hidden;
                lblPhotoPathTeacher.Content = selectedFileName;
            }
        }

        public Teacher GetTeacher()
        {
            return new Teacher
            {
                Name = inputFieldNewTeacherName.txtInput.Text,
                Surname = inputFieldNewTeacherSurname.txtInput.Text,
                Patronymic = inputFieldNewTeacherPatronymic.txtInput.Text,
                YearsOfExpirience = double.Parse(numericNewTeacherExperience.Text),
                PhotoPath = lblPhotoPathTeacher.Content?.ToString()
            };
        }
    }
}
