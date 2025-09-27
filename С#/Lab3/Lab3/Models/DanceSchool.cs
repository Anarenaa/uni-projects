using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3.Forms;
using Lab3.Models;

namespace Lab3
{
    public class DanceSchool
    {
        private ObservableCollection<Student> students { get; } = new ObservableCollection<Student>();
        private ObservableCollection<Teacher> teachers { get; } = new ObservableCollection<Teacher>();
        private ObservableCollection<Group> groups { get; } = new ObservableCollection<Group>();
        private ObservableCollection<Lesson> lessons { get; } = new ObservableCollection<Lesson>();

        public T SearchById<T>(int id, ObservableCollection<T> collection)
            where T : IWithId
        {
            return collection.FirstOrDefault(ob => ob.Id == id);
        }
        public void AddStudent(Student student)
        {
            student.Id = students.Any() ? students.Max(x => x.Id) + 1 : 1;
            students.Add(student);
        }

        public void AddStudents(List<Student> newStudents)
        {
            foreach(Student student in newStudents)
                students.Add(student);
        }
        public void UpdateStudent(int id, Student editedStudent)
        {
            Student student = students.FirstOrDefault(x => x.Id == id);
            int index = students.IndexOf(student);

            editedStudent.Id = student.Id;
            students[index] = editedStudent;
        }
        public void DeleteStudent(int id)
        {
            var student = SearchById(id, students);

            students.Remove(student);

            foreach (Group g in groups)
            {
                if (g.Students.Contains(student))
                    g.Students.Remove(student);
            }
        }
        public void AddTeacher(Teacher teacher)
        {
            teacher.Id = teachers.Any() ? teachers.Max(x => x.Id) + 1 : 1;
            teachers.Add(teacher);
        }
        public void AddTeachers(List<Teacher> newTeachers)
        {
            foreach (Teacher teacher in newTeachers)
                teachers.Add(teacher);
        }
        public void UpdateTeacher(int id, Teacher editedTeacher)
        {
            Teacher teacher = teachers.FirstOrDefault(x => x.Id == id);
            int index = teachers.IndexOf(teacher);

            editedTeacher.Id = teacher.Id;
            teachers[index] = editedTeacher;
        }
        public bool DeleteTeacher(int id)
        {
            var teacher = SearchById(id, teachers);

            foreach (Group g in groups)
            {
                if (g.Teacher == teacher)
                    return false;
            }
            teachers.Remove(teacher);
            return true;
        }
        public void AddGroup(Group group)
        {
            group.Id = groups.Any() ? groups.Max(x => x.Id) + 1 : 1;
            groups.Add(group);
        }
        public void AddGroups(List<Group> newGroups)
        {
            foreach (Group group in newGroups)
                groups.Add(group);
        }
        public void UpdateGroup(int id, Group editedGroup)
        {
            Group group = groups.FirstOrDefault(x => x.Id == id);
            int index = groups.IndexOf(group);

            editedGroup.Id = group.Id;
            groups[index] = editedGroup;
        }
        public void DeleteGroup(int id)
        {
            var group = SearchById(id, groups);
            groups.Remove(group);
        }
        public bool AddToGroup(Student student, Group group)
        {
            foreach (Group g in groups)
            {
                if (g.Students.Contains(student))
                    return false;
            }

            group.Students.Add(student);
            return true;
        }
        public void AddLesson(Lesson lesson)
        {
            lessons.Add(lesson);
        }
        public void AddLessons(List<Lesson> newLessons)
        {
            foreach (Lesson lesson in newLessons)
                lessons.Add(lesson);
        }
        public void UpdateLesson(Lesson lesson, Lesson updatedLesson)
        {
            int index = lessons.IndexOf(lesson);

            lessons[index] = updatedLesson;
        }
        public void DeleteLesson(Lesson lesson)
        {
            lessons.Remove(lesson);
        }
        public ObservableCollection<Student> GetStudents()
        {
            return students;
        }
        public void DeleteAllStudents()
        {
            students.Clear();
        }
        public ObservableCollection<Teacher> GetTeachers()
        {
            return teachers;
        }
        public void DeleteAllTeachers()
        {
            teachers.Clear();
        }
        public ObservableCollection<Group> GetGroups()
        {
            return groups;
        }
        public void DeleteAllGroups()
        {
            groups.Clear();
        }
        public ObservableCollection<Lesson> GetSchedule()
        {
            return lessons;
        }
        public void ClearSchedule()
        {
            lessons.Clear();
        }
    }
}
