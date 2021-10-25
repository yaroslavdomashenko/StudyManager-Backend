using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyManager.Data.Models.Visit;
using StudyManager.ResponseModels;
using StudyManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyManager.Controllers
{
    [Route("api/visits")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;
        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        /// <summary>
        /// Returns 20 last courses visit history
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet("")]
        [ProducesResponseType(typeof(List<VisitModel>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> GetVisits(Guid courseId, int take = 15, int skip = 0)
        {
            try
            {
                var visits = await _visitService.GetVisits(courseId, take, skip);
                return Ok(visits);
            }
            catch(Exception ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            } 
        }

        /// <summary>
        /// Creates new visit in database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        [ProducesResponseType(typeof(VisitModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> CreateVisit(CreateVisitModel model)
        {
            try
            {
                var visit = await _visitService.CreateVisit(model);
                return Ok(visit);
            }catch(Exception ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }

        [HttpGet("user")]
        [ProducesResponseType(typeof(UsersVisitModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<IActionResult> GetVisitsInPeriod([FromQuery]GetVisitsInPeriod model, string login)
        {
            try
            {
                var visits = await _visitService.GetVisitsInPeriod(model, login);
                return Ok(visits);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorModel { Code = 400, Message = ex.Message });
            }
        }
    }
}
