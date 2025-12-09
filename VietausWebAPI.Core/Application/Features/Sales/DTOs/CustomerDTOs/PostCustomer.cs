using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class PostCustomer
    {
        public string? ExternalId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public string CustomerGroup { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public string? IssuedPlace { get; set; }
        public string? FaxNumber { get; set; }
        public string? Product { get; set; }
        public string? TaxNumber { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }

        public string? Notes { get; set; } = string.Empty;   

        public bool AssignNow { get; set; } = false;
        public int ClaimTtlHours { get; set; } = 48;
        public bool ConvertNow { get; set; } = false;   

        public List<PostAddress> Addresses { get; set; } = new();
        public List<PostContact> Contacts { get; set; } = new();
    }
}
