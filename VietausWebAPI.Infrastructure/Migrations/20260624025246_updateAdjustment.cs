using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mfai__manufacturingFormulaAdjustmentId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropIndex(
                name: "ix_mfg_formula_adjustments_company_order_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "ManufacturingFormulaVersionId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "apply_to_next_batches",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "batch_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "batch_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "is_additional_batch",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "trial_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.RenameColumn(
                name: "manufacturing_formula_adjustment_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                newName: "manufacturing_formula_adjustment_batch_id");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formula_adjustment_items_adjustment_line",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                newName: "ix_mfg_formula_adjustment_items_batch_line");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formula_adjustment_items_adjustment_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                newName: "ix_mfg_formula_adjustment_items_batch_id");

            migrationBuilder.AddColumn<int>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "lot_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "citext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaAdjustmentBatches",
                schema: "manufacturing",
                columns: table => new
                {
                    manufacturingFormulaAdjustmentBatchId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    manufacturing_formula_adjustment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    batch_no = table.Column<int>(type: "integer", nullable: false),
                    trial_no = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "citext", nullable: false, defaultValue: "Draft"),
                    is_additional_batch = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    batch_quantity = table.Column<decimal>(type: "numeric(16,4)", precision: 16, scale: 4, nullable: true),
                    apply_to_next_batches = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaAdjustmentBatches__manufacturingFormulaAdjustmentBatchId", x => x.manufacturingFormulaAdjustmentBatchId);
                    table.ForeignKey(
                        name: "FK__Mfab__createdBy",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfab__manufacturingFormulaAdjustmentId",
                        column: x => x.manufacturing_formula_adjustment_id,
                        principalSchema: "manufacturing",
                        principalTable: "ManufacturingFormulaAdjustments",
                        principalColumn: "manufacturingFormulaAdjustmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Mfab__updatedBy",
                        column: x => x.updated_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustments_company_order_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                columns: new[] { "company_id", "mfg_production_order_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustmentBatches_created_by",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustmentBatches_updated_by",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustment_batches_adjustment_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                column: "manufacturing_formula_adjustment_id");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_adjustment_batches_adjustment_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                columns: new[] { "manufacturing_formula_adjustment_id", "batch_no", "trial_no" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Mfai__manufacturingFormulaAdjustmentBatchId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                column: "manufacturing_formula_adjustment_batch_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaAdjustmentBatches",
                principalColumn: "manufacturingFormulaAdjustmentBatchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mfai__manufacturingFormulaAdjustmentBatchId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulaAdjustmentBatches",
                schema: "manufacturing");

            migrationBuilder.DropIndex(
                name: "ix_mfg_formula_adjustments_company_order_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems");

            migrationBuilder.DropColumn(
                name: "lot_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems");

            migrationBuilder.RenameColumn(
                name: "manufacturing_formula_adjustment_batch_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                newName: "manufacturing_formula_adjustment_id");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formula_adjustment_items_batch_line",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                newName: "ix_mfg_formula_adjustment_items_adjustment_line");

            migrationBuilder.RenameIndex(
                name: "ix_mfg_formula_adjustment_items_batch_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                newName: "ix_mfg_formula_adjustment_items_adjustment_id");

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturingFormulaVersionId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "citext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "apply_to_next_batches",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "batch_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "batch_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "numeric(16,4)",
                precision: 16,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_additional_batch",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "trial_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "ManufacturingFormulaVersionId");

            migrationBuilder.CreateIndex(
                name: "ix_mfg_formula_adjustments_company_order_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                columns: new[] { "company_id", "mfg_production_order_id", "batch_no", "trial_no" });

            migrationBuilder.AddForeignKey(
                name: "FK__Mfai__manufacturingFormulaAdjustmentId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                column: "manufacturing_formula_adjustment_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaAdjustments",
                principalColumn: "manufacturingFormulaAdjustmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "ManufacturingFormulaVersionId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId");
        }
    }
}
