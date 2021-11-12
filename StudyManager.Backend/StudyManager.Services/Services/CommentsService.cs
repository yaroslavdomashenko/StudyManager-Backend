using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
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
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Homework> _homeworkRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IMapper _mapper;

        public CommentsService(IRepository<User> userRepository, 
            IRepository<Homework> homeworkRepository,
            IRepository<Comment> commentRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _homeworkRepository = homeworkRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentModel> CreateComment(Guid homeworkId, string userLogin, string text)
        {
            var homework = await _homeworkRepository.Query().FirstOrDefaultAsync(x=>x.Id == homeworkId);
            var user = await _userRepository.Query().FirstOrDefaultAsync(x => x.Login.ToLower() == userLogin.ToLower());

            if (homework == null)
                return null;
            if (user == null)
                return null;

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Homework = homework,
                User = user,
                Text = text
            };
            await _commentRepository.AddAsync(comment);
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task DeleteComment(Guid id)
        {
            await _commentRepository.DeleteAsync(id);
        }

        public async Task<List<CommentModel>> GetComments(Guid homeworkId, int take, int skip)
        {
            var comments = await _commentRepository.Query(include => include.User)
                .Where(x => x.HomeworkId == homeworkId)
                .Skip(skip).Take(take)
                .OrderBy(x=> x.DateCreated)
                .ToListAsync();
            return _mapper.Map<List<CommentModel>>(comments);
        }
    }
}
