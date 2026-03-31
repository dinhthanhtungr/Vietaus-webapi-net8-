using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PostDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures
{
    public interface IColorChipRecordWriteServices
    {
        Task<OperationResult> CreateAsync(CreateColorChipRecordRequest request, CancellationToken cancellationToken = default);
    }
}
