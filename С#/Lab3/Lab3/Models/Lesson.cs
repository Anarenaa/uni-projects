namespace Lab3.Models
{
    public class Lesson
    {
        public DateTime DateOfLesson { get; set; }

        public Group? Group { get; set; }
        public Student? Student { get; set; }
        public string GetGroupOrStudent => Group?.ToString() ?? Student.ToString();

        public Teacher Teacher { get; set; }
        public double Duration { get; set; }
    }
    public class LessonTxt
    {
        public DateTime DateOfLesson { get; set; }
        public int? GroupId { get; set; }
        public int? StudentId { get; set; }
        public int TeacherId { get; set; }
        public double Duration { get; set; }
    }
}
