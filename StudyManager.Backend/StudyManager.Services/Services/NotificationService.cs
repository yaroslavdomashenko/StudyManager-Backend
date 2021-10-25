using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Models.Notification;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public NotificationService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateForUser(Guid id, string text)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new Exception("User not found");

            var notification = new Notification();
            notification.Text = text;
            notification.User = user;

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationModel>> GetNotification(string login, int take, int skip = 0)
        {
            var list = await _context.Notifications.Include(x => x.User).Where(x => x.User.Login == login).OrderBy(x => x.IsRead).Skip(skip).Take(take).ToListAsync();
            return _mapper.Map<List<NotificationModel>>(list);
        }

        public async Task MarkAsRead(Guid id)
        {
            var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            if (notification == null)
                throw new Exception("Notification not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
