using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models.Comment;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }


        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(CommentModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> CreateComment(Guid hwId, string text)
        {
            try
            {
                var comment = await _commentsService.CreateComment(hwId, User.Identity.Name, text);
                return Ok(comment);
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("")]
        [ProducesResponseType(typeof(List<CommentModel>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> GetComments(Guid hwId, int take = 10, int skip = 0)
        {
            try
            {
                var comments = await _commentsService.GetComments(hwId, take, skip);
                return Ok(comments);
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("remove")]
        [ProducesResponseType(typeof(CommentModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentsService.DeleteComment(id);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

    }
}
