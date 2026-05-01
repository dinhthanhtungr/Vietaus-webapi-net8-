using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveProductionSelectVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_psv_mpo",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.CreateIndex(
                name: "ux_psv_one_active_per_mpo",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "mfg_production_order_id",
                unique: true,
                filter: "valid_to IS NULL AND valid_from IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ux_psv_one_active_per_mpo",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.CreateIndex(
                name: "ix_psv_mpo",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "mfg_production_order_id");
        }
    }
}
