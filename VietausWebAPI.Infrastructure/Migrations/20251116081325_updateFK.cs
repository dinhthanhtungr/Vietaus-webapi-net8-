using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_mf_version_items_version",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.RenameIndex(
                name: "ux_mf_version_items_version_material",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                newName: "ux_mfm_version_items_version_material");

            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaVersionItems_category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaVersionItems_materialId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "materialId");

            migrationBuilder.AddForeignKey(
                name: "FK__Mfm__categoryId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "category_id",
                principalSchema: "Material",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Mfm__manufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "manufacturingFormulaVersionId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Mfm__materialId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "materialId",
                principalSchema: "Material",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mfm__categoryId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK__Mfm__manufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropForeignKey(
                name: "FK__Mfm__materialId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaVersionItems_category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaVersionItems_materialId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropColumn(
                name: "category_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.RenameIndex(
                name: "ux_mfm_version_items_version_material",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                newName: "ux_mf_version_items_version_material");

            migrationBuilder.AddForeignKey(
                name: "fk_mf_version_items_version",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "manufacturingFormulaVersionId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
