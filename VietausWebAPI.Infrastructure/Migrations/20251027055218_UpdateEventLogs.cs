using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "EventLogs",
                schema: "Audit",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PositionId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceCode = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false, defaultValue: "Draft"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EventLogs__227429A55C6F1195", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_EventLogs_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventLogs_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_EventType",
                schema: "Audit",
                table: "EventLogs",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_SourceCode",
                schema: "Audit",
                table: "EventLogs",
                column: "SourceCode");

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_SourceId",
                schema: "Audit",
                table: "EventLogs",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_SourceId_EventType",
                schema: "Audit",
                table: "EventLogs",
                columns: new[] { "SourceId", "EventType" });

            migrationBuilder.CreateIndex(
                name: "IX_EventLog_Status",
                schema: "Audit",
                table: "EventLogs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_CompanyId",
                schema: "Audit",
                table: "EventLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogs",
                schema: "Audit");
        }
    }
}
