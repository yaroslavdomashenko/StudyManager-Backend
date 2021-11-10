using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudyManager.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public AuthService(IRepository<User> repository, IMapper mapper, IConfiguration config)
        {
            _repository = repository;
            _mapper = mapper;
            _config = config;
        }

        public async Task<string> Login(LoginModel model)
        {
            var user = await _repository.GetFirstOrDefault(x => x.Login.ToLower() == model.Login.ToLower());
            if (user == null)
                throw new ServiceException("User not found");
            if (!VerifyPassswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                throw new ServiceException("Wrong login or password");

            string token = CreateToken(user);
            return token;
        }
        public async Task<bool> Register(RegisterModel model)
        {
            if (await UserExists(model.Login)) 
                return false;
            CreateHash(model.Password, out byte[] hash, out byte[] salt);

            try
            {
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    DateCreated = DateTime.Now,
                    IsActive = true
                };
                _mapper.Map<RegisterModel, User>(model, user);

                await _repository.Add(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UserExists(string login)
        {
            var user = await _repository.GetFirstOrDefault(x => x.Login.ToLower() == login.ToLower());
            if (user != null) return true;
            return false;
        }

        private string CreateToken(User user)
        {
            var identity = GetIdentity(user);
            if (identity == null)
                return null;
            
            int expire;
            if (!Int32.TryParse(_config.GetSection("TokenSettings").GetSection("LifeTime").Value, out expire))
                expire = 730;

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("TokenSettings").GetSection("KEY").Value));
            var jwt = new JwtSecurityToken(
                issuer: _config.GetSection("TokenSettings").GetSection("ISSUER").Value,
                audience: _config.GetSection("TokenSettings").GetSection("AUDIENCE").Value,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                expires: DateTime.UtcNow.AddHours(expire),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
        private ClaimsIdentity GetIdentity(User user)
        {
            try
            {
                var claims = new List<Claim> {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            catch
            {
                return null;
            }
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
