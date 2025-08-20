using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest
{
    public class SampleRequestDTO
    {
        public Guid SampleRequestId { get; set; }
        public string? ExternalId { get; set; }
        public Guid? ProductId { get; set; }
    }
}
