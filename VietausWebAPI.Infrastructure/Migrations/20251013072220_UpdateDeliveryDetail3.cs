using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryDetail3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPOs_DeliveryOrderId_Mercha~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPOs_DeliveryOrders_DeliveryOrderId",
                table: "DeliveryOrderPOs");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPOs_MerchandiseOrders_MerchandiseOrderId",
                table: "DeliveryOrderPOs");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPOs_WarehouseRequest_WarehouseRequestId",
                table: "DeliveryOrderPOs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryOrderPOs",
                table: "DeliveryOrderPOs");

            migrationBuilder.RenameTable(
                name: "DeliveryOrderPOs",
                newName: "DeliveryOrderPO",
                newSchema: "DeliveryOrder");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderPOs_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                newName: "IX_DeliveryOrderPO_WarehouseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderPOs_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                newName: "IX_DeliveryOrderPO_MerchandiseOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryOrderPO",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                columns: new[] { "DeliveryOrderId", "MerchandiseOrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPO_DeliveryOrderId_Merchan~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                columns: new[] { "DeliveryOrderId", "MerchandiseOrderId" },
                principalSchema: "DeliveryOrder",
                principalTable: "DeliveryOrderPO",
                principalColumns: new[] { "DeliveryOrderId", "MerchandiseOrderId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPO_DeliveryOrders_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                column: "DeliveryOrderId",
                principalSchema: "DeliveryOrder",
                principalTable: "DeliveryOrders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPO_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPO_WarehouseRequest_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                column: "WarehouseRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPO_DeliveryOrderId_Merchan~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPO_DeliveryOrders_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPO_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPO_WarehouseRequest_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryOrderPO",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");

            migrationBuilder.RenameTable(
                name: "DeliveryOrderPO",
                schema: "DeliveryOrder",
                newName: "DeliveryOrderPOs");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderPO_WarehouseRequestId",
                table: "DeliveryOrderPOs",
                newName: "IX_DeliveryOrderPOs_WarehouseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderPO_MerchandiseOrderId",
                table: "DeliveryOrderPOs",
                newName: "IX_DeliveryOrderPOs_MerchandiseOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryOrderPOs",
                table: "DeliveryOrderPOs",
                columns: new[] { "DeliveryOrderId", "MerchandiseOrderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPOs_DeliveryOrderId_Mercha~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                columns: new[] { "DeliveryOrderId", "MerchandiseOrderId" },
                principalTable: "DeliveryOrderPOs",
                principalColumns: new[] { "DeliveryOrderId", "MerchandiseOrderId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPOs_DeliveryOrders_DeliveryOrderId",
                table: "DeliveryOrderPOs",
                column: "DeliveryOrderId",
                principalSchema: "DeliveryOrder",
                principalTable: "DeliveryOrders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPOs_MerchandiseOrders_MerchandiseOrderId",
                table: "DeliveryOrderPOs",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPOs_WarehouseRequest_WarehouseRequestId",
                table: "DeliveryOrderPOs",
                column: "WarehouseRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
