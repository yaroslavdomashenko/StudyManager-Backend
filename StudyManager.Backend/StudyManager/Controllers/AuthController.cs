using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers new user in database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            if (!await _authService.Register(request))
                return BadRequest(new ErrorModel { Code = 400, Message = "Login is busy" });
            return Ok();
        }

        /// <summary>
        /// Log in controller. Returns JWT token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> Login(LoginModel request)
        {
            string jwt = await _authService.Login(request);
            if(jwt is null)
                return BadRequest(new ErrorModel { Code = 400, Message = "Wrong username or password" });
            return Ok(new { token = jwt });
        }
    }
}
