using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeliveryOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_UpdatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrder_UpdatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_Employees_EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_Employees_EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "EmployeeId1",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrder_UpdatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_Employees_EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_Employees_EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_UpdatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "CreateBy");
        }
    }
}
