using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_notifications_company_topic_created",
                schema: "notification",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "topic",
                schema: "notification",
                table: "notifications");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_company_id",
                schema: "notification",
                table: "notifications",
                column: "company_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_notifications_company_id",
                schema: "notification",
                table: "notifications");

            migrationBuilder.AddColumn<string>(
                name: "topic",
                schema: "notification",
                table: "notifications",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_company_topic_created",
                schema: "notification",
                table: "notifications",
                columns: new[] { "company_id", "topic", "created_date" });
        }
    }
}
