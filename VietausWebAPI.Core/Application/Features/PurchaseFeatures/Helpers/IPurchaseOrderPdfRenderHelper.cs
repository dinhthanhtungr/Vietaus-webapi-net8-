using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.PdfPrinter;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Helpers
{
    public interface IPurchaseOrderPdfRenderHelper
    {
        byte[] Render(PdfPrinterPurchaseOrder data);
    }
}
