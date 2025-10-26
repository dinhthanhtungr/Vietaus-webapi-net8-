using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSnáphotWarehouseTemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTempStock_WarehouseSnapshotSet_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropTable(
                name: "WarehouseSnapshotSet",
                schema: "Warehouse");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTempStock_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WarehouseSnapshotSet",
                schema: "Warehouse",
                columns: table => new
                {
                    SnapshotSetId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    VaCode = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WarehouseSnapshotSet__3214EC07A98DEC4E", x => x.SnapshotSetId);
                    table.ForeignKey(
                        name: "FK_WarehouseSnapshotSe_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseSnapshotSet_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "SnapshotSetId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_CompanyId_VaCode_createdDate",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                columns: new[] { "CompanyId", "VaCode", "createdDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTempStock_WarehouseSnapshotSet_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "SnapshotSetId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseSnapshotSet",
                principalColumn: "SnapshotSetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
