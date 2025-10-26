using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufactoryImportNewTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "VUFormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId",
                principalSchema: "labs",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK__Mf__VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "VUFormulaId",
                principalSchema: "labs",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK__Mf__VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "FormulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");
        }
    }
}
