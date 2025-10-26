using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.DTOs.Material_warehouse
{
    public class MaterialStock
    {
        public Guid MaterialId { get; set; }
        public string? MaterialExternalIDSnapshot { get; set; }
        public string? MaterialNameSnapshot { get; set; }

        public string? Package { get; set; }
        public decimal? BaseCostSnapshot { get; set; }

        public decimal? CurrentStock { get; set; }  

        public decimal? ReservedStock { get; set; }
        public string? RequiredVaCodeList { get; set; }   // (optional) danh sách lệnh tạo ra nhu cầu

        public DateTime? lastUpdatePrice { get; set; }
        public DateTime? lastUse { get; set; }

    }
}
