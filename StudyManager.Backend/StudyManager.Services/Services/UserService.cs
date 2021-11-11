using AutoMapper;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> repository, IMapper mapper, INotificationService notificationService)
        {
            _repository = repository;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task ChangeName(string login, ChangeNameModel model)
        {
            var user = await _repository.GetFirstOrDefault(x => x.Login == login);
            if (user == null)
                throw new ServiceException("User not found");
            user.Name = model.Name != null ? model.Name : user.Name;
            user.Surename = model.Surename != null ? model.Surename : user.Surename;
            await _repository.Update(user);
            await _notificationService.CreateForUser(user.Id, "Name successfully changed!");
        }
        public async Task ChangePassword(string login, ChangePasswordModel model)
        {
            var user = await _repository.GetFirstOrDefault(x => x.Login == login);
            if (user == null)
                throw new ServiceException("User not found");
            if (!VerifyPassswordHash(model.OldPassword, user.PasswordHash, user.PasswordSalt))
                throw new ServiceException("Wrong old password");

            CreateHash(model.NewPassword, out byte[] hash, out byte[] salt);
            user.PasswordSalt = salt;
            user.PasswordHash = hash;
            await _repository.Update(user);

            await _notificationService.CreateForUser(user.Id, "Password successfully changed!");
        }

        public async Task<UserModel> Get(Guid id)
        {
            var user = await _repository.Get(id);
            if (user == null)
                return null;
            return _mapper.Map<User, UserModel>(user);
        }

        public async Task<UserModel> Get(string login)
        {
            var user = await _repository.GetFirstOrDefault(
                x => x.Login.ToLower() == login.ToLower(),
                i => i.Courses,
                i => i.CreatedCourses
            );
            if (user == null)
                return null;
            return _mapper.Map<User, UserModel>(user);
        }

        public async Task<List<UserModel>> GetAll(int take, int skip)
        {
            var users = await _repository.GetWithLimit(skip, take);
            return _mapper.Map<List<User>, List<UserModel>>(users);
        }

        private void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPassswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
