using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "notification",
                table: "notifications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByNameSnapshot",
                schema: "notification",
                table: "notifications",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_CreatedBy",
                schema: "notification",
                table: "notifications",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "fk_notifications_created_by_employee",
                schema: "notification",
                table: "notifications",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notifications_created_by_employee",
                schema: "notification",
                table: "notifications");

            migrationBuilder.DropIndex(
                name: "IX_notifications_CreatedBy",
                schema: "notification",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "notification",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "CreatedByNameSnapshot",
                schema: "notification",
                table: "notifications");
        }
    }
}
