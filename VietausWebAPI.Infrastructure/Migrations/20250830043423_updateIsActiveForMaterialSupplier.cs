using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateIsActiveForMaterialSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "inventory",
                table: "Materials_Suppliers",
                type: "boolean",
                nullable: true,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "inventory",
                table: "Materials_Suppliers");
        }
    }
}
