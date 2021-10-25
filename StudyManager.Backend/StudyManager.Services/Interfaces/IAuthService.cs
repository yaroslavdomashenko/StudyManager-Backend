using StudyManager.Data.Models;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(RegisterModel model);
        Task<string> Login(LoginModel model);
        Task<bool> UserExists(string login);
    }
}
