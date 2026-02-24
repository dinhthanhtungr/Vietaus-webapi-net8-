using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.ProductInspectionFeature
{
    public class StatisticalReportQuery
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string? productCode { get; set; }
        public string? types { get; set; } = "QCOUT";
    }
}
