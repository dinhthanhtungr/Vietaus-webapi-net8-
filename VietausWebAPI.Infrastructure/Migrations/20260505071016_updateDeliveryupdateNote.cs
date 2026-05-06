using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDeliveryupdateNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentNameSnapshot",
                schema: "hr",
                table: "employee_work_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitleEnglishSnapshot",
                schema: "hr",
                table: "employee_work_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitleSnapshot",
                schema: "hr",
                table: "employee_work_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SectionNameSnapshot",
                schema: "hr",
                table: "employee_work_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamNameSnapshot",
                schema: "hr",
                table: "employee_work_profiles",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "employee_insurance_books",
                schema: "hr",
                columns: table => new
                {
                    employee_insurance_book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_code_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    employee_name_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    job_title_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    social_insurance_number_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    has_book_cover = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    has_detached_leaf = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    detached_leaf_count = table.Column<int>(type: "integer", nullable: true),
                    detached_leaf_from_date = table.Column<DateOnly>(type: "date", nullable: true),
                    detached_leaf_to_date = table.Column<DateOnly>(type: "date", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_insurance_books", x => x.employee_insurance_book_id);
                    table.ForeignKey(
                        name: "FK_employee_insurance_books_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "fk_employee_insurance_books_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee_insurance_claims",
                schema: "hr",
                columns: table => new
                {
                    employee_insurance_claim_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    claim_month = table.Column<DateOnly>(type: "date", nullable: true),
                    claim_month_label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    sequence_no = table.Column<int>(type: "integer", nullable: true),
                    employee_name_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    social_insurance_number_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    leave_reason = table.Column<string>(type: "text", nullable: true),
                    leave_from_date = table.Column<DateOnly>(type: "date", nullable: true),
                    leave_to_date = table.Column<DateOnly>(type: "date", nullable: true),
                    leave_days = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    claim_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    claim_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    processing_status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    appointment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    returned_to_employee_date = table.Column<DateOnly>(type: "date", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_insurance_claims", x => x.employee_insurance_claim_id);
                    table.ForeignKey(
                        name: "FK_employee_insurance_claims_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "fk_employee_insurance_claims_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "employee_salary_packages",
                schema: "hr",
                columns: table => new
                {
                    employee_salary_package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    basic_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    insurance_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    standard_working_days = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    standard_working_hours = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: false),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
                    is_current = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    bank_account_id = table.Column<Guid>(type: "uuid", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_salary_packages", x => x.employee_salary_package_id);
                    table.ForeignKey(
                        name: "fk_employee_salary_packages_bank_account",
                        column: x => x.bank_account_id,
                        principalSchema: "hr",
                        principalTable: "employee_bank_accounts",
                        principalColumn: "employee_bank_account_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_employee_salary_packages_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payroll_periods",
                schema: "hr",
                columns: table => new
                {
                    payroll_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    month = table.Column<int>(type: "integer", nullable: false),
                    from_date = table.Column<DateOnly>(type: "date", nullable: false),
                    to_date = table.Column<DateOnly>(type: "date", nullable: false),
                    payroll_type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    approved_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payroll_periods", x => x.payroll_period_id);
                    table.ForeignKey(
                        name: "fk_payroll_periods_approved_by",
                        column: x => x.approved_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_payroll_periods_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "salary_component_definitions",
                schema: "hr",
                columns: table => new
                {
                    salary_component_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    category = table.Column<int>(type: "integer", nullable: false),
                    value_type = table.Column<int>(type: "integer", nullable: false),
                    affects_gross = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    affects_net = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    affects_insurance_base = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    affects_taxable_income = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    formula_template = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_salary_component_definitions", x => x.salary_component_definition_id);
                });

            migrationBuilder.CreateTable(
                name: "employee_insurance_contributions",
                schema: "hr",
                columns: table => new
                {
                    employee_insurance_contribution_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payroll_period_id = table.Column<Guid>(type: "uuid", nullable: true),
                    contribution_month = table.Column<DateOnly>(type: "date", nullable: false),
                    employee_name_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    department_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    job_title_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    social_insurance_number_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    social_insurance_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    unemployment_insurance_salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    company_retirement_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    company_sickness_maternity_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    company_occupational_accident_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    company_health_insurance_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    company_unemployment_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    company_total_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    employee_social_insurance_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    employee_health_insurance_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    employee_unemployment_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    employee_total_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    monthly_total_payable_rate = table.Column<decimal>(type: "numeric(9,6)", precision: 9, scale: 6, nullable: true),
                    company_retirement_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    company_sickness_maternity_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    company_occupational_accident_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    company_health_insurance_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    company_unemployment_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    company_total_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    employee_social_insurance_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    employee_health_insurance_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    employee_unemployment_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    employee_total_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    monthly_total_payable_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_insurance_contributions", x => x.employee_insurance_contribution_id);
                    table.ForeignKey(
                        name: "FK_employee_insurance_contributions_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "fk_employee_insurance_contributions_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_employee_insurance_contributions_period",
                        column: x => x.payroll_period_id,
                        principalSchema: "hr",
                        principalTable: "payroll_periods",
                        principalColumn: "payroll_period_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "payroll_employee_runs",
                schema: "hr",
                columns: table => new
                {
                    payroll_employee_run_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payroll_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_code_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    employee_name_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    department_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    job_title_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    bank_account_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    base_salary_snapshot = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    insurance_salary_snapshot = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    gross_income = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_earnings = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    total_deductions = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    employer_contribution_total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    net_income = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    locked_by = table.Column<Guid>(type: "uuid", nullable: true),
                    locked_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payroll_employee_runs", x => x.payroll_employee_run_id);
                    table.ForeignKey(
                        name: "FK_payroll_employee_runs_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "fk_payroll_employee_runs_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payroll_employee_runs_locked_by",
                        column: x => x.locked_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_payroll_employee_runs_period",
                        column: x => x.payroll_period_id,
                        principalSchema: "hr",
                        principalTable: "payroll_periods",
                        principalColumn: "payroll_period_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_salary_package_components",
                schema: "hr",
                columns: table => new
                {
                    employee_salary_package_component_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_salary_package_id = table.Column<Guid>(type: "uuid", nullable: false),
                    salary_component_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    formula_text = table.Column<string>(type: "text", nullable: true),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    effective_from = table.Column<DateOnly>(type: "date", nullable: false),
                    effective_to = table.Column<DateOnly>(type: "date", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_salary_package_components", x => x.employee_salary_package_component_id);
                    table.ForeignKey(
                        name: "fk_employee_salary_package_components_definition",
                        column: x => x.salary_component_definition_id,
                        principalSchema: "hr",
                        principalTable: "salary_component_definitions",
                        principalColumn: "salary_component_definition_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_employee_salary_package_components_package",
                        column: x => x.employee_salary_package_id,
                        principalSchema: "hr",
                        principalTable: "employee_salary_packages",
                        principalColumn: "employee_salary_package_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payroll_employee_run_details",
                schema: "hr",
                columns: table => new
                {
                    payroll_employee_run_detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payroll_employee_run_id = table.Column<Guid>(type: "uuid", nullable: false),
                    salary_component_definition_id = table.Column<Guid>(type: "uuid", nullable: false),
                    component_code_snapshot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    component_name_snapshot = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    category_snapshot = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    source_type = table.Column<int>(type: "integer", nullable: false),
                    source_reference = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    formula_text_snapshot = table.Column<string>(type: "text", nullable: true),
                    sort_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payroll_employee_run_details", x => x.payroll_employee_run_detail_id);
                    table.ForeignKey(
                        name: "fk_payroll_employee_run_details_component",
                        column: x => x.salary_component_definition_id,
                        principalSchema: "hr",
                        principalTable: "salary_component_definitions",
                        principalColumn: "salary_component_definition_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_payroll_employee_run_details_run",
                        column: x => x.payroll_employee_run_id,
                        principalSchema: "hr",
                        principalTable: "payroll_employee_runs",
                        principalColumn: "payroll_employee_run_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_employee_insurance_books_employee",
                schema: "hr",
                table: "employee_insurance_books",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_insurance_books_EmployeeId1",
                schema: "hr",
                table: "employee_insurance_books",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "ix_employee_insurance_books_social_number",
                schema: "hr",
                table: "employee_insurance_books",
                column: "social_insurance_number_snapshot");

            migrationBuilder.CreateIndex(
                name: "ix_employee_insurance_claims_claim_number",
                schema: "hr",
                table: "employee_insurance_claims",
                column: "claim_number");

            migrationBuilder.CreateIndex(
                name: "ix_employee_insurance_claims_employee",
                schema: "hr",
                table: "employee_insurance_claims",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_insurance_claims_EmployeeId1",
                schema: "hr",
                table: "employee_insurance_claims",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "ix_employee_insurance_claims_month",
                schema: "hr",
                table: "employee_insurance_claims",
                column: "claim_month");

            migrationBuilder.CreateIndex(
                name: "ix_employee_insurance_contributions_employee_month",
                schema: "hr",
                table: "employee_insurance_contributions",
                columns: new[] { "employee_id", "contribution_month" });

            migrationBuilder.CreateIndex(
                name: "IX_employee_insurance_contributions_EmployeeId1",
                schema: "hr",
                table: "employee_insurance_contributions",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_employee_insurance_contributions_payroll_period_id",
                schema: "hr",
                table: "employee_insurance_contributions",
                column: "payroll_period_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_salary_package_components_package",
                schema: "hr",
                table: "employee_salary_package_components",
                column: "employee_salary_package_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_salary_package_components_salary_component_definit~",
                schema: "hr",
                table: "employee_salary_package_components",
                column: "salary_component_definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_salary_packages_bank_account_id",
                schema: "hr",
                table: "employee_salary_packages",
                column: "bank_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_employee_salary_packages_employee",
                schema: "hr",
                table: "employee_salary_packages",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ux_employee_salary_packages_one_current",
                schema: "hr",
                table: "employee_salary_packages",
                columns: new[] { "employee_id", "is_current" },
                unique: true,
                filter: "\"is_current\" = true");

            migrationBuilder.CreateIndex(
                name: "ix_payroll_employee_run_details_component",
                schema: "hr",
                table: "payroll_employee_run_details",
                column: "salary_component_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_payroll_employee_run_details_run",
                schema: "hr",
                table: "payroll_employee_run_details",
                column: "payroll_employee_run_id");

            migrationBuilder.CreateIndex(
                name: "IX_payroll_employee_runs_employee_id",
                schema: "hr",
                table: "payroll_employee_runs",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_payroll_employee_runs_EmployeeId1",
                schema: "hr",
                table: "payroll_employee_runs",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_payroll_employee_runs_locked_by",
                schema: "hr",
                table: "payroll_employee_runs",
                column: "locked_by");

            migrationBuilder.CreateIndex(
                name: "ux_payroll_employee_runs_period_employee",
                schema: "hr",
                table: "payroll_employee_runs",
                columns: new[] { "payroll_period_id", "employee_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payroll_periods_approved_by",
                schema: "hr",
                table: "payroll_periods",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "IX_payroll_periods_created_by",
                schema: "hr",
                table: "payroll_periods",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_payroll_periods_year_month_type",
                schema: "hr",
                table: "payroll_periods",
                columns: new[] { "year", "month", "payroll_type" });

            migrationBuilder.CreateIndex(
                name: "ux_payroll_periods_code",
                schema: "hr",
                table: "payroll_periods",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_salary_component_definitions_category_active",
                schema: "hr",
                table: "salary_component_definitions",
                columns: new[] { "category", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ux_salary_component_definitions_code",
                schema: "hr",
                table: "salary_component_definitions",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_insurance_books",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_insurance_claims",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_insurance_contributions",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_salary_package_components",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "payroll_employee_run_details",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "employee_salary_packages",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "salary_component_definitions",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "payroll_employee_runs",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "payroll_periods",
                schema: "hr");

            migrationBuilder.DropColumn(
                name: "DepartmentNameSnapshot",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.DropColumn(
                name: "JobTitleEnglishSnapshot",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.DropColumn(
                name: "JobTitleSnapshot",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.DropColumn(
                name: "SectionNameSnapshot",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.DropColumn(
                name: "TeamNameSnapshot",
                schema: "hr",
                table: "employee_work_profiles");
        }
    }
}
