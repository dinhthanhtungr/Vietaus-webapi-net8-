using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateFormulaCheckInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CheckBy",
                schema: "labs",
                table: "Formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckDate",
                schema: "labs",
                table: "Formulas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckNameSnapshot",
                schema: "labs",
                table: "Formulas",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas",
                column: "CheckBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas",
                column: "CheckBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "CheckBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "CheckDate",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "CheckNameSnapshot",
                schema: "labs",
                table: "Formulas");
        }
    }
}
