using Shared.Enums;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos
{
    public class GetColorChipManufacturingRecordSummary
    {
        public Guid ColorChipMfgRecordId { get; set; }
        public Guid? MfgProductionOrderId { get; set; }
        public string? MfgProductionOrderExternalId { get; set; }
        public Guid? ManufacturingFormulaId { get; set; }
        public string? ManufacturingFormulaExternalId { get; set; }
        public ResinType ResinType { get; set; }
        public LogoType LogoType { get; set; }
        public FormStyle FormStyle { get; set; }
        public DateTime? RecordDate { get; set; }
        public string? Machine { get; set; }
        public bool IsActive { get; set; }
    }
}
