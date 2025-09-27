using System.Windows;

using Lab3.Models;
using MessageBox = System.Windows.MessageBox;

namespace Lab3.Forms
{
    public enum LessonType
    {
        Group,
        Individual
    }
    public partial class AddLesson : Window
    {
        private DanceSchool danceSchool;
        private LessonType lessonType;
        public DateTimePicker datePicker { get; set; }
        public NumericUpDown numericUpDown {  get; set; }
        public AddLesson(DanceSchool danceSchool, LessonType type)
        {
            InitializeComponent();
            this.danceSchool = danceSchool;

            lessonType = type;
            if (lessonType == LessonType.Group)
            {
                lblGroupOrStudent.Content = "Група";
                comboBoxGroupOrStudentCreateLesson.ItemsSource = danceSchool.GetGroups();
            }
            else if (lessonType == LessonType.Individual)
            {
                lblGroupOrStudent.Content = "Учень";
                comboBoxGroupOrStudentCreateLesson.ItemsSource = danceSchool.GetStudents();
            }
            comboBoxGroupOrStudentCreateLesson.SelectedIndex = 0;

            comboBoxTeacherCreateLesson.ItemsSource = danceSchool.GetTeachers();
            comboBoxTeacherCreateLesson.SelectedIndex = 0;

            datePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd-MM-yyyy HH:mm",
                Name = "dateOfLesson",
                MinDate = DateTime.Now,
                MaxDate = DateTime.Now.AddMonths(1)
            };

            winFormsHostDate.Child = datePicker;

            numericUpDown = new NumericUpDown
            {
                Name = "durationOfLesson",
                Minimum = 30,
                Maximum = 90,
                Increment = 30
            };

            winFormsHostDuration.Child = numericUpDown;
        }

        public Lesson GetLesson()
        {
            Lesson lesson = new Lesson
            {
                DateOfLesson = datePicker.Value,
                Duration = (double)numericUpDown.Value,
                Teacher = comboBoxTeacherCreateLesson.SelectedItem as Teacher,
            };

            if (lessonType == LessonType.Group)
                lesson.Group = comboBoxGroupOrStudentCreateLesson.SelectedItem as Group;
            
            if(lessonType == LessonType.Individual)
                lesson.Student = comboBoxGroupOrStudentCreateLesson.SelectedItem as Student;

            return lesson;
        }

        private void buttonSaveLesson_Click(object sender, RoutedEventArgs e)
        {
            
            TimeSpan newLessonStart = datePicker.Value.TimeOfDay;

            double newLessonDuration = (double)numericUpDown.Value;

            TimeSpan newLessonEnd = newLessonStart + TimeSpan.FromMinutes(newLessonDuration);


            foreach (Lesson lesson in danceSchool.GetSchedule())
            {
                bool dateMatch = lesson.DateOfLesson.Date == datePicker.Value.Date;

                if (dateMatch)
                {
                    TimeSpan existingStart = lesson.DateOfLesson.TimeOfDay;
                    TimeSpan existingEnd = existingStart + TimeSpan.FromMinutes(lesson.Duration);

                    bool hasOverlap = (newLessonStart < existingEnd) && (newLessonEnd > existingStart);

                    if (hasOverlap)
                    {
                        MessageBox.Show("На цю годину заняття вже існує.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                        return;
                    }
                }
            }

            this.DialogResult = true;
        }
    }
}
