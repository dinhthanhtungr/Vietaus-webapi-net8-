using VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.Approvals.Services;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.Service;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.DataUnitOfWork;
using VietausWebAPI.Infrastructure.Repositories;
using VietausWebAPI.Infrastructure.Repositories.Approval;
using VietausWebAPI.Infrastructure.Repositories.SupplyRequest;

namespace VietausWebAPI.WebAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IInventoryReceiptsService, InventoryReceiptsService>();
            services.AddScoped<IInventoryReceiptsRepository, InventoryReceiptsRepository>();
            services.AddScoped<ISupplyRequestsMaterialDatumRepository, SupplyRequestsMaterialDatumRepository>();
            services.AddScoped<ISupplyRequestsMaterialDatumService, SupplyRequestsMaterialDatumService>();
            services.AddScoped<IRequestDetailMaterialRepository, RequestDetailMaterialRepository>();
            services.AddScoped<IRequestDetailMaterialService, RequestDetailMaterialService>();
            services.AddScoped<IApprovalHistoryMaterialRepository, ApprovalHistoryMaterialRepository>();
            services.AddScoped<IApprovalHistoryMaterialService, ApprovalHistoryMaterialService>();
            services.AddScoped<IRequestMaterialRepository, RequestMaterialRepository>();
            services.AddScoped<IRequestMaterialService, RequestMaterialService>();
            services.AddScoped<IMaterialSuppliersRepository, MaterialSuppliersRepository>();
            services.AddScoped<IMaterialSupplierService, MaterialSuppliersService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IApprovalHistoryMaterialRepository, ApprovalHistoryMaterialRepository>();
            services.AddScoped<IApprovalHistoryMaterialService, ApprovalHistoryMaterialService>();
            services.AddScoped<IEmployeesCommonRepository, EmployeesCommonRepository>();
            services.AddScoped<IEmployeesCommonService, EmployeesCommonService>();
            services.AddScoped<IMaterialGroupsService, MaterialGroupsService>();
            services.AddScoped<IMaterialGroupsRepository, MaterialGroupsRepository>();

            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddScoped<IApprovalRepository, ApprovalRepository>();

            services.AddScoped<ISupplyRequestRepository, SupplyRequestRepository>();

            return services;
        }
    }
}
