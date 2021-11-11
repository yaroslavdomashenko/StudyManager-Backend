using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Entities;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    /// <summary>
    /// Admin controller. All methods requires "Admin" role.
    /// </summary>
    [ApiController]
    [Route("api/adminpanel")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        /// <summary>
        /// Changes user's role by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("change_role_by_id")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> ChangeRoleId(Guid id, Role role)
        {
            if(!await _adminService.ChangeRole(User.Identity.Name, id, role))
                return BadRequest(new ErrorModel { Code = 400, Message = "User not found" });
            return Ok();
        }

        /// <summary>
        /// Changes user's role by login
        /// </summary>
        /// <param name="login"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("change_role_by_login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> ChangeRoleLogin(string login, Role role)
        {
            if(!await _adminService.ChangeRole(User.Identity.Name, login, role))
                return BadRequest(new ErrorModel { Code = 400, Message = "User not found" });
            return Ok();
        }
    }
}
