using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class WarehouseVoucherDto
    {
        public long VoucherId { get; set; }
        public string VoucherCode { get; set; } = string.Empty;
        public int VoucherType { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? RequestId { get; set; }
        public string RequestCode { get; set; } = string.Empty;
        public string RequestName { get; set; } = string.Empty;
        public WarehouseRequestStatus? ReqStatus { get; set; }
        public WareHouseRequestType? ReqType { get; set; }
        public string CodeFromRequest { get; set; } = string.Empty;

        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;

        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;

        public string? SupplierName { get; set; }
        public string? SupplierExternalId { get; set; }

        public List<WarehouseVoucherDetailDto> Details { get; set; } = new();
    }
}
