using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.ServiceContracts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Suppliers.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRequestMaterialRepository RequestMaterialRepository { get; }
        IApprovalHistoryMaterialRepository ApprovalHistoryMaterialRepository { get; }
        ISupplyRequestsMaterialDatumRepository SupplyRequestsMaterialDatumRepository { get; }
        IEmployeesCommonRepository EmployeesCommonRepository { get; }

        IApprovalRepository ApprovalRepository { get; }
        ISupplyRequestRepository SupplyRequestRepository { get; }
        IMaterialsRepository MaterialsRepository { get; }
        IMaterialRequestDetailRepository MaterialRequestDetailRepository { get; }
        IInventoryReceiptRepository InventoryReceiptRepository { get; }
        IPurchaseOrderDetailsRepository PurchaseOrderDetailsRepository { get; }
        IPurchaseOrdersRepository PurchaseOrdersRepository { get; }
        ISupplierRepository SupplierRepository { get; }

        // Labs
        IProductStandardRepository ProductStandardRepository { get; }
        IProductInspectionRepository ProductInspectionRepository { get; }
        IProductTestRepository ProductTestRepository { get; }
        IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }


        // Thêm các repository khác nếu cần
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
