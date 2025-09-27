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
using Lab3.Models;

namespace Lab3.Forms
{
    /// <summary>
    /// Interaction logic for InfoStudent.xaml
    /// </summary>
    public partial class InfoStudent : Window
    {
        public InfoStudent(Student student)
        {
            InitializeComponent();

            lblStudentInfo.Content = student.Info;

           
                ImageBrush imageBrush = new ImageBrush();
                if (!string.IsNullOrEmpty(student.PhotoPath))
                    imageBrush.ImageSource = new BitmapImage(new Uri(student.PhotoPath, UriKind.RelativeOrAbsolute));
                else
                    imageBrush.ImageSource = new BitmapImage(new Uri(@"../../../img/profile.png", UriKind.Relative));
                imageBrush.Stretch = Stretch.Uniform;
                photoRectangle.Fill = imageBrush;
        }
    }
}
