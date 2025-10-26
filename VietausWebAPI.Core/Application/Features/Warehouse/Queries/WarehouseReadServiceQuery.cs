using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Queries
{
    public class WarehouseReadServiceQuery
    {
        public Guid companyId { get; set; }
        public string KeyWord { get; set; } = string.Empty; 
        public string code { get; set; } = string.Empty;
        public string lotkey { get; set; } = string.Empty;

    }
}
