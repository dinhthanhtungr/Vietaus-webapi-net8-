using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentDeadline",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderDetailId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrderDetails",
                principalColumn: "MerchandiseOrderDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "PaymentDeadline",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "Receiver",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");
        }
    }
}
