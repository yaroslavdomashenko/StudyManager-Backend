using StudyManager.Data.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Data.Models.Comment
{
    public class CommentModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }
        public UserImageUsernameModel User { get; set; }
    }
}
