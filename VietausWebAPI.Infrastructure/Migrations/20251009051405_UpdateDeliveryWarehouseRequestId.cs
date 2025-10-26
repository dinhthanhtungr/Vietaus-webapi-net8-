using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryWarehouseRequestId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "WarehouseRequestRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_WarehouseRequest_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "WarehouseRequestRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_WarehouseRequest_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreateBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true);
        }
    }
}
