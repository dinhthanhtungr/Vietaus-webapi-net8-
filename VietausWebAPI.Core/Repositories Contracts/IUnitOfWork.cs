using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.Services;
using VietausWebAPI.Core.Application.Usecases.Approvals.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.InventoryReceipts.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.MaterialRequestDetail.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.PurchaseOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.Suppliers.RepositoriesContracts;
using VietausWebAPI.Core.Application.Usecases.SupplyRequests.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.ServiceContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRequestMaterialRepository RequestMaterialRepository { get; }
        IApprovalHistoryMaterialRepository ApprovalHistoryMaterialRepository { get; }
        ISupplyRequestsMaterialDatumRepository SupplyRequestsMaterialDatumRepository { get; }
        IEmployeesRepository EmployeesCommonRepository { get; }

        IApprovalRepository ApprovalRepository { get; }
        ISupplyRequestRepository SupplyRequestRepository { get; }
        IMaterialsRepository MaterialsRepository { get; }
        IMaterialRequestDetailRepository MaterialRequestDetailRepository { get; }
        IInventoryReceiptRepository InventoryReceiptRepository { get; }
        IPurchaseOrderDetailsRepository PurchaseOrderDetailsRepository { get; }
        IPurchaseOrdersRepository PurchaseOrdersRepository { get; }
        ISupplierRepository SupplierRepository { get; }

        //HR
        IEmployeesRepository EmployeesRepository { get; }
        IGroupRepository GroupRepository { get; }
        IMemberInGroupRepository MemberInGroupRepository { get; }
        //Sale
        ICustomerRepository CustomerRepository { get; }
        ITransferCustomerRepository TransferCustomerRepository { get; }
        ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        ICustomerTransferLogRepository CustomerTransferLogRepository { get; }
        // Labs
        IProductStandardRepository ProductStandardRepository { get; }
        IProductInspectionRepository ProductInspectionRepository { get; }
        IProductTestRepository ProductTestRepository { get; }
        IProductRepository ProductRepository { get; }
        ISampleRequestRepository SampleRequestRepository { get; }
        //IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }
        IQCDetailRepository IQCDetailRepository { get; }

        //Planning
        IScheduealRepository ScheduealRepository { get; }


        // MfgProductionOrder
        IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }


        // Thêm các repository khác nếu cần
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
