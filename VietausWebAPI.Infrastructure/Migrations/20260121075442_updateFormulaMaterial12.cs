using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFormulaMaterial12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "item_type",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaVersionItems_product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaMaterials_product_id",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                column: "product_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormulaMaterials_Product",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                column: "product_id",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK__Mfm__productId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                column: "product_id",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Mfm__productId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "product_id",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormulaMaterials_Product",
                schema: "SampleRequests",
                table: "FormulaMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK__Mfm__productId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK__Mfm__productId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaVersionItems_product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaMaterials_product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropIndex(
                name: "IX_FormulaMaterials_product_id",
                schema: "SampleRequests",
                table: "FormulaMaterials");

            migrationBuilder.DropColumn(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropColumn(
                name: "product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropColumn(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "product_id",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "item_type",
                schema: "SampleRequests",
                table: "FormulaMaterials");

            migrationBuilder.DropColumn(
                name: "product_id",
                schema: "SampleRequests",
                table: "FormulaMaterials");
        }
    }
}
