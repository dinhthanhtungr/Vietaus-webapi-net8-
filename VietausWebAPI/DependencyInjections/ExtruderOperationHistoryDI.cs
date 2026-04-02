using VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services.SaleReports;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.EndOfShiftReportFeatures;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.ExtruderOperationHistoryRepositories;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.ServiceContracts.ExtruderOperationHistoryFeatures.GetServices;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.Services.ExtruderOperationHistoryFeatures.GetServices;
using VietausWebAPI.Infrastructure.Repositories.ReportFeatures.PLPUReports;
using VietausWebAPI.Infrastructure.Repositories.ShiftReportFeatures.EndOfShiftReportRepositories;
using VietausWebAPI.Infrastructure.Repositories.ShiftReportFeatures.ExtruderOperationHistoryRepositories;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class ExtruderOperationHistoryDI
    {
        public static IServiceCollection AddExtruderOperationHistoryModule(this IServiceCollection services)
        {
            // ======================================== Services ========================================

            services.AddScoped<IExtruderOperationHistoryService, ExtruderOperationHistoryService>();

            // ======================================== Repositories ========================================

            services.AddScoped<IExtruderOperationHistoryReadRepositories, ExtruderOperationHistoryReadRepositories>();
            services.AddScoped<IEndOfShiftReportReadRepositories, EndOfShiftReportReadRepositories>();
            // ======================================== Helpers ========================================


            return services;
        }
    }
}
