using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewIdCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "IdCounters",
                newName: "IdCounters",
                newSchema: "public");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_companyId_externalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                columns: new[] { "companyId", "externalId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_companyId_externalId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.RenameTable(
                name: "IdCounters",
                schema: "public",
                newName: "IdCounters");
        }
    }
}
