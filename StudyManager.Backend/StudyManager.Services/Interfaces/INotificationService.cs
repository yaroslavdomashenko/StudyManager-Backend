using StudyManager.Data.Models.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateForUser(Guid id, string text);
        Task<List<NotificationModel>> GetNotification(string login, int take, int skip = 0);
        Task MarkAsRead(Guid id);
    }
}
