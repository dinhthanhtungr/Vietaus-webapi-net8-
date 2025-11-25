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

        //public Guid? EmployeeId { get; set; }
        public PostCustomerAssignment CustomerAssignment { get; set; } = new PostCustomerAssignment();

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

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public Guid? CompanyId { get; set; }

        public List<PostAddress> Addresses { get; set; } = new List<PostAddress>();

        public List<PostContact> Contacts { get; set; } = new List<PostContact>();
    }
}
