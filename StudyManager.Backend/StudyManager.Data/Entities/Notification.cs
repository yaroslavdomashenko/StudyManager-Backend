using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Entities
{
    public class Notification : BaseEntity
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Text { get; set; }
        public bool IsRead { get; set; }

        public Notification() : base(){

        }
    }
}
