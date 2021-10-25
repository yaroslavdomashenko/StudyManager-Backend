using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudyManager.Data.Entities
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        [ForeignKey("Homework")]
        public Guid HomeworkId { get; set; }
        [JsonIgnore]
        public Homework Homework { get; set; }
    }
}
