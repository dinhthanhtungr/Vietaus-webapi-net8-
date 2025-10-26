using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.Repositories.Sales;
using VietausWebAPI.WebAPI.DatabaseContext;


namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Khai báo các repository
        /// </summary>
        public IEmployeesRepository EmployeesCommonRepository { get; }
        // HR
        public IEmployeesRepository EmployeesRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IMemberInGroupRepository MemberInGroupRepository { get; }
        
        // Sale
        public ICustomerRepository CustomerRepository { get; }
        public ITransferCustomerRepository TransferCustomerRepository { get; }
        public ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        public ICustomerTransferLogRepository CustomerTransferLogRepository { get; }
        public IMerchandiseOrderRepository MerchandiseOrderRepository { get; }
        public IMerchandiseOrderLogRepository MerchandiseOrderLogRepository { get; }
        public IAttachmentRepository AttachmentRepository { get; }
        // Labs
        public IProductStandardRepository ProductStandardRepository { get; }
        public IProductInspectionRepository ProductInspectionRepository { get; }
        public IProductTestRepository ProductTestRepository { get; }
        public IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }
        public IQCDetailRepository IQCDetailRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ISampleRequestRepository SampleRequestRepository { get; }
        public ISampleRequestImageRepository SampleRequestImageRepository { get; }
        public IFormulaRepository FormulaRepository { get; }
        public IFormulaMaterialRepository FormulaMaterialRepository { get; }
        // Planning
        public IScheduealRepository ScheduealRepository { get; }

        //MfgProductOrder
        public IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }

        //Material
        public IMaterialRepository MaterialRepository { get; }
        public ISupplierRepository SupplierRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IMaterialsSupplierRepository MaterialsSupplierRepository { get; }
        public IPriceHistorieRepository PriceHistorieRepository { get; }

        // Manufacturing
        public IMfgProductionOrderRepository MfgProductionOrderRepository { get; }
        public IManufacturingFormulaMaterialRepository ManufacturingFormulaMaterialRepository { get; }
        public IManufacturingFormulaLogRepository ManufacturingFormulaLogRepository { get; }
        public IManufacturingFormulaRepository ManufacturingFormulaRepository { get; }

        // Warehouses
        public IWarehouseShelfStockRepository WarehouseShelfStockRepository { get; }
        public IWarehouseTempStockRepository WarehouseTempStockRepository { get; }
        public IWarehouseRequestDetailRepository WarehouseRequestDetailRepository { get; }
        public IWarehouseRequestRepository WarehouseRequestRepository { get; }

        // Delivery
        public IDeliveryOrderRepository DeliveryOrderRepository { get; }
        public IDelivererRepository DelivererRepository { get; }
        public IDeliveryOrderDetailRepository DeliveryOrderDetailRepository { get; }
        public IDelivererInforRepository DelivererInforRepository { get; }
        public IDeliveryOrderPORepository DeliveryOrderPORepository { get; }

        // Purchase

        public IPurchaseOrderRepository PurchaseOrderRepository { get; }
        public IPurchaseOrderDetailRepository PurchaseOrderDetailRepository { get; }
        public IPurchaseOrderSnapshotRepository PurchaseOrderSnapshotRepository { get; }

        /// <summary>
        /// Khởi tạo UnitOfWork
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestMaterialRepository"></param>
        /// <param name="approvalHistoryMaterialRepository"></param>
        /// <param name="supplyRequestsMaterialDatumRepository"></param>
        /// <param name="inventoryReceiptsRepository"></param>
        /// <param name="employeesCommonRepository"></param>
        public UnitOfWork(ApplicationDbContext context
            , IEmployeesRepository employeesCommonRepository
            , IProductStandardRepository productStandardRepository
            , IProductInspectionRepository productInspectionRepository
            , IProductTestRepository productTestRepository
            , IMfgProductionOrdersPlanRepository mfgProductionOrdersPlanRepository
            , IQCDetailRepository iQCDetailRepository
            , IScheduealRepository scheduealRepository
            , IMfgProductionOrdersPlanRepository iMfgProductionOrdersPlanRepository
            , IEmployeesRepository employeesRepository
            , IGroupRepository groupRepository
            , ICustomerRepository customerRepository
            , ITransferCustomerRepository transferCustomerRepository
            , ICustomerAssignmentRepository customerAssignmentRepository
            , ICustomerTransferLogRepository customerTransferLogRepository
            , IMemberInGroupRepository memberInGroupRepository
            , ISampleRequestRepository sampleRequestRepository
            , IProductRepository productRepository
            , ISampleRequestImageRepository sampleRequestImageRepository
            , IMaterialRepository materialRepository
            , ISupplierRepository supplierRepository
            , ICategoryRepository categoryRepository
            , IMaterialsSupplierRepository materialsSupplierRepository
            , IPriceHistorieRepository priceHistorieRepository
            , IFormulaRepository formulaRepository
            , IFormulaMaterialRepository formulaMaterialRepository
            , IMerchandiseOrderRepository merchandiseOrderRepository
            , IMerchandiseOrderLogRepository merchandiseOrderLogRepository
            , IAttachmentRepository attachmentRepository
            , IMfgProductionOrderRepository mfgProductionOrderRepository
            , IManufacturingFormulaMaterialRepository manufacturingFormulaMaterialRepository
            , IManufacturingFormulaRepository manufacturingFormulaRepository
            , IManufacturingFormulaLogRepository manufacturingFormulaLogRepository
            , IWarehouseShelfStockRepository warehouseShelfStockRepository
            , IWarehouseTempStockRepository warehouseTempStockRepository
            , IWarehouseRequestDetailRepository warehouseRequestDetailRepository
            , IWarehouseRequestRepository warehouseRequestRepository
            , IDeliveryOrderRepository deliveryOrderRepository
            , IDelivererRepository delivererRepository
            , IDeliveryOrderDetailRepository deliveryOrderDetailRepository
            , IDelivererInforRepository delivererInforRepository
            , IDeliveryOrderPORepository deliveryOrderPORepository
            , IPurchaseOrderRepository purchaseOrderRepository
            , IPurchaseOrderDetailRepository purchaseOrderDetailRepository
            , IPurchaseOrderSnapshotRepository purchaseOrderSnapshotRepository

            )

        {
            _context = context;

            // Labs
            ProductStandardRepository = productStandardRepository;
            ProductInspectionRepository = productInspectionRepository;
            ProductTestRepository = productTestRepository;
            MfgProductionOrdersPlanRepository = mfgProductionOrdersPlanRepository;
            IQCDetailRepository = iQCDetailRepository;
            ScheduealRepository = scheduealRepository;
            IMfgProductionOrdersPlanRepository = iMfgProductionOrdersPlanRepository;
            EmployeesRepository = employeesRepository;
            GroupRepository = groupRepository;
            CustomerRepository = customerRepository;
            TransferCustomerRepository = transferCustomerRepository;
            CustomerAssignmentRepository = customerAssignmentRepository;
            CustomerTransferLogRepository = customerTransferLogRepository;
            MemberInGroupRepository = memberInGroupRepository;
            SampleRequestRepository = sampleRequestRepository;
            ProductRepository = productRepository;
            SampleRequestImageRepository = sampleRequestImageRepository;
            MaterialRepository = materialRepository;
            SupplierRepository = supplierRepository;
            CategoryRepository = categoryRepository;
            MaterialsSupplierRepository = materialsSupplierRepository;
            PriceHistorieRepository = priceHistorieRepository;
            FormulaRepository = formulaRepository;
            FormulaMaterialRepository = formulaMaterialRepository;
            MerchandiseOrderRepository = merchandiseOrderRepository;
            MerchandiseOrderLogRepository = merchandiseOrderLogRepository;
            AttachmentRepository = attachmentRepository;

            MfgProductionOrderRepository = mfgProductionOrderRepository;
            ManufacturingFormulaMaterialRepository = manufacturingFormulaMaterialRepository;
            ManufacturingFormulaRepository = manufacturingFormulaRepository;
            ManufacturingFormulaLogRepository = manufacturingFormulaLogRepository;

            WarehouseShelfStockRepository = warehouseShelfStockRepository;
            WarehouseTempStockRepository = warehouseTempStockRepository;
            WarehouseRequestDetailRepository = warehouseRequestDetailRepository;
            WarehouseRequestRepository = warehouseRequestRepository;

            DeliveryOrderRepository = deliveryOrderRepository;
            DelivererInforRepository = delivererInforRepository;
            DelivererRepository = delivererRepository;
            DeliveryOrderDetailRepository = deliveryOrderDetailRepository;
            DeliveryOrderPORepository = deliveryOrderPORepository;

            PurchaseOrderRepository = purchaseOrderRepository;
            PurchaseOrderDetailRepository = purchaseOrderDetailRepository;
            PurchaseOrderSnapshotRepository = purchaseOrderSnapshotRepository;
        }
        /// <summary>
        /// Bắt đầu một transaction
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        /// <summary>
        /// Commit transaction
        /// </summary>
        /// <returns></returns>
        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }
        /// <summary>
        /// Rollback transaction
        /// </summary>
        /// <returns></returns>
        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
        /// <summary>
        /// Lưu thay đổi vào database
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Hủy bỏ context
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
