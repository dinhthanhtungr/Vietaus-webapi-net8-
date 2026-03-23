using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.Queries.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures
{
    public interface IQCInputByQCReadRepository
    {
        // ======================================================================== Get ======================================================================== 
        Task<GetQCInputByQC?> GetDetailByVoucherDetailIdAsync(long voucherDetailId, CancellationToken ct);
        Task<PagedResult<GetSummaryQCInput>> GetPagedSummaryAsync(QCInputQuery query, CancellationToken ct);
        Task<List<GetSummaryQCInput>> GetSummaryRowsForExportAsync(QCInputQuery query, CancellationToken ct);
        Task<QCInputByQC?> GetLatestByVoucherDetailIdAsync(long voucherDetailId, CancellationToken ct);

        Task<List<QCInputByQCExportRow>> GetExportRowsAsync(QCInputQuery query, CancellationToken ct);

    }
}
