using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_manufacturing_formulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_MfgProductionOrders_MerchandiseOrders_MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_manufacturing_formulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_manufacturing_formulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_manufacturing_formulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MfgProductionOrders_MerchandiseOrders_MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");
        }
    }
}
