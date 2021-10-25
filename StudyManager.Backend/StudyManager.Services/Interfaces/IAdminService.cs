using StudyManager.Data.Entities;
using System;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IAdminService
    {
        Task ChangeRole(string login, Guid id, Role role);
        Task ChangeRole(string login, string userLogin, Role role);
    }
}
