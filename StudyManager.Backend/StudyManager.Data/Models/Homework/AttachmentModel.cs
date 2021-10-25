using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Homework
{
    public class AttachmentModel
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public Guid HomeworkId { get; set; }
    }
}
