using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.ProductFeatures;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class GetSampleWithProductRequest
    {
        public GetProduct? Product { get; set; }    // case 2
        public GetSampleRequest? Sample { get; set; }
    }
}
