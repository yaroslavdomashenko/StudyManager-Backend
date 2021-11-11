using StudyManager.Data.Entities;
using System;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> ChangeRole(string login, Guid id, Role role);
        Task<bool> ChangeRole(string login, string userLogin, Role role);
    }
}
