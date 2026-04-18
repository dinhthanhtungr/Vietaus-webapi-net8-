using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColorChipMfgProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_Formulas_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_manufacturing_formulas_ManufacturingForm~",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "ManufacturingFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_Formulas_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_manufacturing_formulas_ManufacturingForm~",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "ManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId");
        }
    }
}
