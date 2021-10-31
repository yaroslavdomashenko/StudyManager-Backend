using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Models.Chat;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [Authorize]
        [HttpGet("messages")]
        public async Task<ActionResult<List<MessageDto>>> GetMessages(Guid chatId, int skip = 0)
        {
            try
            {
                var messages = await _chatService.GetMessages(chatId, skip);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }
    }
}
