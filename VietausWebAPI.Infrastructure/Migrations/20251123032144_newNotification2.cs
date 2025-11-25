using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newNotification2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_Products_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

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
                name: "IX_DeliveryOrderDetail_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
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
                name: "IX_DeliveryOrderDetail_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_Products_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId1",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");

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
    }
}
