using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature
{
    public interface ISampleRequestService
    {
        Task<SampleRequestDTO> CreateAsync(CreateSampleWithProductRequest req, CancellationToken ct = default);
    }
}
