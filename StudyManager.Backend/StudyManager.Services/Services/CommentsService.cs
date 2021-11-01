using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models.Comment;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public CommentsService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentModel> CreateComment(Guid homeworkId, string userLogin, string text)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == homeworkId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userLogin);
            if (homework == null) throw new ServiceException("Homework not found");
            if (user == null) throw new ServiceException("User not found");

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Homework = homework,
                User = user,
                Text = text
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task DeleteComment(Guid id)
        {
            var homework = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");

            _context.Comments.Remove(homework);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentModel>> GetComments(Guid homeworkId, int take, int skip)
        {
            var list =  await _context.Comments.Include(x => x.User).Where(x => x.HomeworkId == homeworkId).OrderBy(x => x.DateCreated).Skip(skip).Take(take).ToListAsync();
            return _mapper.Map<List<CommentModel>>(list);
        }
    }
}
