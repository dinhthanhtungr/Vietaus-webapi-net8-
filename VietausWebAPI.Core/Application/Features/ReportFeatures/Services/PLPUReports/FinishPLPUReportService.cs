using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.ReportFeatures.DTOs.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Queries.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;

namespace VietausWebAPI.Core.Application.Features.ReportFeatures.Services.PLPUReports
{
    public class FinishPLPUReportService : IFinishPLPUReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FinishPLPUReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FinishRow>> GetFinishPLPUReportsAsync(DateTime from, DateTime to, CancellationToken ct = default)
        {
            try
            {
                var result = await _unitOfWork.FinishPLPUReportRepository.GetFinishReportAsync(from, to, ct);
                return result;
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, or return a default value)
                throw;  
            }
        }
    }
}
