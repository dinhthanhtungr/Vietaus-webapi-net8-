using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateGetMfgProductionOrder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "comment",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "plpuNote");

            migrationBuilder.AddColumn<string>(
                name: "labNote",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "labNote",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.RenameColumn(
                name: "plpuNote",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "comment");
        }
    }
}
