using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class insertCustomerFollowUpTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueReminderSentAt",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerFollowUpTaskAssignees",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerFollowUpTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFollowUpTaskAssignees_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTaskAssignees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTaskAssignees_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTaskAssignees_Task",
                        column: x => x.CustomerFollowUpTaskId,
                        principalSchema: "Customer",
                        principalTable: "CustomerFollowUpTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_DueReminder",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                columns: new[] { "Status", "DueDate" },
                filter: "\"IsActive\" = true AND \"DueReminderSentAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTaskAssignees_CreatedBy",
                schema: "Customer",
                table: "CustomerFollowUpTaskAssignees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTaskAssignees_Employee_IsActive",
                schema: "Customer",
                table: "CustomerFollowUpTaskAssignees",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTaskAssignees_Task_Employee",
                schema: "Customer",
                table: "CustomerFollowUpTaskAssignees",
                columns: new[] { "CustomerFollowUpTaskId", "EmployeeId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerFollowUpTaskAssignees",
                schema: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFollowUpTasks_DueReminder",
                schema: "Customer",
                table: "CustomerFollowUpTasks");

            migrationBuilder.DropColumn(
                name: "DueReminderSentAt",
                schema: "Customer",
                table: "CustomerFollowUpTasks");
        }
    }
}
