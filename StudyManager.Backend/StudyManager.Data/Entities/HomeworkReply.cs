using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StudyManager.Data.Entities
{
    public class HomeworkReply : BaseEntity
    {
        public string Text { get; set; }
        public string Mark { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("Homework")]
        public Guid HomeworkId { get; set; }
        [JsonIgnore]
        public Homework Homework { get; set; }
    }
}
