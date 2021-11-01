using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models.Homework;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class HomeworkService : IHomeworkService
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEn;
        private readonly IMapper _mapper;
        public HomeworkService(ApplicationContext context, IWebHostEnvironment webHostEn, IMapper mapper)
        {
            _context = context;
            _webHostEn = webHostEn;
            _mapper = mapper;
        }

        public async Task AddAttachments(Guid id, IFormFile[] files)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
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

            _context.Attachments.AddRange(attachments);
            await _context.SaveChangesAsync();
        }

        public async Task<Guid> CreateHomework(CreateHomeworkModel model)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == model.CourseId);
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

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync();
            return homework.Id;
        }

        public async Task DeleteHomework(Guid id)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");
            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AttachmentModel>> GetAttachment(Guid hwId)
        {
            var attach = await _context.Attachments.Where(x => x.HomeworkId == hwId).ToListAsync();
            return _mapper.Map<List<AttachmentModel>>(attach);
        }

        public async Task<HomeworkModel> GetHomework(Guid id)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");
            return _mapper.Map<HomeworkModel>(homework);
        }

        public async Task<List<HomeworkModel>> GetHomeworks(Guid courseId, int take, int skip)
        {
            var hw = await _context.Homeworks.Where(x => x.CourseId == courseId).OrderByDescending(x => x.DateCreated).Skip(skip).Take(take).ToListAsync();
            return _mapper.Map<List<HomeworkModel>>(hw);
        }

        public async Task UpdateHomework(Guid id, string title, string text)
        {
            var homework = await _context.Homeworks.FirstOrDefaultAsync(x => x.Id == id);
            if (homework == null)
                throw new ServiceException("Homework not found");

            homework.Title = String.IsNullOrEmpty(title) ? homework.Title : title;
            homework.Text = String.IsNullOrEmpty(text) ? homework.Text : text;
            await _context.SaveChangesAsync();
        }
    }
}
