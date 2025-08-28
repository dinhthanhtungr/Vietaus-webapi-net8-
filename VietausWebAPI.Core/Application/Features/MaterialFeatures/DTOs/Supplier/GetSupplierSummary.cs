using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier
{
    public class GetSupplierSummary
    {
        public Guid SupplierId { get; set; }
        public string? ExternalId { get; set; }

        public string? SupplierName { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? Phone { get; set; } 
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
