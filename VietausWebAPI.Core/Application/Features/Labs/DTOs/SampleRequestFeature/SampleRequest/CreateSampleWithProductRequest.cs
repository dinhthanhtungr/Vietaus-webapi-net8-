using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ProductFeature;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    // Trường hợp 1: đã có Product -> dùng ProductId
    // Trường hợp 2: chưa có -> gửi payload Product để tạo mới
    public class CreateSampleWithProductRequest
    {
        public Guid? ProductId { get; set; }                  // case 1
        public CreateProductRequest? Product { get; set; }    // case 2
        public CreateSampleRequest? Sample { get; set; }
    }
}
