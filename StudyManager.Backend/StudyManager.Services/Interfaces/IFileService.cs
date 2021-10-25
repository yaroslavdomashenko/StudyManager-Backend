using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StudyManager.Services.Interfaces
{
    public interface IFileService
    {
        Task UploadAvatar(string userIdentity, IFormFile file);
    }
}
