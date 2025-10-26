using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormulaMaterialSentInfor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SentBy",
                schema: "labs",
                table: "Formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SentByNameSnapshot",
                schema: "labs",
                table: "Formulas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                schema: "labs",
                table: "Formulas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas",
                column: "CheckByNavigationEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "CheckBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Employees_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas",
                column: "CheckByNavigationEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Employees_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "SentByNameSnapshot",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "SentDate",
                schema: "labs",
                table: "Formulas");
        }
    }
}
