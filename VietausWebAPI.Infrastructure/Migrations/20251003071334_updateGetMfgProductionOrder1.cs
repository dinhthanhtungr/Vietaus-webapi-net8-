using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateGetMfgProductionOrder1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantityPerBatch",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.AddColumn<string>(
                name: "QcCheck",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "qualifiedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "rejectedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "wasteQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QcCheck",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "qualifiedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "rejectedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "wasteQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.AddColumn<int>(
                name: "quantityPerBatch",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "integer",
                nullable: true);
        }
    }
}
