using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WarehouseUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_Companies_CompanyId",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_Companies_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_Employees_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSnapshotSet_Companies_CompanyId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSnapshotSet_Companies_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSnapshotSet_Employees_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSnapshotSet_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTempStock_Employees_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTempStock_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseTempStock_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseSnapshotSet_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseSnapshotSet_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseShelfStock_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseShelfStock_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_Company",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSnapshotSe_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSnapshotSet_Company",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTempStock_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_Company",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseShelfStock_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSnapshotSe_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSnapshotSet_Company",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseTempStock_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_Companies_CompanyId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_Companies_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "CompanyId1",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseShelfStock_Employees_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSnapshotSet_Companies_CompanyId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSnapshotSet_Companies_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CompanyId1",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSnapshotSet_Employees_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSnapshotSet_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTempStock_Employees_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseTempStock_Employees_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
