using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Supplier.PatchDtos
{
    public class PatchSupplierContact
    {
        public Guid ContactId { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
        public bool? IsPrimary { get; set; } = false;
        public bool? IsActive { get; set; }
    }
}
