using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DeliveryOrders.DTOs
{
    public class GetSampleDeliveryOrder
    {
        public Guid Id { get; set; }
        public string? ExternalId { get; set; }

        public string? MerchandiseOrderExternalIdSnapShot { get; set; }
        public Guid? MerchandiseOrderId { get; set; }

        public string? CustomerExternalIdSnapShot { get; set; }
        public Guid? CustomerId { get; set; }

        public string? ExportExternalIdSnapShot { get; set; }
        public Guid? ExportId { get; set; }

        public Guid? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public Guid CompanyId { get; set; }
        public string? Note { get; set; }
    }
}
