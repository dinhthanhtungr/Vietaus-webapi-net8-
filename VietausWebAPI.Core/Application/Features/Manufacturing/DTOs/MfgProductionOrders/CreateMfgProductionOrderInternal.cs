using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public sealed class CreateMfgProductionOrderInternal
    {
        // bắt buộc
        public Guid ProductId { get; set; }
        public DateTime RequiredDate { get; set; } // ngày cần hàng

        public int TotalQuantityRequest { get; set; }
        public decimal UnitPriceAgreed { get; set; }
        public string BagType { get; set; } = string.Empty;

        // optional
        public Guid? CustomerId { get; set; }
        public Guid? FormulaId { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public int? NumOfBatches { get; set; }
        public int? TotalQuantity { get; set; }

        public string? LabNote { get; set; }
        public string? Requirement { get; set; }
        public string? PlpuNote { get; set; }
        public string? QcCheck { get; set; }
        public StepOfProduct? StepOfProduct { get; set; }

        // nếu muốn tạo xong nhảy thẳng Scheduling
        public string? InitialStatus { get; set; } // null => New

        // OPTIONAL: nếu muốn link với DHG detail để sync status như update
        public Guid? MerchandiseOrderDetailId { get; set; }
    }
}
