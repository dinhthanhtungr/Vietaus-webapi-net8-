using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseReadServices
{
    public class ReservedVaCodeInfo
    {
        public string VaCode { get; set; } = string.Empty;
        public decimal ReservedKg { get; set; }
        public DateTime CreatedDate { get; set; }   
    }
}
