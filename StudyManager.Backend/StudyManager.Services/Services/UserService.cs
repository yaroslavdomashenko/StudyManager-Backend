using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StudyManager.Data;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class UserService : IUserService
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        public UserService(ApplicationContext context, IMapper mapper, INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task ChangeName(string login, ChangeNameModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            if (user == null)
                throw new ServiceException("User not found");
            user.Name = model.Name != null ? model.Name : user.Name;
            user.Surename = model.Surename != null ? model.Surename : user.Surename;
            await _context.SaveChangesAsync();

            await _notificationService.CreateForUser(user.Id, "Name successfully changed!");
        }
        public async Task ChangePassword(string login, ChangePasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            if (user == null)
                throw new ServiceException("User not found");
            if (!VerifyPassswordHash(model.OldPassword, user.PasswordHash, user.PasswordSalt))
                throw new ServiceException("Wrong old password");

            CreateHash(model.NewPassword, out byte[] hash, out byte[] salt);
            user.PasswordSalt = salt;
            user.PasswordHash = hash;
            await _context.SaveChangesAsync();

            await _notificationService.CreateForUser(user.Id, "Password successfully changed!");
        }

        public async Task<UserModel> Get(Guid id)
        {
            var user = await _context.Users.Include(x => x.Courses).Include(x => x.CreatedCourses).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new ServiceException("User not found");
            return _mapper.Map<User, UserModel>(user);
        }
        public async Task<UserModel> Get(string login)
        {
            var user = await _context.Users.Include(x => x.Courses).Include(x=>x.CreatedCourses).FirstOrDefaultAsync(x => x.Login.ToLower() == login.ToLower());
            if (user == null)
                throw new ServiceException("User not found");
            return _mapper.Map<User, UserModel>(user);
        }
        public async Task<List<UserModel>> GetAll(int take, int skip)
        {
            var users = await _context.Users.Where(x => x.IsActive).Skip(skip).Take(take).ToListAsync();
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
