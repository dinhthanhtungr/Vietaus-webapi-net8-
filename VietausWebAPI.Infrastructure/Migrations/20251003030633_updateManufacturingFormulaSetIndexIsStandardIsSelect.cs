using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingFormulaSetIndexIsStandardIsSelect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_isselect_one_per_order",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "mfgProductionOrderId",
                unique: true,
                filter: "\"isSelect\" = TRUE AND \"isActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_isstandard_one_per_vu",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "VUFormulaId",
                unique: true,
                filter: "\"isStandard\" = TRUE AND \"isActive\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_mfg_formula_isselect_one_per_order",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "ux_mfg_formula_isstandard_one_per_vu",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "VUFormulaId");
        }
    }
}
