using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.SaleReports;
using VietausWebAPI.Core.Application.Shared.Models.PageModels;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.SaleReports
{
    public interface IMerchandiseOrderReportServices
    {
        Task<PagedResult<SummaryMOReportDto>> GetSummaryMOReportAsync(
            MerchandiseOrderReportQuery query,
            CancellationToken cancellationToken = default);

        Task<byte[]> ExportSummaryMOReportExcelAsync(
            MerchandiseOrderReportQuery query,
            CancellationToken ct = default);
    }
}
