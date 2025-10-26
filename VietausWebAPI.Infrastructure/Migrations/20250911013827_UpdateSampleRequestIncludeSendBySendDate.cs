using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSampleRequestIncludeSendBySendDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "SendBy",
                schema: "labs",
                table: "SampleRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                schema: "labs",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests",
                column: "ManagerBy");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedByNavigationEmployeeId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Employees_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_SendBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "SendBy",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "SendDate",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "UpdatedByNavigationEmployeeId",
                schema: "labs",
                table: "SampleRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
