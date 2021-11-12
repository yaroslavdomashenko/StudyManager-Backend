using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models.Homework;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly IRepository<Homework> _homeworkRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IWebHostEnvironment _webHostEn;
        private readonly IMapper _mapper;

        public HomeworkService(
            IRepository<Homework> homeworkRepository,
            IRepository<Attachment> attachmentRepository,
            IRepository<Course> courseRepository,
            IWebHostEnvironment webHostEn, 
            IMapper mapper)
        {
            _homeworkRepository = homeworkRepository;
            _attachmentRepository = attachmentRepository;
            _courseRepository = courseRepository;
            _webHostEn = webHostEn;
            _mapper = mapper;
        }

        public async Task AddAttachments(Guid id, IFormFile[] files)
        {
            var homework = await _homeworkRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");

            var attachments = new List<Attachment>();
            foreach(IFormFile file in files)
            {
                string[] split = file.FileName.Split('.');
                string name = "attach_" + DateTime.Now.ToString("yyyy_MM_HH_mm_ss") + "." + split[1];
                string path = $"/attachments/{name}";
                using(var fileStream = new FileStream(_webHostEn.WebRootPath + path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var attach = new Attachment
                {
                    Id = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    Homework = homework,
                    Path = path,
                    FileName = name
                };
                attachments.Add(attach);
            }
            attachments.ForEach(x => _attachmentRepository.AddAsync(x));
        }

        public async Task<Guid> CreateHomework(CreateHomeworkModel model)
        {
            var course = await _courseRepository.Query().FirstOrDefaultAsync(x => x.Id == model.CourseId);
            if (course == null)
                throw new ServiceException("Course not found");
            var homework = new Homework()
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                ExpireDate = model.ExpireDate,
                Title = model.Title,
                CourseId = model.CourseId,
                Text = string.IsNullOrEmpty(model.Text) ? "" : model.Text
            };

            await _homeworkRepository.AddAsync(homework);
            return homework.Id;
        }

        public async Task DeleteHomework(Guid id)
        {
            await _homeworkRepository.DeleteAsync(id);
        }

        public async Task<List<AttachmentModel>> GetAttachment(Guid hwId)
        {
            var attach = await _attachmentRepository.Query().Where(x => x.HomeworkId == hwId).ToListAsync();
            return _mapper.Map<List<AttachmentModel>>(attach);
        }

        public async Task<HomeworkModel> GetHomework(Guid id)
        {
            var homework = await _homeworkRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");
            return _mapper.Map<HomeworkModel>(homework);
        }

        public async Task<List<HomeworkModel>> GetHomeworks(Guid courseId, int take, int skip)
        {
            var homeworks = await _homeworkRepository.Query().Where(x => x.CourseId == courseId).ToListAsync();
            return _mapper.Map<List<HomeworkModel>>(homeworks);
        }

        public async Task UpdateHomework(Guid id, string title, string text)
        {
            var homework = await _homeworkRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");

            homework.Title = String.IsNullOrEmpty(title) ? homework.Title : title;
            homework.Text = String.IsNullOrEmpty(text) ? homework.Text : text;
            await _homeworkRepository.UpdateAsync(homework);
        }
    }
}
