using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts
{
    public interface IProductStandardService
    {

        // ======================================================================== Get ======================================================================== 
        Task<OperationResult<GetProductStandard>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<OperationResult<GetProductStandard>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken);
        Task<OperationResult<PagedResult<ProductStandardSummaryDTO>>> GetPagedListAsync(ProductStandardQuery query, CancellationToken cancellationToken);

        // ======================================================================== Post ========================================================================
        Task<OperationResult> AddAsync(PostProductStandard productStandard, CancellationToken cancellationToken);

        // ======================================================================== Patch ========================================================================    
        Task<OperationResult> UpdateAsync(PatchProductStandard productStandard, CancellationToken cancellationToken);
        Task<OperationResult> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);        
    }
}
