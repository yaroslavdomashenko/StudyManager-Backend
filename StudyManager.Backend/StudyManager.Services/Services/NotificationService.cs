using AutoMapper;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models.Notification;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IMapper _mapper;
        public NotificationService(IRepository<User> userRepository, IRepository<Notification> notificationRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task CreateForUser(Guid id, string text)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                throw new ServiceException("User not found");

            var notification = new Notification();
            notification.Text = text;
            notification.User = user;
            await _notificationRepository.Add(notification);
        }

        public async Task<List<NotificationModel>> GetNotification(string login, int take, int skip = 0)
        {
            var list = await _notificationRepository.GetWithLimit(skip, take, w => w.User.Login == login, i => i.User);
            return _mapper.Map<List<NotificationModel>>(list);
        }

        public async Task MarkAsRead(Guid id)
        {
            var notification = await _notificationRepository.Get(id);
            if (notification == null)
                throw new ServiceException("Notification not found");

            notification.IsRead = true;
            await _notificationRepository.Update(notification);
        }
    }
}
