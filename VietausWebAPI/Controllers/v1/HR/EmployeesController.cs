using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Employees;
using VietausWebAPI.Core.Application.Features.HR.DTOs.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Employees;
using VietausWebAPI.Core.Application.Features.HR.Querys.Groups;
using VietausWebAPI.Core.Application.Features.HR.Querys.Parts;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.WebAPI.Controllers.v1.HR
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

        [HttpGet("GetByIds")]      
        public async Task<IActionResult> GetEmployeesByIds([FromQuery] Guid id)
        {
            var result = await _employeesService.GetEmployeesByIdsAsync(id);
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

        [HttpGet("GetPagedAccount")]
        public async Task<IActionResult> GetPagedAccount([FromQuery] EmployeeQuery? query)
        {
            var result = await _employeesService.GetPagedAccoutAsync(query);
            return Ok(result);
        }

        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroups([FromQuery] GroupQuery? query)
        {
            var result = await _employeesService.GetAllGroupsAsync(query);
            return Ok(result);
        }
        [HttpPost("CreateNewGroup")]
        public async Task<IActionResult> CreateNewGroup([FromBody] PostGroupDTOs group)
        {
            if (group == null)
            {
                return BadRequest("Group data is required.");
            }
            var result = await _employeesService.CreateNewGroupAsync(group);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        //[HttpDelete("DeleteGroup/{id}")]
        //public async Task<IActionResult> DeleteGroup([FromBody] Guid id)
        //{
        //    var result = await _employeesService.changeLeaderStatus(id);

        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return BadRequest(result.Message);
        //    }
        //}

        [HttpPost("AddMembersInGroup")]
        public async Task<IActionResult> AddMembersInGroup([FromBody] IEnumerable<PostMemberDTO> members)
        {
            if (!members.Any())
            {
                return BadRequest("Thêm thành viên không thành công.");
            }

            var result = await _employeesService.AddMembers(members);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("GetAllMembers")]
        public async Task<IActionResult> GetAllMembers([FromQuery] Guid Id , string? keywork)
        {
            var res = await _employeesService.AllMembers(Id, keywork);
            return Ok(res);
        }

        [HttpPatch("leader")]
        public async Task<IActionResult> ChangeLeaderStatus([FromQuery] GroupMemberQuery query)
        {
            var result = await _employeesService.changeLeaderStatus(query);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPatch("DeleteMemberInGroup")]
        public async Task<IActionResult> DeleteMemberInGroup([FromQuery] GroupMemberQuery query)
        {
            var result = await _employeesService.DeleteMemberInGroupAsync(query);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpGet("with-groups")]
        public async Task<ActionResult<PagedResult<EmployeeGroupDTO>>> Get(
            [FromQuery] GetEmployeesWithGroupsQuery query,
            CancellationToken ct = default)
                {
                    var result = await _employeesService.GetEmployeesWithGroupsAsync(query, ct);
                    return Ok(result);
                }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(CancellationToken ct = default)
        {
            var result = await _employeesService.GetRoleDTOsAsync(ct);
            return Ok(result);
        }

        [HttpGet("parts")]
        public async Task<IActionResult> GetParts([FromQuery] PartQuery query, CancellationToken ct = default)
        {
            try
            {
                var result = await _employeesService.GetSummaryParts(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
