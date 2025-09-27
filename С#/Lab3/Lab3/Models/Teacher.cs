using CsvHelper.Configuration;
using Lab3.Models.ReadersWriters;

namespace Lab3.Models
{
    public class Teacher : Person
    {
        [PropertyOrder(4)]
        public string Patronymic { get; set; }
        public double YearsOfExpirience { get; set; }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
    }
    public class TeacherMap : ClassMap<Teacher>
    {
        public TeacherMap()
        {
            Map(t => t.Id);
            Map(t => t.PhotoPath);
            Map(t => t.Name);
            Map(t => t.Surname);
            Map(t => t.Patronymic);
            Map(t => t.YearsOfExpirience);
        }
    }
}
