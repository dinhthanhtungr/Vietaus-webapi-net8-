using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateworknote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Work");

            migrationBuilder.CreateTable(
                name: "WorkPlans",
                schema: "Work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanName = table.Column<string>(type: "citext", nullable: false),
                    Objective = table.Column<string>(type: "text", nullable: true),
                    Strategy = table.Column<string>(type: "text", nullable: true),
                    DiscussionSummary = table.Column<string>(type: "text", nullable: true),
                    NextAction = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    NextFollowUpDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AssignedToEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlans_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlans_AssignedToEmployee",
                        column: x => x.AssignedToEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkPlans_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkPlans_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkPlans_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WorkTasks",
                schema: "Work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "citext", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DueReminderSentAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompletionNote = table.Column<string>(type: "text", nullable: true),
                    AssignedToEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTasks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTasks_AssignedToEmployee",
                        column: x => x.AssignedToEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkTasks_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkTasks_CompletedBy",
                        column: x => x.CompletedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkTasks_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkTasks_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WorkPlanAssignees",
                schema: "Work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WorkPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlanAssignees_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlanAssignees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkPlanAssignees_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkPlanAssignees_WorkPlan",
                        column: x => x.WorkPlanId,
                        principalSchema: "Work",
                        principalTable: "WorkPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkPlanReferences",
                schema: "Work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WorkPlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceCodeSnapshot = table.Column<string>(type: "citext", nullable: true),
                    ReferenceNameSnapshot = table.Column<string>(type: "citext", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlanReferences_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlanReferences_WorkPlan",
                        column: x => x.WorkPlanId,
                        principalSchema: "Work",
                        principalTable: "WorkPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTaskAssignees",
                schema: "Work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTaskAssignees_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTaskAssignees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkTaskAssignees_Employee",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkTaskAssignees_WorkTask",
                        column: x => x.WorkTaskId,
                        principalSchema: "Work",
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTaskReferences",
                schema: "Work",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceType = table.Column<int>(type: "integer", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceCodeSnapshot = table.Column<string>(type: "citext", nullable: true),
                    ReferenceNameSnapshot = table.Column<string>(type: "citext", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTaskReferences_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTaskReferences_WorkTask",
                        column: x => x.WorkTaskId,
                        principalSchema: "Work",
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlanAssignees_CreatedBy",
                schema: "Work",
                table: "WorkPlanAssignees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlanAssignees_Employee_IsActive",
                schema: "Work",
                table: "WorkPlanAssignees",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_WorkPlanAssignees_Plan_Employee_Active",
                schema: "Work",
                table: "WorkPlanAssignees",
                columns: new[] { "WorkPlanId", "EmployeeId" },
                unique: true,
                filter: "\"IsActive\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlanReferences_Reference",
                schema: "Work",
                table: "WorkPlanReferences",
                columns: new[] { "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "UX_WorkPlanReferences_Plan_Primary",
                schema: "Work",
                table: "WorkPlanReferences",
                column: "WorkPlanId",
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "UX_WorkPlanReferences_Plan_Reference",
                schema: "Work",
                table: "WorkPlanReferences",
                columns: new[] { "WorkPlanId", "ReferenceType", "ReferenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_Assigned_Status_NextFollowUp",
                schema: "Work",
                table: "WorkPlans",
                columns: new[] { "CompanyId", "AssignedToEmployeeId", "Status", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_AssignedToEmployeeId",
                schema: "Work",
                table: "WorkPlans",
                column: "AssignedToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_Company_Status_NextFollowUp",
                schema: "Work",
                table: "WorkPlans",
                columns: new[] { "CompanyId", "Status", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_CreatedBy",
                schema: "Work",
                table: "WorkPlans",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_UpdatedBy",
                schema: "Work",
                table: "WorkPlans",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskAssignees_CreatedBy",
                schema: "Work",
                table: "WorkTaskAssignees",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskAssignees_Employee_IsActive",
                schema: "Work",
                table: "WorkTaskAssignees",
                columns: new[] { "EmployeeId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_WorkTaskAssignees_Task_Employee_Active",
                schema: "Work",
                table: "WorkTaskAssignees",
                columns: new[] { "WorkTaskId", "EmployeeId" },
                unique: true,
                filter: "\"IsActive\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTaskReferences_Reference",
                schema: "Work",
                table: "WorkTaskReferences",
                columns: new[] { "ReferenceType", "ReferenceId" });

            migrationBuilder.CreateIndex(
                name: "UX_WorkTaskReferences_Task_Primary",
                schema: "Work",
                table: "WorkTaskReferences",
                column: "WorkTaskId",
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "UX_WorkTaskReferences_Task_Reference",
                schema: "Work",
                table: "WorkTaskReferences",
                columns: new[] { "WorkTaskId", "ReferenceType", "ReferenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_Assigned_Status_DueDate",
                schema: "Work",
                table: "WorkTasks",
                columns: new[] { "CompanyId", "AssignedToEmployeeId", "Status", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_AssignedToEmployeeId",
                schema: "Work",
                table: "WorkTasks",
                column: "AssignedToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_Company_Status_DueDate",
                schema: "Work",
                table: "WorkTasks",
                columns: new[] { "CompanyId", "Status", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_CompletedBy",
                schema: "Work",
                table: "WorkTasks",
                column: "CompletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_CreatedBy",
                schema: "Work",
                table: "WorkTasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_DueReminder",
                schema: "Work",
                table: "WorkTasks",
                columns: new[] { "Status", "DueDate" },
                filter: "\"IsActive\" = true AND \"DueReminderSentAt\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_UpdatedBy",
                schema: "Work",
                table: "WorkTasks",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkPlanAssignees",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "WorkPlanReferences",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "WorkTaskAssignees",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "WorkTaskReferences",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "WorkPlans",
                schema: "Work");

            migrationBuilder.DropTable(
                name: "WorkTasks",
                schema: "Work");
        }
    }
}
