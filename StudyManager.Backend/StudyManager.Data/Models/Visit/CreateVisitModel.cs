using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Visit
{
    public class CreateVisitModel
    {
        public Guid CourseId { get; set; }
        public List<string> Visitors { get; set; }
    }
}
