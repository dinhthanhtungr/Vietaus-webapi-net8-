using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_MfgProductionOrders_MfgProductionOrde~",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropTable(
                name: "MfgProductionOrderLogs",
                schema: "manufacturing");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_companyId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_customerExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_productExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_status",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_status_expectedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_status_requiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.RenameTable(
                name: "ManufacturingFormulas",
                schema: "manufacturing",
                newName: "manufacturing_formulas",
                newSchema: "manufacturing");

            migrationBuilder.RenameColumn(
                name: "updatedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "updated_date");

            migrationBuilder.RenameColumn(
                name: "updatedBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "totalQuantityRequest",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "total_quantity_request");

            migrationBuilder.RenameColumn(
                name: "totalQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "total_quantity");

            migrationBuilder.RenameColumn(
                name: "productionType",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "production_type");

            migrationBuilder.RenameColumn(
                name: "productNameSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "product_name_snapshot");

            migrationBuilder.RenameColumn(
                name: "productExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "product_externalid_snapshot");

            migrationBuilder.RenameColumn(
                name: "plpuNote",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "plpu_note");

            migrationBuilder.RenameColumn(
                name: "numOfBatches",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "num_of_batches");

            migrationBuilder.RenameColumn(
                name: "manufacturingDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "manufacturing_date");

            migrationBuilder.RenameColumn(
                name: "labNote",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "lab_note");

            migrationBuilder.RenameColumn(
                name: "isActive",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "formulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "formula_externalid_snapshot");

            migrationBuilder.RenameColumn(
                name: "externalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "external_id");

            migrationBuilder.RenameColumn(
                name: "expectedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "expected_date");

            migrationBuilder.RenameColumn(
                name: "customerNameSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "customer_name_snapshot");

            migrationBuilder.RenameColumn(
                name: "customerExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "customer_externalid_snapshot");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "company_id");

            migrationBuilder.RenameColumn(
                name: "bagType",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "bag_type");

            migrationBuilder.RenameColumn(
                name: "UnitPriceAgreed",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "unit_price_agreed");

            migrationBuilder.RenameColumn(
                name: "RequiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "required_date");

            migrationBuilder.RenameColumn(
                name: "QcCheck",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "qc_check");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "product_id");

            migrationBuilder.RenameColumn(
                name: "FormulaId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "formula_id");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "created_date");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_updatedBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_updated_by");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_ProductId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_CustomerId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_createdBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_created_by");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_companyId_externalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "ux_mpo_company_externalid");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "materialNameSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "material_name_snapshot");

            migrationBuilder.RenameColumn(
                name: "materialId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "material_id");

            migrationBuilder.RenameColumn(
                name: "materialExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "material_externalid_snapshot");

            migrationBuilder.RenameColumn(
                name: "manufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "manufacturing_formula_id");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "is_active");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaMaterials_materialId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "IX_ManufacturingFormulaMaterials_material_id");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaMaterials_manufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "ix_mfm_formula_id");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaMaterials_categoryId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "IX_ManufacturingFormulaMaterials_category_id");

            migrationBuilder.RenameColumn(
                name: "updatedDate",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "updated_date");

            migrationBuilder.RenameColumn(
                name: "updatedBy",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "totalPrice",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "isActive",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "company_id");

            migrationBuilder.RenameColumn(
                name: "SourceVUFormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "source_vu_formula_id");

            migrationBuilder.RenameColumn(
                name: "SourceVUExternalIdSnapshot",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "source_vu_externalid_snapshot");

            migrationBuilder.RenameColumn(
                name: "SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "source_manufacturing_formula_id");

            migrationBuilder.RenameColumn(
                name: "SourceManufacturingExternalIdSnapshot",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "source_manufacturing_externalid_snapshot");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "external_id");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_updatedBy",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "IX_manufacturing_formulas_updated_by");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_SourceVUFormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "ix_mfg_formulas_source_vu_formula_id");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "ix_mfg_formulas_source_mfg_formula_id");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_MfgProductionOrderId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "IX_manufacturing_formulas_MfgProductionOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "IX_manufacturing_formulas_FormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_createdBy",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "ix_mfg_formulas_created_by");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_companyId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                newName: "ix_mfg_formulas_company_id");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "formula_externalid_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customer_name_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customer_externalid_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price_agreed",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "source_vu_externalid_snapshot",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "source_manufacturing_externalid_snapshot",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "external_id",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "MfgOrderPOs",
                schema: "manufacturing",
                columns: table => new
                {
                    MerchandiseOrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    MfgProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MfgOrderPOs", x => new { x.MerchandiseOrderDetailId, x.MfgProductionOrderId });
                    table.ForeignKey(
                        name: "FK_MfgOrderPOs_Detail",
                        column: x => x.MerchandiseOrderDetailId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrderDetails",
                        principalColumn: "MerchandiseOrderDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MfgOrderPOs_MfgOrder",
                        column: x => x.MfgProductionOrderId,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_standard_formulas",
                schema: "manufacturing",
                columns: table => new
                {
                    product_standard_formula_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturing_formula_id = table.Column<Guid>(type: "uuid", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    valid_to = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    closed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    note = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_standard_formulas", x => x.product_standard_formula_id);
                    table.ForeignKey(
                        name: "fk_psf_closed_by",
                        column: x => x.closed_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_psf_company",
                        column: x => x.company_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_psf_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_psf_manufacturing_formula",
                        column: x => x.manufacturing_formula_id,
                        principalSchema: "manufacturing",
                        principalTable: "manufacturing_formulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_psf_product",
                        column: x => x.product_id,
                        principalSchema: "SampleRequests",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "production_select_versions",
                schema: "manufacturing",
                columns: table => new
                {
                    production_select_version_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    mfg_production_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturing_formula_id = table.Column<Guid>(type: "uuid", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    valid_to = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    closed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_production_select_versions", x => x.production_select_version_id);
                    table.ForeignKey(
                        name: "fk_psv_closed_by",
                        column: x => x.closed_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_psv_company",
                        column: x => x.company_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_psv_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_psv_manufacturing_formula",
                        column: x => x.manufacturing_formula_id,
                        principalSchema: "manufacturing",
                        principalTable: "manufacturing_formulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_psv_mfg_production_order",
                        column: x => x.mfg_production_order_id,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_mpo_company_active_createddesc",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "company_id", "is_active", "created_date", "mfgProductionOrderId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_mpo_company_product_active",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "company_id", "product_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_mpo_company_status_expecteddesc",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "company_id", "status", "expected_date", "mfgProductionOrderId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_mpo_company_status_requireddesc",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "company_id", "status", "required_date", "mfgProductionOrderId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_mfm_formula_material",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                columns: new[] { "manufacturing_formula_id", "material_id" });

            migrationBuilder.CreateIndex(
                name: "ux_mfm_formula_material_unique_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                columns: new[] { "manufacturing_formula_id", "material_id", "category_id" },
                unique: true,
                filter: "\"is_active\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formulas_company_active_created_desc",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                columns: new[] { "company_id", "is_active", "created_date", "manufacturingFormulaId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formulas_company_external_id",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                columns: new[] { "company_id", "external_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MfgOrderPOs_DetailId",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                column: "MerchandiseOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgOrderPOs_MfgOrderId",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                column: "MfgProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "UX_MfgOrderPOs_Detail_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                columns: new[] { "MerchandiseOrderDetailId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "UX_MfgOrderPOs_MfgOrder_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                columns: new[] { "MfgProductionOrderId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_product_standard_formulas_closed_by",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "closed_by");

            migrationBuilder.CreateIndex(
                name: "IX_product_standard_formulas_created_by",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_psf_company",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_psf_formula",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "manufacturing_formula_id");

            migrationBuilder.CreateIndex(
                name: "ix_psf_product",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_psf_product_validfrom_desc",
                schema: "manufacturing",
                table: "product_standard_formulas",
                columns: new[] { "product_id", "valid_from" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ux_psf_company_product_current",
                schema: "manufacturing",
                table: "product_standard_formulas",
                columns: new[] { "company_id", "product_id" },
                unique: true,
                filter: "\"valid_to\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_production_select_versions_closed_by",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "closed_by");

            migrationBuilder.CreateIndex(
                name: "IX_production_select_versions_created_by",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_psv_formula",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "manufacturing_formula_id");

            migrationBuilder.CreateIndex(
                name: "ix_psv_mpo",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "mfg_production_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_psv_mpo_validfrom_desc",
                schema: "manufacturing",
                table: "production_select_versions",
                columns: new[] { "mfg_production_order_id", "valid_from" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ux_psv_current_per_order",
                schema: "manufacturing",
                table: "production_select_versions",
                columns: new[] { "company_id", "mfg_production_order_id" },
                unique: true,
                filter: "\"valid_to\" IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_manufacturing_formulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_manufacturing_formulas_MfgProductionOrders_MfgProductionOrd~",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                column: "MfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_manufacturing_formulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_manufacturing_formulas_MfgProductionOrders_MfgProductionOrd~",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropTable(
                name: "MfgOrderPOs",
                schema: "manufacturing");

            migrationBuilder.DropTable(
                name: "product_standard_formulas",
                schema: "manufacturing");

            migrationBuilder.DropTable(
                name: "production_select_versions",
                schema: "manufacturing");

            migrationBuilder.DropIndex(
                name: "ix_mpo_company_active_createddesc",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "ix_mpo_company_product_active",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "ix_mpo_company_status_expecteddesc",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "ix_mpo_company_status_requireddesc",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "ix_mfm_formula_material",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropIndex(
                name: "ux_mfm_formula_material_unique_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropIndex(
                name: "ix_mfg_formulas_company_active_created_desc",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropIndex(
                name: "ux_mfg_formulas_company_external_id",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.RenameTable(
                name: "manufacturing_formulas",
                schema: "manufacturing",
                newName: "ManufacturingFormulas",
                newSchema: "manufacturing");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "updatedDate");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "unit_price_agreed",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "UnitPriceAgreed");

            migrationBuilder.RenameColumn(
                name: "total_quantity_request",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "totalQuantityRequest");

            migrationBuilder.RenameColumn(
                name: "total_quantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "totalQuantity");

            migrationBuilder.RenameColumn(
                name: "required_date",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "RequiredDate");

            migrationBuilder.RenameColumn(
                name: "qc_check",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "QcCheck");

            migrationBuilder.RenameColumn(
                name: "production_type",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "productionType");

            migrationBuilder.RenameColumn(
                name: "product_name_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "productNameSnapshot");

            migrationBuilder.RenameColumn(
                name: "product_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "product_externalid_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "productExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "plpu_note",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "plpuNote");

            migrationBuilder.RenameColumn(
                name: "num_of_batches",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "numOfBatches");

            migrationBuilder.RenameColumn(
                name: "manufacturing_date",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "manufacturingDate");

            migrationBuilder.RenameColumn(
                name: "lab_note",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "labNote");

            migrationBuilder.RenameColumn(
                name: "is_active",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "formula_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "FormulaId");

            migrationBuilder.RenameColumn(
                name: "formula_externalid_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "formulaExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "external_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "externalId");

            migrationBuilder.RenameColumn(
                name: "expected_date",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "expectedDate");

            migrationBuilder.RenameColumn(
                name: "customer_name_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "customerNameSnapshot");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "customer_externalid_snapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "customerExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "created_date",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "created_by",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "company_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "bag_type",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "bagType");

            migrationBuilder.RenameIndex(
                name: "ux_mpo_company_externalid",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_companyId_externalId");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_updated_by",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_updatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_product_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_customer_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_MfgProductionOrders_created_by",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "IX_MfgProductionOrders_createdBy");

            migrationBuilder.RenameColumn(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "material_name_snapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "materialNameSnapshot");

            migrationBuilder.RenameColumn(
                name: "material_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "materialId");

            migrationBuilder.RenameColumn(
                name: "material_externalid_snapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "materialExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "manufacturing_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "manufacturingFormulaId");

            migrationBuilder.RenameColumn(
                name: "is_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "ix_mfm_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "IX_ManufacturingFormulaMaterials_manufacturingFormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaMaterials_material_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "IX_ManufacturingFormulaMaterials_materialId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaMaterials_category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "IX_ManufacturingFormulaMaterials_categoryId");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "updatedDate");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "totalPrice");

            migrationBuilder.RenameColumn(
                name: "source_vu_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "SourceVUFormulaId");

            migrationBuilder.RenameColumn(
                name: "source_vu_externalid_snapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "SourceVUExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "source_manufacturing_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "SourceManufacturingFormulaId");

            migrationBuilder.RenameColumn(
                name: "source_manufacturing_externalid_snapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "SourceManufacturingExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "is_active",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "external_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "created_date",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "createdDate");

            migrationBuilder.RenameColumn(
                name: "created_by",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "company_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "companyId");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formulas_source_vu_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_SourceVUFormulaId");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formulas_source_mfg_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_SourceManufacturingFormulaId");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formulas_created_by",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_createdBy");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formulas_company_id",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_companyId");

            migrationBuilder.RenameIndex(
                name: "IX_manufacturing_formulas_updated_by",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_updatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_manufacturing_formulas_MfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_MfgProductionOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_manufacturing_formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_FormulaId");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldDefaultValue: "New");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceAgreed",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "formulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customerNameSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "customerExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32,
                oldDefaultValue: "New");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "SourceVUExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SourceManufacturingExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.CreateTable(
                name: "MfgProductionOrderLogs",
                schema: "manufacturing",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false),
                    MfgProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MfgProductionOrderLogs__LogId", x => x.LogId);
                    table.ForeignKey(
                        name: "FK__MfgProductionOrderLogs__MfgProductionOrderId",
                        column: x => x.MfgProductionOrderId,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MfgProductionOrderLogs__createdBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_companyId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_customerExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "customerExternalIdSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_productExternalIdSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "productExternalIdSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_status_expectedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "status", "expectedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_status_requiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "status", "RequiredDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrderLogs_createdBy",
                schema: "manufacturing",
                table: "MfgProductionOrderLogs",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrderLogs_MfgProductionOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrderLogs",
                column: "MfgProductionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_MfgProductionOrders_MfgProductionOrde~",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "MfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId");
        }
    }
}
