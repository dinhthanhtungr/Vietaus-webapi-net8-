using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newWarehouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__WarehouseTempStock__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WarehouseShelfStock__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WareHouseRequestDetail__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.RenameColumn(
                name: "VaCode",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "vaCode");

            migrationBuilder.RenameColumn(
                name: "ReserveStatus",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "reserveStatus");

            migrationBuilder.RenameColumn(
                name: "QtyRequest",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "qtyRequest");

            migrationBuilder.RenameColumn(
                name: "LotKey",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "lotKey");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "TempId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "tempId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTempStock_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "IX_WarehouseTempStock_createdBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTempStock_CompanyId_VaCode",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "IX_WarehouseTempStock_company_va");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "StockType",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "stockType");

            migrationBuilder.RenameColumn(
                name: "QtyKg",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "qtyKg");

            migrationBuilder.RenameColumn(
                name: "LotKey",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "lotKey");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "SlotCode",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "ShelfStockCode");

            migrationBuilder.RenameColumn(
                name: "BatchNo",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "LotNo");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseShelfStock_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "IX_WarehouseShelfStock_updatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseShelfStock_CompanyId_Code_LotKey",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "IX_WarehouseShelfStock_company_code_lot");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseShelfStock_CompanyId_Code",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "IX_WarehouseShelfStock_company_code");

            migrationBuilder.RenameColumn(
                name: "WeightKg",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "weightKg");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "requestId");

            migrationBuilder.RenameColumn(
                name: "DetailId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "detailId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "RequestName",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "requestName");

            migrationBuilder.RenameColumn(
                name: "ReqType",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "reqType");

            migrationBuilder.RenameColumn(
                name: "ReqStatus",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "reqStatus");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "requestId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequest_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "IX_WarehouseRequest_updatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequest_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "IX_WarehouseRequest_createdBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequest_CompanyId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "IX_WarehouseRequest_companyId");

            migrationBuilder.AlterColumn<int>(
                name: "SlotId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddColumn<int>(
                name: "ShelfStockId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WarehouseTempStock__tempId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "tempId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__WarehouseShelfStock__slotId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "SlotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__WareHouseRequestDetail__detailId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "detailId");

            migrationBuilder.CreateTable(
                name: "UsagePurposes",
                schema: "Warehouse",
                columns: table => new
                {
                    purposeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    purposeCode = table.Column<string>(type: "citext", nullable: false),
                    purposeName = table.Column<string>(type: "text", nullable: false),
                    purposeNote = table.Column<string>(type: "text", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    isReceivable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    isPickable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UsagePurposes__purposeId", x => x.purposeId);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseShelves",
                schema: "Warehouse",
                columns: table => new
                {
                    ShelfStockId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ShelfStockCode = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false),
                    currentWeightKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    maxWeightKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    lastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseShelves_SlotId", x => x.ShelfStockId);
                    table.ForeignKey(
                        name: "FK_WarehouseShelves_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseVouchers",
                schema: "Warehouse",
                columns: table => new
                {
                    voucherId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    voucherCode = table.Column<string>(type: "citext", nullable: false),
                    voucherType = table.Column<int>(type: "integer", nullable: false),
                    requestId = table.Column<int>(type: "integer", nullable: true),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WarehouseVouchers__voucherId", x => x.voucherId);
                    table.ForeignKey(
                        name: "FK_WarehouseVouchers_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseVouchers_CreatedBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseVouchers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_WarehouseVouchers_Request",
                        column: x => x.requestId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseRequest",
                        principalColumn: "requestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseShelfLedger",
                schema: "Warehouse",
                columns: table => new
                {
                    ledgerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    voucherId = table.Column<long>(type: "bigint", nullable: true),
                    voucherDetailId = table.Column<long>(type: "bigint", nullable: true),
                    slotId = table.Column<int>(type: "integer", nullable: false),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false),
                    productCode = table.Column<string>(type: "text", nullable: true),
                    lotNumber = table.Column<string>(type: "text", nullable: true),
                    deltaKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    beforeKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    afterKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    purposeId = table.Column<int>(type: "integer", nullable: true),
                    requestCode = table.Column<string>(type: "text", nullable: true),
                    appSource = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseShelfLedger_LedgerId", x => x.ledgerId);
                    table.CheckConstraint("ck_wsledger_flow", "\"afterKg\" = \"beforeKg\" + \"deltaKg\"");
                    table.ForeignKey(
                        name: "FK_WarehouseShelfLedger_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseShelfLedger_CreatedBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseShelfLedger_SlotId",
                        column: x => x.slotId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseShelves",
                        principalColumn: "ShelfStockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseVoucherDetails",
                schema: "Warehouse",
                columns: table => new
                {
                    voucherDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    voucherId = table.Column<long>(type: "bigint", nullable: false),
                    lineNo = table.Column<int>(type: "integer", nullable: false),
                    productCode = table.Column<string>(type: "citext", nullable: false),
                    productName = table.Column<string>(type: "text", nullable: false),
                    lotNumber = table.Column<string>(type: "citext", nullable: true),
                    qtyKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    bags = table.Column<int>(type: "integer", nullable: true),
                    slotId = table.Column<int>(type: "integer", nullable: true),
                    purposeId = table.Column<int>(type: "integer", nullable: true),
                    isIncrease = table.Column<bool>(type: "boolean", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WarehouseVoucherDetails__voucherDetailId", x => x.voucherDetailId);
                    table.ForeignKey(
                        name: "FK_WarehouseVoucherDetails_Purpose",
                        column: x => x.purposeId,
                        principalSchema: "Warehouse",
                        principalTable: "UsagePurposes",
                        principalColumn: "purposeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseVoucherDetails_Slot",
                        column: x => x.slotId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseShelfStock",
                        principalColumn: "SlotId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseVoucherDetails_Voucher",
                        column: x => x.voucherId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseVouchers",
                        principalColumn: "voucherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ux_usagepurposes_purposecode",
                schema: "Warehouse",
                table: "UsagePurposes",
                column: "purposeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfLedger_createdBy",
                schema: "Warehouse",
                table: "WarehouseShelfLedger",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfLedger_slotId",
                schema: "Warehouse",
                table: "WarehouseShelfLedger",
                column: "slotId");

            migrationBuilder.CreateIndex(
                name: "ix_wsledger_company_slot_created",
                schema: "Warehouse",
                table: "WarehouseShelfLedger",
                columns: new[] { "companyId", "slotId", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "ix_wsledger_voucher",
                schema: "Warehouse",
                table: "WarehouseShelfLedger",
                column: "voucherId");

            migrationBuilder.CreateIndex(
                name: "ix_warehouseshelves_lastupdated",
                schema: "Warehouse",
                table: "WarehouseShelves",
                column: "lastUpdated");

            migrationBuilder.CreateIndex(
                name: "ux_warehouseshelves_company_slotcode",
                schema: "Warehouse",
                table: "WarehouseShelves",
                columns: new[] { "companyId", "ShelfStockCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_voucherdetails_slot",
                schema: "Warehouse",
                table: "WarehouseVoucherDetails",
                column: "slotId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseVoucherDetails_purposeId",
                schema: "Warehouse",
                table: "WarehouseVoucherDetails",
                column: "purposeId");

            migrationBuilder.CreateIndex(
                name: "ux_voucherdetails_voucher_lineno",
                schema: "Warehouse",
                table: "WarehouseVoucherDetails",
                columns: new[] { "voucherId", "lineNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_vouchers_company_created",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                columns: new[] { "companyId", "createdDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseVouchers_createdBy",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseVouchers_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseVouchers_requestId",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                column: "requestId");

            migrationBuilder.CreateIndex(
                name: "ux_vouchers_company_code",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                columns: new[] { "companyId", "voucherCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_WarehouseShelf",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "SlotId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseShelves",
                principalColumn: "ShelfStockId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_WarehouseShelf",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropTable(
                name: "WarehouseShelfLedger",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "WarehouseVoucherDetails",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "WarehouseShelves",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "UsagePurposes",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "WarehouseVouchers",
                schema: "Warehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WarehouseTempStock__tempId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WarehouseShelfStock__slotId",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WareHouseRequestDetail__detailId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropColumn(
                name: "ShelfStockId",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.RenameColumn(
                name: "vaCode",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "VaCode");

            migrationBuilder.RenameColumn(
                name: "reserveStatus",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "ReserveStatus");

            migrationBuilder.RenameColumn(
                name: "qtyRequest",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "QtyRequest");

            migrationBuilder.RenameColumn(
                name: "lotKey",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "LotKey");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "code",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "tempId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "TempId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTempStock_createdBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "IX_WarehouseTempStock_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseTempStock_company_va",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                newName: "IX_WarehouseTempStock_CompanyId_VaCode");

            migrationBuilder.RenameColumn(
                name: "updatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "stockType",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "StockType");

            migrationBuilder.RenameColumn(
                name: "qtyKg",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "QtyKg");

            migrationBuilder.RenameColumn(
                name: "lotKey",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "LotKey");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "code",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "ShelfStockCode",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "SlotCode");

            migrationBuilder.RenameColumn(
                name: "LotNo",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "BatchNo");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseShelfStock_updatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "IX_WarehouseShelfStock_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseShelfStock_company_code_lot",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "IX_WarehouseShelfStock_CompanyId_Code_LotKey");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseShelfStock_company_code",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                newName: "IX_WarehouseShelfStock_CompanyId_Code");

            migrationBuilder.RenameColumn(
                name: "weightKg",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "WeightKg");

            migrationBuilder.RenameColumn(
                name: "requestId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "RequestId");

            migrationBuilder.RenameColumn(
                name: "detailId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "DetailId");

            migrationBuilder.RenameColumn(
                name: "updatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "requestName",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "RequestName");

            migrationBuilder.RenameColumn(
                name: "reqType",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "ReqType");

            migrationBuilder.RenameColumn(
                name: "reqStatus",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "ReqStatus");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "requestId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequest_updatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "IX_WarehouseRequest_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequest_createdBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "IX_WarehouseRequest_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequest_companyId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                newName: "IX_WarehouseRequest_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "SlotId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WarehouseTempStock__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "TempId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__WarehouseShelfStock__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "SlotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__WareHouseRequestDetail__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "DetailId");
        }
    }
}
