using System.Windows;
using MessageBox = System.Windows.MessageBox;
using Lab3.Models;

namespace Lab3.Forms
{
    /// <summary>
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : Window
    {
        private bool isEditStudent;
        public AddStudent(bool isEditStudent)
        {
            InitializeComponent();
            datePickerStudentDateOfBirth.DisplayDateEnd = DateTime.Now.AddYears(-5);
            datePickerStudentDateOfBirth.SelectedDate = DateTime.Today.AddYears(-5); 

            this.isEditStudent = isEditStudent;
        }

        public Student GetStudent()
        {
            return new Student
            {
                Name = inputFieldStudentName.txtInput.Text,
                Surname = inputFieldStudentSurname.txtInput.Text,
                DateOfBirth = datePickerStudentDateOfBirth.SelectedDate.Value,
                PhotoPath = lblPhotoPath.Content?.ToString(),
                Info = txtStudentInfo.Text
            };
        }

        private void btnAddPhoto_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            openFileDialog.Title = "Select a file";
            openFileDialog.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png;*.jpeg";
            openFileDialog.FilterIndex = 1;

            bool result = (bool)openFileDialog.ShowDialog();

            if (result)
            {
                string selectedFileName = openFileDialog.FileName;

                if (!isEditStudent)
                    btnAddPhoto.Visibility = Visibility.Hidden;
                lblPhotoPath.Content = selectedFileName;
            }
        }

        private void btnSaveStudent_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(inputFieldStudentName.txtInput.Text))
            {
                MessageBox.Show("Будь ласка, введіть ім'я",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }
            
            if(string.IsNullOrEmpty(inputFieldStudentSurname.txtInput.Text))
            {
                MessageBox.Show("Будь ласка, введіть прізвище.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }
            
            if(!datePickerStudentDateOfBirth.SelectedDate.HasValue)
            {
                MessageBox.Show("Будь ласка, оберіть дату народження",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }
            if (datePickerStudentDateOfBirth.SelectedDate > DateTime.Now.AddYears(-5))
            {
                MessageBox.Show("Учню повинно бути мінімум 5 років.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }
            if (string.IsNullOrEmpty(txtStudentInfo.Text))
            {
                MessageBox.Show("Будь ласка, введіть інформацію про учня.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            this.DialogResult = true;
        }
    }
}
