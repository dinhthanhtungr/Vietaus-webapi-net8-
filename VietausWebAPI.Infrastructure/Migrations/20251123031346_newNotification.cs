using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "notification_templates",
                schema: "notification",
                columns: table => new
                {
                    template_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    locale = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    title_format = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    message_format = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_templates", x => new { x.template_key, x.locale });
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    severity = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    link = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    payload_json = table.Column<string>(type: "jsonb", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_company",
                        column: x => x.company_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    payload_json = table.Column<string>(type: "jsonb", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    processed_at = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: true),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_notification_settings",
                schema: "notification",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    channels = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    min_severity = table.Column<int>(type: "integer", nullable: false),
                    quiet_from = table.Column<TimeSpan>(type: "interval", nullable: true),
                    quiet_to = table.Column<TimeSpan>(type: "interval", nullable: true),
                    locale = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_notification_settings", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_notification_settings_user",
                        column: x => x.user_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification_recipients",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    target_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    target_role = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    target_team_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_recipients", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_recipients_notification",
                        column: x => x.notification_id,
                        principalSchema: "notification",
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_recipients_team",
                        column: x => x.target_team_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_notification_recipients_user",
                        column: x => x.target_user_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notification_user_states",
                schema: "notification",
                columns: table => new
                {
                    notification_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_date = table.Column<DateTime>(type: "timestamp(6) with time zone", precision: 6, nullable: true),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_user_states", x => new { x.notification_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_notification_user_states_notification",
                        column: x => x.notification_id,
                        principalSchema: "notification",
                        principalTable: "notifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_user_states_user",
                        column: x => x.user_id,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_notification_recipients_notification",
                schema: "notification",
                table: "notification_recipients",
                column: "notification_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_recipients_target_role",
                schema: "notification",
                table: "notification_recipients",
                column: "target_role");

            migrationBuilder.CreateIndex(
                name: "ix_notification_recipients_target_team",
                schema: "notification",
                table: "notification_recipients",
                column: "target_team_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_recipients_target_user",
                schema: "notification",
                table: "notification_recipients",
                column: "target_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_user_states_user_read",
                schema: "notification",
                table: "notification_user_states",
                columns: new[] { "user_id", "is_read" });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_company_topic_created",
                schema: "notification",
                table: "notifications",
                columns: new[] { "company_id", "topic", "created_date" });

            migrationBuilder.CreateIndex(
                name: "ix_outbox_unprocessed",
                schema: "notification",
                table: "outbox_messages",
                column: "processed_at",
                filter: "processed_at IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_recipients",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_templates",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_user_states",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "user_notification_settings",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "notification");
        }
    }
}
