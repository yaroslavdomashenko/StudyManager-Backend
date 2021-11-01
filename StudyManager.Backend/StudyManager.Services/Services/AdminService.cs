using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly INotificationService _notificationService;
        private readonly ApplicationContext _context;
        public AdminService(ApplicationContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task ChangeRole(string login, Guid id, Role role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            var requestor = await _context.Users.FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            if (user == null)
                throw new ServiceException("User not found");
            if (user == requestor)
                throw new ServiceException("You can't change your role");
            user.Role = role;
            await _context.SaveChangesAsync();

            await _notificationService.CreateForUser(user.Id, $"Admin {requestor.Login} changed your role to {role}");
        }
        public async Task ChangeRole(string login, string userLogin, Role role)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login.ToLower() == userLogin.ToLower());
            var requestor = await _context.Users.FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            if (user == null)
                throw new ServiceException("User not found");
            if (user == requestor)
                throw new ServiceException("You can't change your role");
            user.Role = role;
            await _context.SaveChangesAsync();

            await _notificationService.CreateForUser(user.Id, $"Admin {requestor.Login} changed your role to {role}");
        }

        public async Task CloseCourse(Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                throw new ServiceException("Course not found");
            course.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public async Task OpenCourse(Guid courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                throw new ServiceException("Course not found");
            course.IsActive = true;
            await _context.SaveChangesAsync();
        }
    }
}
