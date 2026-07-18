using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class PatchCheckMfgProductionOrder
    {
        public Guid mfgProductionOrderId { get; set; }
        public bool? IsPrintedStock { get; set; } = false; // Đã lưu trữ công thức
    }
}
