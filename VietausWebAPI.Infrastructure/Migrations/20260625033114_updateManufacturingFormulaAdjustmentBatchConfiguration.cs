using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingFormulaAdjustmentBatchConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .OldAnnotation("Npgsql:Enum:manufacturing.adjustment_type", "set_quantity,add_quantity")
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:manufacturing.adjustment_type", "set_quantity,add_quantity")
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AddColumn<int>(
                name: "adjustment_type",
                schema: "manufacturing",
                table: "ManufacturingFormulaAdjustmentBatches",
                type: "manufacturing.adjustment_type",
                nullable: false,
                defaultValue: 0);
        }
    }
}
