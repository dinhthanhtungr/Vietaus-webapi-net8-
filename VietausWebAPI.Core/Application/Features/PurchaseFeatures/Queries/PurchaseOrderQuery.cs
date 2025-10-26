using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.PurchaseFeatures.Queries
{
    public class PurchaseOrderQuery : PaginationQuery
    {
        public Guid? SupplierId { get; set; }
        public string? Status { get; set; }
        public string? OrderType { get; set; }
        public string? Keyword { get; set; }

        public bool? Islack { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
