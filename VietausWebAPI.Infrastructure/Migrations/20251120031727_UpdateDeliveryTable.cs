using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrderPO_DeliveryOrderId_Merchan~",
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
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
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

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPO_WarehouseRequest_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");

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

            migrationBuilder.DropColumn(
                name: "MerchandiseOrderExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.RenameColumn(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                newName: "WarehouseRequestRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderPO_WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                newName: "IX_DeliveryOrderPO_WarehouseRequestRequestId");

            migrationBuilder.RenameColumn(
                name: "MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "ProductId1");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderDetails_ID",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "IX_DeliveryOrderDetail_ProductId1");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrinted",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryPrice",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAttach",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

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
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetail",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderDetailId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrderDetails",
                principalColumn: "MerchandiseOrderDetailId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_Products_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId1",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderPO_WarehouseRequest_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                column: "WarehouseRequestRequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_DeliveryOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetail",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_Products_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderPO_WarehouseRequest_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetail_MerchandiseOrderDetailId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "DeliveryPrice",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.RenameColumn(
                name: "WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                newName: "WarehouseRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderPO_WarehouseRequestRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                newName: "IX_DeliveryOrderPO_WarehouseRequestId");

            migrationBuilder.RenameColumn(
                name: "ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "MerchandiseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryOrderDetail_ProductId1",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "IX_DeliveryOrderDetails_ID");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<bool>(
                name: "HasPrinted",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsAttach",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MerchandiseOrderExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "citext",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MerchandiseOrderDetails_MerchandiseOrderDetailId_Merchandis~",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                columns: new[] { "MerchandiseOrderDetailId", "MerchandiseOrderId" });

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
                name: "FK_DeliveryOrderDetail_MerchandiseOrderDetails_MerchandiseOrde~",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderDetailId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrderDetails",
                principalColumn: "MerchandiseOrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MerchandiseOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_Product",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
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
    }
}
