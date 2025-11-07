using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Attachments.Services;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Helpers;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.ServiceContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.Services;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.ServiceContracts;
using VietausWebAPI.Core.Application.Features.HR.Services;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.Services.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.ProductFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.QAQCFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.Services;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.Services;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Planning.Services;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Helpers;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.ServiceContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.Services;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.ServiceContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.ServiceContracts;
using VietausWebAPI.Core.Application.Features.TimelineFeature.Services;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.ServiceContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.Services;
using VietausWebAPI.Core.Application.Shared.Helper.IdCounter;
//using VietausWebAPI.Core.Application.Shared.Helper.ImageStorage;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.DataUnitOfWork;
using VietausWebAPI.Infrastructure.Helpers.IdCounter;
using VietausWebAPI.Infrastructure.Repositories.Attachments;
using VietausWebAPI.Infrastructure.Repositories.Audits;
using VietausWebAPI.Infrastructure.Repositories.DeliveryOrders;
using VietausWebAPI.Infrastructure.Repositories.HR;
using VietausWebAPI.Infrastructure.Repositories.Labs;
using VietausWebAPI.Infrastructure.Repositories.Labs.FormulaFeatures;
using VietausWebAPI.Infrastructure.Repositories.Labs.QAQC;
using VietausWebAPI.Infrastructure.Repositories.Labs.QAQCFeature;
using VietausWebAPI.Infrastructure.Repositories.Manufacturing;
using VietausWebAPI.Infrastructure.Repositories.Materials;
using VietausWebAPI.Infrastructure.Repositories.Planning.Schedueal;
using VietausWebAPI.Infrastructure.Repositories.Purchases;
using VietausWebAPI.Infrastructure.Repositories.Sales;
using VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature;
using VietausWebAPI.Infrastructure.Repositories.Warehouses;

namespace VietausWebAPI.WebAPI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Attachment
            services.AddScoped<IAttachmentCollectionRepository, AttachmentCollectionRepository>();
            services.AddScoped<IAttachmentModelRepository, AttachmentModelRepository>();
            services.AddScoped<IAttachmentSchemaService, AttachmentSchemaService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IExternalIdService, ExternalIdServicePostgres>();
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            // HR
            services.AddScoped<IEmployeesRepository, EmployeesRepository>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMemberInGroupRepository, MemberInGroupRepository>();

            // Sale
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ITransferCustomerService, TransferCustomerService>();
            services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();
            services.AddScoped<ICustomerTransferLogRepository, CustomerTransferLogRepository>();
            services.AddScoped<ICustomerAssignmentRepository, CustomerAssignmentRepository>();
            services.AddScoped<IMerchandiseOrderRepository, MerchandiseOrderRepository>();
            //services.AddScoped<IMerchandiseOrderLogRepository, MerchandiseOrderLogRepository>();
            services.AddScoped<IMerchandiseOrderService, MerchandiseOrderService>();
            //services.AddScoped<IAttachmentService, AttachmentService>();
            //services.AddScoped<IAttachmentRepository, AttachmentRepository>();


            //services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();
            //services.AddScoped<ITransferCustomerRepository, TransferCustomerRepository>();

            //Materials
            //services.AddScoped<IMaterialsRepository, MaterialsRepository>();
            //services.AddScoped<IMaterialsService, MaterialsService>();

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

            //SampleRequest
            services.AddScoped<ISampleRequestService, SampleRequestService>();
            services.AddScoped<ISampleRequestRepository, SampleRequestRepository>();

            //SampleRequestImage
            //services.AddScoped<ISampleRequestImageService, SampleRequestImageService>();
            //services.AddScoped<ISampleRequestImageRepository, SampleRequestImageRepository>();
            //services.AddScoped<IFileStorage, LocalFileStorage>();    

            //Product
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            //QC 
            services.AddScoped<IQCDetailRepository, QCDetailRepository>();
            services.AddScoped<IQCOutputService, QCOutputService>();

            //Formula
            services.AddScoped<IFormulaRepository, FormulaRepository>();
            services.AddScoped<IFormulaService, FormulaService>();
            services.AddScoped<IFormulaMaterialRepository, FormulaMaterialRepository>();

            //Planning
            //Schedual
            services.AddScoped<IScheduealService, ScheduealService>();
            services.AddScoped<IScheduealRepository, ScheduealRepository>();


            //Material
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IMaterialRepository, MaterialsRepository>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMaterialsSupplierRepository, MaterialsSupplierRepository>();
            services.AddScoped<IPriceHistorieRepository, PriceHistorieRepository>();

            // Manufacturing
            services.AddScoped<IMfgProductionOrderRepository, MfgProductionOrderRepository>();
            services.AddScoped<IMfgProductionOrderService, MfgProductionOrderService>();
            services.AddScoped<IManufacturingFormulaRepository, ManufacturingFormulaRepository>();
            services.AddScoped<IManufacturingFormulaMaterialRepository, ManufacturingFormulaMaterialRepository>();
            services.AddScoped<IMfgFormulaService, MfgFormulaService>();
            //services.AddScoped<IManufacturingFormulaLogRepository, ManufacturingFormulaLogRepository>();

            //services.AddScoped<IMaterialService, MaterialService>

            // Warehouses
            services.AddScoped<IWarehouseShelfStockRepository, WarehouseShelfStockRepository>();
            services.AddScoped<IWarehouseTempStockRepository, WarehouseTempStockRepository>();
            services.AddScoped<IWarehouseRequestDetailRepository, WarehouseRequestDetailRepository>();
            services.AddScoped<IWarehouseRequestRepository, WarehouseRequestRepository>();

            services.AddScoped<IWarehouseReadService, WarehouseReadService>();
            services.AddScoped<IWarehouseReservationService, WarehouseReservationService>();
            services.AddScoped<IWarehouseSnapshotService, WarehouseSnapshotService>();
            // DeliveryOrder
            services.AddScoped<IDeliveryOrderDetailRepository, DeliveryOrderDetailRepository>();
            services.AddScoped<IDelivererInforRepository, DelivererInforRepository>();
            services.AddScoped<IDeliveryOrderRepository, DeliveryOrderRepository>();
            services.AddScoped<IDelivererRepository, DelivererRepository>();
            services.AddScoped<IDeliveryOrderPORepository, DeliveryOrderPORepository>();
            
            services.AddScoped<IDeliveryOrderService, DeliveryOrderService>();
            services.AddScoped<IDeliveryOrderPdfService, DeliveryOrderPdfService>();
            services.AddScoped<IDeliveryOrderPdfRenderHelper, DeliveryOrderPdfRenderHelper>();

            // PurchaseOrder
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderDetailRepository, PurchaseOrderDetailRepository>();
            services.AddScoped<IPurchaseOrderSnapshotRepository, PurchaseOrderSnapshotRepository>();

            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IPurchaseOrderPdfService, PurchaseOrderPdfService>();
            services.AddScoped<IPurchaseOrderPdfRenderHelper, PurchaseOrderPdfRenderHelper>();


            // Audit
            services.AddScoped<IEventLogRepository, EventLogRepository>();

            services.AddScoped<ITimelineService, TimelineService>();



            return services;
        }
    }
}
