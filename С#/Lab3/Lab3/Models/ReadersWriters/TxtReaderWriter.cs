using System.Collections.ObjectModel;
using System.IO;

namespace Lab3.Models.ReadersWriters
{
    class TxtStudentReaderWriter : IReaderWriter<Student>
    {
        public List<Student> Read(string path)
        {
            var students = new List<Student>();
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path);
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 6)
                    {
                        Student student = new Student
                        {
                            Id = int.Parse(parts[0].Trim()),
                            PhotoPath = parts[1].Trim(),
                            Name = parts[2].Trim(),
                            Surname = parts[3].Trim(),
                            DateOfBirth = DateTime.Parse(parts[4].Trim()),
                            Info = parts[5].Trim()
                        };
                        students.Add(student);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException($"The file is not in the correct format.\n" +
                    $"Details: {ex.Message}");
            }
            return students;
        }
        public void Write(List<Student> items, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("Id;PhotoPath;Name;Surname;DateOfBirth;Info");

                    foreach (var student in items)
                    {
                        writer.WriteLine($"{student.Id};{student.PhotoPath};{student.Name};{student.Surname};{student.DateOfBirth.ToString("yyyy-MM-dd")};{student.Info}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    class TxtTeacherReaderWriter : IReaderWriter<Teacher>
    {
        public List<Teacher> Read(string path)
        {
            var teachers = new List<Teacher>();
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(path);
                string line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 6)
                    {
                        Teacher teacher = new Teacher
                        {
                            Id = int.Parse(parts[0].Trim()),
                            PhotoPath = parts[1].Trim(),
                            Name = parts[2].Trim(),
                            Surname = parts[3].Trim(),
                            Patronymic = parts[4].Trim(),
                            YearsOfExpirience = double.Parse(parts[5].Trim())
                        };
                        teachers.Add(teacher);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException($"The file is not in the correct format.\n" +
                    $"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return teachers;
        }

        public void Write(List<Teacher> items, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("Id;PhotoPath;Name;Surname;Patronymic;YearsOfExpirience");

                    foreach (var teacher in items)
                    {
                        writer.WriteLine($"{teacher.Id};{teacher.PhotoPath};{teacher.Name};{teacher.Surname};{teacher.Patronymic};{teacher.YearsOfExpirience}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    class TxtGroupReaderWriter : IReaderWriter<Group>
    {
        private DanceSchool _managerClass;

        public TxtGroupReaderWriter(DanceSchool danceSchool)
        {
            _managerClass = danceSchool;
        }

        public List<Group> Read(string path)
        {
            var result = new List<Group>();

            try
            {
                using var reader = new StreamReader(path);
                string header = reader.ReadLine();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 4)
                    {
                        var txt = new GroupTxt
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1],
                            TeacherId = int.Parse(parts[2]),
                            StudentIds = parts[3]
                                .Trim('"')
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToList()
                        };

                        result.Add(GroupMapper.FromTxt(txt, _managerClass));
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException($"The file is not in the correct format.\n" +
                    $"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка зчитування TXT: {ex.Message}");
            }

            return result;
        }

        public void Write(List<Group> groups, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("Id;Name;TeacherId;StudentIds");

                    foreach (var g in groups)
                    {
                        var group = GroupMapper.ToTxt(g);
                        writer.WriteLine($"{group.Id};{group.Name};{group.TeacherId};\"{string.Join(',', group.StudentIds)}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
    class TxtLessonReaderWriter : IReaderWriter<Lesson>
    {
        private readonly DanceSchool _danceSchool;

        public TxtLessonReaderWriter(DanceSchool danceSchool)
        {
            _danceSchool = danceSchool;
        }

        public List<Lesson> Read(string path)
        {
            var lessons = new List<Lesson>();

            try
            {
                using var reader = new StreamReader(path);
                string header = reader.ReadLine(); // Пропускаємо заголовок

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 5)
                    {
                        DateTime date = DateTime.Parse(parts[0]);
                        int? groupId = string.IsNullOrEmpty(parts[1]) ? (int?)null : int.Parse(parts[1]);
                        int? studentId = string.IsNullOrEmpty(parts[2]) ? (int?)null : int.Parse(parts[2]);
                        int teacherId = int.Parse(parts[3]);
                        double duration = double.Parse(parts[4]);

                        var group = groupId.HasValue ? _danceSchool.GetGroups().FirstOrDefault(g => g.Id == groupId.Value) : null;
                        var student = studentId.HasValue ? _danceSchool.GetStudents().FirstOrDefault(s => s.Id == studentId.Value) : null;
                        var teacher = _danceSchool.GetTeachers().FirstOrDefault(t => t.Id == teacherId);

                        var lesson = new Lesson
                        {
                            DateOfLesson = date,
                            Group = group,
                            Student = student,
                            Teacher = teacher,
                            Duration = duration
                        };

                        lessons.Add(lesson);
                    }
                }
            }
            catch (FormatException ex)
            {
                throw new FormatException($"The file is not in the correct format.\n" +
                    $"Details: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка зчитування TXT: {ex.Message}");
            }

            return lessons;
        }

        public void Write(List<Lesson> lessons, string path)
        {
            try
            {
                using var writer = new StreamWriter(path);
                writer.WriteLine("DateOfLesson;GroupId;StudentId;TeacherId;Duration");

                foreach (var lesson in lessons)
                {
                    string groupIdStr = lesson.Group != null ? lesson.Group.Id.ToString() : "";
                    string studentIdStr = lesson.Student != null ? lesson.Student.Id.ToString() : "";

                    writer.WriteLine($"{lesson.DateOfLesson};{groupIdStr};{studentIdStr};{lesson.Teacher.Id};{lesson.Duration}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Помилка запису Lesson у TXT: {ex.Message}");
            }
        }
    }
}
