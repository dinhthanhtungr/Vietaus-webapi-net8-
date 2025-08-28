using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material
{
    public class PostPriceHistory
    {
        public Guid SupplierId { get; set; }
        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public string? Currency { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreatedBy { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
