using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.DTOs.MaterialRequestDetails;
using VietausWebAPI.Core.DTO.PostDTO;

namespace VietausWebAPI.Core.Application.DTOs.SupplyRequests
{
    public class SupplyRequestData
    {
        public string RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string EmployeeId { get; set; }
        public string PartName { get; set; }     // Thêm PartName của bộ phận
        public string? Note { get; set; }
        public string? NoteCancel { get; set; }
        public string RequestStatus { get; set; }
        public List<MaterialRequestDetailGetDTO> RequestDetails { get; set; }
    }
}
