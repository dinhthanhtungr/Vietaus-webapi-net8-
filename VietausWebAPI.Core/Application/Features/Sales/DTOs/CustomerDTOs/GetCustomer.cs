using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class GetCustomer
    {
        public Guid CustomerId { get; set; }

        public string? ExternalId { get; set; }

        public string? CustomerName { get; set; }

        public string? EmployeeName { get; set; }

        public string? CustomerGroup { get; set; }

        public string? ApplicationName { get; set; }

        public string? RegistrationNumber { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssuedPlace { get; set; }
        public string? FaxNumber { get; set; }
        public string? Product { get; set; }

        public string? TaxNumber { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        //public DateTime? CreatedDate { get; set; }

        //public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public string? CompanyName { get; set; }
        public bool? IsActive { get; set; } = true;
        public List<GetAddress> Addresses { get; set; } = new List<GetAddress>();

        public List<GetContact> Contacts { get; set; } = new List<GetContact>();
    }
}
