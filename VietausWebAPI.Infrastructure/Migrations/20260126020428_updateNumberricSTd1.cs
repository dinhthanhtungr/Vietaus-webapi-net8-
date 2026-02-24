using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNumberricSTd1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unitPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "totalPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                newName: "total_price");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(12,10)",
                precision: 12,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
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
                type: "numeric(12,10)",
                precision: 12,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(12,10)",
                precision: 12,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,10)",
                oldPrecision: 16,
                oldScale: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                newName: "unitPrice");

            migrationBuilder.RenameColumn(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                newName: "totalPrice");

            migrationBuilder.RenameColumn(
                name: "quantity",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "total_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                newName: "TotalPrice");

            migrationBuilder.AlterColumn<decimal>(
                name: "quantity",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,10)",
                oldPrecision: 12,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "unitPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "totalPrice",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
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
                oldType: "numeric(12,10)",
                oldPrecision: 12,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,10)",
                oldPrecision: 12,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(16,10)",
                precision: 16,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
