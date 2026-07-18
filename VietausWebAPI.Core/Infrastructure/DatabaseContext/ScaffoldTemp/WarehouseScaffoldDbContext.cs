using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema_Scaffold;

namespace VietausWebAPI.Core.Infrastructure.DatabaseContext.ScaffoldTemp;

public partial class WarehouseScaffoldDbContext : DbContext
{
    public WarehouseScaffoldDbContext(DbContextOptions<WarehouseScaffoldDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DepartmentTempStock> DepartmentTempStocks { get; set; }

    public virtual DbSet<DepartmentTempStockLog> DepartmentTempStockLogs { get; set; }

    public virtual DbSet<ImportRequestFlowLog> ImportRequestFlowLogs { get; set; }

    public virtual DbSet<OperationMaterialBuffer> OperationMaterialBuffers { get; set; }

    public virtual DbSet<OperationMaterialRequestLink> OperationMaterialRequestLinks { get; set; }

    public virtual DbSet<PicklistDraft> PicklistDrafts { get; set; }

    public virtual DbSet<PicklistDraftLine> PicklistDraftLines { get; set; }

    public virtual DbSet<ProductionOutputReceiptLedger> ProductionOutputReceiptLedgers { get; set; }

    public virtual DbSet<ProductionOutputReceiptSource> ProductionOutputReceiptSources { get; set; }

    public virtual DbSet<TempStockIssueLedger> TempStockIssueLedgers { get; set; }

    public virtual DbSet<UsagePurpose> UsagePurposes { get; set; }

    public virtual DbSet<UsagePurpose1> UsagePurposes1 { get; set; }

    public virtual DbSet<WarehouseRequest> WarehouseRequests { get; set; }

    public virtual DbSet<WarehouseRequestDetail> WarehouseRequestDetails { get; set; }

    public virtual DbSet<WarehouseShelf> WarehouseShelves { get; set; }

    public virtual DbSet<WarehouseShelfLedger> WarehouseShelfLedgers { get; set; }

    public virtual DbSet<WarehouseShelfStock> WarehouseShelfStocks { get; set; }

    public virtual DbSet<WarehouseStockFifoLayer> WarehouseStockFifoLayers { get; set; }

    public virtual DbSet<WarehouseTempStock> WarehouseTempStocks { get; set; }

    public virtual DbSet<WarehouseVoucher> WarehouseVouchers { get; set; }

    public virtual DbSet<WarehouseVoucherDetail> WarehouseVoucherDetails { get; set; }

    public virtual DbSet<v_ShelfHistory_Canonical> v_ShelfHistory_Canonicals { get; set; }

    public virtual DbSet<v_ShelfStockCurrent> v_ShelfStockCurrents { get; set; }

    public virtual DbSet<v_StockByProduct> v_StockByProducts { get; set; }

    public virtual DbSet<v_outbound_due_line> v_outbound_due_lines { get; set; }

    public virtual DbSet<vw_RequestDetailsProgress> vw_RequestDetailsProgresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("citext")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("unaccent");

        modelBuilder.Entity<DepartmentTempStock>(entity =>
        {
            entity.HasKey(e => e.TempStockId).HasName("DepartmentTempStock_pkey");

            entity.ToTable("DepartmentTempStock", "Warehouse");

            entity.HasIndex(e => e.VoucherDetailId, "UQ_DepartmentTempStock_VoucherDetail").IsUnique();

            entity.HasIndex(e => new { e.companyId, e.RequestCode, e.PartID, e.ProductCode, e.LotNumber }, "ix_department_temp_stock_request");

            entity.HasIndex(e => new { e.companyId, e.PartID, e.Status, e.CostStatus }, "ix_department_temp_stock_status");

            entity.Property(e => e.TempStockId).UseIdentityAlwaysColumn();
            entity.Property(e => e.AmountCost).HasPrecision(18, 2);
            entity.Property(e => e.AvgUnitCost).HasPrecision(18, 6);
            entity.Property(e => e.CostStatus).HasDefaultValueSql("'PENDING'::text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IssuedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.RemainingQtyKg)
                .HasPrecision(18, 3)
                .HasComputedColumnSql("((\"IssuedQtyKg\" - \"UsedQtyKg\") - \"ReturnedQtyKg\")", true);
            entity.Property(e => e.ReturnedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.Status).HasDefaultValueSql("'OPEN'::text");
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UsedQtyKg).HasPrecision(18, 3);
        });

        modelBuilder.Entity<DepartmentTempStockLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("DepartmentTempStockLog_pkey");

            entity.ToTable("DepartmentTempStockLog", "Warehouse");

            entity.HasIndex(e => new { e.companyId, e.PartID, e.CreatedAt }, "ix_department_temp_stock_log_part_date")
                .IsDescending(false, false, true)
                .HasNullSortOrder(new[] { NullSortOrder.NullsLast, NullSortOrder.NullsLast, NullSortOrder.NullsLast });

            entity.HasIndex(e => new { e.companyId, e.RequestCode, e.ProductCode, e.LotNumber, e.CreatedAt }, "ix_department_temp_stock_log_request");

            entity.Property(e => e.LogId).UseIdentityAlwaysColumn();
            entity.Property(e => e.AmountCost).HasPrecision(18, 2);
            entity.Property(e => e.AvgUnitCost).HasPrecision(18, 6);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.DeltaKg).HasPrecision(18, 3);
            entity.Property(e => e.QtyAfter).HasPrecision(18, 3);
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
        });

        modelBuilder.Entity<ImportRequestFlowLog>(entity =>
        {
            entity.HasKey(e => e.FlowLogId).HasName("ImportRequestFlowLog_pkey");

            entity.ToTable("ImportRequestFlowLog", "Warehouse");

            entity.HasIndex(e => new { e.companyId, e.RequestCode, e.RequestDetailId, e.LogStatus }, "IX_ImportRequestFlowLog_ApplyLookup");

            entity.HasIndex(e => new { e.companyId, e.LogStatus, e.CreatedAt }, "IX_ImportRequestFlowLog_Pending").HasFilter("(\"LogStatus\" = ANY (ARRAY['PENDING'::text, 'PARTIAL'::text]))");

            entity.HasIndex(e => new { e.companyId, e.ProductCode, e.LotNumber }, "IX_ImportRequestFlowLog_Product");

            entity.HasIndex(e => new { e.companyId, e.SourceKind, e.SourceRowId }, "IX_ImportRequestFlowLog_Source");

            entity.HasIndex(e => new { e.companyId, e.RequestCode, e.RequestDetailId }, "UX_ImportRequestFlowLog_RequestDetail")
                .IsUnique()
                .HasFilter("(\"RequestDetailId\" IS NOT NULL)");

            entity.HasIndex(e => new { e.companyId, e.RequestCode, e.LineNo }, "UX_ImportRequestFlowLog_Request_Line").IsUnique();

            entity.Property(e => e.FlowLogId).UseIdentityAlwaysColumn();
            entity.Property(e => e.AppliedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.AppliedBagNumber).HasDefaultValue(0);
            entity.Property(e => e.AppliedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.BagNumber).HasDefaultValue(0);
            entity.Property(e => e.CancelledAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ConfirmedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.LogStatus).HasDefaultValueSql("'PENDING'::text");
            entity.Property(e => e.QtyKg).HasPrecision(18, 3);
            entity.Property(e => e.SourceAvailableQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<OperationMaterialBuffer>(entity =>
        {
            entity.HasKey(e => e.BufferId).HasName("OperationMaterialBuffer_pkey");

            entity.ToTable("OperationMaterialBuffer", "Warehouse");

            entity.HasIndex(e => e.VoucherDetailId, "UQ_OperationMaterialBuffer_VoucherDetail").IsUnique();

            entity.HasIndex(e => new { e.companyId, e.RequestCode, e.ProductCode, e.LotNumber }, "ix_operation_material_buffer_request");

            entity.HasIndex(e => new { e.companyId, e.Status, e.CostStatus }, "ix_operation_material_buffer_status");

            entity.Property(e => e.BufferId).UseIdentityAlwaysColumn();
            entity.Property(e => e.AmountCost).HasPrecision(18, 2);
            entity.Property(e => e.AvgUnitCost).HasPrecision(18, 6);
            entity.Property(e => e.CostStatus).HasDefaultValueSql("'PENDING'::text");
            entity.Property(e => e.CostedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IssuedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.RemainingQtyKg)
                .HasPrecision(18, 3)
                .HasComputedColumnSql("((\"IssuedQtyKg\" - \"UsedQtyKg\") - \"ReturnedQtyKg\")", true);
            entity.Property(e => e.ReturnedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.Status).HasDefaultValueSql("'OPEN'::text");
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UsedQtyKg).HasPrecision(18, 3);
        });

        modelBuilder.Entity<OperationMaterialRequestLink>(entity =>
        {
            entity.HasKey(e => e.LinkId).HasName("OperationMaterialRequestLink_pkey");

            entity.ToTable("OperationMaterialRequestLink", "Warehouse");

            entity.HasIndex(e => e.RequestId, "IX_OperationMaterialRequestLink_RequestId");

            entity.Property(e => e.LinkId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.RequestedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
        });

        modelBuilder.Entity<PicklistDraft>(entity =>
        {
            entity.HasKey(e => e.draftId).HasName("PicklistDraft_pkey");

            entity.ToTable("PicklistDraft", "Warehouse");

            entity.HasIndex(e => e.status, "ix_picklistdraft_status");

            entity.HasIndex(e => new { e.companyId, e.requestCode, e.kind }, "ux_picklistdraft_company_req_kind_legacy_null_round")
                .IsUnique()
                .HasFilter("(\"roundNo\" IS NULL)");

            entity.HasIndex(e => new { e.companyId, e.requestCode, e.kind, e.roundNo }, "ux_picklistdraft_company_req_kind_round")
                .IsUnique()
                .HasFilter("(\"roundNo\" IS NOT NULL)");

            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.exportedAt)
                .HasComment("Thời điểm kho xác nhận xuất đợt này. Nullable để không ảnh hưởng dữ liệu cũ.")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.exportedBy).HasComment("Người kho xác nhận xuất đợt này. Nullable để không ảnh hưởng dữ liệu cũ.");
            entity.Property(e => e.frozenAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.receivedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.roundNo).HasComment("Số đợt xuất cho cùng một phiếu CT. NULL để tương thích draft cũ.");
            entity.Property(e => e.status).HasDefaultValueSql("'DRAFT'::text");
            entity.Property(e => e.updatedAt).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<PicklistDraftLine>(entity =>
        {
            entity.HasKey(e => e.lineId).HasName("PicklistDraftLine_pkey");

            entity.ToTable("PicklistDraftLine", "Warehouse");

            entity.HasIndex(e => new { e.draftId, e.shelfStockId }, "ix_picklistdraftline_draft_shelf");

            entity.HasIndex(e => new { e.draftId, e.lineNo }, "ux_picklistdraftline_draft_lineno").IsUnique();

            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.lotKey).HasColumnType("citext");
            entity.Property(e => e.qtyKg).HasPrecision(18, 3);
            entity.Property(e => e.stdBagKg).HasPrecision(18, 3);

            entity.HasOne(d => d.draft).WithMany(p => p.PicklistDraftLines)
                .HasForeignKey(d => d.draftId)
                .HasConstraintName("PicklistDraftLine_draftId_fkey");
        });

        modelBuilder.Entity<ProductionOutputReceiptLedger>(entity =>
        {
            entity.HasKey(e => e.LedgerId).HasName("ProductionOutputReceiptLedger_pkey");

            entity.ToTable("ProductionOutputReceiptLedger", "Warehouse");

            entity.HasIndex(e => new { e.companyId, e.VoucherDetailId }, "UX_ProductionOutputReceiptLedger_VoucherDetail").IsUnique();

            entity.Property(e => e.LedgerId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.QtyKg).HasPrecision(18, 3);
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");

            entity.HasOne(d => d.ProductionOutput).WithMany(p => p.ProductionOutputReceiptLedgers)
                .HasForeignKey(d => d.ProductionOutputId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_ProductionOutputReceiptLedger_Source");
        });

        modelBuilder.Entity<ProductionOutputReceiptSource>(entity =>
        {
            entity.HasKey(e => e.ProductionOutputId).HasName("ProductionOutputReceiptSource_pkey");

            entity.ToTable("ProductionOutputReceiptSource", "Warehouse");

            entity.HasIndex(e => new { e.companyId, e.LinkStatus, e.CostStatus, e.Status }, "IX_ProductionOutputReceiptSource_LinkCost");

            entity.HasIndex(e => new { e.companyId, e.MfgExternalId, e.VaExternalId, e.Status }, "IX_ProductionOutputReceiptSource_MfgVa");

            entity.HasIndex(e => new { e.companyId, e.ProductCode, e.ItemStockType, e.Status }, "IX_ProductionOutputReceiptSource_Open_Product");

            entity.HasIndex(e => new { e.companyId, e.ShiftReportForAllId, e.ShiftReportDetailForAllId }, "IX_ProductionOutputReceiptSource_ShiftReport");

            entity.HasIndex(e => new { e.companyId, e.SourceKey }, "UX_ProductionOutputReceiptSource_SourceKey").IsUnique();

            entity.Property(e => e.ProductionOutputId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CompletedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.CostStatus).HasDefaultValueSql("'PENDING'::text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.DefectQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.GoodQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.ImportedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.IsOpenEnded).HasDefaultValue(true);
            entity.Property(e => e.ItemStockType).HasDefaultValue(1);
            entity.Property(e => e.LineNo).HasDefaultValue(1);
            entity.Property(e => e.LinkStatus).HasDefaultValueSql("'UNLINKED'::text");
            entity.Property(e => e.MaterialIssueFoundAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.OverrideAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.PlannedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.ProducedQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.ReceiptCount).HasDefaultValue(0);
            entity.Property(e => e.RemainingQtyKg)
                .HasPrecision(18, 3)
                .HasComputedColumnSql("\nCASE\n    WHEN (\"Status\" <> 'OPEN'::text) THEN (0)::numeric\n    WHEN (\"IsOpenEnded\" = true) THEN 999999999.999\n    ELSE GREATEST((\"ProducedQtyKg\" - \"ImportedQtyKg\"), (0)::numeric)\nEND", true);
            entity.Property(e => e.ScrapQtyKg).HasPrecision(18, 3);
            entity.Property(e => e.SourceKind).HasDefaultValueSql("'SHIFT_REPORT'::text");
            entity.Property(e => e.Status).HasDefaultValueSql("'OPEN'::text");
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<TempStockIssueLedger>(entity =>
        {
            entity.HasKey(e => e.ledgerId).HasName("TempStockIssueLedger_pkey");

            entity.ToTable("TempStockIssueLedger", "Warehouse");

            entity.HasIndex(e => new { e.companyId, e.sourceKind, e.sourceKey }, "UX_TempStockIssueLedger_Source").IsUnique();

            entity.Property(e => e.ledgerId).UseIdentityAlwaysColumn();
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.qtyKg).HasPrecision(18, 3);
        });

        modelBuilder.Entity<UsagePurpose>(entity =>
        {
            entity.HasKey(e => e.usage_purpose_id).HasName("UsagePurposes_pkey");

            entity.ToTable("UsagePurposes", "SupplyRequest");

            entity.HasIndex(e => e.purpose_code, "UsagePurposes_purpose_code_key").IsUnique();

            entity.Property(e => e.usage_purpose_id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.created_at)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.is_active).HasDefaultValue(true);
            entity.Property(e => e.sort_no).HasDefaultValue(0);
        });

        modelBuilder.Entity<UsagePurpose1>(entity =>
        {
            entity.HasKey(e => e.purposeId).HasName("PK__UsagePurposes__purposeId");

            entity.ToTable("UsagePurposes", "Warehouse");

            entity.HasIndex(e => e.purposeCode, "ux_usagepurposes_purposecode").IsUnique();

            entity.Property(e => e.purposeId).UseIdentityAlwaysColumn();
            entity.Property(e => e.isActive).HasDefaultValue(true);
            entity.Property(e => e.isPickable).HasDefaultValue(false);
            entity.Property(e => e.isReceivable).HasDefaultValue(false);
            entity.Property(e => e.purposeCode).HasColumnType("citext");
        });

        modelBuilder.Entity<WarehouseRequest>(entity =>
        {
            entity.HasKey(e => e.requestId).HasName("PK__WareHouseRequest__3214EC07A98DEC4E");

            entity.ToTable("WarehouseRequest", "Warehouse");

            entity.HasIndex(e => e.companyId, "IX_WarehouseRequest_companyId");

            entity.HasIndex(e => e.createdBy, "IX_WarehouseRequest_createdBy");

            entity.HasIndex(e => e.updatedBy, "IX_WarehouseRequest_updatedBy");

            entity.Property(e => e.requestId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("'-infinity'::timestamp without time zone")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.codeFromRequest).HasDefaultValueSql("''::text");
            entity.Property(e => e.reqStatus).HasDefaultValue(0);
            entity.Property(e => e.reqType).HasDefaultValue(0);
        });

        modelBuilder.Entity<WarehouseRequestDetail>(entity =>
        {
            entity.HasKey(e => e.detailId).HasName("PK__WareHouseRequestDetail__detailId");

            entity.ToTable("WarehouseRequestDetail", "Warehouse");

            entity.HasIndex(e => e.requestId, "IX_WarehouseRequestDetail_RequestCode");

            entity.Property(e => e.detailId).UseIdentityAlwaysColumn();
            entity.Property(e => e.BagNumber).HasDefaultValue(0);
            entity.Property(e => e.ExpiryDate).HasComment("Hạn sử dụng được ghi nhận trên dòng phiếu yêu cầu nhập/xuất kho.");
            entity.Property(e => e.ItemStockType).HasComment("Loại hàng theo từng dòng: 1=FinishedGood, 2=DefectiveFinishedGood, 3=RawMaterial, 4=DefectiveRawMaterial, 5=Other. Nullable để không ảnh hưởng dữ liệu/luồng cũ.");
            entity.Property(e => e.LotNumber).HasDefaultValueSql("''::text");
            entity.Property(e => e.ProductCode).HasDefaultValueSql("''::text");
            entity.Property(e => e.ProductName).HasDefaultValueSql("''::text");
            entity.Property(e => e.StockStatus).HasDefaultValueSql("''::text");
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.isActive).HasDefaultValue(true);
            entity.Property(e => e.requestId).HasDefaultValue(0);
            entity.Property(e => e.weightKg).HasPrecision(18, 3);

            entity.HasOne(d => d.request).WithMany(p => p.WarehouseRequestDetails)
                .HasForeignKey(d => d.requestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarehouseRequestDetail_RequestId");
        });

        modelBuilder.Entity<WarehouseShelf>(entity =>
        {
            entity.HasKey(e => e.ShelfStockId).HasName("PK_WarehouseShelves_SlotId");

            entity.ToTable("WarehouseShelves", "Warehouse");

            entity.HasIndex(e => e.lastUpdated, "ix_warehouseshelves_lastupdated");

            entity.HasIndex(e => new { e.companyId, e.ShelfStockCode }, "ux_warehouseshelves_company_slotcode").IsUnique();

            entity.Property(e => e.ShelfStockId).UseIdentityAlwaysColumn();
            entity.Property(e => e.ShelfStockCode).HasMaxLength(64);
            entity.Property(e => e.currentWeightKg).HasPrecision(10, 2);
            entity.Property(e => e.isActive).HasDefaultValue(true);
            entity.Property(e => e.lastUpdated)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.maxWeightKg).HasPrecision(10, 2);
        });

        modelBuilder.Entity<WarehouseShelfLedger>(entity =>
        {
            entity.HasKey(e => e.ledgerId).HasName("PK_WarehouseShelfLedger_LedgerId");

            entity.ToTable("WarehouseShelfLedger", "Warehouse");

            entity.HasIndex(e => e.createdBy, "IX_WarehouseShelfLedger_createdBy");

            entity.HasIndex(e => e.slotId, "IX_WarehouseShelfLedger_slotId");

            entity.HasIndex(e => new { e.companyId, e.createdAt, e.productCode, e.stockType }, "ix_wsledger_company_created_product_stocktype_fast");

            entity.HasIndex(e => new { e.companyId, e.productCode, e.stockType, e.createdAt }, "ix_wsledger_company_product_stocktype_created_fast");

            entity.HasIndex(e => new { e.companyId, e.slotId, e.createdAt }, "ix_wsledger_company_slot_created");

            entity.HasIndex(e => e.voucherId, "ix_wsledger_voucher");

            entity.Property(e => e.ledgerId).UseIdentityAlwaysColumn();
            entity.Property(e => e.ExpiryDate).HasComment("Hạn sử dụng tại thời điểm phát sinh log kho. Nullable để không phá dữ liệu cũ.");
            entity.Property(e => e.UnitName)
                .HasDefaultValueSql("'Kg'::text")
                .HasComment("Đơn vị tính tại thời điểm phát sinh ledger");
            entity.Property(e => e.afterKg).HasPrecision(18, 3);
            entity.Property(e => e.beforeKg).HasPrecision(18, 3);
            entity.Property(e => e.createdAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.deltaKg).HasPrecision(18, 3);

            entity.HasOne(d => d.slot).WithMany(p => p.WarehouseShelfLedgers)
                .HasForeignKey(d => d.slotId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WarehouseShelfLedger_SlotId");
        });

        modelBuilder.Entity<WarehouseShelfStock>(entity =>
        {
            entity.HasKey(e => e.ShelfStockId);

            entity.ToTable("WarehouseShelfStock", "Warehouse");

            entity.HasIndex(e => e.SlotId, "IX_WarehouseShelfStock_SlotId");

            entity.HasIndex(e => new { e.companyId, e.code }, "IX_WarehouseShelfStock_company_code");

            entity.HasIndex(e => new { e.companyId, e.code, e.lotKey }, "IX_WarehouseShelfStock_company_code_lot");

            entity.HasIndex(e => e.updatedBy, "IX_WarehouseShelfStock_updatedBy");

            entity.HasIndex(e => new { e.companyId, e.code, e.stockType }, "ix_wsstock_company_code_stocktype_fast");

            entity.Property(e => e.ShelfStockId).UseIdentityAlwaysColumn();
            entity.Property(e => e.LotNo).HasMaxLength(50);
            entity.Property(e => e.ShelfStockCode).HasMaxLength(50);
            entity.Property(e => e.UnitName)
                .HasDefaultValueSql("'Kg'::text")
                .HasComment("Đơn vị tính");
            entity.Property(e => e.UpdatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.code).HasColumnType("citext");
            entity.Property(e => e.lotKey).HasColumnType("citext");
            entity.Property(e => e.qtyKg).HasPrecision(18, 3);
            entity.Property(e => e.stockType).HasDefaultValue(0);

            entity.HasOne(d => d.Slot).WithMany(p => p.WarehouseShelfStocks)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK_WarehouseShelfStock_WarehouseShelf");
        });

        modelBuilder.Entity<WarehouseStockFifoLayer>(entity =>
        {
            entity.HasKey(e => e.layerId).HasName("WarehouseStockFifoLayer_pkey");

            entity.ToTable("WarehouseStockFifoLayer", "Warehouse");

            entity.HasIndex(e => e.inLedgerId, "ux_wh_fifo_layer_inledger").IsUnique();

            entity.Property(e => e.layerId).UseIdentityAlwaysColumn();
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.inAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.lotNumber).HasDefaultValueSql("''::text");
            entity.Property(e => e.qtyInKg).HasPrecision(18, 3);
            entity.Property(e => e.qtyUsedKg).HasPrecision(18, 3);
            entity.Property(e => e.remainingKg)
                .HasPrecision(18, 3)
                .HasComputedColumnSql("round((\"qtyInKg\" - \"qtyUsedKg\"), 3)", true);
        });

        modelBuilder.Entity<WarehouseTempStock>(entity =>
        {
            entity.HasKey(e => e.tempId).HasName("PK__WarehouseTempStock__tempId");

            entity.ToTable("WarehouseTempStock", "Warehouse");

            entity.HasIndex(e => new { e.companyId, e.vaCode }, "IX_WarehouseTempStock_company_va");

            entity.HasIndex(e => e.createdBy, "IX_WarehouseTempStock_createdBy");

            entity.Property(e => e.tempId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.QtyUsed).HasPrecision(18, 3);
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.code).HasColumnType("citext");
            entity.Property(e => e.lotKey).HasColumnType("citext");
            entity.Property(e => e.qtyRequest).HasPrecision(18, 3);
            entity.Property(e => e.vaCode).HasColumnType("citext");
        });

        modelBuilder.Entity<WarehouseVoucher>(entity =>
        {
            entity.HasKey(e => e.voucherId).HasName("PK__WarehouseVouchers__voucherId");

            entity.ToTable("WarehouseVouchers", "Warehouse");

            entity.HasIndex(e => e.createdBy, "IX_WarehouseVouchers_createdBy");

            entity.HasIndex(e => e.requestId, "IX_WarehouseVouchers_requestId");

            entity.HasIndex(e => new { e.companyId, e.createdDate }, "ix_vouchers_company_created");

            entity.HasIndex(e => new { e.companyId, e.voucherCode }, "ux_vouchers_company_code").IsUnique();

            entity.Property(e => e.voucherId).UseIdentityAlwaysColumn();
            entity.Property(e => e.createdDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.voucherCode).HasColumnType("citext");

            entity.HasOne(d => d.request).WithMany(p => p.WarehouseVouchers)
                .HasForeignKey(d => d.requestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WarehouseVouchers_Request");
        });

        modelBuilder.Entity<WarehouseVoucherDetail>(entity =>
        {
            entity.HasKey(e => e.voucherDetailId).HasName("PK__WarehouseVoucherDetails__voucherDetailId");

            entity.ToTable("WarehouseVoucherDetails", "Warehouse");

            entity.HasIndex(e => e.purposeId, "IX_WarehouseVoucherDetails_purposeId");

            entity.HasIndex(e => e.slotId, "ix_voucherdetails_slot");

            entity.HasIndex(e => new { e.voucherId, e.lineNo }, "ux_voucherdetails_voucher_lineno").IsUnique();

            entity.Property(e => e.voucherDetailId).UseIdentityAlwaysColumn();
            entity.Property(e => e.UnitName).HasDefaultValueSql("'Kg'::text");
            entity.Property(e => e.expirydate)
                .HasDefaultValueSql("'-infinity'::timestamp without time zone")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.isApplied).HasDefaultValue(false);
            entity.Property(e => e.lotNumber).HasColumnType("citext");
            entity.Property(e => e.productCode).HasColumnType("citext");
            entity.Property(e => e.qtyKg).HasPrecision(10, 2);
            entity.Property(e => e.voucherType).HasDefaultValue(0);

            entity.HasOne(d => d.purpose).WithMany(p => p.WarehouseVoucherDetails)
                .HasForeignKey(d => d.purposeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WarehouseVoucherDetails_Purpose");

            entity.HasOne(d => d.slot).WithMany(p => p.WarehouseVoucherDetails)
                .HasForeignKey(d => d.slotId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WarehouseVoucherDetails_Slot");

            entity.HasOne(d => d.voucher).WithMany(p => p.WarehouseVoucherDetails)
                .HasForeignKey(d => d.voucherId)
                .HasConstraintName("FK_WarehouseVoucherDetails_Voucher");
        });

        modelBuilder.Entity<v_ShelfHistory_Canonical>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_ShelfHistory_Canonical", "Warehouse");

            entity.Property(e => e.AfterKg).HasPrecision(18, 3);
            entity.Property(e => e.AssignedByName).HasColumnType("citext");
            entity.Property(e => e.BeforeKg).HasPrecision(18, 3);
            entity.Property(e => e.DeltaKg).HasPrecision(18, 3);
            entity.Property(e => e.InKg).HasPrecision(18, 3);
            entity.Property(e => e.OutKg).HasPrecision(18, 3);
            entity.Property(e => e.ProductName).HasColumnType("citext");
            entity.Property(e => e.SlotCode).HasMaxLength(64);
            entity.Property(e => e.purposeCode).HasColumnType("citext");
        });

        modelBuilder.Entity<v_ShelfStockCurrent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_ShelfStockCurrent", "Warehouse");

            entity.Property(e => e.FreeCapacityKg).HasPrecision(10, 2);
            entity.Property(e => e.LotNo).HasMaxLength(50);
            entity.Property(e => e.ProductCode).HasColumnType("citext");
            entity.Property(e => e.ProductName).HasColumnType("citext");
            entity.Property(e => e.SlotCode).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.currentWeightKg).HasPrecision(10, 2);
            entity.Property(e => e.lotKey).HasColumnType("citext");
            entity.Property(e => e.maxWeightKg).HasPrecision(10, 2);
            entity.Property(e => e.qtyKg).HasPrecision(18, 3);
        });

        modelBuilder.Entity<v_StockByProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_StockByProduct", "Warehouse");

            entity.Property(e => e.ProductCode).HasColumnType("citext");
            entity.Property(e => e.ProductName).HasColumnType("citext");
            entity.Property(e => e.lotKey).HasColumnType("citext");
        });

        modelBuilder.Entity<v_outbound_due_line>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_outbound_due_lines", "Warehouse");

            entity.Property(e => e.delivered_qty).HasPrecision(18, 3);
            entity.Property(e => e.due_ts).HasColumnType("timestamp without time zone");
            entity.Property(e => e.expected_qty).HasPrecision(18, 3);
            entity.Property(e => e.last_pgh_ts).HasColumnType("timestamp without time zone");
            entity.Property(e => e.max_mfg_expected_ts).HasColumnType("timestamp without time zone");
            entity.Property(e => e.remain_qty).HasPrecision(18, 3);
        });

        modelBuilder.Entity<vw_RequestDetailsProgress>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_RequestDetailsProgress", "Warehouse");

            entity.Property(e => e.DoneKg).HasPrecision(18, 3);
            entity.Property(e => e.RemainingKg).HasPrecision(18, 3);
            entity.Property(e => e.WeightKg).HasPrecision(18, 3);
        });
        modelBuilder.HasSequence("DepartmentTempStockTransferDetailSeq", "Warehouse");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
