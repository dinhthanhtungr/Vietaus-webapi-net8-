using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public sealed class AssignLeadRequest
    {
        public Guid CustomerId { get; set; }
        public Guid SalesEmployeeId { get; set; }
        public string? note { get; set; } = null;
        public int ExpiredInDays { get; set; } = 600;
    }
}
