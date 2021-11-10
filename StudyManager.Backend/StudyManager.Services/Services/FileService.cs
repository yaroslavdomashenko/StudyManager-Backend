using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IWebHostEnvironment _webHostEn;
        private readonly INotificationService _notificationService;

        public FileService(IRepository<User> userRepository, 
            IWebHostEnvironment webHostEn,
            INotificationService notificationService)
        {
            _userRepository = userRepository;
            _webHostEn = webHostEn;
            _notificationService = notificationService;
        }

        public async Task UploadAvatar(string userIdentity, IFormFile file)
        {
            var user = await _userRepository.GetFirstOrDefault(x => x.Login == userIdentity);
            if (user == null)
                throw new ServiceException("User not found");
            if (user.Avatar != null)
                DeleteImage(user.Avatar);

            string name = userIdentity + DateTime.Now.ToString("yyyy_MM_HH_mm_ss") + file.FileName.Substring(file.FileName.Length - 4);
            string path = $"/files/{name}";
            using (var fileStream = new FileStream(_webHostEn.WebRootPath + path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            user.Avatar = path;
            await _userRepository.Update(user);
            await _notificationService.CreateForUser(user.Id, "Avatar has been updated!");
        }

        private void DeleteImage(string path)
        {
            if(File.Exists(_webHostEn.WebRootPath + path))
            {
                File.Delete(_webHostEn.WebRootPath + path);
            }
        }
    }
}
