using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpadetMaterialSupplierData_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPrice",
                schema: "inventory",
                table: "Materials_Suppliers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LastPrice",
                schema: "inventory",
                table: "Materials_Suppliers",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);
        }
    }
}
