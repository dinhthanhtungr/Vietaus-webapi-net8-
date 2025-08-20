using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.HR.Querys.Groups
{
    public class GetEmployeesWithGroupsQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public bool OnlyActiveMembership { get; set; } = true;
        public string? Keyword { get; set; }
        public string? GroupType { get; set; } 
        public string? ExternalId { get; set; } 
    }
}
