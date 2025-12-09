using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Shared.DTO.Visibility;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;

namespace VietausWebAPI.Core.Application.Features.Shared.ServiceContracts
{
    public interface IVisibilityHelper
    {
        Task<ViewerScope> BuildViewerScopeAsync(CancellationToken ct = default);
        IQueryable<Customer> ApplyCustomer(IQueryable<Customer> q, ViewerScope v);
        IQueryable<SampleRequest> ApplySampleRequest(IQueryable<SampleRequest> q, ViewerScope v);
    }
}
