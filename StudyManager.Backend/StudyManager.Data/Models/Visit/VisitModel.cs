using StudyManager.Data.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Visit
{
    public class VisitModel
    {
        public Guid Id { get; set; }
        public string Titile { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid CourseId { get; set; }

        public List<UserSimpleModel> Visitors { get; set; } = new List<UserSimpleModel>();
    }
}
