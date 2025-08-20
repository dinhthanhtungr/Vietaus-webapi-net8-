using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Groups
{
    public class GetMemberDTO
    {
        public Guid MemberId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? ExternalId { get; set; }
        public string? MemberName { get; set; }
        public bool? IsAdmin { get; set; }
        //public Guid? GroupId {  get; set; }
    }
}
