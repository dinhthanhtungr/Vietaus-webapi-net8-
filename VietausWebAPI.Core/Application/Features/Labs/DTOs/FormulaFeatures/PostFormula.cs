using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.FormulaFeatures
{
    public class PostFormula
    {
        public string? ExternalId { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public Guid ProductId { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public Guid? CompanyId { get; set; } = Guid.Parse("f54b3c96-4faa-43d1-8446-9d98c459c630");

        public List<PostMaterialFormula> materialFormulas { get; set; } = new List<PostMaterialFormula>();
    }
}
