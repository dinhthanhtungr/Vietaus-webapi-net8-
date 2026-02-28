using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers.Excels
{
    public interface IExportDeliveryPlan
    {
        byte[] ExportDeliveryPlanExcel(List<DeliveryPlanRow> rows);
    }
}
