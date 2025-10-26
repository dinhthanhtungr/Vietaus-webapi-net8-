using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Warehouse.Queries
{
    public class WarehouseReservationServiceQuery
    {
        public Guid companyId { get; set; }
        public Guid snapshotSetId { get; set; }
        public string vaCode { get; set; } = string.Empty;
        public string vaLineCode { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string? lotKey { get; set; } = string.Empty;
        public decimal qtyRequest { get; set; }
        public string KeyWord { get; set; } = string.Empty;

        public Guid issueId { get; set; }

        public Guid createdBy { get; set; }
    }
}
