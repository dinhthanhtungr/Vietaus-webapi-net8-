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
        //public bool? HasSupplierDeliveryNote { get; set; }
        //public bool? HasCOA { get; set; }
        //public bool? HasPOConfirmation { get; set; }
        //public bool? HasInvoice { get; set; }

        //public Guid? AttachmentCollectionId { get; set; }
        public bool? IsActive { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
