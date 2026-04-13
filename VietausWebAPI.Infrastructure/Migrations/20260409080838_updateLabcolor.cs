using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateLabcolor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "b",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "g",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "r",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.AddColumn<decimal>(
                name: "a_value",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "numeric(10,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "b_value",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "numeric(10,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "lightness",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "numeric(10,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "a_value",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "b_value",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "lightness",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.AddColumn<int>(
                name: "b",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "g",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "r",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
