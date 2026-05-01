using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class WarehouseVoucherDetailDto
    {
        public long VoucherDetailId { get; set; }
        public long VoucherId { get; set; }
        public int LineNo { get; set; }

        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string? LotNumber { get; set; }

        public decimal QtyKg { get; set; }
        public int? Bags { get; set; }

        public int? SlotId { get; set; }
        public int? PurposeId { get; set; }
        public bool IsIncrease { get; set; }
        public DateTime? MovementDate { get; set; }

        public DateTime ExpiryDate { get; set; }
        public VoucherDetailType VoucherType { get; set; }
        public string? Note { get; set; }
    }
}
