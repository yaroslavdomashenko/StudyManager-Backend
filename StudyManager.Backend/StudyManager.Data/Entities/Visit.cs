using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Entities
{
    public class Visit : BaseEntity
    {
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        public List<User> Visitors { get; set; } = new List<User>();
    }
}
