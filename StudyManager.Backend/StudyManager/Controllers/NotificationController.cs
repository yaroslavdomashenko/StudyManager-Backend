using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Exceptions;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetNotifications(int take, int skip = 0)
        {
            try
            {
                var notifications = await _notificationService.GetNotification(User.Identity.Name, take, skip);
                return Ok(notifications);
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("read")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            try
            {
                await _notificationService.MarkAsRead(id);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }
    }
}
