using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Queries
{
    public class WarehouseSnapshotServiceQuery
    {
        public Guid companyId { get; set; }
        public string KeyWord { get; set; } = string.Empty;
        public string vaCode { get; set; } = string.Empty;
        public IEnumerable<string> materialCodes { get; set; } = Enumerable.Empty<string>();
        public Guid createdBy { get; set; }
    }
}
