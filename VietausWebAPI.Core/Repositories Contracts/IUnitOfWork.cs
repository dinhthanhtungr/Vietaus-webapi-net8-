using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
namespace VietausWebAPI.Core.Repositories_Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeesRepository EmployeesCommonRepository { get; }

        //HR
        IEmployeesRepository EmployeesRepository { get; }
        IGroupRepository GroupRepository { get; }
        IMemberInGroupRepository MemberInGroupRepository { get; }
        //Sale
        ICustomerRepository CustomerRepository { get; }
        ITransferCustomerRepository TransferCustomerRepository { get; }
        ICustomerAssignmentRepository CustomerAssignmentRepository { get; }
        ICustomerTransferLogRepository CustomerTransferLogRepository { get; }
        IMerchandiseOrderRepository MerchandiseOrderRepository { get; }
        IMerchandiseOrderLogRepository MerchandiseOrderLogRepository { get; }
        IAttachmentRepository AttachmentRepository { get; }
        // Labs
        IProductStandardRepository ProductStandardRepository { get; }
        IProductInspectionRepository ProductInspectionRepository { get; }
        IProductTestRepository ProductTestRepository { get; }
        IProductRepository ProductRepository { get; }
        ISampleRequestRepository SampleRequestRepository { get; }
        IFormulaRepository FormulaRepository { get; }
        IFormulaMaterialRepository FormulaMaterialRepository { get; }
        //IMfgProductionOrdersPlanRepository MfgProductionOrdersPlanRepository { get; }
        IQCDetailRepository IQCDetailRepository { get; }
        ISampleRequestImageRepository SampleRequestImageRepository { get; }

        //Planning
        IScheduealRepository ScheduealRepository { get; }


        // MfgProductionOrder
        IMfgProductionOrdersPlanRepository IMfgProductionOrdersPlanRepository { get; }

        // Material
        IMaterialRepository MaterialRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IMaterialsSupplierRepository MaterialsSupplierRepository { get; }
        IPriceHistorieRepository PriceHistorieRepository { get; }

        // Manufacturing 
        IMfgProductionOrderRepository MfgProductionOrderRepository { get; }
        IManufacturingFormulaMaterialRepository ManufacturingFormulaMaterialRepository { get; }
        IManufacturingFormulaRepository ManufacturingFormulaRepository { get; }

        IManufacturingFormulaLogRepository ManufacturingFormulaLogRepository { get; }

        // Warehouses
        IWarehouseShelfStockRepository WarehouseShelfStockRepository { get; }
        IWarehouseTempStockRepository WarehouseTempStockRepository  { get; }
        IWarehouseRequestDetailRepository WarehouseRequestDetailRepository { get; }
        IWarehouseRequestRepository WarehouseRequestRepository { get; }

        // Delivery
        IDeliveryOrderRepository DeliveryOrderRepository { get; }
        IDeliveryOrderDetailRepository DeliveryOrderDetailRepository { get; }
        IDelivererRepository DelivererRepository { get; }
        IDelivererInforRepository DelivererInforRepository { get; }
        IDeliveryOrderPORepository DeliveryOrderPORepository { get; }

        // Purchase
        IPurchaseOrderRepository PurchaseOrderRepository { get; }
        IPurchaseOrderDetailRepository PurchaseOrderDetailRepository { get; }
        IPurchaseOrderSnapshotRepository PurchaseOrderSnapshotRepository { get; }

        // Thêm các repository khác nếu cần
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
