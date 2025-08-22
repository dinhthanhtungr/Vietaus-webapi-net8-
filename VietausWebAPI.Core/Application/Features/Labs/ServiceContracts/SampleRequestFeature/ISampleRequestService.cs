using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.SampleRequest;
using VietausWebAPI.Core.Application.Features.Labs.Queries.CreateSampleRequest;
using VietausWebAPI.Core.Application.Features.Sales.DTOs.TransferCustomerDTOs;
using VietausWebAPI.Core.Application.Features.Sales.Querys;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature
{
    public interface ISampleRequestService
    {
        Task<OperationResult> CreateAsync(CreateSampleWithProductRequest req, CancellationToken ct = default);
        Task<PagedResult<SampleRequestSummaryDTO>> GetAllAsync(
         SampleRequestQuery query,
         CancellationToken ct = default);
    }
}
