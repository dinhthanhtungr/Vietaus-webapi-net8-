using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormulaMaterialSentInfor3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Employees_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Employees_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.RenameIndex(
                name: "IX_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests",
                newName: "IX_SampleRequests_SendBy1");

            migrationBuilder.RenameIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                newName: "IX_Formulas_SentBy1");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests",
                column: "SendBy");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "SentBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "labs",
                table: "Formulas",
                column: "CheckBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "SentBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests",
                column: "SendBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedBy",
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

            migrationBuilder.DropForeignKey(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.RenameIndex(
                name: "IX_SampleRequests_SendBy1",
                schema: "labs",
                table: "SampleRequests",
                newName: "IX_SampleRequests_SendBy");

            migrationBuilder.RenameIndex(
                name: "IX_Formulas_SentBy1",
                schema: "labs",
                table: "Formulas",
                newName: "IX_Formulas_SentBy");

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedByNavigationEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas",
                column: "CheckByNavigationEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Employees_CheckByNavigationEmployeeId",
                schema: "labs",
                table: "Formulas",
                column: "CheckByNavigationEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "CheckBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Employees_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedByNavigationEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
