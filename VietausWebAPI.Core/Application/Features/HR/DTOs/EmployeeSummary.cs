using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs
{
    public class EmployeeSummary
    {
        public string? ExternalId { get; set; }

        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? PartName { get; set; }
    }
}
