using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.Querys.Groups
{
    public class GroupMemberQuery
    {
        public Guid GroupId { get; set; }   
        public Guid MemberId { get; set; }
    }
}
