using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDelivery2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
