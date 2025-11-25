using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.Queries.MfgFormulas
{
    public class MfgFormulaQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public Guid? ProductId { get; set; }
        public string? Keyword { get; set; }
        public string? fromVuOrVa { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public FormulaSource Source { get; set; }
    }
}
    