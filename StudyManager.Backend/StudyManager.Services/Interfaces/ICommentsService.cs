using StudyManager.Data.Models.Comment;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface ICommentsService
    {
        Task<CommentModel> CreateComment(Guid homeworkId, string userLogin, string text);
        Task<List<CommentModel>> GetComments(Guid homeworkId, int take, int skip);
        Task DeleteComment(Guid id);
    }
}
