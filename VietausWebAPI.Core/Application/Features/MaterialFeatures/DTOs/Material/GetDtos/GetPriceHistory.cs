using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material.GetDtos
{
    public class GetPriceHistory
    {
        public decimal OldPrice { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
