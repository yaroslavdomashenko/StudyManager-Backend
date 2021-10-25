using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Visit
{
    public class GetVisitsInPeriod
    {
        public Guid CourseId { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime SecondDate { get; set; }
    }
}
