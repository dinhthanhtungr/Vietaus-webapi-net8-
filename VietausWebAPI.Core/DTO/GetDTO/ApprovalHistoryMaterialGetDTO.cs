using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class ApprovalHistoryMaterialGetDTO : RequestStatusPostDTO
    {

        //public string RequestId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        //public string requestStatus { get; set; } = null!;
    }
}
