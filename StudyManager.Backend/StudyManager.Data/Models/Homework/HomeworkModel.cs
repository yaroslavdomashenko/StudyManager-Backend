using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Homework
{
    public class HomeworkModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Text { get; set; }
        public Guid CourseId { get; set; }
    }
}
