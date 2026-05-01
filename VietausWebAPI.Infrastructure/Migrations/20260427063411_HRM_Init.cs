using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HRM_Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                schema: "Audit",
                columns: table => new
                {
                    AuditLogId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    SchemaName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TableName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RecordId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OldValues = table.Column<string>(type: "jsonb", nullable: true),
                    NewValues = table.Column<string>(type: "jsonb", nullable: true),
                    ChangedValues = table.Column<string>(type: "jsonb", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedByNavigationEmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditLogId);
                    table.ForeignKey(
                        name: "FK_AuditLogs_ChangedBy",
                        column: x => x.ChangedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId");
                    table.ForeignKey(
                        name: "FK_AuditLogs_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Employees_ChangedByNavigationEmployeeId",
                        column: x => x.ChangedByNavigationEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "employee_bank_accounts",
                schema: "hr",
                columns: table => new
                {
                    employee_bank_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    bank_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    account_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    account_holder = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    is_payroll_account = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_bank_accounts", x => x.employee_bank_account_id);
                    table.ForeignKey(
                        name: "fk_employee_bank_accounts_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_contracts",
                schema: "hr",
                columns: table => new
                {
                    employee_contract_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contract_no = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    contract_type = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    is_current = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_contracts", x => x.employee_contract_id);
                    table.ForeignKey(
                        name: "fk_employee_contracts_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_documents",
                schema: "hr",
                columns: table => new
                {
                    employee_document_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    document_type = table.Column<int>(type: "integer", nullable: false),
                    document_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_documents", x => x.employee_document_id);
                    table.ForeignKey(
                        name: "fk_employee_documents_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_insurance_profiles",
                schema: "hr",
                columns: table => new
                {
                    employee_insurance_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    social_insurance_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tax_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    health_insurance_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_insurance_profiles", x => x.employee_insurance_profile_id);
                    table.ForeignKey(
                        name: "fk_employee_insurance_profiles_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_profiles",
                schema: "hr",
                columns: table => new
                {
                    employee_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ethnicity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    education_level = table.Column<int>(type: "integer", nullable: true),
                    identifier_issue_date = table.Column<DateOnly>(type: "date", nullable: true),
                    identifier_issue_place = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    permanent_address = table.Column<string>(type: "text", nullable: true),
                    temporary_address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_profiles", x => x.employee_profile_id);
                    table.ForeignKey(
                        name: "fk_employee_profiles_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_relatives",
                schema: "hr",
                columns: table => new
                {
                    employee_relative_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    relationship = table.Column<int>(type: "integer", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_emergency_contact = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_relatives", x => x.employee_relative_id);
                    table.ForeignKey(
                        name: "fk_employee_relatives_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_titles",
                schema: "hr",
                columns: table => new
                {
                    job_title_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    english_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_titles", x => x.job_title_id);
                });

            migrationBuilder.CreateTable(
                name: "employee_work_profiles",
                schema: "hr",
                columns: table => new
                {
                    employee_work_profile_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attendance_code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    part_id = table.Column<Guid>(type: "uuid", nullable: true),
                    group_id = table.Column<Guid>(type: "uuid", nullable: true),
                    job_title_id = table.Column<Guid>(type: "uuid", nullable: true),
                    work_location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    probation_end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    is_current = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: false),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    onboarding_training_date = table.Column<DateOnly>(type: "date", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_work_profiles", x => x.employee_work_profile_id);
                    table.ForeignKey(
                        name: "FK_employee_work_profiles_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "fk_employee_work_profiles_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_employee_work_profiles_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_employee_work_profiles_group",
                        column: x => x.group_id,
                        principalSchema: "company",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_employee_work_profiles_job_title",
                        column: x => x.job_title_id,
                        principalSchema: "hr",
                        principalTable: "job_titles",
                        principalColumn: "job_title_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_employee_work_profiles_part",
                        column: x => x.part_id,
                        principalSchema: "hr",
                        principalTable: "Parts",
                        principalColumn: "PartID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ActionType",
                schema: "Audit",
                table: "AuditLogs",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangedBy",
                schema: "Audit",
                table: "AuditLogs",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangedByNavigationEmployeeId",
                schema: "Audit",
                table: "AuditLogs",
                column: "ChangedByNavigationEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Company_ChangedAt",
                schema: "Audit",
                table: "AuditLogs",
                columns: new[] { "CompanyId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CompanyId",
                schema: "Audit",
                table: "AuditLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CompanyId1",
                schema: "Audit",
                table: "AuditLogs",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CorrelationId",
                schema: "Audit",
                table: "AuditLogs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Entity_Record_ChangedAt",
                schema: "Audit",
                table: "AuditLogs",
                columns: new[] { "SchemaName", "TableName", "RecordId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "ix_employee_bank_accounts_employee_id",
                schema: "hr",
                table: "employee_bank_accounts",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_contracts_employee_id",
                schema: "hr",
                table: "employee_contracts",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_documents_employee_id",
                schema: "hr",
                table: "employee_documents",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ux_employee_insurance_profiles_employee_id",
                schema: "hr",
                table: "employee_insurance_profiles",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_employee_profiles_employee_id",
                schema: "hr",
                table: "employee_profiles",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employee_relatives_employee_id",
                schema: "hr",
                table: "employee_relatives",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_work_profiles_created_by",
                schema: "hr",
                table: "employee_work_profiles",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_employee_work_profiles_employee_id",
                schema: "hr",
                table: "employee_work_profiles",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_work_profiles_EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "ix_employee_work_profiles_group_id",
                schema: "hr",
                table: "employee_work_profiles",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_work_profiles_job_title_id",
                schema: "hr",
                table: "employee_work_profiles",
                column: "job_title_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_work_profiles_part_id",
                schema: "hr",
                table: "employee_work_profiles",
                column: "part_id");

            migrationBuilder.CreateIndex(
                name: "ux_employee_work_profiles_one_current",
                schema: "hr",
                table: "employee_work_profiles",
                columns: new[] { "employee_id", "is_current" },
                unique: true,
                filter: "\"is_current\" = true");

            migrationBuilder.CreateIndex(
                name: "ux_job_titles_code",
                schema: "hr",
                table: "job_titles",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "employee_bank_accounts",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_contracts",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_documents",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_insurance_profiles",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_profiles",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_relatives",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_work_profiles",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "job_titles",
                schema: "hr");
        }
    }
}
