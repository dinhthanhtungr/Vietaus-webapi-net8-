using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries
{
    public class ProductStandardQuery : PaginationQuery
    {
        public string? Keyword { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ProductStandard { get; set; }
    }
}
