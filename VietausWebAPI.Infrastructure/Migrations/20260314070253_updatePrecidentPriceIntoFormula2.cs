using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePrecidentPriceIntoFormula2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProductionPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "PresidentPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveDate",
                schema: "SampleRequests",
                table: "Formulas",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveDate",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.AlterColumn<decimal>(
                name: "ProductionPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PresidentPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
