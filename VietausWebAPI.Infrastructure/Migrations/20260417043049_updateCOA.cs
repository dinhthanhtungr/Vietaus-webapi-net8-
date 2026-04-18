using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCOA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection");

            migrationBuilder.DropColumn(
                name: "IsIntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection");
        }
    }
}
