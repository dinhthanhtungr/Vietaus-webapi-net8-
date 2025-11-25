using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateTimettemp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "start_time",
                schema: "energy",
                table: "tou_windows",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_time",
                schema: "energy",
                table: "tou_windows",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_time",
                schema: "energy",
                table: "tou_exceptions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_time",
                schema: "energy",
                table: "tou_exceptions",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "start_date",
                schema: "energy",
                table: "tou_calendar",
                type: "date",
                nullable: true,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "end_date",
                schema: "energy",
                table: "tou_calendar",
                type: "date",
                nullable: true,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_to",
                schema: "energy",
                table: "tariff_versions",
                type: "date",
                nullable: true,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_from",
                schema: "energy",
                table: "tariff_versions",
                type: "date",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ts_hour_vn",
                schema: "energy",
                table: "readings_hourly_vn",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ts_utc",
                schema: "energy",
                table: "readings_hourly",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_to",
                schema: "energy",
                table: "meter_group_history",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_from",
                schema: "energy",
                table: "meter_group_history",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "valid_to",
                schema: "energy",
                table: "group_tariff_map",
                type: "date",
                nullable: true,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "valid_from",
                schema: "energy",
                table: "group_tariff_map",
                type: "date",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "start_time",
                schema: "energy",
                table: "tou_windows",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_time",
                schema: "energy",
                table: "tou_windows",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "start_time",
                schema: "energy",
                table: "tou_exceptions",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "end_time",
                schema: "energy",
                table: "tou_exceptions",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "start_date",
                schema: "energy",
                table: "tou_calendar",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "end_date",
                schema: "energy",
                table: "tou_calendar",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_to",
                schema: "energy",
                table: "tariff_versions",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_from",
                schema: "energy",
                table: "tariff_versions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ts_hour_vn",
                schema: "energy",
                table: "readings_hourly_vn",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ts_utc",
                schema: "energy",
                table: "readings_hourly",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_to",
                schema: "energy",
                table: "meter_group_history",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "valid_from",
                schema: "energy",
                table: "meter_group_history",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "valid_to",
                schema: "energy",
                table: "group_tariff_map",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true,
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "valid_from",
                schema: "energy",
                table: "group_tariff_map",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValueSql: "timezone('utc', now())");
        }
    }
}
