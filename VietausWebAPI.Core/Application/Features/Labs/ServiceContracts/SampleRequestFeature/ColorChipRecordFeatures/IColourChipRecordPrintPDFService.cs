using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.DTOs.SampleRequestFeature.ColorChipRecordFeatures.PDFDtos;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature.ColorChipRecordFeatures
{
    public interface IColourChipRecordPrintPDFService
    {
        Task<OperationResult<byte[]>> PrintByProductIdAsync(
            Guid productId,
            CancellationToken cancellationToken = default);
    }
}
