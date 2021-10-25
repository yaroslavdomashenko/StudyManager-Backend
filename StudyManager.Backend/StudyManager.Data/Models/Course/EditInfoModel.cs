using System;

namespace StudyManager.Data.Models.Course
{
    public class EditInfoModel
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
