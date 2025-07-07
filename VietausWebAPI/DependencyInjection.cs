using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Labs.Services;
using VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Approvals.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.Approvals.Services;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.Services;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.Services;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.Services;
using VietausWebAPI.Core.Application.Usecases.Suppliers.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Suppliers.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.Suppliers.Services;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.Services;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.Service;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.DataUnitOfWork;
using VietausWebAPI.Infrastructure.Repositories;
using VietausWebAPI.Infrastructure.Repositories.Approval;
using VietausWebAPI.Infrastructure.Repositories.InventoryReceipts;
using VietausWebAPI.Infrastructure.Repositories.Labs;
using VietausWebAPI.Infrastructure.Repositories.MaterialRequestDetail;
using VietausWebAPI.Infrastructure.Repositories.PurchaseOrders;
using VietausWebAPI.Infrastructure.Repositories.Supplier;
using VietausWebAPI.Infrastructure.Repositories.SupplyRequest;

namespace VietausWebAPI.WebAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
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

            // Approval
            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddScoped<IApprovalRepository, ApprovalRepository>();
            services.AddScoped<IGetApprovalRequestAndInventoryService, GetApprovalRequestAndInventoryService>();

            // SupplyRequest
            services.AddScoped<ISupplyRequestRepository, SupplyRequestRepository>();
            services.AddScoped<ISupplyRequestService, SupplyRequestService>();

            // MaterialRequestDetail
            services.AddScoped<IMaterialRequestDetailRepository, MaterialRequestDetailRepository>();
            services.AddScoped<IMaterialRequestDetailService, MaterialRequestDetailService>();

            // MaterialRequest
            services.AddScoped<IMaterialsService, MaterialsService>();
            services.AddScoped<IMaterialsRepository, MaterialsRepository>();

            // InventoryReceipt
            services.AddScoped<IInventoryReceiptRepository, InventoryReceiptRepository>();
            //services.AddScoped<IInventoryReceiptService, InventoryReceiptService>();

            // PurchaseOrder
            services.AddScoped<IPurchaseOrdersRepository, PurchaseOrdersRepository>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IPurchaseOrderDetailsRepository, PurchaseOrderDetailsRepository>();
            services.AddScoped<IPurchaseOrderDetailsService, PurchaseOrderDetailsService>();

            // Supplier
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();

            //Materials
            //services.AddScoped<IMaterialsRepository, MaterialsRepository>();
            //services.AddScoped<IMaterialsService, MaterialsService>();

            //Inventory
            services.AddScoped<IInventoryReceiptService, InventoryReceiptService>();
            services.AddScoped<IInventoryReceiptRepository, InventoryReceiptRepository>();

            //Labs
            //ProductStandard
            services.AddScoped<IProductStandardRepository, ProductStandardRepository>();
            services.AddScoped<IProductStandardService, ProductStandardService>();
            //ProductInspection
            services.AddScoped<IProductInspectionRepository, ProductInspectionRepository>();
            services.AddScoped<IProductInspectionService, ProductInspectionService>();
            //ProductTest
            services.AddScoped<IProductTestRepository, ProductTestRepository>();
            services.AddScoped<IProductTestService, ProductTestService>();
            //MfgProductionOrdersPlanRepository
            services.AddScoped<IMfgProductionOrdersPlanRepository, MfgProductionOrdersPlanRepository>();


            return services;
        }
    }
}
