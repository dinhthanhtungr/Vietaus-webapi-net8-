using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufactoryImportNewTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "manufacturing");

            migrationBuilder.RenameTable(
                name: "MfgProductionOrders",
                newName: "MfgProductionOrders",
                newSchema: "manufacturing");

            migrationBuilder.RenameTable(
                name: "ManufacturingFormulas",
                newName: "ManufacturingFormulas",
                newSchema: "manufacturing");

            migrationBuilder.RenameTable(
                name: "ManufacturingFormulaMaterials",
                newName: "ManufacturingFormulaMaterials",
                newSchema: "manufacturing");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "MfgProductionOrders",
                schema: "manufacturing",
                newName: "MfgProductionOrders");

            migrationBuilder.RenameTable(
                name: "ManufacturingFormulas",
                schema: "manufacturing",
                newName: "ManufacturingFormulas");

            migrationBuilder.RenameTable(
                name: "ManufacturingFormulaMaterials",
                schema: "manufacturing",
                newName: "ManufacturingFormulaMaterials");
        }
    }
}
