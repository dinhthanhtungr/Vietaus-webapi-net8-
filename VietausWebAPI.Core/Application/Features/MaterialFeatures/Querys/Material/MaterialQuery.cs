using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Material
{
    public class MaterialQuery : PaginationQuery
    {
        public Guid? MaterialId { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Keyword { get; set; }
        public Guid? SupplierId { get; set; }
        public string? Category { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
