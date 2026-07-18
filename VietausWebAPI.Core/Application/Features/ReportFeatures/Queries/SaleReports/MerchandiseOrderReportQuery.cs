using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports
{
    public class MerchandiseOrderReportQuery : PaginationQuery
    {
        public bool IncludeVat { get; set; } = false;
        public Guid? GroupId { get; set; }
        public Guid? EmployeeId { get; set; }
        public string? Keyword { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
