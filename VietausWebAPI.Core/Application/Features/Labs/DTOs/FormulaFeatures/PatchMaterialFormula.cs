using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class PatchMaterialFormula
    {
        public Guid MaterialId { get; set; }
        public Guid CategoryId { get; set; }

        public decimal Quantity { get; set; }            // DECIMAL(18,6)
        public decimal UnitPrice { get; set; }           // DECIMAL(16,2)
        public decimal TotalPrice { get; set; }          // DECIMAL(16,2)


        public string? MaterialNameSnapshot { get; set; }   // NVARCHAR
        public string? MaterialExternalIdSnapshot { get; set; }   // VARCHAR
        public string? Unit { get; set; }                   // VARCHAR
    }
}
