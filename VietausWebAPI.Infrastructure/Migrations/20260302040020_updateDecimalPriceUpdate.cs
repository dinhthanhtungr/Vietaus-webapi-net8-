using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDecimalPriceUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceAgreed",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPriceAgreed",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "oldPrice",
                schema: "Material",
                table: "PriceHistory",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceAgreed",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPriceAgreed",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "RecommendedUnitPrice",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(22,6)",
                precision: 22,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceAgreed",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPriceAgreed",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "oldPrice",
                schema: "Material",
                table: "PriceHistory",
                type: "numeric(18,4)",
                precision: 18,
                scale: 4,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPriceAgreed",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPriceAgreed",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "RecommendedUnitPrice",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots",
                type: "numeric(16,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots",
                type: "numeric(16,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "unit_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_price",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(22,6)",
                oldPrecision: 22,
                oldScale: 6);
        }
    }
}
