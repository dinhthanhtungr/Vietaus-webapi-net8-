using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports
{
    public class PLPUPurchaseOverviewQuery : PaginationQuery
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? PurchaseOrderId { get; set; }
        public Guid? SupplierId { get; set; }
        public Guid? MaterialId { get; set; }
        public string? OrderType { get; set; }
        public string? Status { get; set; }
        public string? Keyword { get; set; }
        public bool IncludeInactive { get; set; } = false;
        public bool OnlyNeedAction { get; set; } = false;
    }
}
