using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Models.User;
using StudyManager.Data.Models.Visit;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class VisitService : IVisitService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        public VisitService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<VisitModel> CreateVisit(CreateVisitModel model)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == model.CourseId);
            if (course == null)
                throw new Exception("Course not found");
            if(!course.IsActive)
                throw new Exception("Course is closed");

            List<User> users = new List<User>();
            foreach(string id in model.Visitors)
            {
                var user = await _context.Users.Include(x => x.Courses).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                if(user != null)
                {
                    if (user.Courses.Any(x => x.Id == model.CourseId))
                        users.Add(user);
                    else
                        throw new Exception($"User {id} not studying in {course.Id} course");
                }
            }

            Visit newVisit = new Visit
            {
                Id = Guid.NewGuid(),
                CourseId = model.CourseId,
                DateCreated = DateTime.Now,
                Visitors = users
            };

            _context.Visits.Add(newVisit);
            await _context.SaveChangesAsync();

            return _mapper.Map<VisitModel>(newVisit);
        }

        public async Task<List<VisitModel>> GetVisits(Guid courseId, int take, int skip)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
                throw new Exception("Course not found");

            var visits = await _context.Visits.Include(x => x.Visitors)
                .Where(x => x.CourseId == courseId).Skip(skip).Take(take).OrderBy(x => x.DateCreated).ToListAsync();
            return _mapper.Map<List<VisitModel>>(visits);
        }

        public async Task<List<UsersVisitModel>> GetVisitsInPeriod(GetVisitsInPeriod model, string login)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == model.CourseId);
            var user = await _context.Users.Include(x => x.Visits).FirstOrDefaultAsync(x => x.Login == login);

            if (course == null) throw new Exception("Course not found");
            if (user == null) throw new Exception("User not found");

            var visits = user.Visits.Where(x => x.CourseId == course.Id && x.DateCreated >= model.FirstDate && x.DateCreated <= model.SecondDate);
            return _mapper.Map<List<UsersVisitModel>>(visits);
        }
    }
}
