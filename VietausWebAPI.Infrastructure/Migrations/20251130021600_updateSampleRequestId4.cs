using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSampleRequestId4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "standardparameters");

            migrationBuilder.CreateTable(
                name: "machine_productivity",
                schema: "standardparameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    machineid = table.Column<string>(type: "citext", nullable: false),
                    productioncode = table.Column<string>(type: "citext", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_standardparameters_machine_productivity", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parameterstandard",
                schema: "standardparameters",
                columns: table => new
                {
                    machineid = table.Column<string>(type: "citext", nullable: false),
                    productioncode = table.Column<string>(type: "citext", nullable: false),
                    set1_standard = table.Column<int>(type: "integer", nullable: false),
                    set2_standard = table.Column<int>(type: "integer", nullable: false),
                    set3_standard = table.Column<int>(type: "integer", nullable: false),
                    set4_standard = table.Column<int>(type: "integer", nullable: false),
                    set5_standard = table.Column<int>(type: "integer", nullable: false),
                    set6_standard = table.Column<int>(type: "integer", nullable: false),
                    set7_standard = table.Column<int>(type: "integer", nullable: false),
                    set8_standard = table.Column<int>(type: "integer", nullable: false),
                    set9_standard = table.Column<int>(type: "integer", nullable: false),
                    set10_standard = table.Column<int>(type: "integer", nullable: false),
                    set11_standard = table.Column<int>(type: "integer", nullable: false),
                    set12_standard = table.Column<int>(type: "integer", nullable: false),
                    set13_standard = table.Column<int>(type: "integer", nullable: false),
                    screwspeed_standard = table.Column<int>(type: "integer", nullable: false),
                    screwcurrent_standard = table.Column<int>(type: "integer", nullable: false),
                    feederspeed_standard = table.Column<int>(type: "integer", nullable: false),
                    employeeid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_standardparameters_parameterstandard", x => new { x.machineid, x.productioncode });
                });

            migrationBuilder.CreateIndex(
                name: "ix_machine_productivity_machineid_productioncode",
                schema: "standardparameters",
                table: "machine_productivity",
                columns: new[] { "machineid", "productioncode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "machine_productivity",
                schema: "standardparameters");

            migrationBuilder.DropTable(
                name: "parameterstandard",
                schema: "standardparameters");
        }
    }
}
