using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.DTOs.Approval
{
    public class ApprovalResponseDetailDTO
    {
        public string? note { get; set; } = string.Empty;
        public List<MetarialListResponseDTO> approvalRequestDetailDTOs { get; set; } = new List<MetarialListResponseDTO>();
    }
}
