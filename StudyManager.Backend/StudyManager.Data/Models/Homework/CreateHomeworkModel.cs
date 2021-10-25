using System;

namespace StudyManager.Data.Models.Homework
{
    public class CreateHomeworkModel
    {
        public Guid CourseId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
