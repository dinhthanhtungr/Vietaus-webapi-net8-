using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.QAQCFeature.QCOutputFeature
{
    public class MfgProductDTOs
    {
        public Guid Id { get; set; }

        public string? ExternalId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductExternalId { get; set; }
        public string? ProductCustomerExternalId { get; set; }
        public string? requirement { get; set; }
        public string? ProductPackage { get; set; }
        public int? ProductWeight { get; set; }
        public Guid ProductId { get; set; }
    }
}
