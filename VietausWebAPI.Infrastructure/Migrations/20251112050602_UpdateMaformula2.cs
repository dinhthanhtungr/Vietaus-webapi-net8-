using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaformula2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "equipment_specs",
                schema: "mro",
                columns: table => new
                {
                    spec_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipment_id = table.Column<int>(type: "integer", nullable: false),
                    spec_key = table.Column<string>(type: "text", nullable: true),
                    spec_value = table.Column<string>(type: "text", nullable: true),
                    unit = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    entered_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    entered_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_specs", x => x.spec_id);
                    table.ForeignKey(
                        name: "fk_equipment_specs_equipment",
                        column: x => x.equipment_id,
                        principalSchema: "mro",
                        principalTable: "equipment",
                        principalColumn: "equipment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "equipmenttype",
                schema: "mro",
                columns: table => new
                {
                    equipmenttype_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipmenttype_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipmenttype", x => x.equipmenttype_id);
                });

            migrationBuilder.CreateTable(
                name: "equipment_details",
                schema: "mro",
                columns: table => new
                {
                    equipment_id = table.Column<int>(type: "integer", nullable: false),
                    serial_no = table.Column<string>(type: "text", nullable: true),
                    manufacturer = table.Column<string>(type: "text", nullable: true),
                    model = table.Column<string>(type: "text", nullable: true),
                    purchase_date = table.Column<DateTime>(type: "date", nullable: true),
                    commissioning_date = table.Column<DateTime>(type: "date", nullable: true),
                    warranty_until = table.Column<DateTime>(type: "date", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    equipmenttype_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_details", x => x.equipment_id);
                    table.ForeignKey(
                        name: "fk_equipment_details_equipment",
                        column: x => x.equipment_id,
                        principalSchema: "mro",
                        principalTable: "equipment",
                        principalColumn: "equipment_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_equipment_details_equipmenttype",
                        column: x => x.equipmenttype_id,
                        principalSchema: "mro",
                        principalTable: "equipmenttype",
                        principalColumn: "equipmenttype_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_equipment_details_equipmenttype_id",
                schema: "mro",
                table: "equipment_details",
                column: "equipmenttype_id");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_specs_equipment_id",
                schema: "mro",
                table: "equipment_specs",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_equipmenttype_name",
                schema: "mro",
                table: "equipmenttype",
                column: "equipmenttype_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment_details",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "equipment_specs",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "equipmenttype",
                schema: "mro");
        }
    }
}
