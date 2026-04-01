using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.MfgProductionOrders
{
    public class GetMFGFormulaHistories
    {
        public string MfgFormulaExternalId { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public decimal TotalQuantity { get; set; }

        public decimal ErrorQuantity { get; set; }  
        public decimal GoodQuantity { get; set; }
    }
}
