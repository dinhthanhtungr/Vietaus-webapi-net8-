using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSnaphotWarehouseTemp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WarehouseTempStock_CompanyId_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTempStock_CompanyId_TempType_Code_LotKey",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropColumn(
                name: "SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropColumn(
                name: "TempType",
                schema: "Warehouse",
                table: "WarehouseTempStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TempType",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_CompanyId_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                columns: new[] { "CompanyId", "SnapshotSetId" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_CompanyId_TempType_Code_LotKey",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                columns: new[] { "CompanyId", "TempType", "Code", "LotKey" });
        }
    }
}
