using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudyManager.Data.Entities
{
    public class Course : BaseEntity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }

        public Guid? UserId { get; set; }

        public User User { get; set; }

        public List<User> Students { get; set; } = new List<User>();
    }
}
