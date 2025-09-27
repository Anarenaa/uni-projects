using System.Collections.ObjectModel;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Lab3.Models
{
    public class Group : IWithId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        public override string ToString()
        {
            return $"\"{Name}\"";
        }
    }
    public class GroupTxt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public List<int> StudentIds { get; set; }
    }
    public static class GroupMapper //TxtReaderWriter
    {
        public static GroupTxt ToTxt(Group group)
        {
            return new GroupTxt
            {
                Id = group.Id,
                Name = group.Name,
                TeacherId = group.Teacher?.Id ?? 0,
                StudentIds = group.Students.Select(s => s.Id).ToList()
            };
        }

        public static Group FromTxt(GroupTxt txt, DanceSchool ds)
        {
            var teacher = ds.GetTeachers().FirstOrDefault(t => t.Id == txt.TeacherId);
            var students = ds.GetStudents().Where(s => txt.StudentIds.Contains(s.Id));

            return new Group
            {
                Id = txt.Id,
                Name = txt.Name,
                Teacher = teacher,
                Students = new ObservableCollection<Student>(students)
            };
        }
    }
}
