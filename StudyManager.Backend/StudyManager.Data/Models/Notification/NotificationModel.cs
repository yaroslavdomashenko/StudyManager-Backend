using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Notification
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
    }
}
