using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Chat
{
    public class MessageModel
    {
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
    }
}
