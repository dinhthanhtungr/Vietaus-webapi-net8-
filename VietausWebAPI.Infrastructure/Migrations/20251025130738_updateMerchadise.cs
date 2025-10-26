using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMerchadise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryActualDate",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryRequestDate",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedDeliveryDate",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.RenameColumn(
                name: "DeliveryDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                newName: "ExpectedDeliveryDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryActualDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryRequestDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryActualDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "DeliveryRequestDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "ExpectedDeliveryDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                newName: "DeliveryDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryActualDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryRequestDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedDeliveryDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true);
        }
    }
}
