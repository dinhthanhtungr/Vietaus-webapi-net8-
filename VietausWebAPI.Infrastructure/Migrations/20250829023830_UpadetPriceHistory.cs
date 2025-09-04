using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpadetPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "inventory",
                table: "PriceHistory");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "inventory",
                table: "PriceHistory",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_PriceHistory_UpdatedBy",
                schema: "inventory",
                table: "PriceHistory",
                newName: "IX_PriceHistory_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceHistory_Employees_EmployeeId",
                schema: "inventory",
                table: "PriceHistory",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceHistory_Employees_EmployeeId",
                schema: "inventory",
                table: "PriceHistory");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                schema: "inventory",
                table: "PriceHistory",
                newName: "UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_PriceHistory_EmployeeId",
                schema: "inventory",
                table: "PriceHistory",
                newName: "IX_PriceHistory_UpdatedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "inventory",
                table: "PriceHistory",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceHistory_UpdatedBy",
                schema: "inventory",
                table: "PriceHistory",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
