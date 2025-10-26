using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderDetailPackage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "DeliveryDate",
                schema: "Orders",
                table: "PurchaseOrders",
                newName: "RequestDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "BaseDateSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseDateSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "RequestDate",
                schema: "Orders",
                table: "PurchaseOrders",
                newName: "DeliveryDate");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "citext",
                nullable: true);
        }
    }
}
