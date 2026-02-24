using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QCInputByQC",
                schema: "devandqa",
                columns: table => new
                {
                    qcinputbyqcid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    inspectionmethod = table.Column<string>(type: "citext", nullable: true),
                    ischeckcoa = table.Column<bool>(type: "boolean", nullable: true),
                    ischeckmsdstds = table.Column<bool>(type: "boolean", nullable: true),
                    ischeckmetaldetection = table.Column<bool>(type: "boolean", nullable: true),
                    isallowinput = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInputByQC", x => x.qcinputbyqcid);
                });

            migrationBuilder.CreateTable(
                name: "QCInputByWarehouse",
                schema: "devandqa",
                columns: table => new
                {
                    qcinputbywarehouseid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    materialid = table.Column<Guid>(type: "uuid", nullable: false),
                    customerid = table.Column<Guid>(type: "uuid", nullable: false),
                    qcinputbyqcid = table.Column<Guid>(type: "uuid", nullable: false),
                    customernamesnapshot = table.Column<string>(type: "citext", nullable: true),
                    customerexternalidsnapshot = table.Column<string>(type: "citext", nullable: true),
                    materialexternalidsnapshot = table.Column<string>(type: "citext", nullable: true),
                    materialnamesnapshot = table.Column<string>(type: "citext", nullable: true),
                    lotno = table.Column<string>(type: "citext", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInputByWarehouse", x => x.qcinputbywarehouseid);
                    table.ForeignKey(
                        name: "FK_QCInputByWarehouse_Customer",
                        column: x => x.customerid,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
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
                name: "ix_qcinputbywarehouse_createddate",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "createddate");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbywarehouse_customerid",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "customerid");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QCInputByWarehouse",
                schema: "devandqa");

            migrationBuilder.DropTable(
                name: "QCInputByQC",
                schema: "devandqa");
        }
    }
}
