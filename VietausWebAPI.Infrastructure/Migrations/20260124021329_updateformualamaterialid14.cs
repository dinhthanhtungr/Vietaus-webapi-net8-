using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateformualamaterialid14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "totalPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,6)",
                oldPrecision: 18,
                oldScale: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "totalPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldPrecision: 18,
                oldScale: 10);
        }
    }
}
