using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs
{
    public class EmployeesPostDTOs
    {
        public string? ExternalId { get; set; }

        public string FullName { get; set; } = null!;

        public string? Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Identifier { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public Guid? PartId { get; set; }

        public DateTime? DateHired { get; set; }

        public string? Status { get; set; } = "Active";

        public DateOnly? EndDate { get; set; }

    }
}
