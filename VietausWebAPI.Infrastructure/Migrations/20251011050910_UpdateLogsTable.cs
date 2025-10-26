using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DelivererInfor",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MerchandiseOrderLogs",
                schema: "Orders",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MerchandiseOrderLogs__LogId", x => x.LogId);
                    table.ForeignKey(
                        name: "FK__MerchandiseOrderLogs__MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MerchandiseOrderLogs__createdBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MfgProductionOrderLogs",
                schema: "manufacturing",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    MfgProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MfgProductionOrderLogs__LogId", x => x.LogId);
                    table.ForeignKey(
                        name: "FK__MfgProductionOrderLogs__MfgProductionOrderId",
                        column: x => x.MfgProductionOrderId,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MfgProductionOrderLogs__createdBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderLogs_createdBy",
                schema: "Orders",
                table: "MerchandiseOrderLogs",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderLogs_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderLogs",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrderLogs_createdBy",
                schema: "manufacturing",
                table: "MfgProductionOrderLogs",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrderLogs_MfgProductionOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrderLogs",
                column: "MfgProductionOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchandiseOrderLogs",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "MfgProductionOrderLogs",
                schema: "manufacturing");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DelivererInfor");
        }
    }
}
