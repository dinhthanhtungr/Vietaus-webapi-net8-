using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateElectrostaticinColourChips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "anti_static_info",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.AddColumn<bool>(
                name: "electrostatic",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "electrostatic",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.AddColumn<string>(
                name: "anti_static_info",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
