using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRequestMaterialRepository RequestMaterialRepository { get; }
        IApprovalHistoryMaterialRepository ApprovalHistoryMaterialRepository { get; }
        ISupplyRequestsMaterialDatumRepository SupplyRequestsMaterialDatumRepository { get; }
        IInventoryReceiptsRepository InventoryReceiptsRepository { get; }
        IEmployeesCommonRepository EmployeesCommonRepository { get; }
        // Thêm các repository khác nếu cần
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
