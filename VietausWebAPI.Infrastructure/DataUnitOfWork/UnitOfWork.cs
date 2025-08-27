using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Repositories_Contracts;
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

        // Labs
        public IProductStandardRepository ProductStandardRepository { get; }
        public IProductInspectionRepository ProductInspectionRepository { get; }
        public IProductTestRepository ProductTestRepository { get; }
        public IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }
        public IQCDetailRepository IQCDetailRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ISampleRequestRepository SampleRequestRepository { get; }
        public ISampleRequestImageRepository SampleRequestImageRepository { get; }
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
            , ISampleRequestImageRepository sampleRequestImageRepository)
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
