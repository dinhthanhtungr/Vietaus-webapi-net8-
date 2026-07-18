using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class LotNumberOptionDto
    {
        public string LotNo { get; set; } = string.Empty;
        public StockType StockType { get; set; }
        public string QualityStatus { get; set; } = string.Empty;
        public string QualityStatusName { get; set; } = string.Empty;
        public bool IsDefective { get; set; }
        public decimal QuantityKg { get; set; }
        public int? Bags { get; set; }
    }
}
