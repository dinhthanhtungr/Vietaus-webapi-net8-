using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs.ExcelBuilds;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers.Excels
{
    public interface IExportDeliveryPlan
    {
        byte[] ExportDeliveryPlanExcel(List<DeliveryPlanRow> rows);

        byte[] ExportDeliveryFinish(List<DeliveryFinishRow> rows, DateTime fromDate, DateTime toDate);

        byte[] ExportTransportWorkbook(
            DeliveryTransportWorkbookData data,
            DateTime fromDate,
            DateTime toDate);
    }
}
