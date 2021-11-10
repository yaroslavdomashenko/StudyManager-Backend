using AutoMapper;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models.Chat;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IMapper _mapper;
        public ChatService(IRepository<User> userRepository, IRepository<Course> courseRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<MessageDto> AddMessage(MessageModel message)
        {
            var user = await _userRepository.GetFirstOrDefault(x => x.Id == message.UserId);
            var course = await _courseRepository.GetFirstOrDefault(x => x.Id == message.ChatId);
            if (user == null || course == null)
                throw new ServiceException("Wrong user id or course id");

            var newMessage = new Message
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Text = message.Text,
                Course = course,
                User = user
            };
            await _messageRepository.Add(newMessage);
            return _mapper.Map<MessageDto>(newMessage);
        }

        public async Task<List<MessageDto>> GetMessages(Guid chatId, int skip)
        {
            var messages = await _messageRepository.GetWithLimit(skip, 25, where => where.CourseId == chatId, includes => includes.User);
            return _mapper.Map<List<MessageDto>>(messages);
        }
    }
}
