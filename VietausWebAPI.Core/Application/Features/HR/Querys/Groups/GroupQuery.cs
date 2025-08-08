using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.HR.Querys.Groups
{
    public class GroupQuery : PaginationQuery
    {
        public string? keyword { get; set; } = string.Empty;
        public string? groupType { get; set; }
    }
}
