using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models.Chat;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IMapper _mapper;
        public ChatService(IRepository<User> userRepository, IRepository<Course> courseRepository, IRepository<Message> messageRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<MessageDto> AddMessage(MessageModel message)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(x => x.Id == message.UserId);
            var course = await _courseRepository.Query().FirstOrDefaultAsync(x => x.Id == message.ChatId);
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

            await _messageRepository.AddAsync(newMessage);
            return _mapper.Map<MessageDto>(newMessage);
        }

        public async Task<List<MessageDto>> GetMessages(Guid chatId, int skip)
        {
            var messages = await _messageRepository
                .Query(include => include.User)
                .OrderBy(x => x.DateCreated)
                .ToListAsync();
            return _mapper.Map<List<MessageDto>>(messages);
        }
    }
}
