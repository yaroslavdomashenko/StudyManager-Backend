using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Exceptions;
using StudyManager.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _webHostEn;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        public FileService(ApplicationContext context, IWebHostEnvironment webHostEn, IMapper mapper, INotificationService notificationService)
        {
            _context = context;
            _webHostEn = webHostEn;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task UploadAvatar(string userIdentity, IFormFile file)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login == userIdentity);
            if (user == null)
                throw new ServiceException("User not found");
            if (user.Avatar != null)
                await DeleteImage(user.Avatar);

            string name = userIdentity + DateTime.Now.ToString("yyyy_MM_HH_mm_ss") + file.FileName.Substring(file.FileName.Length - 4);
            string path = $"/files/{name}";
            using (var fileStream = new FileStream(_webHostEn.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            user.Avatar = path;
            await _context.SaveChangesAsync();
            await _notificationService.CreateForUser(user.Id, "Avatar has been updated!");
        }

        private async Task DeleteImage(string path)
        {
            if(File.Exists(_webHostEn.WebRootPath + path))
            {
                File.Delete(_webHostEn.WebRootPath + path);
            }
        }
    }
}
