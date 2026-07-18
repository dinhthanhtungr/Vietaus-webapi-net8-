using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys
{
    public class CustomerCrmActivityHeaderQuery : PaginationQuery
    {
        public List<string> ActivityTypes { get; set; } = new List<string> { nameof(EventTypeColorKey.FollowUpTask) };
        public Guid? AssignedSaleEmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
        public bool OnlyMine { get; set; }
        public bool IncludeCompleted { get; set; } = true;
        public bool IncludeCanceled { get; set; } = true;
        public string? Keyword { get; set; }
    }
}
