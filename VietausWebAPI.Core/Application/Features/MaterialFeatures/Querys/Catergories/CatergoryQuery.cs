using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Category;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Catergories
{
    public class CategoryQuery : PaginationQuery
    {
        public CategoryTypes Type { get; set; }
    }
}
