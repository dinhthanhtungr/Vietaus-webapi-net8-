using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.WareHouses;

namespace VietausWebAPI.Core.Domain.Entities
{
    public class WarehouseShelfStock
    {
        public int SlotId { get; set; }                       // PK (identity/serial)

        public string SlotCode { get; set; } = "";            // kệ/ngăn (bin)
        public string Code { get; set; } = "";                // mã NVL
        public string? BatchNo { get; set; }
        public string? LotKey { get; set; }                   // số LOT
        public decimal QtyKg { get; set; }                    // khối lượng còn
        public int? Bags { get; set; }

        public StockType StockType { get; set; }               // tình trạng NVL (nguyên/không nguyên)

        public Guid CompanyId { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // PostgreSQL xmin (row version) — cấu hình trong Fluent API
        //public uint xmin { get; private set; }

        public virtual Employee? UpdatedByNavigation { get; set; }
        public virtual Company? Company { get; set; }
    }
}
