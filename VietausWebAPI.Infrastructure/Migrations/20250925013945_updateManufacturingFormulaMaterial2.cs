using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingFormulaMaterial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Companies_CompanyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId1",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId2",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_MfgProductionOrders_Companies_CompanyId1",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MfgProductionOrders_Employees_EmployeeId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MfgProductionOrders_Employees_EmployeeId1",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_CompanyId1",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_EmployeeId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_EmployeeId1",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_CompanyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_EmployeeId1",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_EmployeeId2",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "EmployeeId2",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId2",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_CompanyId1",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_EmployeeId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_EmployeeId1",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_CompanyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId1",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId2",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId2");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Companies_CompanyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId1",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId1",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId2",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId2",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MfgProductionOrders_Companies_CompanyId1",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "CompanyId1",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MfgProductionOrders_Employees_EmployeeId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_MfgProductionOrders_Employees_EmployeeId1",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "EmployeeId1",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
