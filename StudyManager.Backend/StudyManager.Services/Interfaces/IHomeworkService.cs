using Microsoft.AspNetCore.Http;
using StudyManager.Data.Models.Homework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<HomeworkModel> GetHomework(Guid id);
        Task<List<AttachmentModel>> GetAttachment(Guid hwId);
        Task<List<HomeworkModel>> GetHomeworks(Guid courseId, int take, int skip);
        Task<Guid> CreateHomework(CreateHomeworkModel model);
        Task AddAttachments(Guid id, IFormFile[] files);
        Task UpdateHomework(Guid id, string title, string text);
        Task DeleteHomework(Guid id);
    }
}
