using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Infrastructure;
using StudyManager.Data.Models;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        public UsersController(IUserService userService, IFileService fileService)
        {
            _userService = userService;
            _fileService = fileService;
        }

        [Authorize]
        [HttpPost("upload-avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null)
                return BadRequest(new ErrorModel { Code = 400, Message = "File is null" });
            try
            {
                await _fileService.UploadAvatar(User.Identity.Name, file);
                return Ok();
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Returns user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserModel), 200)]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var user = await _userService.Get(User.Identity.Name);
                return Ok(user);
            }
            catch (ServiceException ex)
            {
                return NotFound(new ErrorModel { Code = 404, Message = ex.Message });
            }
        }

        /// <summary>
        /// Returns user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        public async Task<ActionResult<UserModel>> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.Get(id);
                return Ok(user);
            }catch(ServiceException ex)
            {
                return NotFound(new ErrorModel { Code = 404, Message = ex.Message });
            }
        }

        /// <summary>
        /// Rreturns user by login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("{login}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        public async Task<ActionResult<UserModel>> GetUser(string login)
        {
            try
            {
                var user = await _userService.Get(login);
                return Ok(user);
            }
            catch (ServiceException ex)
            {
                return NotFound(new ErrorModel { Code = 404, Message = ex.Message });
            }
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<ActionResult<List<UserModel>>> GetAll(int take = 20, int skip = 0)
        {
            return Ok(new { users = await _userService.GetAll(take, skip) });
        }

        /// <summary>
        /// Changes user's password. Must be authorized
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("change_passowod")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                await _userService.ChangePassword(User.Identity.Name, model);
                return Ok();
            }
            catch(ServiceException ex)
            {;
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Changes user's name
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("change_name")]
        [ProducesResponseType(typeof(ChangeNameModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> ChangeName(ChangeNameModel model)
        {
            try
            {
                await _userService.ChangeName(User.Identity.Name, model);
                return Ok(model);
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }
    }
}
