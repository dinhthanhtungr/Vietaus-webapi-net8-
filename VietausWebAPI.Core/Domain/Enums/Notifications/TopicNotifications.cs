using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Enums.Notifications
{
    public enum TopicNotifications
    {
        // ==================== Products ==================== 
        ProductSampleCreated,
        ProductSampleUpdated,
        ProductSampleDeleted,

        // ==================== Merchandise Orders ====================
        MerchandiseOrderCreated,
        MerchandiseOrderUpdated,
        MerchandiseOrderDeleted,

        // ==================== Merchandise Orders ====================
        ManufacturingOrderCreated,
        ManufacturingOrderUpdated,
        ManufacturingOrderDeleted,

        // ==================== Price Over Sell ====================
        PriceOverSellCreated,
        WarehouseStockLost,


    }
}
