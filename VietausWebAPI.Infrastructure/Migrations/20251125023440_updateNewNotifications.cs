using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notification_recipients_team",
                schema: "notification",
                table: "notification_recipients");

            migrationBuilder.EnsureSchema(
                name: "forscada");

            migrationBuilder.AlterColumn<DateTime>(
                name: "processed_at",
                schema: "notification",
                table: "outbox_messages",
                type: "timestamp(6) without time zone",
                precision: 6,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) with time zone",
                oldPrecision: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "notification",
                table: "outbox_messages",
                type: "timestamp(6) without time zone",
                precision: 6,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) with time zone",
                oldPrecision: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "read_date",
                schema: "notification",
                table: "notification_user_states",
                type: "timestamp(6) without time zone",
                precision: 6,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) with time zone",
                oldPrecision: 6,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "infobatfor_plc",
                schema: "forscada",
                columns: table => new
                {
                    d0 = table.Column<int>(type: "integer", nullable: true),
                    d1 = table.Column<int>(type: "integer", nullable: true),
                    d2 = table.Column<int>(type: "integer", nullable: true),
                    d3 = table.Column<int>(type: "integer", nullable: true),
                    d4 = table.Column<int>(type: "integer", nullable: true),
                    d5 = table.Column<int>(type: "integer", nullable: true),
                    d6 = table.Column<int>(type: "integer", nullable: true),
                    d7 = table.Column<int>(type: "integer", nullable: true),
                    d8 = table.Column<int>(type: "integer", nullable: true),
                    d9 = table.Column<int>(type: "integer", nullable: true),
                    machineid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "infoprofor_plc",
                schema: "forscada",
                columns: table => new
                {
                    d0 = table.Column<int>(type: "integer", nullable: true),
                    d1 = table.Column<int>(type: "integer", nullable: true),
                    d2 = table.Column<int>(type: "integer", nullable: true),
                    d3 = table.Column<int>(type: "integer", nullable: true),
                    d4 = table.Column<int>(type: "integer", nullable: true),
                    d5 = table.Column<int>(type: "integer", nullable: true),
                    d6 = table.Column<int>(type: "integer", nullable: true),
                    d7 = table.Column<int>(type: "integer", nullable: true),
                    d8 = table.Column<int>(type: "integer", nullable: true),
                    d9 = table.Column<int>(type: "integer", nullable: true),
                    machineid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "operatorforrecordto_plc",
                schema: "forscada",
                columns: table => new
                {
                    d0 = table.Column<int>(type: "integer", nullable: true),
                    d1 = table.Column<int>(type: "integer", nullable: true),
                    d2 = table.Column<int>(type: "integer", nullable: true),
                    d3 = table.Column<int>(type: "integer", nullable: true),
                    d4 = table.Column<int>(type: "integer", nullable: true),
                    d5 = table.Column<int>(type: "integer", nullable: true),
                    d6 = table.Column<int>(type: "integer", nullable: true),
                    d7 = table.Column<int>(type: "integer", nullable: true),
                    d8 = table.Column<int>(type: "integer", nullable: true),
                    d9 = table.Column<int>(type: "integer", nullable: true),
                    d10 = table.Column<int>(type: "integer", nullable: true),
                    d11 = table.Column<int>(type: "integer", nullable: true),
                    machineid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "productionplan_plpu",
                schema: "forscada",
                columns: table => new
                {
                    machineid = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: true),
                    color = table.Column<string>(type: "text", nullable: true),
                    productioncode = table.Column<string>(type: "text", nullable: true),
                    note1 = table.Column<string>(type: "text", nullable: true),
                    note2 = table.Column<string>(type: "text", nullable: true),
                    note3 = table.Column<string>(type: "text", nullable: true),
                    batchno = table.Column<string>(type: "text", nullable: true),
                    requestdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "productionstatus",
                schema: "forscada",
                columns: table => new
                {
                    machineid = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: true),
                    productioncode = table.Column<string>(type: "text", nullable: true),
                    note1 = table.Column<string>(type: "text", nullable: true),
                    note2 = table.Column<string>(type: "text", nullable: true),
                    note3 = table.Column<string>(type: "text", nullable: true),
                    note4 = table.Column<string>(type: "text", nullable: true),
                    batchno = table.Column<string>(type: "text", nullable: true),
                    requestdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "shiftleaderforrecordto_plc",
                schema: "forscada",
                columns: table => new
                {
                    d0 = table.Column<int>(type: "integer", nullable: true),
                    d1 = table.Column<int>(type: "integer", nullable: true),
                    d2 = table.Column<int>(type: "integer", nullable: true),
                    d3 = table.Column<int>(type: "integer", nullable: true),
                    d4 = table.Column<int>(type: "integer", nullable: true),
                    d5 = table.Column<int>(type: "integer", nullable: true),
                    d6 = table.Column<int>(type: "integer", nullable: true),
                    d7 = table.Column<int>(type: "integer", nullable: true),
                    d8 = table.Column<int>(type: "integer", nullable: true),
                    d9 = table.Column<int>(type: "integer", nullable: true),
                    d10 = table.Column<int>(type: "integer", nullable: true),
                    d11 = table.Column<int>(type: "integer", nullable: true),
                    machineid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "ix_notification_user_states_notification",
                schema: "notification",
                table: "notification_user_states",
                column: "notification_id");

            migrationBuilder.AddForeignKey(
                name: "fk_notification_recipients_team",
                schema: "notification",
                table: "notification_recipients",
                column: "target_team_id",
                principalSchema: "hr",
                principalTable: "Parts",
                principalColumn: "PartID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_notification_recipients_team",
                schema: "notification",
                table: "notification_recipients");

            migrationBuilder.DropTable(
                name: "infobatfor_plc",
                schema: "forscada");

            migrationBuilder.DropTable(
                name: "infoprofor_plc",
                schema: "forscada");

            migrationBuilder.DropTable(
                name: "operatorforrecordto_plc",
                schema: "forscada");

            migrationBuilder.DropTable(
                name: "productionplan_plpu",
                schema: "forscada");

            migrationBuilder.DropTable(
                name: "productionstatus",
                schema: "forscada");

            migrationBuilder.DropTable(
                name: "shiftleaderforrecordto_plc",
                schema: "forscada");

            migrationBuilder.DropIndex(
                name: "ix_notification_user_states_notification",
                schema: "notification",
                table: "notification_user_states");

            migrationBuilder.AlterColumn<DateTime>(
                name: "processed_at",
                schema: "notification",
                table: "outbox_messages",
                type: "timestamp(6) with time zone",
                precision: 6,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) without time zone",
                oldPrecision: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "notification",
                table: "outbox_messages",
                type: "timestamp(6) with time zone",
                precision: 6,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) without time zone",
                oldPrecision: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "read_date",
                schema: "notification",
                table: "notification_user_states",
                type: "timestamp(6) with time zone",
                precision: 6,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) without time zone",
                oldPrecision: 6,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_notification_recipients_team",
                schema: "notification",
                table: "notification_recipients",
                column: "target_team_id",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
