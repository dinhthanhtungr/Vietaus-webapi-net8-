using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.Queries.FormulaFeature
{
    public class ManufacturingVUFormulaQuery : PaginationQuery
    {
        public string? Keyword { get; set; }
        public Guid? ManufacturingVUFormulaId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
