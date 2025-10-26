using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryWarehouseRequestId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_WarehouseRequest_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.RenameColumn(
                name: "WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "WarehouseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrders_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "IX_DeliveryOrders_WarehouseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrder_WarehouseRequest",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "WarehouseRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrder_WarehouseRequest",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.RenameColumn(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "WarehouseRequestRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrders_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "IX_DeliveryOrders_WarehouseRequestRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_WarehouseRequest_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "WarehouseRequestRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId");
        }
    }
}
