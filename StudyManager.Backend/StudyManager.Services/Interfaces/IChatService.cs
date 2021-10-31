using StudyManager.Data.Entities;
using StudyManager.Data.Models.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IChatService
    {
        Task<MessageDto> AddMessage(MessageModel message);
        Task<List<MessageDto>> GetMessages(Guid chatId, int skip);
    }
}
