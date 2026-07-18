using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrderRWs.UpsertInformationDtos
{
    public class CreateMfgProductionOrderFormulaItemRequest
    {
        public Guid ItemId { get; set; }
        public ItemType ItemType { get; set; }
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public string? MaterialNameSnapshot { get; set; }
        public string? MaterialExternalIdSnapshot { get; set; }
        public LotNumberOptionDto? LotNumber { get; set; } = null;
        public string? Unit { get; set; }

        public bool IsActive { get; set; } = true;
        public int LineNo { get; set; }
    }
}
