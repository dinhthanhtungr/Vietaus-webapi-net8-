using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewNotifications2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                schema: "notification",
                table: "notifications",
                type: "timestamp(6) without time zone",
                precision: 6,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) with time zone",
                oldPrecision: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                schema: "notification",
                table: "notifications",
                type: "timestamp(6) with time zone",
                precision: 6,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6) without time zone",
                oldPrecision: 6);
        }
    }
}
