using StudyManager.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Avatar { get; set; }
        public Role Role { get; set; }
        public DateTime DateCreated { get; set; }
        public List<CourseModel> Courses { get; set; } = new List<CourseModel>();
        public List<CourseModel> CreatedCourses { get; set; } = new List<CourseModel>();
    }
}
