using System.Windows;
using System.Windows.Controls;
using System.IO;
using CsvHelper.Configuration;
using Lab3.Forms;
using Lab3.Models;
using Lab3.Models.ReadersWriters;
using MessageBox = System.Windows.MessageBox;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DanceSchool danceSchool = new DanceSchool();
        bool isEditStudent = false;
        bool isEditTeacher = false;
        DateTime day = DateTime.Today;


        private IReaderWriter<Student> _studentReaderWriter;
        private IReaderWriter<Teacher> _teacherReaderWriter;
        private IReaderWriter<Group> _groupReaderWriter;
        private IReaderWriter<Lesson> _lessonReaderWriter;

        private IReaderWriter<Student> _studentReaderWriterAuto = new JsonReaderWriter<Student>();
        private IReaderWriter<Teacher> _teacherReaderWriterAuto = new JsonReaderWriter<Teacher>();
        private IReaderWriter<Group> _groupReaderWriterAuto = new JsonReaderWriter<Group>();
        private IReaderWriter<Lesson> _lessonReaderWriterAuto = new JsonReaderWriter<Lesson>();

        string pathAutoSave = @"D:\C#\LabOA\Official-Labs\Lab3\Lab3\files\autosave";

        public MainWindow()
        {
            InitializeComponent();

            dataGridStudents.ItemsSource = danceSchool.GetStudents();
            dataGridTeachers.ItemsSource = danceSchool.GetTeachers();
            dataGridGroups.ItemsSource = danceSchool.GetGroups();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridStudents_SelectionChanged(null, null);
            dataGridTeachers_SelectionChanged(null, null);
            dataGridGroups_SelectionChanged(null, null);
            dataGridSchedule_SelectionChanged(null, null);
            updateDate();

            
            if(!string.IsNullOrWhiteSpace(File.ReadAllText(Path.Combine(pathAutoSave, "students.json"))) )
                danceSchool.AddStudents(_studentReaderWriterAuto.Read(Path.Combine(pathAutoSave, "students.json")));

            if (!string.IsNullOrWhiteSpace(File.ReadAllText(Path.Combine(pathAutoSave, "teachers.json"))))
                danceSchool.AddTeachers(_teacherReaderWriterAuto.Read(Path.Combine(pathAutoSave, "teachers.json")));

            if (!string.IsNullOrWhiteSpace(File.ReadAllText(Path.Combine(pathAutoSave, "groups.json"))))
                danceSchool.AddGroups(_groupReaderWriterAuto.Read(Path.Combine(pathAutoSave, "groups.json")));

            if (!string.IsNullOrWhiteSpace(File.ReadAllText(Path.Combine(pathAutoSave, "lessons.json"))))
                danceSchool.AddLessons(_lessonReaderWriterAuto.Read(Path.Combine(pathAutoSave, "lessons.json")));

            updateScheduleGrid();

            bool noStudents = danceSchool.GetStudents().Count != 0;
            bool noTeachers = danceSchool.GetTeachers().Count != 0;
            bool noGroups = danceSchool.GetGroups().Count != 0;
            bool noLessons = danceSchool.GetSchedule().Count != 0;

            ButtonSaveAsStudents.IsEnabled = ButtonfExportStudents.IsEnabled = noStudents;
            ButtonSaveAsTeachers.IsEnabled = ButtonExportTeachers.IsEnabled = noTeachers;
            ButtonSaveAsGroups.IsEnabled = ButtonfExportGroups.IsEnabled = noGroups;
            ButtonSaveAsSchedule.IsEnabled = ButtonExportSchedule.IsEnabled = noLessons;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                _studentReaderWriterAuto.Write(danceSchool.GetStudents().ToList(), Path.Combine(pathAutoSave, "students.json"));
                _teacherReaderWriterAuto.Write(danceSchool.GetTeachers().ToList(), Path.Combine(pathAutoSave, "teachers.json"));
                _groupReaderWriterAuto.Write(danceSchool.GetGroups().ToList(), Path.Combine(pathAutoSave, "groups.json"));
                _lessonReaderWriterAuto.Write(danceSchool.GetSchedule().ToList(), Path.Combine(pathAutoSave, "lessons.json"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void updateScheduleGrid()
        {
            dataGridSchedule.ItemsSource = danceSchool.GetSchedule().Where(x => x.DateOfLesson.Date == day.Date).OrderBy(x => x.DateOfLesson.TimeOfDay);
        }
        private void updateDate()
        {
            lblDateSchedule.Content = day.ToShortDateString();
            lblDayOfWeekSchedule.Content = day.DayOfWeek;
        }
        private void ButtonAddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudent addStudentForm = new AddStudent(isEditStudent);
            bool? res = addStudentForm.ShowDialog();

            if (res == true)
            {
                danceSchool.AddStudent(addStudentForm.GetStudent());
                ButtonSaveAsStudents.IsEnabled = ButtonfExportStudents.IsEnabled = true;
            }
        }

        private void dataGridStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool stateSelection = dataGridStudents.SelectedItem != null;
            ButtonAddStudentToGroup.IsEnabled = ButtonStudentInfo.IsEnabled = ButtonEditStudent.IsEnabled = ButtonDeleteStudent.IsEnabled = stateSelection;
        }

        private void dataGridTeachers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool stateSelection = dataGridTeachers.SelectedItem != null;
            ButtonEditTeacher.IsEnabled = ButtonDeleteTeacher.IsEnabled = stateSelection;
        }

        private void dataGridGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            bool stateSelection = dataGridGroups.SelectedItem != null;
            ButtonGroupInfo.IsEnabled = ButtonEditGroup.IsEnabled = ButtonDeleteGroup.IsEnabled = stateSelection;
        }
        private void dataGridSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool stateSelection = dataGridSchedule.SelectedItem != null;
            ButtonEditLesson.IsEnabled = ButtonDeleteLesson.IsEnabled = stateSelection;
        }
        private void ButtonAddStudentToGroup_Click(object sender, RoutedEventArgs e)
        {
            if(danceSchool.GetGroups().Count == 0)
            {
                MessageBox.Show("Створіть хоча б одну групу у вкладці \"Групи\"", 
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }

            AddToGroup addToGroupForm = new AddToGroup(danceSchool);
            bool? res = addToGroupForm.ShowDialog();

            if(res == true)
            {
                Student addedStudent = dataGridStudents.SelectedItem as Student;
                Group selectedGroup = addToGroupForm.GetSelectedGroup();

                bool result = danceSchool.AddToGroup(addedStudent, selectedGroup);

                if (!result)
                {
                    MessageBox.Show("Учень вже присутній в одній з груп",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    return;
                }
            }
        }

        private void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
        {
            if(danceSchool.GetTeachers().Count == 0)
            {
                MessageBox.Show("Додайте хоча б одного викладача у вкладці \"Викладачі\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }

            AddGroup addGroupForm = new AddGroup(danceSchool);
            bool? res = addGroupForm.ShowDialog();

            if (res == true)
            {
                danceSchool.AddGroup(addGroupForm.GetGroup());
                ButtonSaveAsGroups.IsEnabled = ButtonfExportGroups.IsEnabled = true;
            }
        }

        private void ButtonAddTeacher_Click(object sender, RoutedEventArgs e)
        {
            AddTeacher addTeacherForm = new AddTeacher(isEditTeacher);
            bool? res = addTeacherForm.ShowDialog();

            if(res == true)
            {
                danceSchool.AddTeacher(addTeacherForm.GetTeacher());
                ButtonSaveAsTeachers.IsEnabled = ButtonExportTeachers.IsEnabled = true;
            }
        }

        private void ButtonAddGroupLesson_Click(object sender, RoutedEventArgs e)
        {
            if (danceSchool.GetGroups().Count == 0)
            {
                MessageBox.Show("Створіть хоча б одну групу у вкладці \"Групи\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }
            if (danceSchool.GetTeachers().Count == 0)
            {
                MessageBox.Show("Додайте хоча б одного викладача у вкладці \"Викладачі\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
            }

            AddLesson addLessonForm = new AddLesson(danceSchool, LessonType.Group);
            bool? res = addLessonForm.ShowDialog();

            if (res == true)
            {
                danceSchool.AddLesson(addLessonForm.GetLesson());
                updateScheduleGrid();

                ButtonSaveAsSchedule.IsEnabled = ButtonExportSchedule.IsEnabled = true;
            }
        }

        private void ButtonAddIndpLesson_Click(object sender, RoutedEventArgs e)
        {
            if (danceSchool.GetStudents().Count == 0)
            {
                MessageBox.Show("Додайте хоча б одного учня у вкладці \"Учні\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }
            if (danceSchool.GetTeachers().Count == 0)
            {
                MessageBox.Show("Додайте хоча б одного викладача у вкладці \"Викладачі\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }

            AddLesson addLessonForm = new AddLesson(danceSchool, LessonType.Individual);
            bool? res = addLessonForm.ShowDialog();

            if(res == true)
            {
                danceSchool.AddLesson(addLessonForm.GetLesson());
                updateScheduleGrid();

                ButtonSaveAsSchedule.IsEnabled = ButtonExportSchedule.IsEnabled = true;
            }
        }

        private void ButtonDeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            Student student = dataGridStudents.SelectedItem as Student;
            danceSchool.DeleteStudent(student.Id);


            bool noItems = danceSchool.GetStudents().Count != 0;
            ButtonSaveAsStudents.IsEnabled = ButtonfExportStudents.IsEnabled = noItems;
        }
        private void ButtonDeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            Teacher teacher = dataGridTeachers.SelectedItem as Teacher;
            
            bool result = danceSchool.DeleteTeacher(teacher.Id);

            if (!result)
            {
                MessageBox.Show("Цей викладач веде групу.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                return;
            }

            bool noItems = danceSchool.GetTeachers().Count != 0;
            ButtonSaveAsTeachers.IsEnabled = ButtonExportTeachers.IsEnabled = noItems;
        }

        private void ButtonDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            Group group = dataGridGroups.SelectedItem as Group;
            danceSchool.DeleteGroup(group.Id);

            bool noItems = danceSchool.GetGroups().Count != 0;
            ButtonSaveAsGroups.IsEnabled = ButtonfExportGroups.IsEnabled = noItems;
        }
        private void ButtonDeleteLesson_Click(object sender, RoutedEventArgs e)
        {
            Lesson lesson = dataGridSchedule.SelectedItem as Lesson;
            danceSchool.DeleteLesson(lesson);
            updateScheduleGrid();

            bool noItems = danceSchool.GetSchedule().Count != 0;
            ButtonSaveAsSchedule.IsEnabled = ButtonExportSchedule.IsEnabled = noItems;
        }

        private void ButtonEditStudent_Click(object sender, RoutedEventArgs e)
        {
            Student student = dataGridStudents.SelectedItem as Student;

            isEditStudent = true;
            AddStudent addStudentForm = new AddStudent(isEditStudent);


            addStudentForm.inputFieldStudentName.txtInput.Text = student.Name;
            addStudentForm.inputFieldStudentSurname.txtInput.Text = student.Surname;
            addStudentForm.datePickerStudentDateOfBirth.SelectedDate = student.DateOfBirth;
            addStudentForm.txtStudentInfo.Text = student.Info;
            addStudentForm.lblPhotoPath.Content = student.PhotoPath;

            addStudentForm.labelForPhoto.Visibility = Visibility.Hidden;
            addStudentForm.btnAddPhoto.Content = "Change";
            Grid.SetColumn(addStudentForm.btnAddPhoto, 0);

            bool? res = addStudentForm.ShowDialog();
            isEditStudent = false;

            if (res == true)
            {
                danceSchool.UpdateStudent(student.Id, addStudentForm.GetStudent());
            }
        }

        private void ButtonEditTeacher_Click(object sender, RoutedEventArgs e)
        {
            Teacher teacher = dataGridTeachers.SelectedItem as Teacher;

            isEditTeacher = true;
            AddTeacher addTeacherForm = new AddTeacher(isEditTeacher);

            addTeacherForm.inputFieldNewTeacherSurname.txtInput.Text = teacher.Surname;
            addTeacherForm.inputFieldNewTeacherName.txtInput.Text = teacher.Name;
            addTeacherForm.inputFieldNewTeacherPatronymic.txtInput.Text = teacher.Patronymic;
            addTeacherForm.numericNewTeacherExperience.Text = teacher.YearsOfExpirience.ToString();
            addTeacherForm.lblPhotoPathTeacher.Content = teacher.PhotoPath;

            addTeacherForm.labelForPhotoTeacher.Visibility = Visibility.Hidden;
            addTeacherForm.btnAddPhotoTeacher.Content = "Change";
            Grid.SetColumn(addTeacherForm.btnAddPhotoTeacher, 0);

            bool? res = addTeacherForm.ShowDialog();
            isEditTeacher = false;

            if (res == true)
            {
                danceSchool.UpdateTeacher(teacher.Id, addTeacherForm.GetTeacher());
            }
        }

        private void ButtonEditGroup_Click(object sender, RoutedEventArgs e)
        {
            Group group = dataGridGroups.SelectedItem as Group;

            AddGroup addGroupForm = new AddGroup(danceSchool);

            addGroupForm.nameOfTheGroup.txtInput.Text = group.Name;
            addGroupForm.comboBoxTeachersCreateGroup.SelectedItem = group.Teacher;

            bool? res = addGroupForm.ShowDialog();

            if (res == true)
            {
                danceSchool.UpdateGroup(group.Id, addGroupForm.GetGroup());
            }
        }

        private void ButtonStudentInfo_Click(object sender, RoutedEventArgs e)
        {
            Student student = dataGridStudents.SelectedItem as Student;

            InfoStudent infoStudentWindow = new InfoStudent(student);
            infoStudentWindow.ShowDialog();
        }

        private void ButtonGroupInfo_Click(object sender, RoutedEventArgs e)
        {
            Group group = dataGridGroups.SelectedItem as Group;

            InfoGroup infoGroupWindow = new InfoGroup(group);
            infoGroupWindow.ShowDialog();
        }

        private void NextButtonSchedule_Click(object sender, RoutedEventArgs e)
        {
            day = day.AddDays(1);
            updateDate();
            updateScheduleGrid();
        }

        private void PrevButtonSchedule_Click(object sender, RoutedEventArgs e)
        {
            day = day.AddDays(-1);
            updateDate();
            updateScheduleGrid();
        }

        private void ButtonEditLesson_Click(object sender, RoutedEventArgs e)
        {
            Lesson lesson = dataGridSchedule.SelectedItem as Lesson;
            LessonType type = lesson.Group != null ? LessonType.Group : LessonType.Individual;
            AddLesson addLessonForm = new AddLesson(danceSchool, type);

            if(type == LessonType.Group)
                addLessonForm.comboBoxGroupOrStudentCreateLesson.SelectedItem = lesson.Group;
            else
                addLessonForm.comboBoxGroupOrStudentCreateLesson.SelectedItem = lesson.Student;

            addLessonForm.comboBoxTeacherCreateLesson.SelectedItem = lesson.Teacher;
            addLessonForm.datePicker.Value = lesson.DateOfLesson;
            addLessonForm.numericUpDown.Value = (decimal)lesson.Duration;

            bool? res = addLessonForm.ShowDialog();
            
            if(res == true)
            {
                danceSchool.UpdateLesson(lesson, addLessonForm.GetLesson());
                updateScheduleGrid();
            }
            
        }

        private string chooseFile()
        {
            string filePath = null;

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Виберіть файл для завантаження",
                Filter = "Текстові файли|*.txt|CSV файли (*.csv)|*.csv|XML файли (*.xml)|*.xml|JSON Files (*.json)|*.json|Excel файли(*.xlsx)|*.xlsx"
            };
            bool? res = openFileDialog.ShowDialog();

            if (res == true)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }
        private string saveFile()
        {
            string filePath = null;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстові файли|*.txt|CSV файли (*.csv)|*.csv|XML файли (*.xml)|*.xml|JSON Files (*.json)|*.json|Excel файли(*.xlsx)|*.xlsx",
                Title = "Save File"
            };
            var res = saveFileDialog.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }
        private string exportFile()
        {
            string filePath = null;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF файли (*.pdf)|*.pdf",
                Title = "Export File"
            };
            var res = saveFileDialog.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }
        private void successfulMessage(string filePath)
        {
            MessageBox.Show($"Вітаю! Операція успішна. Файл: {filePath}",
                "Успішно",
                MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
        }

        private void ButtonImportStudents_Click(object sender, RoutedEventArgs e)
        {
            string filePath = chooseFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                _studentReaderWriter = ReaderWriterHelper<Student>.readerWriter<StudentMap>(filePath, new TxtStudentReaderWriter());

                List<Student> students = _studentReaderWriter.Read(filePath);

                danceSchool.DeleteAllStudents();
                danceSchool.AddStudents(students);
                ButtonSaveAsStudents.IsEnabled = ButtonfExportStudents.IsEnabled = true;
            } 
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonSaveAsStudents_Click(object sender, RoutedEventArgs e)
        {
            string filePath = saveFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                _studentReaderWriter = ReaderWriterHelper<Student>.readerWriter<StudentMap>(filePath, new TxtStudentReaderWriter());

                _studentReaderWriter.Write(danceSchool.GetStudents().ToList(), filePath);

                successfulMessage(filePath);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonfExportStudents_Click(object sender, RoutedEventArgs e)
        {
            string filePath = exportFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                PdfReportGenerator.GenerateReport<Student>(danceSchool.GetStudents().ToList(), "Учні", filePath);
                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonImportTeachers_Click(object sender, RoutedEventArgs e)
        {
            string filePath = chooseFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                _teacherReaderWriter = ReaderWriterHelper<Teacher>.readerWriter<TeacherMap>(filePath, new TxtTeacherReaderWriter());

                List<Teacher> teachers = _teacherReaderWriter.Read(filePath);

                danceSchool.DeleteAllTeachers();
                danceSchool.AddTeachers(teachers);

                ButtonSaveAsTeachers.IsEnabled = ButtonExportTeachers.IsEnabled = true;
            } catch (FormatException ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

        }

        private void ButtonSaveAsTeachers_Click(object sender, RoutedEventArgs e)
        {
            string filePath = saveFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                _teacherReaderWriter = ReaderWriterHelper<Teacher>.readerWriter<TeacherMap>(filePath, new TxtTeacherReaderWriter());

                _teacherReaderWriter.Write(danceSchool.GetTeachers().ToList(), filePath);

                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonExportTeachers_Click(object sender, RoutedEventArgs e)
        {
            string filePath = exportFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                PdfReportGenerator.GenerateReport<Teacher>(danceSchool.GetTeachers().ToList(), "Викладачі", filePath);

                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonImportGroups_Click(object sender, RoutedEventArgs e)
        {
            if (danceSchool.GetTeachers().Count == 0)
            {
                MessageBox.Show("Спочатку імпортуйте або додайте викладачів у вкладці \"Викладачі\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }
            if (danceSchool.GetStudents().Count == 0)
            {
                MessageBox.Show("Спочатку імпортуйте або додайте учнів у вкладці \"Учні\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }

            string filePath = chooseFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var ext = Path.GetExtension(filePath).ToLower().TrimStart('.');


                switch (ext)
                {
                    case "txt":
                        _groupReaderWriter = new TxtGroupReaderWriter(danceSchool);
                        break;
                    case "xml":
                        _groupReaderWriter = new XmlReaderWriter<Group>();
                        break;
                    case "json":
                        _groupReaderWriter = new JsonReaderWriter<Group>();
                        break;
                    default:
                        throw new NotSupportedException($"Формат файлу '{ext}' не підтримується.");
                }

                List<Group> groups = _groupReaderWriter.Read(filePath);

                foreach(Group group in groups)
                {
                    Teacher existingTeacher = danceSchool.SearchById<Teacher>(group.Teacher.Id, danceSchool.GetTeachers());
                    if(existingTeacher == null)
                    {
                        MessageBox.Show($"Викладача \"{group.Teacher.ToString()}\" не існує.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error );

                        return;
                    }
                    
                    foreach(Student student in group.Students)
                    {
                        Student existingStudent = danceSchool.SearchById<Student>(student.Id, danceSchool.GetStudents());
                        if (existingStudent == null)
                        {
                            MessageBox.Show($"Учня \"{student.ToString()}\" не існує.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                            return;
                        }
                    }
                }

                danceSchool.DeleteAllGroups();
                danceSchool.AddGroups(groups);

                ButtonSaveAsGroups.IsEnabled = ButtonfExportGroups.IsEnabled = true;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonSaveAsGroups_Click(object sender, RoutedEventArgs e)
        {
            string filePath = saveFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var ext = Path.GetExtension(filePath).ToLower().TrimStart('.');

                switch (ext)
                {
                    case "txt":
                        _groupReaderWriter = new TxtGroupReaderWriter(danceSchool);
                        break;
                    case "xml":
                        _groupReaderWriter = new XmlReaderWriter<Group>();
                        break;
                    case "json":
                        _groupReaderWriter = new JsonReaderWriter<Group>();
                        break;
                    default:
                        throw new NotSupportedException($"Формат файлу '{ext}' не підтримується.");
                }

                _groupReaderWriter.Write(danceSchool.GetGroups().ToList(), filePath);

                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonfExportGroups_Click(object sender, RoutedEventArgs e)
        {
            string filePath = exportFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                PdfReportGenerator.GenerateGroupReport(danceSchool, filePath);
                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonImportSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (danceSchool.GetTeachers().Count == 0)
            {
                MessageBox.Show("Спочатку імпортуйте або додайте викладачів у вкладці \"Викладачі\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }
            if (danceSchool.GetStudents().Count == 0)
            {
                MessageBox.Show("Спочатку імпортуйте або додайте учнів у вкладці \"Учні\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }
            if (danceSchool.GetGroups().Count == 0)
            {
                MessageBox.Show("Спочатку імпортуйте або додайте групи у вкладці \"Групи\"",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                 );
                return;
            }

            string filePath = chooseFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var ext = Path.GetExtension(filePath).ToLower().TrimStart('.');


                switch (ext)
                {
                    case "txt":
                        _lessonReaderWriter = new TxtLessonReaderWriter(danceSchool);
                        break;
                    case "xml":
                        _lessonReaderWriter = new XmlReaderWriter<Lesson>();
                        break;
                    case "json":
                        _lessonReaderWriter = new JsonReaderWriter<Lesson>();
                        break;
                    default:
                        throw new NotSupportedException($"Формат файлу '{ext}' не підтримується.");
                }

                List<Lesson> lessons = _lessonReaderWriter.Read(filePath);

                foreach (Lesson lesson in lessons)
                {
                    Teacher existingTeacher = danceSchool.SearchById<Teacher>(lesson.Teacher.Id, danceSchool.GetTeachers());
                    if (existingTeacher == null)
                    {
                        MessageBox.Show($"Викладача \"{lesson.Teacher.ToString()}\" не існує.",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                        return;
                    }

                    if (lesson.Student != null)
                    {
                        Student existingStudent = danceSchool.SearchById<Student>(lesson.Student.Id, danceSchool.GetStudents());
                        if (existingStudent == null)
                        {
                            MessageBox.Show($"Учня \"{lesson.Student.ToString()}\" не існує.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                            return;
                        }
                    }
                    if (lesson.Group != null)
                    {
                        Group existingGroup = danceSchool.SearchById<Group>(lesson.Group.Id, danceSchool.GetGroups());
                        if(existingGroup == null)
                        {
                            MessageBox.Show($"Групи \"{lesson.Group.ToString()}\" не існує.",
                            "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                            return;
                        }
                    }
                }

                danceSchool.ClearSchedule();
                danceSchool.AddLessons(lessons);

                updateScheduleGrid();
                ButtonSaveAsSchedule.IsEnabled = ButtonExportSchedule.IsEnabled = true;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonSaveAsSchedule_Click(object sender, RoutedEventArgs e)
        {
            string filePath = saveFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var ext = Path.GetExtension(filePath).ToLower().TrimStart('.');

                switch (ext)
                {
                    case "txt":
                        _lessonReaderWriter = new TxtLessonReaderWriter(danceSchool);
                        break;
                    case "xml":
                        _lessonReaderWriter = new XmlReaderWriter<Lesson>();
                        break;
                    case "json":
                        _lessonReaderWriter = new JsonReaderWriter<Lesson>();
                        break;
                    default:
                        throw new NotSupportedException($"Формат файлу '{ext}' не підтримується.");
                }

                _lessonReaderWriter.Write(danceSchool.GetSchedule().ToList(), filePath);

                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ButtonExportSchedule_Click(object sender, RoutedEventArgs e)
        {
            string filePath = exportFile();
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                PdfReportGenerator.GenerateReport<Lesson>(danceSchool.GetSchedule().OrderBy(lesson => lesson.DateOfLesson).ToList(), "Розклад", filePath);
                successfulMessage(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}