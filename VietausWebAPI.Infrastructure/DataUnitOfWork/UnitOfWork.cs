using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;
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
        public IInventoryReceiptsRepository InventoryReceiptsRepository { get; }

        /// <summary>
        /// Khởi tạo UnitOfWork
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requestMaterialRepository"></param>
        /// <param name="approvalHistoryMaterialRepository"></param>
        /// <param name="supplyRequestsMaterialDatumRepository"></param>
        /// <param name="inventoryReceiptsRepository"></param>
        public UnitOfWork(ApplicationDbContext context 
            , IRequestMaterialRepository requestMaterialRepository
            , IApprovalHistoryMaterialRepository approvalHistoryMaterialRepository
            , ISupplyRequestsMaterialDatumRepository supplyRequestsMaterialDatumRepository
            , IInventoryReceiptsRepository inventoryReceiptsRepository)
        {
            _context = context;
            RequestMaterialRepository = requestMaterialRepository;
            ApprovalHistoryMaterialRepository = approvalHistoryMaterialRepository;
            SupplyRequestsMaterialDatumRepository = supplyRequestsMaterialDatumRepository;
            InventoryReceiptsRepository = inventoryReceiptsRepository; ;
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
