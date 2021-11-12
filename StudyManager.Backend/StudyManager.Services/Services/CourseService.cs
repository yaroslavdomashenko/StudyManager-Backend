using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models;
using StudyManager.Data.Models.Course;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly INotificationService _notificationService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IMapper _mapper;
        public CourseService(IRepository<User> userRepository, 
            IRepository<Course> courseRepository,
            IMapper mapper, 
            INotificationService notificationService)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task Add(string requestor, string userLogin, Guid courseId)
        {
            var req = await _userRepository.Query().FirstOrDefaultAsync(x => x.Login.ToLower() == requestor.ToLower());
            var user = await _userRepository.Query().FirstOrDefaultAsync(x => x.Login.ToLower() == userLogin.ToLower());
            var course = await _courseRepository.Query(include => include.Students).FirstOrDefaultAsync(x => x.Id == courseId);

            if (user == null || course == null)
                throw new ServiceException("User or course not found");
            if (user == req)
                throw new ServiceException("You can't add yourself in course as student");
            if (!course.IsActive) throw new Exception("Course is closed");
            if (course.Students.Any(x => x.Login == user.Login))
                throw new ServiceException("User already in course");

            course.Students.Add(user);
            await _courseRepository.UpdateAsync(course);

            await _notificationService.CreateForUser(user.Id, $"{req.Role} {req.Login} added you to \n{course.Title}\" course.");
        }

        public async Task Remove(Guid userId, Guid courseId)
        {
            var user = await _userRepository.Query().FirstOrDefaultAsync(x => x.Id == userId);
            var course = await _courseRepository.Query(include => include.Students).FirstOrDefaultAsync(x => x.Id == courseId);

            if (user == null || course == null)
                throw new ServiceException("User or course not found");

            course.Students.Remove(user);
            await _courseRepository.UpdateAsync(course);

            await _notificationService.CreateForUser(user.Id, $"You have been removed from \"{course.Title}\" course.");
        }

        public async Task ChangeActive(Guid courseId)
        {
            var course = await _courseRepository.Query().FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null)
                throw new ServiceException("Course not found");
            course.IsActive = !course.IsActive;
            await _courseRepository.UpdateAsync(course);
        }

        public async Task<Course> Create(string login, string title, decimal price)
        {
            if (price <= 0)
                throw new ServiceException("Price can't be less or equal 0");

            var teacher = await _userRepository.Query().FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            if (teacher == null)
                throw new ServiceException("User not found");
            Course course = new Course
            {
                Id = Guid.NewGuid(),
                Title = title,
                DateCreated = DateTime.Now,
                User = teacher,
                IsActive = true,
                Price = price
            };
            await _courseRepository.AddAsync(course);
            return course;
        }

        public async Task<CourseModel> Get(Guid id)
        {
            var course = await _courseRepository.Query(include => include.Students, include => include.User).FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
                throw new ServiceException("Course not found");
            return _mapper.Map<CourseModel>(course);
        }

        public async Task<List<Course>> GetAll(int take, int skip)
        {
            return await _courseRepository.Query().Skip(skip).Take(take).ToListAsync();
        }

        public async Task AddTeacher(string teacherLogin, Guid courseId)
        {
            var user = await _userRepository.Query(i => i.Courses).FirstOrDefaultAsync(x => x.Login.ToLower() == teacherLogin.ToLower());
            var course = await _courseRepository.Query(i => i.User).FirstOrDefaultAsync(x => x.Id == courseId);

            if (course == null || user == null)
                throw new ServiceException("User or course not found");
            if (course.User != null)
                throw new ServiceException("Course already has a teacher. Remove it firstly");
            if (!course.IsActive)
                throw new ServiceException("Course is closed");

            course.UserId = user.Id;
            await _courseRepository.UpdateAsync(course);

            await _notificationService.CreateForUser(user.Id, $"You have been added as a teacher to \"{course.Title}\" course.");
        }

        public async Task RemoveTeacher(Guid courseId)
        {
            var course = await _courseRepository.Query(i => i.User).FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                throw new ServiceException("Course not found");
            if (course.User == null)
                throw new ServiceException("Course already doesn't have a teacher");

            var user = await _userRepository.Query(i => i.Courses).FirstOrDefaultAsync(x => x.Id == course.UserId);
            course.UserId = null;
            await _courseRepository.UpdateAsync(course);

            await _notificationService.CreateForUser(user.Id, $"You have been removed as a teacher from \"{course.Title}\" course.");
        }

        public async Task EditInfo(EditInfoModel model)
        {
            var course = await _courseRepository.Query(i => i.User).FirstOrDefaultAsync(x => x.Id == model.CourseId);
            if (course == null)
                throw new ServiceException("Course not found");

            if (!String.IsNullOrEmpty(model.Title))
                course.Title = model.Title;
            if (model.Price >= 0.1m)
                course.Price = model.Price;
            await _courseRepository.UpdateAsync(course);
        }
    }
}
