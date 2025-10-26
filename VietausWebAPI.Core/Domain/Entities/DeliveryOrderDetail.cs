using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class DeliveryOrderDetail
    {
        public Guid Id { get; set; }

        // FK bắt buộc: chi tiết thuộc một DeliveryOrder
        public Guid DeliveryOrderId { get; set; }
        public DeliveryOrder DeliveryOrder { get; set; } = default!;

        // FK TÙY CHỌN (cho phép null khi IsAttach = true)
        public Guid? MerchandiseOrderId { get; set; }
        public string? MerchandiseOrderExternalIdSnapShot { get; set; }

        public Guid? MerchandiseOrderDetailId { get; set; }

        // TÙY CHỌN nếu có “hàng ngoài danh mục”
        public Guid? ProductId { get; set; }
        public string? ProductExternalIdSnapShot { get; set; }
        public string? ProductNameSnapShot { get; set; }

        public string? LotNoList { get; set; } = null;
        public string? PONo { get; set; } = null;

        public decimal Quantity { get; set; }
        public int NumOfBags { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAttach { get; set; } = true;

        // Navigations TÙY CHỌN tương ứng
        public MerchandiseOrderDetail? MerchandiseOrderDetail { get; set; }
        public Product? Product { get; set; }
    }
}
    