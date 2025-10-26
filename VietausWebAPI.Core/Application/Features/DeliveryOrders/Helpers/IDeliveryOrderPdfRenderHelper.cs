using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers
{
    public  interface IDeliveryOrderPdfRenderHelper
    {
        byte[] Render(PdfPrinterDeliveryOrder data);
    }
}
