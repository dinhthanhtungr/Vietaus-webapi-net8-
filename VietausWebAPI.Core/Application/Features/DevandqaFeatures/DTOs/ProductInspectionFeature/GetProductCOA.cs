using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.ProductInspectionFeature
{
    public class GetProductCOA
    {
        // Data lấy từ productTest
        public Guid? id { get; set; }
        public string? externalId { get; set; }
        public DateTime? manufacturingDate { get; set; }
        public DateTime? expiryDate { get; set; }  
        public string? productPackage { get; set; } // Mã loại bao bì của sản phẩm
        public float? ProductWeight { get; set; } // Trọng lượng sản phẩm tiêu chuẩn

        public Guid? ProductId { get; set; }
        public string? productExternalId { get; set; } // color code
        public string? productName { get; set; } // Tên sản phẩm
    }
}
