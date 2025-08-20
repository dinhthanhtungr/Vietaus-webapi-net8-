using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.ServiceContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Suppliers.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Infrastructure.Repositories.Sales;
using VietausWebAPI.Infrastructure.Repositories.Share.SampleRequestFeature;
using VietausWebAPI.WebAPI.DatabaseContext;


namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        /// <summary>
        /// Khai báo các repository
        /// </summary>
        public IRequestMaterialRepository RequestMaterialRepository { get; }
        public IApprovalHistoryMaterialRepository ApprovalHistoryMaterialRepository { get; }
        public ISupplyRequestsMaterialDatumRepository SupplyRequestsMaterialDatumRepository { get; }
        public IEmployeesRepository EmployeesCommonRepository { get; }

        public IApprovalRepository ApprovalRepository { get; }
        public ISupplyRequestRepository SupplyRequestRepository { get; }
        public IMaterialsRepository MaterialsRepository { get; }
        public IMaterialRequestDetailRepository MaterialRequestDetailRepository { get; }
        public IInventoryReceiptRepository InventoryReceiptRepository { get; }
        public IPurchaseOrderDetailsRepository PurchaseOrderDetailsRepository { get; }
        public IPurchaseOrdersRepository PurchaseOrdersRepository { get; }
        public ISupplierRepository SupplierRepository { get; }

        // HR
        public IEmployeesRepository EmployeesRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IMemberInGroupRepository MemberInGroupRepository { get; }
        
        // Sale
        public ICustomerRepository CustomerRepository { get; }
        public ITransferCustomerRepository TransferCustomerRepository { get; }
        public ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        public ICustomerTransferLogRepository CustomerTransferLogRepository { get; }

        // Labs
        public IProductStandardRepository ProductStandardRepository { get; }
        public IProductInspectionRepository ProductInspectionRepository { get; }
        public IProductTestRepository ProductTestRepository { get; }
        public IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }
        public IQCDetailRepository IQCDetailRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ISampleRequestRepository SampleRequestRepository { get; }
        // Planning
        public IScheduealRepository ScheduealRepository { get; }

        //MfgProductOrder
        public IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }

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
            , IRequestMaterialRepository requestMaterialRepository
            , IApprovalHistoryMaterialRepository approvalHistoryMaterialRepository
            , ISupplyRequestsMaterialDatumRepository supplyRequestsMaterialDatumRepository
            , IEmployeesRepository employeesCommonRepository
            , IApprovalRepository approvalRepository
            , ISupplyRequestRepository supplyRequestRepository
            , IMaterialsRepository materialsRepository
            , IMaterialRequestDetailRepository materialRequestDetailRepository
            , IInventoryReceiptRepository inventoryReceiptRepository
            , IPurchaseOrdersRepository purchaseOrdersRepository
            , IPurchaseOrderDetailsRepository purchaseOrderDetailsRepository
            , ISupplierRepository supplierRepository
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
            , IProductRepository productRepository)
        {
            _context = context;
            RequestMaterialRepository = requestMaterialRepository;
            ApprovalHistoryMaterialRepository = approvalHistoryMaterialRepository;
            SupplyRequestsMaterialDatumRepository = supplyRequestsMaterialDatumRepository;
            EmployeesCommonRepository = employeesCommonRepository;
            ApprovalRepository = approvalRepository;
            SupplyRequestRepository = supplyRequestRepository;
            MaterialsRepository = materialsRepository;
            MaterialRequestDetailRepository = materialRequestDetailRepository;
            InventoryReceiptRepository = inventoryReceiptRepository;
            PurchaseOrdersRepository = purchaseOrdersRepository;
            PurchaseOrderDetailsRepository = purchaseOrderDetailsRepository;
            SupplierRepository = supplierRepository;

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
