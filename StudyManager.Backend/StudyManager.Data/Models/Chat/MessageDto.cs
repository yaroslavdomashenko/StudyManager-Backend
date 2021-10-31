using StudyManager.Data.Models.User;
using System;

namespace StudyManager.Data.Models.Chat
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public UserSimpleModel User { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
