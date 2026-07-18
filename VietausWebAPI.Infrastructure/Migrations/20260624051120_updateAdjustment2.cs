using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAdjustment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_mfg_formula_adjustment_batches_adjustment_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches");

            migrationBuilder.AddColumn<string>(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                type: "citext",
                nullable: false,
                defaultValue: "AddQuantity");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_adjustment_batches_adjustment_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                columns: new[] { "manufacturing_formula_adjustment_id", "batch_no", "trial_no" },
                unique: true,
                filter: "is_active = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_mfg_formula_adjustment_batches_adjustment_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches");

            migrationBuilder.DropColumn(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_adjustment_batches_adjustment_batch_trial",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                columns: new[] { "manufacturing_formula_adjustment_id", "batch_no", "trial_no" },
                unique: true);
        }
    }
}
