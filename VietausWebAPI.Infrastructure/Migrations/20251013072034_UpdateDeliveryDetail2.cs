using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryDetail2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrder_WarehouseRequest",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestCodeSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MerchandiseOrderDetails_MerchandiseOrderDetailId_Merchandis~",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                columns: new[] { "MerchandiseOrderDetailId", "MerchandiseOrderId" });

            migrationBuilder.CreateTable(
                name: "DeliveryOrderPOs",
                columns: table => new
                {
                    DeliveryOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseRequestId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOrderPOs", x => new { x.DeliveryOrderId, x.MerchandiseOrderId });
                    table.ForeignKey(
                        name: "FK_DeliveryOrderPOs_DeliveryOrders_DeliveryOrderId",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryOrderPOs_MerchandiseOrders_MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryOrderPOs_WarehouseRequest_WarehouseRequestId",
                        column: x => x.WarehouseRequestId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseRequest",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_DeliveryOrderId_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                columns: new[] { "DeliveryOrderId", "MerchandiseOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId_MerchandiseOrd~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                columns: new[] { "MerchandiseOrderDetailId", "MerchandiseOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderPOs_MerchandiseOrderId",
                table: "DeliveryOrderPOs",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderPOs_WarehouseRequestId",
                table: "DeliveryOrderPOs",
                column: "WarehouseRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPOs_DeliveryOrderId_Mercha~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                columns: new[] { "DeliveryOrderId", "MerchandiseOrderId" },
                principalTable: "DeliveryOrderPOs",
                principalColumns: new[] { "DeliveryOrderId", "MerchandiseOrderId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrders_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "DeliveryOrderId",
                principalSchema: "DeliveryOrder",
                principalTable: "DeliveryOrders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                columns: new[] { "MerchandiseOrderDetailId", "MerchandiseOrderId" },
                principalSchema: "Orders",
                principalTable: "MerchandiseOrderDetails",
                principalColumns: new[] { "MerchandiseOrderDetailId", "MerchandiseOrderId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPOs_DeliveryOrderId_Mercha~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrders_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropTable(
                name: "DeliveryOrderPOs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MerchandiseOrderDetails_MerchandiseOrderDetailId_Merchandis~",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetail_DeliveryOrderId_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId_MerchandiseOrd~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.AddColumn<string>(
                name: "WarehouseRequestCodeSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "WarehouseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "DeliveryOrderId",
                principalSchema: "DeliveryOrder",
                principalTable: "DeliveryOrders",
                principalColumn: "ID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrder_WarehouseRequest",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "WarehouseRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId");
        }
    }
}
