using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewNotifications3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "historyrecords");

            migrationBuilder.EnsureSchema(
                name: "shiftreports");

            migrationBuilder.EnsureSchema(
                name: "printect");

            migrationBuilder.EnsureSchema(
                name: "printcell");

            migrationBuilder.CreateTable(
                name: "assign_task",
                schema: "historyrecords",
                columns: table => new
                {
                    assign_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    shiftid = table.Column<short>(type: "smallint", nullable: false),
                    machineid = table.Column<string>(type: "citext", nullable: false),
                    @operator = table.Column<Guid>(name: "operator", type: "uuid", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "endofshiftreportdetailforall",
                schema: "shiftreports",
                columns: table => new
                {
                    shiftreportdetailforall_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    externalid = table.Column<string>(type: "citext", nullable: true),
                    producttype = table.Column<string>(type: "text", nullable: true),
                    netweight = table.Column<decimal>(type: "numeric(16,3)", precision: 16, scale: 3, nullable: true),
                    weightstockedkg = table.Column<decimal>(type: "numeric(16,3)", precision: 16, scale: 3, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "endofshiftreportforall",
                schema: "shiftreports",
                columns: table => new
                {
                    shiftreportforall_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    shiftid = table.Column<short>(type: "smallint", nullable: false),
                    machineid = table.Column<Guid>(type: "uuid", nullable: false),
                    @operator = table.Column<Guid>(name: "operator", type: "uuid", nullable: false),
                    productcode = table.Column<string>(type: "citext", nullable: true),
                    externalid = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    productstatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "EventHistory",
                schema: "historyrecords",
                columns: table => new
                {
                    eventhistory_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    machine_id = table.Column<string>(type: "citext", nullable: true),
                    event_id = table.Column<short>(type: "smallint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "history_print_label_for_all",
                schema: "printect",
                columns: table => new
                {
                    historyprintlabelforall_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    externalid = table.Column<string>(type: "citext", nullable: true),
                    numberofcopies = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    shiftid = table.Column<short>(type: "smallint", nullable: true),
                    lognumber = table.Column<int>(type: "integer", nullable: true),
                    productiondate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "labelelements",
                schema: "printcell",
                columns: table => new
                {
                    elementid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    templateid = table.Column<int>(type: "integer", nullable: false),
                    labeltype = table.Column<string>(type: "citext", nullable: true),
                    x = table.Column<int>(type: "integer", nullable: false),
                    y = table.Column<int>(type: "integer", nullable: false),
                    width = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    fontsize = table.Column<int>(type: "integer", nullable: false),
                    alignment = table.Column<string>(type: "citext", nullable: true),
                    bold = table.Column<bool>(type: "boolean", nullable: false),
                    italic = table.Column<bool>(type: "boolean", nullable: false),
                    valuetype = table.Column<string>(type: "citext", nullable: true),
                    prefixtext = table.Column<string>(type: "text", nullable: true),
                    rendertype = table.Column<string>(type: "citext", nullable: true),
                    fontname = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "labeltemplates",
                schema: "printcell",
                columns: table => new
                {
                    templateid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    widthmm = table.Column<int>(type: "integer", nullable: false),
                    heightmm = table.Column<int>(type: "integer", nullable: false),
                    labeltype = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "machine_history",
                schema: "historyrecords",
                columns: table => new
                {
                    machine_history_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    producing_time_of_day = table.Column<int>(type: "integer", nullable: false),
                    waiting_time_of_day = table.Column<int>(type: "integer", nullable: false),
                    machine_cleansing_time_of_day = table.Column<int>(type: "integer", nullable: false),
                    energy_total_of_day = table.Column<int>(type: "integer", nullable: false),
                    producing_energy_of_day = table.Column<int>(type: "integer", nullable: false),
                    machine_cleansing_energy_of_day = table.Column<int>(type: "integer", nullable: false),
                    waiting_energy_of_day = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OperationHistory_MD",
                schema: "shiftreports",
                columns: table => new
                {
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    productcode = table.Column<string>(type: "text", nullable: true),
                    externalId = table.Column<string>(type: "text", nullable: true),
                    set1 = table.Column<int>(type: "integer", nullable: true),
                    act1 = table.Column<int>(type: "integer", nullable: true),
                    set2 = table.Column<int>(type: "integer", nullable: true),
                    act2 = table.Column<int>(type: "integer", nullable: true),
                    set3 = table.Column<int>(type: "integer", nullable: true),
                    act3 = table.Column<int>(type: "integer", nullable: true),
                    set4 = table.Column<int>(type: "integer", nullable: true),
                    act4 = table.Column<int>(type: "integer", nullable: true),
                    set5 = table.Column<int>(type: "integer", nullable: true),
                    act5 = table.Column<int>(type: "integer", nullable: true),
                    set6 = table.Column<int>(type: "integer", nullable: true),
                    act6 = table.Column<int>(type: "integer", nullable: true),
                    set7 = table.Column<int>(type: "integer", nullable: true),
                    act7 = table.Column<int>(type: "integer", nullable: true),
                    set8 = table.Column<int>(type: "integer", nullable: true),
                    act8 = table.Column<int>(type: "integer", nullable: true),
                    set9 = table.Column<int>(type: "integer", nullable: true),
                    act9 = table.Column<int>(type: "integer", nullable: true),
                    set10 = table.Column<int>(type: "integer", nullable: true),
                    act10 = table.Column<int>(type: "integer", nullable: true),
                    set11 = table.Column<int>(type: "integer", nullable: true),
                    act11 = table.Column<int>(type: "integer", nullable: true),
                    set12 = table.Column<int>(type: "integer", nullable: true),
                    act12 = table.Column<int>(type: "integer", nullable: true),
                    set13 = table.Column<int>(type: "integer", nullable: true),
                    act13 = table.Column<int>(type: "integer", nullable: true),
                    screwspeed = table.Column<int>(type: "integer", nullable: true),
                    screwcurrent = table.Column<int>(type: "integer", nullable: true),
                    feederspeed = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "plan_getback_history",
                schema: "historyrecords",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    externalid = table.Column<string>(type: "citext", nullable: true),
                    colorcode = table.Column<string>(type: "citext", nullable: true),
                    statusbefore = table.Column<string>(type: "citext", nullable: true),
                    reason = table.Column<string>(type: "text", nullable: true),
                    performedby = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "shifts",
                schema: "historyrecords",
                columns: table => new
                {
                    shiftid = table.Column<short>(type: "smallint", nullable: false),
                    shiftname = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "shiftsevent",
                schema: "historyrecords",
                columns: table => new
                {
                    eventid = table.Column<short>(type: "smallint", nullable: false),
                    eventidname = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tempendofshiftreportforall",
                schema: "shiftreports",
                columns: table => new
                {
                    temshiftreportforall_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    shiftid = table.Column<short>(type: "smallint", nullable: false),
                    machineid = table.Column<Guid>(type: "uuid", nullable: false),
                    @operator = table.Column<Guid>(name: "operator", type: "uuid", nullable: false),
                    productcode = table.Column<string>(type: "citext", nullable: true),
                    externalid = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    productstatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "ix_assign_created",
                schema: "historyrecords",
                table: "assign_task",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_assign_machine_created",
                schema: "historyrecords",
                table: "assign_task",
                columns: new[] { "machineid", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_assign_shift",
                schema: "historyrecords",
                table: "assign_task",
                column: "shiftid");

            migrationBuilder.CreateIndex(
                name: "ix_eosrdf_externalid",
                schema: "shiftreports",
                table: "endofshiftreportdetailforall",
                column: "externalid");

            migrationBuilder.CreateIndex(
                name: "ix_eosrf_created_at",
                schema: "shiftreports",
                table: "endofshiftreportforall",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_eosrf_machine_shift",
                schema: "shiftreports",
                table: "endofshiftreportforall",
                columns: new[] { "machineid", "shiftid" });

            migrationBuilder.CreateIndex(
                name: "ix_eosrf_productcode",
                schema: "shiftreports",
                table: "endofshiftreportforall",
                column: "productcode");

            migrationBuilder.CreateIndex(
                name: "ix_eventhistory_eventdate",
                schema: "historyrecords",
                table: "EventHistory",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_eventhistory_machine_eventdate",
                schema: "historyrecords",
                table: "EventHistory",
                columns: new[] { "machine_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_hpla_created_at",
                schema: "printect",
                table: "history_print_label_for_all",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_hpla_externalid_created_at",
                schema: "printect",
                table: "history_print_label_for_all",
                columns: new[] { "externalid", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_hpla_lognumber_created_at",
                schema: "printect",
                table: "history_print_label_for_all",
                columns: new[] { "lognumber", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_labelelements_template_labeltype",
                schema: "printcell",
                table: "labelelements",
                columns: new[] { "templateid", "labeltype" });

            migrationBuilder.CreateIndex(
                name: "ix_labelelements_templateid",
                schema: "printcell",
                table: "labelelements",
                column: "templateid");

            migrationBuilder.CreateIndex(
                name: "ux_labeltemplates_size_type",
                schema: "printcell",
                table: "labeltemplates",
                columns: new[] { "widthmm", "heightmm", "labeltype" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_machinehistory_created_at",
                schema: "historyrecords",
                table: "machine_history",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_op_history_created_at",
                schema: "shiftreports",
                table: "OperationHistory_MD",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_op_history_prod_created_at",
                schema: "shiftreports",
                table: "OperationHistory_MD",
                columns: new[] { "productcode", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_plan_getback_history_colorcode_created_at",
                schema: "historyrecords",
                table: "plan_getback_history",
                columns: new[] { "colorcode", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_plan_getback_history_created_at",
                schema: "historyrecords",
                table: "plan_getback_history",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_plan_getback_history_externalid_created_at",
                schema: "historyrecords",
                table: "plan_getback_history",
                columns: new[] { "externalid", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_shifts_name",
                schema: "historyrecords",
                table: "shifts",
                column: "shiftname");

            migrationBuilder.CreateIndex(
                name: "ix_shiftsevent_name",
                schema: "historyrecords",
                table: "shiftsevent",
                column: "eventidname");

            migrationBuilder.CreateIndex(
                name: "ix_temp_eosrf_created_at",
                schema: "shiftreports",
                table: "tempendofshiftreportforall",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_temp_eosrf_machine_shift",
                schema: "shiftreports",
                table: "tempendofshiftreportforall",
                columns: new[] { "machineid", "shiftid" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assign_task",
                schema: "historyrecords");

            migrationBuilder.DropTable(
                name: "endofshiftreportdetailforall",
                schema: "shiftreports");

            migrationBuilder.DropTable(
                name: "endofshiftreportforall",
                schema: "shiftreports");

            migrationBuilder.DropTable(
                name: "EventHistory",
                schema: "historyrecords");

            migrationBuilder.DropTable(
                name: "history_print_label_for_all",
                schema: "printect");

            migrationBuilder.DropTable(
                name: "labelelements",
                schema: "printcell");

            migrationBuilder.DropTable(
                name: "labeltemplates",
                schema: "printcell");

            migrationBuilder.DropTable(
                name: "machine_history",
                schema: "historyrecords");

            migrationBuilder.DropTable(
                name: "OperationHistory_MD",
                schema: "shiftreports");

            migrationBuilder.DropTable(
                name: "plan_getback_history",
                schema: "historyrecords");

            migrationBuilder.DropTable(
                name: "shifts",
                schema: "historyrecords");

            migrationBuilder.DropTable(
                name: "shiftsevent",
                schema: "historyrecords");

            migrationBuilder.DropTable(
                name: "tempendofshiftreportforall",
                schema: "shiftreports");
        }
    }
}
