using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateEmployeeCU1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "hr",
                table: "Employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "hr",
                table: "Employees",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_date",
                schema: "hr",
                table: "Employees",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "updated_by",
                schema: "hr",
                table: "Employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedBy",
                schema: "hr",
                table: "Employees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_updated_by",
                schema: "hr",
                table: "Employees",
                column: "updated_by");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_CreatedBy",
                schema: "hr",
                table: "Employees",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_updated_by",
                schema: "hr",
                table: "Employees",
                column: "updated_by",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_CreatedBy",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_updated_by",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedBy",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_updated_by",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "created_date",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "updated_by",
                schema: "hr",
                table: "Employees");
        }
    }
}
