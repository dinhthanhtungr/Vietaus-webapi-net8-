using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.HR.Querys.Parts
{
    public class PartQuery : PaginationQuery
    {
        public string? keyword { get; set; } = null;
    }
}
