﻿using AutoMapper;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models.Visit;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class VisitService : IVisitService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Visit> _visitRepository;
        private readonly IMapper _mapper;

        public VisitService(
            IRepository<User> userRepository, 
            IRepository<Course> courseRepository,
            IRepository<Visit> visitRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _visitRepository = visitRepository;
            _mapper = mapper;
        }

        public async Task<VisitModel> CreateVisit(CreateVisitModel model)
        {
            var course = await _courseRepository.Get(model.CourseId);
            if (course == null)
                throw new ServiceException("Course not found");
            if(!course.IsActive)
                throw new ServiceException("Course is closed");

            List<User> users = new List<User>();
            foreach(string id in model.Visitors)
            {
                var user = await _userRepository.Get(Guid.Parse(id), i => i.Courses);
                if(user != null)
                {
                    if (user.Courses.Any(x => x.Id == model.CourseId))
                        users.Add(user);
                    else
                        throw new ServiceException($"User {id} not studying in {course.Id} course");
                }
            }

            Visit newVisit = new Visit
            {
                Id = Guid.NewGuid(),
                CourseId = model.CourseId,
                DateCreated = DateTime.Now,
                Visitors = users
            };

            await _visitRepository.Add(newVisit);
            return _mapper.Map<VisitModel>(newVisit);
        }

        public async Task<List<VisitModel>> GetVisits(Guid courseId, int take, int skip)
        {
            var course = await _courseRepository.Get(courseId);
            if (course == null)
                throw new ServiceException("Course not found");

            var visits = await _visitRepository.GetWithLimit(skip, take, w => w.CourseId == courseId, i => i.Visitors);
            return _mapper.Map<List<VisitModel>>(visits);
        }

        public async Task<List<UsersVisitModel>> GetVisitsInPeriod(GetVisitsInPeriod model, string login)
        {
            var course = await _courseRepository.Get(model.CourseId);
            var user = await _userRepository.GetFirstOrDefault(x => x.Login.ToLower() == login, i => i.Visits);

            if (course == null) 
                throw new ServiceException("Course not found");
            if (user == null) 
                throw new ServiceException("User not found");

            var visits = user.Visits.Where(x => x.CourseId == course.Id && x.DateCreated >= model.FirstDate && x.DateCreated <= model.SecondDate);
            return _mapper.Map<List<UsersVisitModel>>(visits);
        }
    }
}
