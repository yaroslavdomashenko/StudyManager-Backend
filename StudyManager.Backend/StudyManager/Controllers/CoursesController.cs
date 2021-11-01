using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Entities;
using StudyManager.Data.Exceptions;
using StudyManager.Data.Models;
using StudyManager.Data.Models.Course;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Returns course by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseModel),200)]
        [ProducesResponseType(typeof(ErrorModel), 404)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _courseService.Get(id));
            }catch(ServiceException ex)
            {
                return NotFound(new ErrorModel { Code = 404, Message = ex.Message });
            }
        }

        /// <summary>
        /// Returns all courses. Requires "Admin" or "Teacher" role.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("")]
        public async Task<ActionResult<List<CourseModel>>> GetAll(int take = 15, int skip = 0)
        {
            return Ok(new { items = await _courseService.GetAll(take, skip) });
        }

        /// <summary>
        /// Method creates a new course. Requires "Admin" or "Teacher" role.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(Course), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> Create(string title, decimal price)
        {
            try
            {
                var newCourse = await _courseService.Create(User.Identity.Name, title, price);
                return Ok(newCourse);
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Change status of course: true - is opened, false - closed. Requires "Admin" or "Teacher" role.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost("change_active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<ActionResult> ChangeActive(Guid course)
        {
            try
            {
                await _courseService.ChangeActive(course);
                return Ok();
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new student into the course by user's id, courses id. Requires "Admin" or "Teacher" role.
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost("add")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> Add(string userLogin, Guid course)
        {
            try
            {
                await _courseService.Add(User.Identity.Name, userLogin, course);
                return Ok();
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Removes student from the course. Requires "Admin" or "Teacher" role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost("remove")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> Remove(Guid user, Guid course)
        {
            try
            {
                await _courseService.Remove(user, course);
                return Ok();
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Adds a new teacher to the course by teacher's id, courses id. Requires "Admin" role.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("add_teacher")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> AddTeacher(string user, Guid course)
        {
            try
            {
                await _courseService.AddTeacher(user, course);
                return Ok();
            }catch(ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        /// <summary>
        /// Removes teacher from course. Requires "Admin" role.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("remove_teacher")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> RevomeTeacher(Guid course)
        {
            try
            {
                await _courseService.RemoveTeacher(course);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("change")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> ChangeCourseInfo(EditInfoModel model)
        {
            try
            {
                await _courseService.EditInfo(model);
                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }
    }
}
