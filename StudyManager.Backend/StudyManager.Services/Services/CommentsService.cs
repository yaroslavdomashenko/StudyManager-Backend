using AutoMapper;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models.Comment;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            var homework = await _homeworkRepository.Get(homeworkId);
            var user = await _userRepository.GetFirstOrDefault(x => x.Login.ToLower() == userLogin.ToLower());
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
            await _commentRepository.Add(comment);
            return _mapper.Map<CommentModel>(comment);
        }

        public async Task DeleteComment(Guid id)
        {
            await _commentRepository.Delete(id);
        }

        public async Task<List<CommentModel>> GetComments(Guid homeworkId, int take, int skip)
        {
            var comments = await _commentRepository.GetWithLimit(skip, take, where => where.HomeworkId == homeworkId, include => include.User);
            return _mapper.Map<List<CommentModel>>(comments);
        }
    }
}
