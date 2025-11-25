using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Repositories_Contracts;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;
using VietausWebAPI.Infrastructure.Repositories.Labs;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    /// <summary>
    /// File CORE: giữ DbContext, constructor duy nhất, methods Tx/Save/Dispose.
    /// Tất cả Properties được tách sang các file partial theo module.
    /// </summary>
    public sealed partial class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(
            ApplicationDbContext context,

            // ==== Attachments ====
            IAttachmentCollectionRepository attachmentCollectionRepository,
            IAttachmentModelRepository attachmentModelRepository,

            // ==== Company/Common ====
            ICompanyRepository companyRepository,
            IEmployeesRepository employeesCommonRepository,

            // ==== HR ====
            IApplicationUserRepository applicationUserRepository,
            IEmployeesRepository employeesRepository,
            IGroupRepository groupRepository,
            IMemberInGroupRepository memberInGroupRepository,

            // ==== Sales ====
            ICustomerRepository customerRepository,
            ITransferCustomerRepository transferCustomerRepository,
            ICustomerAssignmentRepository customerAssignmentRepository,
            ICustomerTransferLogRepository customerTransferLogRepository,
            IMerchandiseOrderRepository merchandiseOrderRepository,

            // ==== Labs (QA/QC, Formula, Sample) ====
            //IProductStandardRepository productStandardRepository,
            //IProductInspectionRepository productInspectionRepository,
            //IProductTestRepository productTestRepository,
            IManufacturingFormulaVersionRepository manufacturingFormulaVersionRepository,
            IFormulaRepository formulaRepository,
            IFormulaMaterialRepository formulaMaterialRepository,
            ISampleRequestRepository sampleRequestRepository,

            // ==== Planning ====
            IScheduealRepository scheduealRepository,
            //IMfgProductionOrdersPlanRepository mfgProductionOrdersPlanRepository,
            //IMfgProductionOrdersPlanRepository iMfgProductionOrdersPlanRepository, // bạn đang dùng 2 biến cho cùng 1 interface

            // ==== QAQC Detail (thuộc Labs) ====
            //IQCDetailRepository iQCDetailRepository,

            // ==== Product (bạn đang để trong Manufacturing namespace) ====
            IProductRepository productRepository,

            // ==== Material ====
            IMaterialRepository materialRepository,
            ISupplierRepository supplierRepository,
            ICategoryRepository categoryRepository,
            IMaterialsSupplierRepository materialsSupplierRepository,
            IPriceHistorieRepository priceHistorieRepository,

            // ==== Manufacturing ====
            IMfgProductionOrderRepository mfgProductionOrderRepository,
            IManufacturingFormulaMaterialRepository manufacturingFormulaMaterialRepository,
            IManufacturingFormulaRepository manufacturingFormulaRepository,
            IProductStandardFormulaRepository productStandardFormulaRepository,
            IProductionSelectVersionRepository productionSelectVersionRepository,
            IMfgOrderPORepository mfgOrderPORepository,
            ISchedualMfgRepository schedualMfgRepository,

            // ==== Warehouse ====
            IWarehouseShelfStockRepository warehouseShelfStockRepository,
            IWarehouseTempStockRepository warehouseTempStockRepository,
            IWarehouseRequestDetailRepository warehouseRequestDetailRepository,
            IWarehouseRequestRepository warehouseRequestRepository,

            // ==== Delivery ====
            IDeliveryOrderRepository deliveryOrderRepository,
            IDeliveryOrderDetailRepository deliveryOrderDetailRepository,
            IDelivererRepository delivererRepository,
            IDelivererInforRepository delivererInforRepository,
            IDeliveryOrderPORepository deliveryOrderPORepository,

            // ==== Purchase ====
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            IPurchaseOrderSnapshotRepository purchaseOrderSnapshotRepository,
            IPurchaseOrderLinkRepository purchaseOrderLinkRepository,

            // ==== Audit / Timeline ====
            IEventLogRepository eventLogRepository,

            // ==== Notifications (MỚI THÊM) ====
            INotificationRepository notifications,
            INotificationRecipientRepository notificationRecipients,
            INotificationUserStateRepository notificationUserStates,
            IUserNotificationSettingRepository userNotificationSettings,
            INotificationTemplateRepository notificationTemplates,
            IOutboxMessageRepository outboxMessages
        )
        {
            _context = context;

            // ===== Attachments =====
            AttachmentCollectionRepository = attachmentCollectionRepository;
            AttachmentModelRepository = attachmentModelRepository;

            // ===== Company/Common =====
            CompanyRepository = companyRepository;
            EmployeesCommonRepository = employeesCommonRepository;

            // ===== HR =====
            ApplicationUserRepository = applicationUserRepository;
            EmployeesRepository = employeesRepository;
            GroupRepository = groupRepository;
            MemberInGroupRepository = memberInGroupRepository;

            // ===== Sales =====
            CustomerRepository = customerRepository;
            TransferCustomerRepository = transferCustomerRepository;
            CustomerAssignmentRepository = customerAssignmentRepository;
            CustomerTransferLogRepository = customerTransferLogRepository;
            MerchandiseOrderRepository = merchandiseOrderRepository;

            // ===== Labs =====
            //ProductStandardRepository = productStandardRepository;
            //ProductInspectionRepository = productInspectionRepository;
            //ProductTestRepository = productTestRepository;
            ManufacturingFormulaVersionRepository = manufacturingFormulaVersionRepository;
            FormulaRepository = formulaRepository;
            FormulaMaterialRepository = formulaMaterialRepository;
            SampleRequestRepository = sampleRequestRepository;

            // ===== Planning =====
            ScheduealRepository = scheduealRepository;
            //IMfgProductionOrdersPlanRepository = iMfgProductionOrdersPlanRepository;

            // ===== QAQC Detail =====
            //IQCDetailRepository = iQCDetailRepository;

            // ===== Product =====
            ProductRepository = productRepository;

            // ===== Material =====
            MaterialRepository = materialRepository;
            SupplierRepository = supplierRepository;
            CategoryRepository = categoryRepository;
            MaterialsSupplierRepository = materialsSupplierRepository;
            PriceHistorieRepository = priceHistorieRepository;

            // ===== Manufacturing =====
            MfgProductionOrderRepository = mfgProductionOrderRepository;
            ManufacturingFormulaMaterialRepository = manufacturingFormulaMaterialRepository;
            ManufacturingFormulaRepository = manufacturingFormulaRepository;
            ProductStandardFormulaRepository = productStandardFormulaRepository;
            ProductionSelectVersionRepository = productionSelectVersionRepository;
            MfgOrderPORepository = mfgOrderPORepository;
            SchedualMfgRepository = schedualMfgRepository;

            // ===== Warehouse =====
            WarehouseShelfStockRepository = warehouseShelfStockRepository;
            WarehouseTempStockRepository = warehouseTempStockRepository;
            WarehouseRequestDetailRepository = warehouseRequestDetailRepository;
            WarehouseRequestRepository = warehouseRequestRepository;

            // ===== Delivery =====
            DeliveryOrderRepository = deliveryOrderRepository;
            DeliveryOrderDetailRepository = deliveryOrderDetailRepository;
            DelivererRepository = delivererRepository;
            DelivererInforRepository = delivererInforRepository;
            DeliveryOrderPORepository = deliveryOrderPORepository;

            // ===== Purchase =====
            PurchaseOrderRepository = purchaseOrderRepository;
            PurchaseOrderDetailRepository = purchaseOrderDetailRepository;
            PurchaseOrderSnapshotRepository = purchaseOrderSnapshotRepository;
            PurchaseOrderLinkRepository = purchaseOrderLinkRepository;

            // ===== Audit / Timeline =====
            EventLogRepository = eventLogRepository;

            // ===== Notifications (MỚI THÊM) =====
            Notifications = notifications;
            NotificationRecipients = notificationRecipients;
            NotificationUserStates = notificationUserStates;
            UserNotificationSettings = userNotificationSettings;
            NotificationTemplates = notificationTemplates;
            OutboxMessages = outboxMessages;
        }

        // ==== Tx/Save/Dispose ====
        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
            => await _context.Database.CommitTransactionAsync();

        public async Task RollbackTransactionAsync()
            => await _context.Database.RollbackTransactionAsync();

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
