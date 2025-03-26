using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRequestMaterialRepository RequestMaterialRepository { get; }
        public IApprovalHistoryMaterialRepository ApprovalHistoryMaterialRepository { get; }
        public ISupplyRequestsMaterialDatumRepository SupplyRequestsMaterialDatumRepository { get; }

        public IInventoryReceiptsRepository InventoryReceiptsRepository { get; }


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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
