

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;
using VietausWebAPI.Core.Application.Features.Audits.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts.GalleryItemFeatures;
using VietausWebAPI.Core.Application.Features.CompanyFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DeliveryOrders.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.RepositoriesContracts.QCInputByQCFeatures;
using VietausWebAPI.Core.Application.Features.HR.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Identity.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Labs.Services.SampleRequestFeature.ColorChipRecordFeatures;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.ColorChipManufacturingRecords;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts.GetRepositories;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.MaterialFeatures.RepositoriesContracts.SupplierFeatures;
using VietausWebAPI.Core.Application.Features.Notifications.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Planning.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.PurchaseFeatures.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.PLPUReports;
using VietausWebAPI.Core.Application.Features.ReportFeatures.RepositoriesContracts.SaleReports;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.CustomerFeatures;
using VietausWebAPI.Core.Application.Features.Sales.RepositoriesContracts.MerchandiseOrderFeatures;
using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.EndOfShiftReportFeatures;
using VietausWebAPI.Core.Application.Features.ShiftReportFeatures.RepositoriesContracts.ExtruderOperationHistoryRepositories;
using VietausWebAPI.Core.Application.Features.TimelineFeature.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts;
using VietausWebAPI.Core.Application.Features.Warehouse.RepositoriesContracts.ReadRepositories;
using VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs;

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
            // ==== Report ====

            IFinishPLPUReportRepository finishPLPUReportRepository,
            
            IMerchandiseOrderReportRepositorys merchandiseOrderReportRepositorys,

            // ==== Audits ======
            IAuditLogRepository auditLogRepository,

            // ==== Attachments ====
            IAttachmentCollectionRepository attachmentCollectionRepository,
            IAttachmentModelRepository attachmentModelRepository,

            IImageGalleryReadRepository imageGalleryReadRepository,

            // ==== Identity ====
            IApplicationUserRepository applicationUserRepository,
            IApplicationUserRoleRepository applicationUserRoleRepository,
            IApplicationRoleRepository applicationRoleRepository,

            // ==== Company/Common ====
            ICompanyRepository companyRepository,
            IEmployeesRepository employeesCommonRepository,

            // ==== HR ====
            IEmployeesRepository employeesRepository,
            IGroupRepository groupRepository,
            IMemberInGroupRepository memberInGroupRepository,
            IPartRepository partRepository,

            // ==== Sales ====
            ICustomerRepository customerRepository,
            ITransferCustomerRepository transferCustomerRepository,
            ICustomerAssignmentRepository customerAssignmentRepository,
            ICustomerClaimRepository customerClaimRepository,
            ICustomerNoteRepository customerNoteRepository, 
            ICustomerTransferLogRepository customerTransferLogRepository,
            IMerchandiseOrderRepository merchandiseOrderRepository,

            // ==== Labs (QA/QC, Formula, Sample) ====
            IManufacturingFormulaVersionRepository manufacturingFormulaVersionRepository,
            IFormulaRepository formulaRepository,
            IFormulaMaterialRepository formulaMaterialRepository,
            ISampleRequestRepository sampleRequestRepository,
            IManufacturingVUFormulaRepository manufacturingVUFormulaRepository,
            IFormulaMaterialSnapshotRepository formulaMaterialSnapshotRepository,

            IColorChipRecordReadRepositories colorChipRecordReadRepositories,
            IColorChipRecordWriteRepositories colorChipRecordWriteRepositories,
            IColorChipRecordUpsertRepositories colorChipRecordUpsertRepositories,

            // ==== Planning ====
            IScheduealRepository scheduealRepository,
            //IMfgProductionOrdersPlanRepository mfgProductionOrdersPlanRepository,
            //IMfgProductionOrdersPlanRepository iMfgProductionOrdersPlanRepository, // bạn đang dùng 2 biến cho cùng 1 interface

            // ==== Devandqa (thuộc Labs) ====
            IProductStandardRepository productStandardRepository,
            IProductTestRepository productTestRepository,
            IProductInspectionRepository productInspectionRepository,
            IQCInputByQCRepository iQCDetailRepository,

            // QCInputByQCFeatures
            IQCInputByQCReadRepository qCInputByQCReadRepository,
            IQCInputByQCWriteRepository qCInputByQCWriteRepository,
            // ==== Product (bạn đang để trong Manufacturing namespace) ====
            IProductRepository productRepository,

            // ==== Material ====
            IMaterialRepository materialRepository,
            ICategoryRepository categoryRepository,
            IUnitRepository unitRepository,
            IMaterialsSupplierRepository materialsSupplierRepository,
            IPriceHistorieRepository priceHistorieRepository,

            // ======================================================================== SupplierFeature ======================================================================== 
            ISupplierReadRepository supplierReadRepository,
            ISupplierWriteRepository supplierWriteRepository,

            // ==== Manufacturing ====
            IColorChipManufacturingRecordReadRepository colorChipManufacturingRecordReadRepository,
            IColorChipManufacturingRecordWriteRepository colorChipManufacturingRecordWriteRepository,

            IMfgProductionOrderRepository mfgProductionOrderRepository,
            IManufacturingFormulaMaterialRepository manufacturingFormulaMaterialRepository,
            IManufacturingFormulaRepository manufacturingFormulaRepository,
            IProductStandardFormulaRepository productStandardFormulaRepository,
            IProductionSelectVersionRepository productionSelectVersionRepository,
            IProductionSelectVersionReadRepository productionSelectVersionReadRepository,
            IMfgOrderPORepository mfgOrderPORepository,
            ISchedualMfgRepository schedualMfgRepository,

            // ==== Warehouse ====
            IWarehouseShelfStockRepository warehouseShelfStockRepository,
            IWarehouseTempStockRepository warehouseTempStockRepository,
            IWarehouseRequestDetailRepository warehouseRequestDetailRepository,
            IWarehouseRequestRepository warehouseRequestRepository,

            IWarehouseShelfLedgerReadRepository warehouseShelfLedgerReadRepository,
            IWarehouseVoucherReadRepository warehouseVoucherReadRepository,
            IWarehouseVoucherDetailReadRepository warehouseVoucherDetailReadRepository,

            // ==== Delivery ====
            IDeliveryOrderRepository deliveryOrderRepository,
            IDeliveryOrderDetailRepository deliveryOrderDetailRepository,
            IDelivererRepository delivererRepository,
            IDelivererInforRepository delivererInforRepository,
            IDeliveryOrderPORepository deliveryOrderPORepository,

            // ==== ExtruderOperationHistory ====
            IExtruderOperationHistoryReadRepositories extruderOperationHistoryReadRepositories,
            IEndOfShiftReportReadRepositories endOfShiftReportReadRepositories,


            // ==== Purchase ====
            IPurchaseOrderRepository purchaseOrderRepository,
            IPurchaseOrderDetailRepository purchaseOrderDetailRepository,
            IPurchaseOrderDocumentRepository purchaseOrderDocumentRepository,
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
            // ===== Report =====
            FinishPLPUReportRepository = finishPLPUReportRepository;

            MerchandiseOrderReportRepositorys = merchandiseOrderReportRepositorys;

            // ===== Audits ====
            AuditLogRepository = auditLogRepository;

            // ===== Attachments =====
            AttachmentCollectionRepository = attachmentCollectionRepository;
            AttachmentModelRepository = attachmentModelRepository;

            ImageGalleryReadRepository = imageGalleryReadRepository;

            // ===== Company/Common =====
            CompanyRepository = companyRepository;
            EmployeesCommonRepository = employeesCommonRepository;

            // ===== Identity =====
            ApplicationUserRoleRepository = applicationUserRoleRepository;
            ApplicationUserRepository = applicationUserRepository;
            ApplicationRoleRepository = applicationRoleRepository;

            // ===== HR =====
            EmployeesRepository = employeesRepository;
            GroupRepository = groupRepository;
            MemberInGroupRepository = memberInGroupRepository;
            PartRepository = partRepository;

            // ===== Sales =====
            CustomerRepository = customerRepository;
            TransferCustomerRepository = transferCustomerRepository;
            CustomerAssignmentRepository = customerAssignmentRepository;
            CustomerClaimRepository = customerClaimRepository;
            CustomerNoteRepository = customerNoteRepository;
            CustomerTransferLogRepository = customerTransferLogRepository;
            MerchandiseOrderRepository = merchandiseOrderRepository;

            // ===== Labs =====
            ProductStandardRepository = productStandardRepository;
            ProductTestRepository = productTestRepository;
            FormulaMaterialSnapshotRepository = formulaMaterialSnapshotRepository;

            ManufacturingFormulaVersionRepository = manufacturingFormulaVersionRepository;
            FormulaRepository = formulaRepository;
            FormulaMaterialRepository = formulaMaterialRepository;
            SampleRequestRepository = sampleRequestRepository;
            ManufacturingVUFormulaRepository = manufacturingVUFormulaRepository;

            ColorChipRecordReadRepositories = colorChipRecordReadRepositories;
            ColorChipRecordWriteRepositories = colorChipRecordWriteRepositories;
            ColorChipRecordUpsertRepositories = colorChipRecordUpsertRepositories;

            // ===== Planning =====
            ScheduealRepository = scheduealRepository;
            //IMfgProductionOrdersPlanRepository = iMfgProductionOrdersPlanRepository;

            // ===== QAQC Detail =====
            //IQCDetailRepository = iQCDetailRepository;
            ProductInspectionRepository = productInspectionRepository;
            ProductRepository = productRepository;
            //QCInputByWarehouseRepository = qCInputByWarehouseRepository;
            QCInputByQCRepository = iQCDetailRepository;

            // QCInputByQCFeatures
            QCInputByQCReadRepository = qCInputByQCReadRepository;
            QCInputByQCWriteRepository = qCInputByQCWriteRepository;

            // ===== Material =====
            MaterialRepository = materialRepository;
            CategoryRepository = categoryRepository;
            UnitRepository = unitRepository;
            MaterialsSupplierRepository = materialsSupplierRepository;
            PriceHistorieRepository = priceHistorieRepository;

            // ======================================================================== SupplierFeature ======================================================================== 
            SupplierReadRepository = supplierReadRepository;
            SupplierWriteRepository = supplierWriteRepository;

            // ===== Manufacturing =====
            ColorChipManufacturingRecordReadRepository = colorChipManufacturingRecordReadRepository;
            ColorChipManufacturingRecordWriteRepository = colorChipManufacturingRecordWriteRepository;

            MfgProductionOrderRepository = mfgProductionOrderRepository;
            ManufacturingFormulaMaterialRepository = manufacturingFormulaMaterialRepository;
            ManufacturingFormulaRepository = manufacturingFormulaRepository;
            ProductStandardFormulaRepository = productStandardFormulaRepository;
            ProductionSelectVersionRepository = productionSelectVersionRepository;
            ProductionSelectVersionReadRepository = productionSelectVersionReadRepository;
            MfgOrderPORepository = mfgOrderPORepository;
            SchedualMfgRepository = schedualMfgRepository;

            // ===== Warehouse =====
            WarehouseShelfStockRepository = warehouseShelfStockRepository;
            WarehouseTempStockRepository = warehouseTempStockRepository;
            WarehouseRequestDetailRepository = warehouseRequestDetailRepository;
            WarehouseRequestRepository = warehouseRequestRepository;

            WarehouseShelfLedgerReadRepository = warehouseShelfLedgerReadRepository;
            WarehouseVoucherReadRepository = warehouseVoucherReadRepository;
            WarehouseVoucherDetailReadRepository = warehouseVoucherDetailReadRepository;

            // ===== Delivery =====
            DeliveryOrderRepository = deliveryOrderRepository;
            DeliveryOrderDetailRepository = deliveryOrderDetailRepository;
            DelivererRepository = delivererRepository;
            DelivererInforRepository = delivererInforRepository;
            DeliveryOrderPORepository = deliveryOrderPORepository;

            // ==== ExtruderOperationHistory ====
            ExtruderOperationHistoryReadRepositories = extruderOperationHistoryReadRepositories;
            EndOfShiftReportReadRepositories = endOfShiftReportReadRepositories;
            // ===== Purchase =====
            PurchaseOrderRepository = purchaseOrderRepository;
            PurchaseOrderDetailRepository = purchaseOrderDetailRepository;
            PurchaseOrderDocumentRepository = purchaseOrderDocumentRepository;
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
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
            => await _context.Database.BeginTransactionAsync(ct);

        public async Task CommitTransactionAsync(CancellationToken ct = default)
            => await _context.Database.CommitTransactionAsync(ct);

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
            => await _context.Database.RollbackTransactionAsync(ct);

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            try
            {
                return await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var details = new StringBuilder();
                details.AppendLine("Concurrency conflict entries:");

                foreach (var entry in ex.Entries)
                {
                    details.Append(entry.Metadata.ClrType.Name)
                        .Append(" State=")
                        .Append(entry.State)
                        .Append(" Keys=");

                    var keys = entry.Metadata.FindPrimaryKey()?.Properties;
                    if (keys != null)
                    {
                        var keyValues = keys.Select(x => $"{x.Name}:{entry.Property(x.Name).CurrentValue}");
                        details.Append(string.Join(",", keyValues));
                    }

                    details.AppendLine();
                }

                throw new DbUpdateConcurrencyException(details.ToString(), ex);
            }
        }

        public void Dispose() => _context.Dispose();
    }
}
