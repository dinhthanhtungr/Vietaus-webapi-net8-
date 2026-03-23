using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Formulas;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports
{
    public class FinishReportQuery : PaginationQuery
    {
        public Guid? CompanyId { get; set; }
        public Guid? ProductId { get; set; }
        public string? Keyword { get; set; }
        public string? fromVuOrVa { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
