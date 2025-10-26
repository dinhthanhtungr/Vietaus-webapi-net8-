using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatemfgorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalPrice",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.AddColumn<int>(
                name: "totalQuantityRequest",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalQuantityRequest",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.AddColumn<decimal>(
                name: "totalPrice",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
