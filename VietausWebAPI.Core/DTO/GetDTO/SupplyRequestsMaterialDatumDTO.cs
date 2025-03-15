

namespace VietausWebAPI.Core.DTO.GetDTO
{
    public class SupplyRequestsMaterialDatumDTO
    {
        public string RequestId { get; set; } = null!;

        public DateTime RequestDate { get; set; }

        public string EmployeeId { get; set; } = null!;

        public string RequestStatus { get; set; } = null!;
    }
}
