using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMFGFormula1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_standard_formulas_manufacturing_formulas_Manufactur~",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_production_select_versions_manufacturing_formulas_Manufactu~",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropIndex(
                name: "IX_production_select_versions_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropIndex(
                name: "IX_product_standard_formulas_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropColumn(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropColumn(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.AddForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_production_select_versions_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "ManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_product_standard_formulas_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "ManufacturingFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_product_standard_formulas_manufacturing_formulas_Manufactur~",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "ManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId");

            migrationBuilder.AddForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_production_select_versions_manufacturing_formulas_Manufactu~",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "ManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId");

            migrationBuilder.AddForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
