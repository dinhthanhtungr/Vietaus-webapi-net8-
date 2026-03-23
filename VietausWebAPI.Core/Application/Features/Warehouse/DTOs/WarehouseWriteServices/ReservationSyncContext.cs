using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.DTOs.WarehouseWriteServices
{
    public class ReservationSyncContext
    {
        public Guid MfgProductionOrderId { get; set; }
        public string VaCode { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
    }
}
