using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomerInteraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentCrmStatus",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentSaleId",
                schema: "Customer",
                table: "Customer",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastContactDate",
                schema: "Customer",
                table: "Customer",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextFollowUpDate",
                schema: "Customer",
                table: "Customer",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerInteractions",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: true),
                    InteractionType = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Subject = table.Column<string>(type: "citext", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Outcome = table.Column<string>(type: "text", nullable: true),
                    NextAction = table.Column<string>(type: "text", nullable: true),
                    InteractionAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    NextFollowUpDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AssignedSaleEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInteractions_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInteractions_AssignedSaleEmployee",
                        column: x => x.AssignedSaleEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerInteractions_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInteractions_Contact",
                        column: x => x.ContactId,
                        principalSchema: "Customer",
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerInteractions_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInteractions_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInteractions_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CustomerWorkPlans",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
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
                    AssignedSaleEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerWorkPlans_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerWorkPlans_AssignedSaleEmployee",
                        column: x => x.AssignedSaleEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerWorkPlans_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerWorkPlans_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerWorkPlans_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerWorkPlans_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CustomerFollowUpTasks",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerInteractionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "citext", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    NextAction = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AssignedSaleEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFollowUpTasks_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTasks_AssignedSaleEmployee",
                        column: x => x.AssignedSaleEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTasks_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTasks_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTasks_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTasks_Interaction",
                        column: x => x.CustomerInteractionId,
                        principalSchema: "Customer",
                        principalTable: "CustomerInteractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerFollowUpTasks_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Company_CrmStatus_NextFollowUp",
                schema: "Customer",
                table: "Customer",
                columns: new[] { "CompanyId", "CurrentCrmStatus", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Company_CurrentSale_NextFollowUp",
                schema: "Customer",
                table: "Customer",
                columns: new[] { "CompanyId", "CurrentSaleId", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_Assigned_Status_DueDate",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                columns: new[] { "CompanyId", "AssignedSaleEmployeeId", "Status", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_AssignedSaleEmployeeId",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "AssignedSaleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_CreatedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_Customer_DueDate",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                columns: new[] { "CompanyId", "CustomerId", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_CustomerId",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_CustomerInteractionId",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "CustomerInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFollowUpTasks_UpdatedBy",
                schema: "Customer",
                table: "CustomerFollowUpTasks",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_AssignedSaleEmployeeId",
                schema: "Customer",
                table: "CustomerInteractions",
                column: "AssignedSaleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_Company_Assigned_NextFollowUp",
                schema: "Customer",
                table: "CustomerInteractions",
                columns: new[] { "CompanyId", "AssignedSaleEmployeeId", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_Company_Customer_InteractionAtDesc",
                schema: "Customer",
                table: "CustomerInteractions",
                columns: new[] { "CompanyId", "CustomerId", "InteractionAt" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_ContactId",
                schema: "Customer",
                table: "CustomerInteractions",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_CreatedBy",
                schema: "Customer",
                table: "CustomerInteractions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_CustomerId",
                schema: "Customer",
                table: "CustomerInteractions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractions_UpdatedBy",
                schema: "Customer",
                table: "CustomerInteractions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWorkPlans_Assigned_Status_NextFollowUp",
                schema: "Customer",
                table: "CustomerWorkPlans",
                columns: new[] { "CompanyId", "AssignedSaleEmployeeId", "Status", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWorkPlans_AssignedSaleEmployeeId",
                schema: "Customer",
                table: "CustomerWorkPlans",
                column: "AssignedSaleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWorkPlans_CreatedBy",
                schema: "Customer",
                table: "CustomerWorkPlans",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWorkPlans_Customer_Status_NextFollowUp",
                schema: "Customer",
                table: "CustomerWorkPlans",
                columns: new[] { "CompanyId", "CustomerId", "Status", "NextFollowUpDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWorkPlans_CustomerId",
                schema: "Customer",
                table: "CustomerWorkPlans",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWorkPlans_UpdatedBy",
                schema: "Customer",
                table: "CustomerWorkPlans",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerFollowUpTasks",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerWorkPlans",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerInteractions",
                schema: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Company_CrmStatus_NextFollowUp",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Company_CurrentSale_NextFollowUp",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CurrentCrmStatus",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "CurrentSaleId",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastContactDate",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "NextFollowUpDate",
                schema: "Customer",
                table: "Customer");
        }
    }
}
