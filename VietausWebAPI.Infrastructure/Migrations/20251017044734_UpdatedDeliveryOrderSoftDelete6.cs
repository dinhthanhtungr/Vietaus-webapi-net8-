using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeliveryOrderSoftDelete6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequest_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequest_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequest_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "Warehouse",
                table: "WarehouseRequest");
        }
    }
}
