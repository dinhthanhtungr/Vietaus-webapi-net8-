using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateColorchip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "color_code",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "product_name",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "color_code",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records");

            migrationBuilder.DropColumn(
                name: "product_name",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records");
        }
    }
}
