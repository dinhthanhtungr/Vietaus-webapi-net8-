using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCOAExpiryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "weight",
                schema: "devandqa",
                table: "ProductInspection",
                type: "numeric(16,3)",
                precision: 16,
                scale: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "weight",
                schema: "devandqa",
                table: "ProductInspection",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,3)",
                oldPrecision: 16,
                oldScale: 3,
                oldNullable: true);
        }
    }
}
