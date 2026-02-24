using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QCInputByWarehouse",
                schema: "devandqa");

            migrationBuilder.AddColumn<string>(
                name: "CSExternalIdSnapshot",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CSNameSnapshot",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImportWarehouse",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialExternalIdSnapshot",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "MaterialNameSnapshot",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQC_MaterialId",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQC_Materials_MaterialId",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "MaterialId",
                principalSchema: "Material",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQC_Materials_MaterialId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "IX_QCInputByQC_MaterialId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "CSExternalIdSnapshot",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "CSNameSnapshot",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "ImportWarehouse",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "MaterialExternalIdSnapshot",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "MaterialNameSnapshot",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "Note",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.CreateTable(
                name: "QCInputByWarehouse",
                schema: "devandqa",
                columns: table => new
                {
                    qcinputbywarehouseid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    materialid = table.Column<Guid>(type: "uuid", nullable: false),
                    qcinputbyqcid = table.Column<Guid>(type: "uuid", nullable: false),
                    csexternalidsnapshot = table.Column<string>(type: "citext", nullable: true),
                    csnamesnapshot = table.Column<string>(type: "citext", nullable: true),
                    createdby = table.Column<Guid>(type: "uuid", nullable: false),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    lotno = table.Column<string>(type: "citext", nullable: true),
                    materialexternalidsnapshot = table.Column<string>(type: "citext", nullable: true),
                    materialnamesnapshot = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInputByWarehouse", x => x.qcinputbywarehouseid);
                    table.ForeignKey(
                        name: "FK_QCInputByWarehouse_Material",
                        column: x => x.materialid,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_QCInputByWarehouse_QCInputByQC",
                        column: x => x.qcinputbyqcid,
                        principalSchema: "devandqa",
                        principalTable: "QCInputByQC",
                        principalColumn: "qcinputbyqcid");
                });

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbywarehouse_createddate",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "createddate");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbywarehouse_lotno",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "lotno");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbywarehouse_materialid",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbywarehouse_qcinputbyqcid",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "qcinputbyqcid");
        }
    }
}
