using StudyManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> Get(Guid id);
        Task<UserModel> Get(string login);
        Task<List<UserModel>> GetAll(int take, int skip);

        Task ChangePassword(string login, ChangePasswordModel model);
        Task ChangeName(string login, ChangeNameModel model);
    }
}
