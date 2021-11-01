using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models.Homework;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/homework")]
    [ApiController]
    public class HomeworkController : ControllerBase
    {
        private readonly IHomeworkService _hwService;
        public HomeworkController(IHomeworkService hwService)
        {
            _hwService = hwService;
        }

        [Authorize]
        [HttpGet("task")]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> GetTask(Guid id)
        {
            try
            {
                var homework = await _hwService.GetHomework(id);
                var attach = await _hwService.GetAttachment(homework.Id);
                return Ok(new { homework, attach });
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("homeworks")]
        [ProducesResponseType(typeof(List<HomeworkModel>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> GetHomework(Guid courseId, int take = 15, int skip = 0)
        {
            try
            {
                var homeworks = await _hwService.GetHomeworks(courseId, take, skip);
                return Ok(homeworks);
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<ActionResult<Guid>> CreateHomework(CreateHomeworkModel model)
        {
            try
            {
                var id = await _hwService.CreateHomework(model);
                return Ok(id);
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("attach")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> AddAttachments(Guid id, IFormFile[] files)
        {
            if (files.Length == 0)
                return BadRequest(new ErrorModel { Code = 400, Message = "File data is empty" });
            try
            {
                await _hwService.AddAttachments(id, files);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> Update(Guid id, string title, string text)
        {
            try
            {
                await _hwService.UpdateHomework(id, title, text);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> DeleteHomework(Guid id)
        {
            try
            {
                await _hwService.DeleteHomework(id);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }


    }
}
