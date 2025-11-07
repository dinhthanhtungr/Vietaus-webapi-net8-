using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMroFinal2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "areas",
                schema: "mro",
                columns: table => new
                {
                    area_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    area_externalid = table.Column<string>(type: "text", nullable: false),
                    area_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_areas", x => x.area_id);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                schema: "mro",
                columns: table => new
                {
                    equipment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipment_externalid = table.Column<string>(type: "text", nullable: false),
                    equipment_name = table.Column<string>(type: "text", nullable: false),
                    area_id = table.Column<int>(type: "integer", nullable: false),
                    area_externalid = table.Column<string>(type: "text", nullable: false),
                    factory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    factory_externalid = table.Column<string>(type: "text", nullable: false),
                    part_id = table.Column<Guid>(type: "uuid", nullable: false),
                    part_externalid = table.Column<string>(type: "citext", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    PartId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment", x => x.equipment_id);
                    table.ForeignKey(
                        name: "FK_equipment_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_equipment_Parts_PartId1",
                        column: x => x.PartId1,
                        principalSchema: "hr",
                        principalTable: "Parts",
                        principalColumn: "PartID");
                    table.ForeignKey(
                        name: "fk_equipment_area_id",
                        column: x => x.area_id,
                        principalSchema: "mro",
                        principalTable: "areas",
                        principalColumn: "area_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_equipment_factory_id",
                        column: x => x.factory_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_equipment_part_id",
                        column: x => x.part_id,
                        principalSchema: "hr",
                        principalTable: "Parts",
                        principalColumn: "PartID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ux_areas_area_externalid",
                schema: "mro",
                table: "areas",
                column: "area_externalid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_equipment_area_externalid",
                schema: "mro",
                table: "equipment",
                column: "area_externalid");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_area_id",
                schema: "mro",
                table: "equipment",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_CompanyId",
                schema: "mro",
                table: "equipment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_factory_externalid",
                schema: "mro",
                table: "equipment",
                column: "factory_externalid");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_part_externalid",
                schema: "mro",
                table: "equipment",
                column: "part_externalid");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_part_id",
                schema: "mro",
                table: "equipment",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_PartId1",
                schema: "mro",
                table: "equipment",
                column: "PartId1");

            migrationBuilder.CreateIndex(
                name: "ux_equipment_factory_extid",
                schema: "mro",
                table: "equipment",
                columns: new[] { "factory_id", "equipment_externalid" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "areas",
                schema: "mro");
        }
    }
}
