using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class GetAddress
    {
        public Guid AddressId { get; set; }
        public string? AddressLine { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? Province { get; set; }

        public string? Country { get; set; }

        public bool? IsPrimary { get; set; } = false;

        public string? PostalCode { get; set; }
    }
}
