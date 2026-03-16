using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs.ExcelBuilds
{
    public class DeliveryTransportWorkbookData
    {
        public List<DeliveryTransportSummaryRow> SummaryRows { get; set; } = new();
        public Dictionary<string, List<DeliveryTransportReportRow>> DetailByDeliverer { get; set; } = new();
    }
}
