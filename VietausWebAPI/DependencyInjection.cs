using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.Service;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.DataUnitOfWork;
using VietausWebAPI.Infrastructure.Repositories;

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

            return services;
        }
    }
}
