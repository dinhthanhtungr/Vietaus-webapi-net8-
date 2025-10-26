using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgProductionOrders
{
    public class FormulaAndMfgFormulaQuery : PaginationQuery
    {
        public string? Keyword { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
