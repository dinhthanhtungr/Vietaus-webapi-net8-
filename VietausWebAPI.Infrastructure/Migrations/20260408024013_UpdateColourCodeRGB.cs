using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColourCodeRGB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
