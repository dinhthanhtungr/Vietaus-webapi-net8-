using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingFormulaMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulaMaterials_Categories_CategoryId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulaMaterials_Materials_MaterialId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaMaterials_CategoryId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulaMaterials_MaterialId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "MaterialId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_CategoryId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_MaterialId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                column: "MaterialId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulaMaterials_Categories_CategoryId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                column: "CategoryId1",
                principalSchema: "Material",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulaMaterials_Materials_MaterialId1",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                column: "MaterialId1",
                principalSchema: "Material",
                principalTable: "Materials",
                principalColumn: "MaterialId");
        }
    }
}
