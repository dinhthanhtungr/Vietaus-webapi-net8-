using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSchedual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrdersSchedules",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "PurchaseOrderStatusHistory",
                schema: "Orders");

            migrationBuilder.DropColumn(
                name: "BagType",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "CustomerRequiredDate",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductCustomerExternalId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductName",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.RenameColumn(
                name: "CustomerExternalId",
                schema: "Schedual",
                table: "SchedualMfg",
                newName: "BTPStatus");

            migrationBuilder.AlterColumn<string>(
                name: "requirement",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductExpiryType",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Area",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Idpk",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReachStandard",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StepOfProduct",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchedualMfg_MfgProductionOrderId",
                schema: "Schedual",
                table: "SchedualMfg",
                column: "MfgProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SchedualMfg_ProductId",
                schema: "Schedual",
                table: "SchedualMfg",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "fk_SchedualMfg_MfgProductionOrder_id",
                schema: "Schedual",
                table: "SchedualMfg",
                column: "MfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_SchedualMfg_Product_id",
                schema: "Schedual",
                table: "SchedualMfg",
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
                name: "fk_SchedualMfg_MfgProductionOrder_id",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropForeignKey(
                name: "fk_SchedualMfg_Product_id",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropIndex(
                name: "IX_SchedualMfg_MfgProductionOrderId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropIndex(
                name: "IX_SchedualMfg_ProductId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "Area",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "Idpk",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ReachStandard",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "StepOfProduct",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.RenameColumn(
                name: "BTPStatus",
                schema: "Schedual",
                table: "SchedualMfg",
                newName: "CustomerExternalId");

            migrationBuilder.AlterColumn<string>(
                name: "requirement",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductExpiryType",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BagType",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CustomerRequiredDate",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCustomerExternalId",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseOrdersSchedules",
                schema: "Orders",
                columns: table => new
                {
                    PurchaseOrdersScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__658C1532BFBEF664", x => x.PurchaseOrdersScheduleId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersSchedules_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Orders",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderStatusHistory",
                schema: "Orders",
                columns: table => new
                {
                    StatusHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    StatusFrom = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    StatusTo = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__DB973491370CD24F", x => x.StatusHistoryId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderStatusHistory_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderStatusHistory_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Orders",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrdersSchedules_PurchaseOrderId",
                schema: "Orders",
                table: "PurchaseOrdersSchedules",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderStatusHistory_EmployeeId",
                schema: "Orders",
                table: "PurchaseOrderStatusHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderStatusHistory_PurchaseOrderId",
                schema: "Orders",
                table: "PurchaseOrderStatusHistory",
                column: "PurchaseOrderId");
        }
    }
}
