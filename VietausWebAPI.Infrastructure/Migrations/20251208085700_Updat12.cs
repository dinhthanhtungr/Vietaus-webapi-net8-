using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updat12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdCounters",
                schema: "public",
                table: "IdCounters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdCounters",
                schema: "public",
                table: "IdCounters",
                columns: new[] { "CompanyId", "Prefix" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdCounters",
                schema: "public",
                table: "IdCounters");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdCounters",
                schema: "public",
                table: "IdCounters",
                columns: new[] { "CompanyId", "Prefix", "Period" });
        }
    }
}
