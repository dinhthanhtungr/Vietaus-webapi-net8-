using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInternalMessage4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "web_push_subscriptions",
                schema: "notification",
                columns: table => new
                {
                    web_push_subscription_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    endpoint = table.Column<string>(type: "text", nullable: false),
                    p256dh = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    auth = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    device_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", precision: 6, nullable: false),
                    last_success_at = table.Column<DateTime>(type: "timestamp(6) without time zone", precision: 6, nullable: true),
                    last_failure_at = table.Column<DateTime>(type: "timestamp(6) without time zone", precision: 6, nullable: true),
                    failure_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_web_push_subscriptions", x => x.web_push_subscription_id);
                    table.ForeignKey(
                        name: "fk_web_push_subscriptions_company",
                        column: x => x.company_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_web_push_subscriptions_employee",
                        column: x => x.employee_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_web_push_subscriptions_company_employee_active",
                schema: "notification",
                table: "web_push_subscriptions",
                columns: new[] { "company_id", "employee_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_web_push_subscriptions_employee_id",
                schema: "notification",
                table: "web_push_subscriptions",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ux_web_push_subscriptions_endpoint",
                schema: "notification",
                table: "web_push_subscriptions",
                column: "endpoint",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "web_push_subscriptions",
                schema: "notification");
        }
    }
}
