using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Groups
{
    public class GetGroupInfor
    {
        public Guid GroupId { get; set; }
        public string? GroupType { get; set; }
        public string? Name { get; set; }
        public string ExternalId { get; set; } = null!;
        public IEnumerable<GetMemberDTO> members { get; set; } = Enumerable.Empty<GetMemberDTO>();
    }
}
