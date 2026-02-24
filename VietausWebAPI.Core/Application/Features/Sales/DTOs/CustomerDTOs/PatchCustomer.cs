using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class PatchCustomer
    {
        public Guid CustomerId { get; set; }

        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
        public string? CustomerGroup { get; set; }
        public string? ApplicationName { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? RegistrationAddress { get; set; }
        public string? TaxNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssuedPlace { get; set; }
        public string? FaxNumber { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsLead { get; set; }
        public LeadStatus? LeadStatus { get; set; }

        public PatchCustomerNote PatchCustomerNote { get; set; } = new PatchCustomerNote();
        public List<PatchAddress> Addresses { get; set; } = new List<PatchAddress>();
        public List<PatchContact> Contacts { get; set; } = new List<PatchContact>();
    }
}
