using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMaterialNameGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialGroupNames",
                schema: "Material",
                columns: table => new
                {
                    MaterialGroupNameId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialGroupNameText = table.Column<string>(type: "citext", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialGroupNames", x => x.MaterialGroupNameId);
                    table.ForeignKey(
                        name: "FK_MaterialGroupNames_Materials",
                        column: x => x.MaterialId,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialGroupNames_MaterialId",
                schema: "Material",
                table: "MaterialGroupNames",
                column: "MaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialGroupNames",
                schema: "Material");
        }
    }
}
