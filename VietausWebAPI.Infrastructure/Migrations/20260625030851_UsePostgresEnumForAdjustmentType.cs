using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsePostgresEnumForAdjustmentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:manufacturing.adjustment_type", "set_quantity,add_quantity")
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.Sql("""
                ALTER TABLE manufacturing."ManufacturingFormulaAdjustmentBatches"
                ALTER COLUMN adjustment_type DROP DEFAULT;

                ALTER TABLE manufacturing."ManufacturingFormulaAdjustmentBatches"
                ALTER COLUMN adjustment_type TYPE manufacturing.adjustment_type
                USING (
                    CASE lower(replace(adjustment_type::text, '_', ''))
                        WHEN 'setquantity' THEN 'set_quantity'::manufacturing.adjustment_type
                        WHEN 'addquantity' THEN 'add_quantity'::manufacturing.adjustment_type
                        ELSE 'add_quantity'::manufacturing.adjustment_type
                    END
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,")
                .OldAnnotation("Npgsql:Enum:manufacturing.adjustment_type", "set_quantity,add_quantity")
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.Sql("""
                ALTER TABLE manufacturing."ManufacturingFormulaAdjustmentBatches"
                ALTER COLUMN adjustment_type TYPE citext
                USING (
                    CASE adjustment_type::text
                        WHEN 'set_quantity' THEN 'SetQuantity'
                        WHEN 'add_quantity' THEN 'AddQuantity'
                        ELSE 'AddQuantity'
                    END
                );

                ALTER TABLE manufacturing."ManufacturingFormulaAdjustmentBatches"
                ALTER COLUMN adjustment_type SET DEFAULT 'AddQuantity';
                """);
        }
    }
}
