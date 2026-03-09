using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelotNoManufacturinglineno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LineNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LineNo",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LineNo",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropColumn(
                name: "LineNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "LineNo",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots");

            migrationBuilder.DropColumn(
                name: "LineNo",
                schema: "SampleRequests",
                table: "FormulaMaterials");
        }
    }
}
