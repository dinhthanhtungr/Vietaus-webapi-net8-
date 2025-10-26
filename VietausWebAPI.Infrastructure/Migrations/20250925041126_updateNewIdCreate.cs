using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewIdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormulaNameSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.CreateTable(
                name: "IdCounters",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Prefix = table.Column<string>(type: "text", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: false),
                    LastNo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdCounters", x => new { x.CompanyId, x.Prefix, x.Period });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdCounters");

            migrationBuilder.AddColumn<string>(
                name: "FormulaNameSnapshot",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true);
        }
    }
}
