using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderDetai4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PackageType",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PaymentDays",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RequestSourceType",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "VAT",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "RequestSourceId",
                schema: "Orders",
                table: "PurchaseOrders",
                newName: "PurchaseOrderSnapshotId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLink",
                schema: "Orders",
                columns: table => new
                {
                    PurchaseOrderLinkId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PurchaseOrderLink__5026B698A94EFE60", x => x.PurchaseOrderLinkId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLink_MerchandiseOrders_MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLink_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Orders",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderSnapshot",
                schema: "Orders",
                columns: table => new
                {
                    PurchaseOrderSnapshotId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    EmployeeExternalIdSnapshot = table.Column<string>(type: "text", nullable: false),
                    EmployeeFullNameSnapshot = table.Column<string>(type: "text", nullable: false),
                    PhoneNumberSnapshot = table.Column<string>(type: "text", nullable: true),
                    EmailSnapshot = table.Column<string>(type: "text", nullable: true),
                    SupplierExternalIdSnapshot = table.Column<string>(type: "text", nullable: true),
                    SupplierNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    SupplierContactSnapshot = table.Column<string>(type: "text", nullable: true),
                    SupplierPhoneNumberSnapshot = table.Column<string>(type: "text", nullable: true),
                    SupplierAddressSnapshot = table.Column<string>(type: "text", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    DeliveryAddress = table.Column<string>(type: "text", nullable: true),
                    PackageType = table.Column<string>(type: "text", nullable: true),
                    PaymentDays = table.Column<string>(type: "text", nullable: true),
                    Vat = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PurchaseOrderSnapshot__5026B698A94EFE60", x => x.PurchaseOrderSnapshotId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PurchaseOrderSnapshotId",
                schema: "Orders",
                table: "PurchaseOrders",
                column: "PurchaseOrderSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLink_MerchandiseOrderId",
                schema: "Orders",
                table: "PurchaseOrderLink",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLink_PurchaseOrderId",
                schema: "Orders",
                table: "PurchaseOrderLink",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderSnapshot",
                schema: "Orders",
                table: "PurchaseOrders",
                column: "PurchaseOrderSnapshotId",
                principalSchema: "Orders",
                principalTable: "PurchaseOrderSnapshot",
                principalColumn: "PurchaseOrderSnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PurchaseOrderSnapshot",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLink",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "PurchaseOrderSnapshot",
                schema: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PurchaseOrderSnapshotId",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderSnapshotId",
                schema: "Orders",
                table: "PurchaseOrders",
                newName: "RequestSourceId");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentDays",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestSourceType",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VAT",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "integer",
                nullable: true);
        }
    }
}
