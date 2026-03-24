using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.GetDtos
{
    public class GetSupplier
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
        public List<GetSupplierAddress> SupplierAddresses { get; set; } = new List<GetSupplierAddress>();

        public List<GetSupplierContact> SupplierContacts { get; set; } = new List<GetSupplierContact>();

        //public List<>
    }
}
