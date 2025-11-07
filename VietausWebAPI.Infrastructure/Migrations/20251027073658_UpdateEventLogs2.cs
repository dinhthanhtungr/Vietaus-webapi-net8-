using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventLogs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                schema: "Audit",
                table: "EventLogs",
                newName: "EmployeeID");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                schema: "Audit",
                table: "EventLogs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_DepartmentId",
                schema: "Audit",
                table: "EventLogs",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs",
                column: "EmployeeID",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_DepartmentId",
                schema: "Audit",
                table: "EventLogs",
                column: "DepartmentId",
                principalSchema: "hr",
                principalTable: "Parts",
                principalColumn: "PartID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_DepartmentId",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_DepartmentId",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "Note",
                schema: "Audit",
                table: "EventLogs");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                schema: "Audit",
                table: "EventLogs",
                newName: "PositionId");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "Audit",
                table: "EventLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_CreatedBy",
                schema: "Audit",
                table: "EventLogs",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
