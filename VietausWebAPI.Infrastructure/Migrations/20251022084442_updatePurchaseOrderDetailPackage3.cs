using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderDetailPackage3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestDate",
                schema: "Orders",
                table: "PurchaseOrders",
                newName: "RequestDeliveryDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "boolean",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RealDeliveryDate",
                schema: "Orders",
                table: "PurchaseOrders",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "RealDeliveryDate",
                schema: "Orders",
                table: "PurchaseOrders");

            migrationBuilder.RenameColumn(
                name: "RequestDeliveryDate",
                schema: "Orders",
                table: "PurchaseOrders",
                newName: "RequestDate");
        }
    }
}
