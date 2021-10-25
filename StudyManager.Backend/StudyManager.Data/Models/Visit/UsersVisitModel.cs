using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Visit
{
    public class UsersVisitModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CourseId { get; set; }
    }
}
