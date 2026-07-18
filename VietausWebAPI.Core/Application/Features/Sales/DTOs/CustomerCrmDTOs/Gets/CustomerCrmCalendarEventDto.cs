using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerCrmDTOs.Gets
{
    public class CustomerCrmCalendarEventDto
    {
        public string Id { get; set; } = string.Empty;
        public Guid SourceId { get; set; }
        public string SourceType { get; set; } = string.Empty;

        public Guid CustomerId { get; set; }
        public string? CustomerExternalId { get; set; }
        public string? CustomerName { get; set; }

        public Guid? AssignedSaleEmployeeId { get; set; }
        public string? AssignedSaleEmployeeName { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public string? Status { get; set; }
        public string? Priority { get; set; }
        public bool IsOverdue { get; set; }
        public string ColorKey { get; set; } = string.Empty;
    }
}
