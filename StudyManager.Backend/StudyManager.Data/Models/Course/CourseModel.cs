using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public Guid? UserId { get; set; }
        public UserModel User { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        public List<UserModel> Students { get; set; } = new List<UserModel>();
    }
}
