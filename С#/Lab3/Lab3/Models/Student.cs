using CsvHelper.Configuration;

namespace Lab3.Models
{
    public class Student : Person
    {
        public DateTime DateOfBirth { get; set; }
        public string Info {  get; set; }
    }
    public class StudentMap : ClassMap<Student>
    {
        public StudentMap()
        {
            Map(s => s.Id);
            Map(s => s.PhotoPath);
            Map(s => s.Name);
            Map(s => s.Surname);
            Map(s => s.DateOfBirth).TypeConverterOption.Format("dd.MM.yyyy");
            Map(s => s.Info);
        }
    }
}
