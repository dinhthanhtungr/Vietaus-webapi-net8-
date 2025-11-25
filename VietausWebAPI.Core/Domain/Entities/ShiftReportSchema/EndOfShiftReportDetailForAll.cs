using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities.ShiftReportSchema
{
    public class EndOfShiftReportDetailForAll
    {
        public long ShiftReportDetailForAllId { get; set; }  // PK (bigint)
        public string? ExternalId { get; set; }              // text/citext (liên kết mềm)
        public string? ProductType { get; set; }             // text
        public decimal? NetWeight { get; set; }              // numeric
        public decimal? WeightStockedKg { get; set; }        // numeric
    }
}
