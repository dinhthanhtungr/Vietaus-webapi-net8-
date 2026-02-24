using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts.QCInputByQCFeatures
{
    public interface IQCInputByQCService
    {
        Task<OperationResult<PagedResult<GetSummaryQCInput>>> GetPagedSummaryAsync(QCInputQuery query, CancellationToken ct);

        Task<OperationResult<GetQCInputByQC?>> GetByVoucherDetailIdAsync(long voucherDetailId, CancellationToken ct);

        Task<OperationResult<GetQCInputByQC>> CreateAsync(PostQCInputByQC input, CancellationToken ct);

        Task<OperationResult<GetQCInputByQC>> PatchByVoucherDetailIdAsync(
            PatchQCInputByQC input, long voucherDetailId, CancellationToken ct);
    }

}
