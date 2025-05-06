using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.WebAPI.Controllers.v1
{
    [ApiController]
    [Route("api/EmployeesCommon")]
    [AllowAnonymous]
    public class EmployeesCommonController : Controller
    {
        private readonly IEmployeesCommonService _employeesCommonService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="employeesCommonService"></param>
        public EmployeesCommonController(IEmployeesCommonService employeesCommonService)
        {
            _employeesCommonService = employeesCommonService;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> GetAllEmployeesCommon([FromQuery] string? Email)
        {
            var result = await _employeesCommonService.GetEmployeesWithIdServiceAsync(Email);
            return Ok(result);
        }
    }
}
