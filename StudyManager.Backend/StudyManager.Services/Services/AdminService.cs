using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class AdminService : IAdminService
    {
        private readonly INotificationService _notificationService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Course> _courseRepository;
        public AdminService(IRepository<User> userRepository, IRepository<Course> courseRepository, INotificationService notificationService)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _notificationService = notificationService;
        }

        public async Task ChangeRole(string login, Guid id, Role role)
        {
            var user = await _userRepository.Get(id);
            var requestor = await _userRepository.GetFirstOrDefault(x => x.Login == login);
            if (user == null)
                throw new ServiceException("User not found");
            if (user == requestor)
                throw new ServiceException("You can't change your role");
            user.Role = role;
            await _userRepository.Update(user);

            await _notificationService.CreateForUser(user.Id, $"Admin {requestor.Login} changed your role to {role}");
        }
        public async Task ChangeRole(string login, string userLogin, Role role)
        {
            var user = await _userRepository.GetFirstOrDefault(x => x.Login == userLogin);
            var requestor = await _userRepository.GetFirstOrDefault(x => x.Login == login);
            if (user == null)
                throw new ServiceException("User not found");
            if (user == requestor)
                throw new ServiceException("You can't change your role");
            user.Role = role;
            await _userRepository.Update(user);

            await _notificationService.CreateForUser(user.Id, $"Admin {requestor.Login} changed your role to {role}");
        }

        public async Task CloseCourse(Guid courseId)
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null)
                throw new ServiceException("Course not found");
            course.IsActive = false;
            await _courseRepository.Update(course);
        }

        public async Task OpenCourse(Guid courseId)
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null)
                throw new ServiceException("Course not found");
            course.IsActive = true;
            await _courseRepository.Update(course);
        }
    }
}
