using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
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
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public ChatService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MessageDto> AddMessage(MessageModel message)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == message.UserId);
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == message.ChatId);
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

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();
            return _mapper.Map<MessageDto>(newMessage);
        }

        public async Task<List<MessageDto>> GetMessages(Guid chatId, int skip)
        {
            var messages = await _context.Messages
                .Include(x => x.User)
                .Where(x => x.CourseId == chatId)
                .OrderBy(x => x.DateCreated)
                .Skip(skip)
                .Take(25)
                .ToListAsync();
            return _mapper.Map<List<MessageDto>>(messages);
        }
    }
}
