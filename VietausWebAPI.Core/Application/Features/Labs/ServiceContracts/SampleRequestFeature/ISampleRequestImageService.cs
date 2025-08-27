using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature
{
    public interface ISampleRequestImageService
    {
        Task<UploadImageResultDto> UploadAsync(Guid sampleRequestId, string originalFileName, string contentType, long contentLength, Stream content, CancellationToken ct);
        Task<List<SampleRequestImageDto>> ListAsync(Guid sampleRequestId, CancellationToken ct);
    }
}
