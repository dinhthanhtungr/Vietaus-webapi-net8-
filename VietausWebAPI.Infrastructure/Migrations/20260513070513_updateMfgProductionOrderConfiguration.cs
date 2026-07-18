using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMfgProductionOrderConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "checked_date",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "checker",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_printed_stock",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_checker",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "checker");

            migrationBuilder.AddForeignKey(
                name: "FK__Mpo__checker",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "checker",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mpo__checker",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_checker",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "checked_date",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "checker",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "is_printed_stock",
                schema: "manufacturing",
                table: "MfgProductionOrders");
        }
    }
}
