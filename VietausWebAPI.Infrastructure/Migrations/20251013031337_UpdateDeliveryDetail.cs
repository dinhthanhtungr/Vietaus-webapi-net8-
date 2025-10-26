using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrder_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "MerchandiseOrderExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetails_ID",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderDetailId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrderDetails",
                principalColumn: "MerchandiseOrderDetailId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetails_ID",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "MerchandiseOrderExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "MerchandiseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrder_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");
        }
    }
}
