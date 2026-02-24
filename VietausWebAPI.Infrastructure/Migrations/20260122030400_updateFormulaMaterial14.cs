using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFormulaMaterial14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "item_type",
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
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropColumn(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "item_type",
                schema: "SampleRequests",
                table: "FormulaMaterials");
        }
    }
}
