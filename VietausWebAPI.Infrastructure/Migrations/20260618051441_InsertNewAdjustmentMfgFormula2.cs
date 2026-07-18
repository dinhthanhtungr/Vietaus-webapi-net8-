using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertNewAdjustmentMfgFormula2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.AlterColumn<decimal>(
                name: "delta_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,10)",
                oldPrecision: 12,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "base_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,10)",
                oldPrecision: 12,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "adjusted_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,10)",
                oldPrecision: 12,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Mfa__manufacturingFormulaVersionId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "manufacturing_formula_version_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mfa__manufacturingFormulaVersionId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.AlterColumn<decimal>(
                name: "delta_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "numeric(12,10)",
                precision: 12,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "base_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "numeric(12,10)",
                precision: 12,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "adjusted_quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "numeric(12,10)",
                precision: 12,
                scale: 10,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "manufacturing_formula_version_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId");
        }
    }
}
