using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.GetDtos;
using VietausWebAPI.Core.Application.Features.Manufacturing.DTOs.ColorChipManufacturingRecords.PatchDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts.ColorChipManufacturingRecords
{
    public interface IUpsertColorChipManufacturingRecordService
    {
        Task<OperationResult<GetColorChipManufacturingRecord>> UpsertAsync(
                UpsertColorChipManufacturingRecordRequest request,
                CancellationToken ct = default);
    }
}
