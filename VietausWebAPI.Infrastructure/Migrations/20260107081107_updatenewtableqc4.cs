using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QCInputByQC",
                schema: "devandqa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QCInputByQC",
                schema: "devandqa",
                columns: table => new
                {
                    qcinputbyqcid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    CSExternalIdSnapshot = table.Column<string>(type: "text", nullable: true),
                    CSNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ImportWarehouse = table.Column<string>(type: "text", nullable: true),
                    inspectionmethod = table.Column<string>(type: "citext", nullable: true),
                    isallowinput = table.Column<bool>(type: "boolean", nullable: true),
                    ischeckcoa = table.Column<bool>(type: "boolean", nullable: true),
                    ischeckmsdstds = table.Column<bool>(type: "boolean", nullable: true),
                    ischeckmetaldetection = table.Column<bool>(type: "boolean", nullable: true),
                    MaterialExternalIdSnapshot = table.Column<string>(type: "text", nullable: true),
                    MaterialNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInputByQC", x => x.qcinputbyqcid);
                    table.ForeignKey(
                        name: "FK_QCInputByQC_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbyqc_inspectionmethod",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "inspectionmethod");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbyqc_isallowinput",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "isallowinput");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQC_MaterialId",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "MaterialId");
        }
    }
}
