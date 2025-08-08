using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Groups
{
    public class GetGroupDTOs
    {
        public Guid GroupId { get; set; }

        public string? GroupType { get; set; }

        public string ExternalId { get; set; } = null!;

        public string? Name { get; set; }
        public string? LeaderName { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }
        public int MemberCount { get; set; }
    }
}
