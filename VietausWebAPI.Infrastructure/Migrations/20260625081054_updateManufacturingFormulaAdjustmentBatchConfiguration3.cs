using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingFormulaAdjustmentBatchConfiguration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "final_quantity_gram",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                type: "numeric(16,4)",
                precision: 16,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "taken_quantity_gram",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                type: "numeric(16,4)",
                precision: 16,
                scale: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "final_quantity_gram",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches");

            migrationBuilder.DropColumn(
                name: "taken_quantity_gram",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches");
        }
    }
}
