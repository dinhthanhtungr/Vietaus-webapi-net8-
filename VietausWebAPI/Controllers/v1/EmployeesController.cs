using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.HR.DTOs;
using VietausWebAPI.Core.Application.Features.HR.Querys;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/Employees")]
    [AllowAnonymous]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _employeesService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="employeesCommonService"></param>
        public EmployeesController(IEmployeesService employeesCommonService)
        {
            _employeesService = employeesCommonService;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> GetAllEmployeesCommon([FromQuery] string? Email)
        {
            var result = await _employeesService.GetEmployeesWithIdServiceAsync(Email);
            return Ok(result);
        }

        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetPagedEmployees([FromQuery] EmployeeQuery? query)
        {
            var result = await _employeesService.GetPagedAsync(query);
            return Ok(result);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostEmployees([FromBody] EmployeesPostDTOs employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee data is required.");
            }
            var result = await _employeesService.PostEmployees(employee);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
