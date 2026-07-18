using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.Querys.CustomerCrmQuerys
{
    public class CustomerInteractionQuery : PaginationQuery
    {
        public Guid? CustomerId { get; set; }
        public Guid? AssignedSaleEmployeeId { get; set; }
        public CustomerInteractionType? InteractionType { get; set; }
        public string? Keyword { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool IncludeInactive { get; set; } = false;
    }
}
