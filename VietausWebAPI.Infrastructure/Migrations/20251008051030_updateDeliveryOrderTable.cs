using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDeliveryOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DeliveryOrder");

            migrationBuilder.EnsureSchema(
                name: "DeliveryOrderDetail");

            migrationBuilder.CreateTable(
                name: "DelivererInfor",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DelivererType = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DelivererInfor__3214EC27B1E3D8F1", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOrders",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "citext", nullable: true),
                    MerchandiseOrderExternalIdSnapShot = table.Column<string>(type: "citext", nullable: true),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerExternalIdSnapShot = table.Column<string>(type: "citext", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExportExternalIdSnapShot = table.Column<string>(type: "citext", nullable: true),
                    ExportId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DeliveryOrder__A2F6B5D8D1C1E3E3", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeliveryOrder_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_DeliveryOrder_CreatedBy",
                        column: x => x.CreateBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_DeliveryOrder_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_DeliveryOrder_MerchandiseOrder",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId");
                });

            migrationBuilder.CreateTable(
                name: "Deliverer",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DeliveryOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DelivererInforId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Deliverer__3214EC27D1AFC3E3", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Deliverer_DelivererInforId",
                        column: x => x.DelivererInforId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DelivererInfor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deliverer_DeliveryOrderId",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOrders",
                schema: "DeliveryOrderDetail",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DeliveryOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductExternalIdSnapShot = table.Column<string>(type: "citext", nullable: true),
                    MfgProductionOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    MfgProductionOrderExternalId = table.Column<string>(type: "citext", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    NumOfBags = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DeliveryOrderDetail__A2F6B5D8D1C1E3E3", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeliveryOrderDetail_DeliveryOrder",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryOrders",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DeliveryOrderDetail_MfgProductionOrder",
                        column: x => x.MfgProductionOrderId,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId");
                    table.ForeignKey(
                        name: "FK_DeliveryOrderDetail_Product",
                        column: x => x.ProductId,
                        principalSchema: "SampleRequests",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliverer_DelivererInforId",
                schema: "DeliveryOrder",
                table: "Deliverer",
                column: "DelivererInforId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliverer_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "Deliverer",
                column: "DeliveryOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_CompanyId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_CreatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_CustomerId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_MerchandiseOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_DeliveryOrderId",
                schema: "DeliveryOrderDetail",
                table: "DeliveryOrders",
                column: "DeliveryOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_MfgProductionOrderId",
                schema: "DeliveryOrderDetail",
                table: "DeliveryOrders",
                column: "MfgProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_ProductId",
                schema: "DeliveryOrderDetail",
                table: "DeliveryOrders",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deliverer",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryOrders",
                schema: "DeliveryOrderDetail");

            migrationBuilder.DropTable(
                name: "DelivererInfor",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryOrders",
                schema: "DeliveryOrder");
        }
    }
}
