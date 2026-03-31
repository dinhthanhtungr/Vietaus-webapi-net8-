using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Querys.ExtruderOperationHistoryQuery
{
    public class ExtruderOperationHistoryQuery : PaginationQuery
    {
        public string? ProductCode { get; set; }
        public string? ExternalId { get; set; }
    }
}
