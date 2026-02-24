using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMfgProductionOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_formula_id",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "formula_id");

            migrationBuilder.AddForeignKey(
                name: "FK__Mpo__formulaId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "formula_id",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mpo__formulaId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_formula_id",
                schema: "manufacturing",
                table: "MfgProductionOrders");
        }
    }
}
