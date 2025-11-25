using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "incidenthistory",
                schema: "mro",
                columns: table => new
                {
                    incidenthistory_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    incident_id = table.Column<long>(type: "bigint", nullable: false),
                    incident_code = table.Column<string>(type: "citext", nullable: false),
                    action = table.Column<string>(type: "text", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    stock_out_id = table.Column<long>(type: "bigint", nullable: true),
                    wo_ref = table.Column<string>(type: "citext", nullable: true),
                    performed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    performed_at = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    root_cause = table.Column<string>(type: "text", nullable: true),
                    corrective_action = table.Column<string>(type: "text", nullable: true),
                    preventive_action = table.Column<string>(type: "text", nullable: true),
                    downtime_minutes = table.Column<int>(type: "integer", nullable: true),
                    cost_estimate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_incidenthistory", x => x.incidenthistory_id);
                    table.ForeignKey(
                        name: "fk_incidenthistory_incident",
                        column: x => x.incident_id,
                        principalSchema: "mro",
                        principalTable: "incident_hdr",
                        principalColumn: "incident_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_incidenthistory_performed_by",
                        column: x => x.performed_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incidenthistory_stockout",
                        column: x => x.stock_out_id,
                        principalSchema: "mro",
                        principalTable: "stock_out_hdr",
                        principalColumn: "stock_out_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_incidenthistory_incident",
                schema: "mro",
                table: "incidenthistory",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "ix_incidenthistory_incident_performed_desc",
                schema: "mro",
                table: "incidenthistory",
                columns: new[] { "incident_id", "performed_at", "incidenthistory_id" },
                descending: new[] { false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_incidenthistory_performed_by",
                schema: "mro",
                table: "incidenthistory",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "ix_incidenthistory_stockout",
                schema: "mro",
                table: "incidenthistory",
                column: "stock_out_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "incidenthistory",
                schema: "mro");
        }
    }
}
