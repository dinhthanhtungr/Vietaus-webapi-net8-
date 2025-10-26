using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Bigupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
