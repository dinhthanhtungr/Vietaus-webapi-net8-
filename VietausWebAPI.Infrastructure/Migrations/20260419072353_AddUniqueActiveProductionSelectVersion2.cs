using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveProductionSelectVersion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX ux_psv_one_pending_per_mpo
                ON manufacturing.production_select_versions (mfg_production_order_id)
                WHERE valid_to IS NULL AND valid_from IS NULL;
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP INDEX IF EXISTS manufacturing.ux_psv_one_pending_per_mpo;
            """);
        }
    }
}
