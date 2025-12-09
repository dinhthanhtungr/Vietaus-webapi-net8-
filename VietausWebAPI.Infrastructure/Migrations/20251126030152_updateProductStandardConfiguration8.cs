using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseVouchers_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseVouchers_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseVouchers_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseVouchers_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseVouchers",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
