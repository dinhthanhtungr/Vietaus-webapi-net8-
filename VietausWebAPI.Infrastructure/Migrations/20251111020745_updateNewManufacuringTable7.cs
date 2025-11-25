using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_manufacturing_formulas_MfgProductionOrders_MfgProductionOrd~",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropIndex(
                name: "IX_manufacturing_formulas_MfgProductionOrderId",
                schema: "manufacturing",
                table: "manufacturing_formulas");

            migrationBuilder.DropColumn(
                name: "MfgProductionOrderId",
                schema: "manufacturing",
                table: "manufacturing_formulas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MfgProductionOrderId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_manufacturing_formulas_MfgProductionOrderId",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                column: "MfgProductionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_manufacturing_formulas_MfgProductionOrders_MfgProductionOrd~",
                schema: "manufacturing",
                table: "manufacturing_formulas",
                column: "MfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId");
        }
    }
}
