using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertNewAdjustmentMfgFormula4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mfa__manufacturingFormulaVersionId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems");

            migrationBuilder.RenameColumn(
                name: "manufacturing_formula_version_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                newName: "ManufacturingFormulaVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaAdjustments_manufacturing_formula_versi~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                newName: "IX_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~");

            migrationBuilder.AddColumn<string>(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                type: "citext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                column: "ManufacturingFormulaVersionId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.DropColumn(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments");

            migrationBuilder.RenameColumn(
                name: "ManufacturingFormulaVersionId",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                newName: "manufacturing_formula_version_id");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulaAdjustments_ManufacturingFormulaVersion~",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustments",
                newName: "IX_ManufacturingFormulaAdjustments_manufacturing_formula_versi~");

            migrationBuilder.AddColumn<string>(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "citext",
                nullable: false,
                defaultValue: "");

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
    }
}
