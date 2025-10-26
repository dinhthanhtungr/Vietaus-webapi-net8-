using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeliveryOrderSoftDelete2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReqStatus",
                schema: "Warehouse",
                table: "WarehouseRequest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReqStatus",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "citext",
                nullable: false,
                defaultValue: "");
        }
    }
}
