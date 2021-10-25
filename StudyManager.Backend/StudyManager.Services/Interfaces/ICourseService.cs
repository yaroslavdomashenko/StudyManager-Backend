using StudyManager.Data.Entities;
using StudyManager.Data.Models;
using StudyManager.Data.Models.Course;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseModel> Get(Guid id);
        Task<List<Course>> GetAll(int take, int skip);
        Task<Course> Create(string login, string title, decimal price);
        Task Add(string requestor, string userLogin, Guid course);
        Task Remove(Guid userId, Guid courseId);
        Task ChangeActive(Guid courseId);
        Task EditInfo(EditInfoModel model);
        Task AddTeacher(string teacherLogin, Guid course);
        Task RemoveTeacher(Guid course);
    }
}
