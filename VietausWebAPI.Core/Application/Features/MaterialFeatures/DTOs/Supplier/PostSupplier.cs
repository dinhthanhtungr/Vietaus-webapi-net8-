using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier
{
    public class PostSupplier
    {
        public string? ExternalId { get; set; }

        public string? SupplierName { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? TaxNumber { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public string? Note { get; set; }

        public DateTime? IssueDate { get; set; }

        public string? IssuedPlace { get; set; }

        public string? FaxNumber { get; set; }
        public List<PostSupplierAddress> SupplierAddresses { get; set; } = new List<PostSupplierAddress>();

        public List<PostSupplierContact> SupplierContacts { get; set; } = new List<PostSupplierContact>();
    }
}
