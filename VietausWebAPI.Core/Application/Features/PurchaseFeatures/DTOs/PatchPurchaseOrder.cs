using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs
{
    public class PatchPurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }
        public string? Comment { get; set; }
        public string? PLPUComment { get; set; }
        public bool? IsActive { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
