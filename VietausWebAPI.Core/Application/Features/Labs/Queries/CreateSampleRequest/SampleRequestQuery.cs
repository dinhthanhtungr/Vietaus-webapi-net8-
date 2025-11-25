
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest
{
    public class SampleRequestQuery : PaginationQuery
    {
        public string? Keyword { get; set; }
        public List<string> Statuses { get; set; } = new List<string>();
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
