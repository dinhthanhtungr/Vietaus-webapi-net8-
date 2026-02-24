using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFormulaMaterial13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "item_type",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "text",
                nullable: true);
        }
    }
}
