using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.MfgGetInformationDtos
{
    public class GetMfgProductionOrderFormulaItemsInfor
    {
        public Guid ManufacturingFormulaMaterialId { get; set; }
        public Guid ItemId { get; set; }
        public ItemType itemType { get; set; }
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public string? MaterialNameSnapshot { get; set; }
        public string? MaterialExternalIdSnapshot { get; set; }
        public string? Unit { get; set; }
        public bool IsActive { get; set; } = true;

        public int LineNo { get; set; }

        // Lấy dữ liệu tồn kho
        public decimal OnHandKg { get; set; }
        public decimal? ReservedOpenAllKg { get; set; }
        public decimal? AvailableKg { get; set; }
    }
}
