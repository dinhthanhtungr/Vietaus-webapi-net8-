using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.DTOs.Material;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Querys.Material;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts
{
    public interface IMaterialService
    {
        Task<PagedResult<GetMaterialSummary>> GetAllAsync(MaterialQuery query, CancellationToken ct = default);
        Task<OperationResult> AddNewMaterialAsync(PostMaterial material, CancellationToken ct = default);
    }
}
