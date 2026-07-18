using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertNewAdjustmentMfgFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaAdjustments",
                schema: "manufacturing",
                columns: table => new
                {
                    manufacturingFormulaAdjustmentId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    mfg_production_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    manufacturing_formula_id = table.Column<Guid>(type: "uuid", nullable: true),
                    manufacturing_formula_version_id = table.Column<Guid>(type: "uuid", nullable: true),
                    batch_no = table.Column<int>(type: "integer", nullable: false),
                    trial_no = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "citext", nullable: false, defaultValue: "Draft"),
                    is_additional_batch = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    batch_quantity = table.Column<decimal>(type: "numeric(16,4)", precision: 16, scale: 4, nullable: true),
                    apply_to_next_batches = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaAdjustments__manufacturingFormulaAdjustmentId", x => x.manufacturingFormulaAdjustmentId);
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                        column: x => x.manufacturing_formula_version_id,
                        principalSchema: "manufacturing",
                        principalTable: "ManufacturingFormulaVersions",
                        principalColumn: "manufacturingFormulaVersionId");
                    table.ForeignKey(
                        name: "FK__Mfa__companyId",
                        column: x => x.company_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfa__createdBy",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfa__manufacturingFormulaId",
                        column: x => x.manufacturing_formula_id,
                        principalSchema: "manufacturing",
                        principalTable: "manufacturing_formulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfa__mfgProductionOrderId",
                        column: x => x.mfg_production_order_id,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfa__updatedBy",
                        column: x => x.updated_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaAdjustmentItems",
                schema: "manufacturing",
                columns: table => new
                {
                    manufacturingFormulaAdjustmentItemId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    manufacturing_formula_adjustment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    adjustment_type = table.Column<string>(type: "citext", nullable: false),
                    base_quantity = table.Column<decimal>(type: "numeric(12,10)", precision: 12, scale: 10, nullable: true),
                    adjusted_quantity = table.Column<decimal>(type: "numeric(12,10)", precision: 12, scale: 10, nullable: true),
                    delta_quantity = table.Column<decimal>(type: "numeric(12,10)", precision: 12, scale: 10, nullable: true),
                    unit = table.Column<string>(type: "text", nullable: true),
                    lot_no = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    line_no = table.Column<int>(type: "integer", nullable: false),
                    material_name_snapshot = table.Column<string>(type: "text", nullable: true),
                    material_externalid_snapshot = table.Column<string>(type: "text", nullable: true),
                    product_name_snapshot = table.Column<string>(type: "text", nullable: true),
                    product_externalid_snapshot = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaAdjustmentItems__manufacturingFormulaAdjustmentItemId", x => x.manufacturingFormulaAdjustmentItemId);
                    table.CheckConstraint("ck_mfg_formula_adjustment_items_one_source", "(material_id IS NOT NULL AND product_id IS NULL) OR (material_id IS NULL AND product_id IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK__Mfai__categoryId",
                        column: x => x.category_id,
                        principalSchema: "Material",
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfai__manufacturingFormulaAdjustmentId",
                        column: x => x.manufacturing_formula_adjustment_id,
                        principalSchema: "manufacturing",
                        principalTable: "ManufacturingFormulaAdjustments",
                        principalColumn: "manufacturingFormulaAdjustmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Mfai__materialId",
                        column: x => x.material_id,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfai__productId",
                        column: x => x.product_id,
                        principalSchema: "SampleRequests",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustmentItems_category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustmentItems_material_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustmentItems_product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustment_items_adjustment_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                column: "manufacturing_formula_adjustment_id");

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustment_items_adjustment_line",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                columns: new[] { "manufacturing_formula_adjustment_id", "line_no" });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustments_created_by",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustments_manufacturing_formula_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "manufacturing_formula_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustments_manufacturing_formula_versi~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "manufacturing_formula_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustments_mfg_production_order_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "mfg_production_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustments_updated_by",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustments_company_order_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                columns: new[] { "company_id", "mfg_production_order_id", "batch_no", "trial_no" });

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustments_company_status_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                columns: new[] { "company_id", "status", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturingFormulaAdjustmentItems",
                schema: "manufacturing");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulaAdjustments",
                schema: "manufacturing");
        }
    }
}
