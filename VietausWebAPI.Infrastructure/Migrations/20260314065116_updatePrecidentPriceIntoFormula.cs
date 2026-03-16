using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePrecidentPriceIntoFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PresidentPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ProductionPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresidentPrice",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "ProductionPrice",
                schema: "SampleRequests",
                table: "Formulas");
        }
    }
}
