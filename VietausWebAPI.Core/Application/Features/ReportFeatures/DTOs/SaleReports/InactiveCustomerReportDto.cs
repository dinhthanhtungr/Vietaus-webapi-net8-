using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports
{
    public class InactiveCustomerReportDto
    {
        public Guid CustomerId { get; set; }

        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;

        public Guid ManagerById { get; set; }
        public string ManagerName { get; set; } = string.Empty;

        public DateTime? LastOrderDate { get; set; }

        public int DaysSinceLastOrder { get; set; }

        public int TotalOrderCount { get; set; }
        public decimal TotalOrderAmount { get; set; }

        public int OrderCountInPeriod { get; set; }
        public decimal OrderAmountInPeriod { get; set; }

        public string RiskLevel { get; set; } = string.Empty;
    }
}
