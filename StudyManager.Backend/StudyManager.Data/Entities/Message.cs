using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudyManager.Data.Entities
{
    public class Message : BaseEntity
    {
        public string Text { get; set; } = string.Empty;

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

    }
}
