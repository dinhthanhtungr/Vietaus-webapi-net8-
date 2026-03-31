using VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Helpers.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.ServiceContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.Services.SaleReports;
using VietausWebAPI.Infrastructure.Repositories.ReportFeatures.PLPUReports;

namespace VietausWebAPI.WebAPI.DependencyInjections
{
    public static class FinishReportDI
    {
        public static IServiceCollection AddFinishReportModule(this IServiceCollection services)
        {
            // ======================================== Services ========================================

            // PLPU Report
            services.AddScoped<IFinishPLPUReportService, FinishPLPUReportService>();

            // Sale Report
            services.AddScoped<IMerchandiseOrderReportServices, MerchandiseOrderReportService>();

            // ======================================== Repositories ========================================

            // PLPU Report
            services.AddScoped<IFinishPLPUReportRepository, FinishPLPUReportRepository>();
            
            // Sale Report
            services.AddScoped<IMerchandiseOrderReportRepositorys, MerchandiseOrderReportRepository>();

            // ======================================== Helpers ========================================

            // PLPU Report
            services.AddScoped<IExportFinishReportExcel, ExportFinishReportExcelHelper>();


            return services;
        }
    }
}
