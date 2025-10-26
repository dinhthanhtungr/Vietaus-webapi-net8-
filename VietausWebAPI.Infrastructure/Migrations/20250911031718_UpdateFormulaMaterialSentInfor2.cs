using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormulaMaterialSentInfor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.AddForeignKey(
                name: "IX_Formulas_SentBy",
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
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas",
                column: "CheckBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
