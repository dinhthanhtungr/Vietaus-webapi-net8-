using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs.Groups
{
    public class PostMemberDTO
    {
        public Guid? GroupId { get; set; }
        public Guid? Profile { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
