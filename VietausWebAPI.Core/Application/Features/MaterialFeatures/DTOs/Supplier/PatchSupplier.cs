using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier
{
    public class PatchSupplier
    {
        public Guid SupplierId { get; set; }

        public string? ExternalId { get; set; }

        public string? SupplierName { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? RegistrationAddress { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssuedPlace { get; set; }
        public string? FaxNumber { get; set; }
        public string? Product { get; set; }

        public string? TaxNumber { get; set; }

        public string? Phone { get; set; }

        public string? Website { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public string? CompanyName { get; set; }
        public bool? IsActive { get; set; } = true;
        public List<PatchSupplierAddress> SupplierAddresses { get; set; } = new List<PatchSupplierAddress>();

        public List<PatchSupplierContact> SupplierContacts { get; set; } = new List<PatchSupplierContact>();
    }
}
