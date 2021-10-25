using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudyManager.Data.Entities
{
    public class Homework : BaseEntity
    {
        public string Title { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Text { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
    }
}
