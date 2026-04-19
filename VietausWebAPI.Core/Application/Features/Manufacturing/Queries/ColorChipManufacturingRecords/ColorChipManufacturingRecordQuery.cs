using Shared.Enums;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.SampleRequests;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Queries.ColorChipManufacturingRecords
{
    public class ColorChipManufacturingRecordQuery : PaginationQuery
    {
        public Guid? ColorChipMfgRecordId { get; set; }
        public Guid? MfgProductionOrderId { get; set; }
        public Guid? ManufacturingFormulaId { get; set; }
        public Guid? CompanyId { get; set; }

        public ResinType? ResinType { get; set; }
        public LogoType? LogoType { get; set; }
        public FormStyle? FormStyle { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool? IsActive { get; set; }
        public string? Keyword { get; set; }
    }
}
