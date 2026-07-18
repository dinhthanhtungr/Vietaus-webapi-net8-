using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomerInteraction2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompletedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompletionNote",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_CompletedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "CompletedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFollowUpTasks_CompletedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "CompletedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFollowUpTasks_CompletedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFollowUpTasks_CompletedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks");

            migrationBuilder.DropColumn(
                name: "CompletedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks");

            migrationBuilder.DropColumn(
                name: "CompletionNote",
                schema: "Customer",
                table: "CustomerFollowUpTasks");
        }
    }
}
