using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.ShiftReportFeatures.DTOs.EndOfShiftReportFeatureDTOs.EndOfShiftReportReadRepositories
{
    public class MfgFormulaQuantityRow
    {
        public string MfgProductionOrderExternalId { get; set; } = string.Empty;
        public string MfgFormulaExternalId { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public decimal GoodQuantity { get; set; }
        public decimal ErrorQuantity { get; set; }
    }
}
