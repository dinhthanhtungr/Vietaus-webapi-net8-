using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedeliveryOrderPause : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryPauseReason",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryPauseType",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryPausedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryPausedFrom",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryPausedTo",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryPaused",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_MO_Company_DeliveryPaused",
                schema: "Orders",
                table: "MerchandiseOrders",
                columns: new[] { "CompanyId", "IsDeliveryPaused" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MO_Company_DeliveryPaused",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryPauseReason",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryPauseType",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryPausedBy",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryPausedFrom",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryPausedTo",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "IsDeliveryPaused",
                schema: "Orders",
                table: "MerchandiseOrders");
        }
    }
}
