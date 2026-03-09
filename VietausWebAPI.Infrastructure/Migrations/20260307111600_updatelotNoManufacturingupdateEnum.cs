using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelotNoManufacturingupdateEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE manufacturing."MfgProductionOrders"
                ALTER COLUMN step_of_product TYPE integer
                USING step_of_product::integer;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE manufacturing."MfgProductionOrders"
                ALTER COLUMN step_of_product TYPE text
                USING step_of_product::text;
            """);
        }
    }
}