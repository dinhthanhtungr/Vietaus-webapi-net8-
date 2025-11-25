using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_Companies_CompanyId1",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId1",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId2",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId3",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_CompanyId1",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_EmployeeId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_EmployeeId1",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_EmployeeId2",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_EmployeeId3",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "EmployeeId2",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "EmployeeId3",
                schema: "mro",
                table: "incident_hdr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId1",
                schema: "mro",
                table: "incident_hdr",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "mro",
                table: "incident_hdr",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                schema: "mro",
                table: "incident_hdr",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId2",
                schema: "mro",
                table: "incident_hdr",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId3",
                schema: "mro",
                table: "incident_hdr",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_CompanyId1",
                schema: "mro",
                table: "incident_hdr",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId1",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId2",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId2");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EmployeeId3",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId3");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_Companies_CompanyId1",
                schema: "mro",
                table: "incident_hdr",
                column: "CompanyId1",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId1",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId1",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId2",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId2",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_Employees_EmployeeId3",
                schema: "mro",
                table: "incident_hdr",
                column: "EmployeeId3",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
