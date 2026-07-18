using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertNewAdjustmentMfgFormula3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lot_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lot_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentItems",
                type: "text",
                nullable: true);
        }
    }
}
