using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.QCOutputFeature
{
    public class MfgProductDTOs
    {
        public Guid Id { get; set; }

        public string? ExternalId { get; set; }
        public string? product_name { get; set; }
        public string? Product_ExternalId { get; set; }
        public string? Product_CustomerExternalId { get; set; }
        public string? requirement { get; set; }
        public string? Product_Package { get; set; }
        public int? Product_Weight { get; set; }
        public Guid Product_Id { get; set; }
    }
}
