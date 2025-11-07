using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Enums.Logs;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.Queries
{
    public class TimelineQuery : PaginationQuery
    {
        public Guid? CreatedBy { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? id { get; set; }
        public EventType EventType { get; set; }
        public string? Status { get; set; }
        //public string? SourceCode { get; set; }

        //public string? CustomerExtternalId { get; set; }

        public string? Keyword { get; set; }

        // Khoảng ngày tạo — sẽ áp theo CreatedScope
        public DateTime? FromCreated { get; set; }
        public DateTime? ToCreated { get; set; }


        // PHẦN MỚI: chỉ rõ “Created” áp vào thực thể nào
        public string? CreatedScope { get; set; }
    }


    public enum TimelineScope
    {
        Merchandise,
        Manufacturing,
        Delivery,
        Requisition
    }

}
