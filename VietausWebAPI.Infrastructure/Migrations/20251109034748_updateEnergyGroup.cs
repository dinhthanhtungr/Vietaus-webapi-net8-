using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateEnergyGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "energy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "mro",
                table: "transferheaders",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "mro",
                table: "pmplan",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "moved_at",
                schema: "mro",
                table: "movements",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.CreateTable(
                name: "groups",
                schema: "energy",
                columns: table => new
                {
                    group_id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_groups", x => x.group_id);
                });

            migrationBuilder.CreateTable(
                name: "tariffs",
                schema: "energy",
                columns: table => new
                {
                    tariff_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: false),
                    currency = table.Column<string>(type: "citext", nullable: false, defaultValue: "VND"),
                    utility = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_tariffs", x => x.tariff_id);
                });

            migrationBuilder.CreateTable(
                name: "tou_calendar",
                schema: "energy",
                columns: table => new
                {
                    calendar_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    tz = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_tou_calendar", x => x.calendar_id);
                });

            migrationBuilder.CreateTable(
                name: "meters",
                schema: "energy",
                columns: table => new
                {
                    meter_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "citext", nullable: false),
                    name = table.Column<string>(type: "citext", nullable: false),
                    multiplier = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: false, defaultValue: 1.0000m),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    group_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_meters", x => x.meter_id);
                    table.ForeignKey(
                        name: "fk_energy_meters_group",
                        column: x => x.group_id,
                        principalSchema: "energy",
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "group_tariff_map",
                schema: "energy",
                columns: table => new
                {
                    group_id = table.Column<short>(type: "smallint", nullable: false),
                    valid_from = table.Column<DateOnly>(type: "date", nullable: false),
                    tariff_id = table.Column<int>(type: "integer", nullable: false),
                    valid_to = table.Column<DateOnly>(type: "date", nullable: true),
                    GroupId1 = table.Column<short>(type: "smallint", nullable: true),
                    TariffId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_group_tariff_map", x => new { x.group_id, x.valid_from });
                    table.ForeignKey(
                        name: "FK_group_tariff_map_groups_GroupId1",
                        column: x => x.GroupId1,
                        principalSchema: "energy",
                        principalTable: "groups",
                        principalColumn: "group_id");
                    table.ForeignKey(
                        name: "FK_group_tariff_map_tariffs_TariffId1",
                        column: x => x.TariffId1,
                        principalSchema: "energy",
                        principalTable: "tariffs",
                        principalColumn: "tariff_id");
                    table.ForeignKey(
                        name: "fk_energy_gtm_group",
                        column: x => x.group_id,
                        principalSchema: "energy",
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_energy_gtm_tariff",
                        column: x => x.tariff_id,
                        principalSchema: "energy",
                        principalTable: "tariffs",
                        principalColumn: "tariff_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tariff_versions",
                schema: "energy",
                columns: table => new
                {
                    version_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tariff_id = table.Column<int>(type: "integer", nullable: false),
                    valid_from = table.Column<DateTime>(type: "date", nullable: false),
                    valid_to = table.Column<DateTime>(type: "date", nullable: true),
                    vat_rate = table.Column<decimal>(type: "numeric(6,4)", precision: 6, scale: 4, nullable: false, defaultValue: 0m),
                    fuel_adj_vnd_per_kwh = table.Column<decimal>(type: "numeric(12,4)", precision: 12, scale: 4, nullable: false, defaultValue: 0m),
                    service_fixed_vnd_per_month = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false, defaultValue: 0m),
                    demand_rate_vnd_per_kw = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_tariff_versions", x => x.version_id);
                    table.ForeignKey(
                        name: "fk_energy_tariff_versions_tariff",
                        column: x => x.tariff_id,
                        principalSchema: "energy",
                        principalTable: "tariffs",
                        principalColumn: "tariff_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tou_exceptions",
                schema: "energy",
                columns: table => new
                {
                    exception_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    calendar_id = table.Column<int>(type: "integer", nullable: false),
                    the_date = table.Column<DateOnly>(type: "date", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    band = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_tou_exceptions", x => x.exception_id);
                    table.ForeignKey(
                        name: "fk_energy_tou_exc_calendar",
                        column: x => x.calendar_id,
                        principalSchema: "energy",
                        principalTable: "tou_calendar",
                        principalColumn: "calendar_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tou_windows",
                schema: "energy",
                columns: table => new
                {
                    window_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    calendar_id = table.Column<int>(type: "integer", nullable: false),
                    weekday = table.Column<short>(type: "smallint", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    band = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_tou_windows", x => x.window_id);
                    table.ForeignKey(
                        name: "fk_energy_tou_windows_calendar",
                        column: x => x.calendar_id,
                        principalSchema: "energy",
                        principalTable: "tou_calendar",
                        principalColumn: "calendar_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meter_comm_config",
                schema: "energy",
                columns: table => new
                {
                    meter_id = table.Column<long>(type: "bigint", nullable: false),
                    protocol = table.Column<string>(type: "text", nullable: true),
                    serial_port = table.Column<string>(type: "text", nullable: true),
                    baud_rate = table.Column<int>(type: "integer", nullable: true),
                    parity = table.Column<string>(type: "text", nullable: true),
                    data_bits = table.Column<short>(type: "smallint", nullable: true),
                    stop_bits = table.Column<short>(type: "smallint", nullable: true),
                    slave_id = table.Column<short>(type: "smallint", nullable: true),
                    reg_kwh_addr = table.Column<int>(type: "integer", nullable: true),
                    reg_kwh_len = table.Column<short>(type: "smallint", nullable: true),
                    word_order = table.Column<string>(type: "text", nullable: true),
                    scale = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_meter_comm_config", x => x.meter_id);
                    table.ForeignKey(
                        name: "fk_energy_mcc_meter",
                        column: x => x.meter_id,
                        principalSchema: "energy",
                        principalTable: "meters",
                        principalColumn: "meter_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meter_group_history",
                schema: "energy",
                columns: table => new
                {
                    meter_id = table.Column<long>(type: "bigint", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    group_id = table.Column<short>(type: "smallint", nullable: false),
                    valid_to = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_meter_group_history", x => new { x.meter_id, x.valid_from });
                    table.ForeignKey(
                        name: "fk_energy_mgh_group",
                        column: x => x.group_id,
                        principalSchema: "energy",
                        principalTable: "groups",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_energy_mgh_meter",
                        column: x => x.meter_id,
                        principalSchema: "energy",
                        principalTable: "meters",
                        principalColumn: "meter_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "readings_hourly",
                schema: "energy",
                columns: table => new
                {
                    meter_id = table.Column<long>(type: "bigint", nullable: false),
                    ts_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    kwh_import = table.Column<decimal>(type: "numeric(14,5)", precision: 14, scale: 5, nullable: false),
                    quality = table.Column<string>(type: "text", nullable: true),
                    source = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_readings_hourly", x => new { x.meter_id, x.ts_utc });
                    table.ForeignKey(
                        name: "fk_energy_rh_meter",
                        column: x => x.meter_id,
                        principalSchema: "energy",
                        principalTable: "meters",
                        principalColumn: "meter_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "readings_hourly_vn",
                schema: "energy",
                columns: table => new
                {
                    meter_id = table.Column<long>(type: "bigint", nullable: false),
                    ts_hour_vn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    kwh_import = table.Column<decimal>(type: "numeric(14,5)", precision: 14, scale: 5, nullable: false),
                    quality = table.Column<string>(type: "text", nullable: true),
                    source = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_readings_hourly_vn", x => new { x.meter_id, x.ts_hour_vn });
                    table.ForeignKey(
                        name: "fk_energy_rhvn_meter",
                        column: x => x.meter_id,
                        principalSchema: "energy",
                        principalTable: "meters",
                        principalColumn: "meter_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "register_snapshots",
                schema: "energy",
                columns: table => new
                {
                    meter_id = table.Column<long>(type: "bigint", nullable: false),
                    ts_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    kwh_total = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    source = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_register_snapshots", x => new { x.meter_id, x.ts_utc });
                    table.ForeignKey(
                        name: "fk_energy_register_snapshots_meter",
                        column: x => x.meter_id,
                        principalSchema: "energy",
                        principalTable: "meters",
                        principalColumn: "meter_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tariff_band_rates",
                schema: "energy",
                columns: table => new
                {
                    version_id = table.Column<int>(type: "integer", nullable: false),
                    band = table.Column<string>(type: "citext", nullable: false),
                    price_vnd_per_kwh = table.Column<decimal>(type: "numeric(12,4)", precision: 12, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_energy_tariff_band_rates", x => new { x.version_id, x.band });
                    table.ForeignKey(
                        name: "fk_energy_tariff_band_rates_version",
                        column: x => x.version_id,
                        principalSchema: "energy",
                        principalTable: "tariff_versions",
                        principalColumn: "version_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_energy_gtm_group_validfrom_desc",
                schema: "energy",
                table: "group_tariff_map",
                columns: new[] { "group_id", "valid_from" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ix_energy_gtm_tariff",
                schema: "energy",
                table: "group_tariff_map",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_tariff_map_GroupId1",
                schema: "energy",
                table: "group_tariff_map",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_group_tariff_map_TariffId1",
                schema: "energy",
                table: "group_tariff_map",
                column: "TariffId1");

            migrationBuilder.CreateIndex(
                name: "ux_energy_groups_code",
                schema: "energy",
                table: "groups",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_energy_mgh_meter",
                schema: "energy",
                table: "meter_group_history",
                column: "meter_id");

            migrationBuilder.CreateIndex(
                name: "ix_energy_mgh_meter_validfrom_desc",
                schema: "energy",
                table: "meter_group_history",
                columns: new[] { "meter_id", "valid_from" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_meter_group_history_group_id",
                schema: "energy",
                table: "meter_group_history",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "ix_energy_meters_group_active",
                schema: "energy",
                table: "meters",
                columns: new[] { "group_id", "is_active", "meter_id" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "ux_energy_meters_code",
                schema: "energy",
                table: "meters",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_energy_rh_meter_ts_desc",
                schema: "energy",
                table: "readings_hourly",
                columns: new[] { "meter_id", "ts_utc" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ix_energy_rhvn_meter_tshour_desc",
                schema: "energy",
                table: "readings_hourly_vn",
                columns: new[] { "meter_id", "ts_hour_vn" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ix_energy_rhvn_tshour",
                schema: "energy",
                table: "readings_hourly_vn",
                column: "ts_hour_vn");

            migrationBuilder.CreateIndex(
                name: "ix_energy_register_snapshots_meter_ts",
                schema: "energy",
                table: "register_snapshots",
                columns: new[] { "meter_id", "ts_utc" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "ix_energy_tariff_band_rates_version",
                schema: "energy",
                table: "tariff_band_rates",
                column: "version_id");

            migrationBuilder.CreateIndex(
                name: "ix_energy_tariff_versions_tariff_id",
                schema: "energy",
                table: "tariff_versions",
                column: "tariff_id");

            migrationBuilder.CreateIndex(
                name: "ux_energy_tariff_versions_tariff_validfrom",
                schema: "energy",
                table: "tariff_versions",
                columns: new[] { "tariff_id", "valid_from" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_energy_tariffs_code",
                schema: "energy",
                table: "tariffs",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_energy_tou_calendar_code",
                schema: "energy",
                table: "tou_calendar",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_energy_tou_exc_calendar_date",
                schema: "energy",
                table: "tou_exceptions",
                columns: new[] { "calendar_id", "the_date" });

            migrationBuilder.CreateIndex(
                name: "ux_energy_tou_exc_unique_slot",
                schema: "energy",
                table: "tou_exceptions",
                columns: new[] { "calendar_id", "the_date", "band", "start_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_energy_tou_windows_cal_wd_start",
                schema: "energy",
                table: "tou_windows",
                columns: new[] { "calendar_id", "weekday", "start_time" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_tariff_map",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "meter_comm_config",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "meter_group_history",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "readings_hourly",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "readings_hourly_vn",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "register_snapshots",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "tariff_band_rates",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "tou_exceptions",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "tou_windows",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "meters",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "tariff_versions",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "tou_calendar",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "groups",
                schema: "energy");

            migrationBuilder.DropTable(
                name: "tariffs",
                schema: "energy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "mro",
                table: "transferheaders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                schema: "mro",
                table: "pmplan",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "moved_at",
                schema: "mro",
                table: "movements",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
