using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomerLead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "devandqa",
                table: "ProductStandard",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLead",
                schema: "Customer",
                table: "Customer",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "LeadStatus",
                schema: "Customer",
                table: "Customer",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CustomerClaims",
                schema: "crm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    customerId = table.Column<Guid>(type: "uuid", nullable: false),
                    employeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    groupId = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    expiresAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerClaims_Id", x => x.id);
                    table.ForeignKey(
                        name: "FK_CustomerClaims_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerClaims_Customer",
                        column: x => x.customerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerClaims_Employee",
                        column: x => x.employeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerClaims_Group",
                        column: x => x.groupId,
                        principalSchema: "company",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerNotes",
                schema: "crm",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    customerId = table.Column<Guid>(type: "uuid", nullable: false),
                    authorEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    authorGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    visibility = table.Column<int>(type: "integer", nullable: false),
                    isApprovedShare = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    createdAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNotes_Id", x => x.id);
                    table.ForeignKey(
                        name: "FK_CustomerNotes_AuthorEmployee",
                        column: x => x.authorEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerNotes_AuthorGroup",
                        column: x => x.authorGroupId,
                        principalSchema: "company",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerNotes_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerNotes_Customer",
                        column: x => x.customerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_customer_company_islead_status_created_desc",
                schema: "Customer",
                table: "Customer",
                columns: new[] { "CompanyId", "IsLead", "LeadStatus", "CreatedDate" },
                descending: new[] { false, false, false, true });

            migrationBuilder.CreateIndex(
                name: "ix_customerclaims_active_by_company",
                schema: "crm",
                table: "CustomerClaims",
                columns: new[] { "companyId", "expiresAt" },
                filter: "\"isActive\" = TRUE AND \"type\" = 1");

            migrationBuilder.CreateIndex(
                name: "ix_customerclaims_by_customer_active",
                schema: "crm",
                table: "CustomerClaims",
                columns: new[] { "customerId", "expiresAt" },
                filter: "\"isActive\" = TRUE AND \"type\" = 1");

            migrationBuilder.CreateIndex(
                name: "ix_customerclaims_by_group_active",
                schema: "crm",
                table: "CustomerClaims",
                columns: new[] { "companyId", "groupId", "expiresAt" },
                filter: "\"isActive\" = TRUE AND \"type\" = 1");

            migrationBuilder.CreateIndex(
                name: "ix_customerclaims_company_customer",
                schema: "crm",
                table: "CustomerClaims",
                columns: new[] { "companyId", "customerId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerClaims_employeeId",
                schema: "crm",
                table: "CustomerClaims",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "ix_customerclaims_expiresat",
                schema: "crm",
                table: "CustomerClaims",
                column: "expiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerClaims_groupId",
                schema: "crm",
                table: "CustomerClaims",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "ux_customerclaims_active_unique",
                schema: "crm",
                table: "CustomerClaims",
                columns: new[] { "customerId", "employeeId", "groupId", "type" },
                unique: true,
                filter: "\"isActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_authorEmployeeId",
                schema: "crm",
                table: "CustomerNotes",
                column: "authorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_authorGroupId",
                schema: "crm",
                table: "CustomerNotes",
                column: "authorGroupId");

            migrationBuilder.CreateIndex(
                name: "ix_customernotes_company_customer_createdat",
                schema: "crm",
                table: "CustomerNotes",
                columns: new[] { "companyId", "customerId", "createdAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerNotes_customerId",
                schema: "crm",
                table: "CustomerNotes",
                column: "customerId");

            migrationBuilder.CreateIndex(
                name: "ix_customernotes_public",
                schema: "crm",
                table: "CustomerNotes",
                columns: new[] { "companyId", "customerId" },
                filter: "\"isApprovedShare\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerClaims",
                schema: "crm");

            migrationBuilder.DropTable(
                name: "CustomerNotes",
                schema: "crm");

            migrationBuilder.DropIndex(
                name: "ix_customer_company_islead_status_created_desc",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "devandqa",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "IsLead",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LeadStatus",
                schema: "Customer",
                table: "Customer");
        }
    }
}
