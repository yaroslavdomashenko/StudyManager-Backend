using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StudyManager.Data.Entities
{
    public class User : BaseEntity
    {
        public string Login { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public Role Role { get; set; } = Role.Student;
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Course> CreatedCourses { get; set; } = new List<Course>();
        public List<Visit> Visits { get; set; } = new List<Visit>();
        public List<HomeworkReply> HomeworkReplies { get; set; } = new List<HomeworkReply>();
    }
}
