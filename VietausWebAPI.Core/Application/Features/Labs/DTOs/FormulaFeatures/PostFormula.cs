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


        public List<PostMaterialFormula> materialFormulas { get; set; } = new List<PostMaterialFormula>();
    }
}
