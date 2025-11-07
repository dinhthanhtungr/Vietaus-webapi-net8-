using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMroFinal3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "incident_hdr",
                schema: "mro",
                columns: table => new
                {
                    incident_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    incident_code = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    equipment_id = table.Column<int>(type: "integer", nullable: true),
                    equipment_code = table.Column<string>(type: "text", nullable: true),
                    area_id = table.Column<int>(type: "integer", nullable: true),
                    area_code = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_prefix = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    exec_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    exec_by = table.Column<Guid>(type: "uuid", nullable: true),
                    done_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    done_by = table.Column<Guid>(type: "uuid", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    closed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    wait_min = table.Column<int>(type: "integer", nullable: true),
                    repair_min = table.Column<int>(type: "integer", nullable: true),
                    total_min = table.Column<int>(type: "integer", nullable: true),
                    AreaMROAreaId = table.Column<int>(type: "integer", nullable: true),
                    CompanyId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId2 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId3 = table.Column<Guid>(type: "uuid", nullable: true),
                    EquipmentMROEquipmentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_incident_hdr", x => x.incident_id);
                    table.ForeignKey(
                        name: "FK_incident_hdr_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_incident_hdr_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_incident_hdr_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_incident_hdr_Employees_EmployeeId2",
                        column: x => x.EmployeeId2,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_incident_hdr_Employees_EmployeeId3",
                        column: x => x.EmployeeId3,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_incident_hdr_areas_AreaMROAreaId",
                        column: x => x.AreaMROAreaId,
                        principalSchema: "mro",
                        principalTable: "areas",
                        principalColumn: "area_id");
                    table.ForeignKey(
                        name: "FK_incident_hdr_equipment_EquipmentMROEquipmentId",
                        column: x => x.EquipmentMROEquipmentId,
                        principalSchema: "mro",
                        principalTable: "equipment",
                        principalColumn: "equipment_id");
                    table.ForeignKey(
                        name: "fk_incident_hdr_area_id",
                        column: x => x.area_id,
                        principalSchema: "mro",
                        principalTable: "areas",
                        principalColumn: "area_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incident_hdr_closed_by",
                        column: x => x.closed_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incident_hdr_company_id",
                        column: x => x.company_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incident_hdr_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incident_hdr_done_by",
                        column: x => x.done_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incident_hdr_equipment_id",
                        column: x => x.equipment_id,
                        principalSchema: "mro",
                        principalTable: "equipment",
                        principalColumn: "equipment_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_incident_hdr_exec_by",
                        column: x => x.exec_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_incident_hdr_area",
                schema: "mro",
                table: "incident_hdr",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr",
                column: "AreaMROAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_closed_by",
                schema: "mro",
                table: "incident_hdr",
                column: "closed_by");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_CompanyId1",
                schema: "mro",
                table: "incident_hdr",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "ix_incident_hdr_created_at",
                schema: "mro",
                table: "incident_hdr",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_created_by",
                schema: "mro",
                table: "incident_hdr",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_done_by",
                schema: "mro",
                table: "incident_hdr",
                column: "done_by");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId1",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId2",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId2");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId3",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId3");

            migrationBuilder.CreateIndex(
                name: "ix_incident_hdr_equipment",
                schema: "mro",
                table: "incident_hdr",
                columns: new[] { "equipment_id", "equipment_code" });

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr",
                column: "EquipmentMROEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_exec_by",
                schema: "mro",
                table: "incident_hdr",
                column: "exec_by");

            migrationBuilder.CreateIndex(
                name: "ix_incident_hdr_status",
                schema: "mro",
                table: "incident_hdr",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ux_incident_hdr_company_code",
                schema: "mro",
                table: "incident_hdr",
                columns: new[] { "company_id", "incident_code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "incident_hdr",
                schema: "mro");
        }
    }
}
