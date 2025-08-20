using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Groups
{
    public class GroupLiteDTO
    {
        public Guid GroupId { get; set; }
        public string GroupCode { get; set; } = "";
        public string GroupName { get; set; } = "";
        public string GroupType { get; set; } = "";
        public bool? IsAdmin { get; set; }
    }

    public class EmployeeGroupDTO
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeCode { get; set; } = "";
        public string FullName { get; set; } = "";
        public List<GroupLiteDTO> Groups { get; set; } = new();
    }

}
