using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeliveryOrderSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "codeFromRequest",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codeFromRequest",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryOrderPO");
        }
    }
}
