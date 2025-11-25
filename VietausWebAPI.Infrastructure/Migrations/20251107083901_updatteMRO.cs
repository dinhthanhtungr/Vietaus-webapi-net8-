using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatteMRO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mf__VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK__Mf__mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK__Mpo__merchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulaLog",
                schema: "manufacturing");

            migrationBuilder.DropIndex(
                name: "IX_MfgProductionOrders_MerchandiseOrderExternalId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropIndex(
                name: "ux_mfg_formula_isselect_one_per_order",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "ux_mfg_formula_isstandard_one_per_vu",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "merchandiseOrderExternalId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "qualifiedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "rejectedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "wasteQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "FormulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "SourceType",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "isSelect",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "isStandard",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "mfgProductionOrderExternalId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "lotNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropColumn(
                name: "stockId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.RenameColumn(
                name: "requiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "RequiredDate");

            migrationBuilder.RenameColumn(
                name: "mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "MfgProductionOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_MfgProductionOrderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "updatedBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "totalQuantityRequest",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "externalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "createdBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "companyId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bagType",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedDate",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "updatedBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdDate",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "companyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "improvement_hdr",
                schema: "mro",
                columns: table => new
                {
                    proposal_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    proposal_external = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    area_externalid = table.Column<string>(type: "text", nullable: true),
                    equipment_externalid = table.Column<string>(type: "text", nullable: true),
                    factory_externalid = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Draft"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    approved_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    approved_by = table.Column<Guid>(type: "uuid", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    started_by = table.Column<Guid>(type: "uuid", nullable: true),
                    done_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    done_by = table.Column<Guid>(type: "uuid", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    closed_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_improvement_hdr", x => x.proposal_id);
                    table.ForeignKey(
                        name: "fk_improvement_hdr_approved_by",
                        column: x => x.approved_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_improvement_hdr_closed_by",
                        column: x => x.closed_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_improvement_hdr_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_improvement_hdr_done_by",
                        column: x => x.done_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_improvement_hdr_started_by",
                        column: x => x.started_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "improvementhistory",
                schema: "mro",
                columns: table => new
                {
                    history_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    proposal_id = table.Column<long>(type: "bigint", nullable: false),
                    proposal_external = table.Column<string>(type: "text", nullable: false),
                    action = table.Column<string>(type: "text", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    minutes = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    wo_ref = table.Column<string>(type: "text", nullable: true),
                    performed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    performed_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_improvementhistory", x => x.history_id);
                    table.ForeignKey(
                        name: "fk_imprhist_performed_by",
                        column: x => x.performed_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movements",
                schema: "mro",
                columns: table => new
                {
                    movement_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fromslot_id = table.Column<int>(type: "integer", nullable: true),
                    toslot_id = table.Column<int>(type: "integer", nullable: true),
                    qty = table.Column<int>(type: "integer", nullable: false),
                    moved_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    note = table.Column<string>(type: "text", nullable: true),
                    requestexternal = table.Column<string>(type: "text", nullable: true),
                    move_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movements", x => x.movement_id);
                    table.ForeignKey(
                        name: "fk_MovementMRO_SlotMROFrom",
                        column: x => x.fromslot_id,
                        principalSchema: "mro",
                        principalTable: "slots",
                        principalColumn: "slot_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_MovementMRO_SlotMROTo",
                        column: x => x.toslot_id,
                        principalSchema: "mro",
                        principalTable: "slots",
                        principalColumn: "slot_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_MovementMRO_material",
                        column: x => x.material_id,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pmplan",
                schema: "mro",
                columns: table => new
                {
                    plan_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    plan_externalid = table.Column<string>(type: "text", nullable: false),
                    equipment_externalid = table.Column<string>(type: "text", nullable: true),
                    department_externalid = table.Column<string>(type: "text", nullable: true),
                    factory_externalid = table.Column<string>(type: "text", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    interval_val = table.Column<int>(type: "integer", nullable: false),
                    interval_unit = table.Column<string>(type: "text", nullable: false),
                    anchor_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Draft"),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pmplan", x => x.plan_id);
                });

            migrationBuilder.CreateTable(
                name: "stock_out_hdr",
                schema: "mro",
                columns: table => new
                {
                    stock_out_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stock_out_code = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Draft"),
                    reason = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    factory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    factory_code = table.Column<string>(type: "text", nullable: true),
                    source_type = table.Column<string>(type: "text", nullable: true),
                    source_id = table.Column<long>(type: "bigint", nullable: true),
                    source_code = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    posted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    posted_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_out_hdr", x => x.stock_out_id);
                    table.ForeignKey(
                        name: "fk_stock_out_hdr_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_stock_out_hdr_factory",
                        column: x => x.factory_id,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_stock_out_hdr_incident ",
                        column: x => x.source_id,
                        principalSchema: "mro",
                        principalTable: "incident_hdr",
                        principalColumn: "incident_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_stock_out_hdr_posted_by",
                        column: x => x.posted_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transferheaders",
                schema: "mro",
                columns: table => new
                {
                    transferheaders_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transferheaders_externalid = table.Column<string>(type: "text", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    posted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    posted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    posted_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transferheaders", x => x.transferheaders_id);
                    table.ForeignKey(
                        name: "fk_transferheaders_created_by",
                        column: x => x.created_by,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pmplanhistory",
                schema: "mro",
                columns: table => new
                {
                    hist_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    plan_id = table.Column<long>(type: "bigint", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    details = table.Column<string>(type: "jsonb", nullable: true),
                    performed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    performed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    wo_ref = table.Column<string>(type: "text", nullable: true),
                    plan_externalid = table.Column<string>(type: "text", nullable: false),
                    minutesspent = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pmplanhistory", x => x.hist_id);
                    table.ForeignKey(
                        name: "fk_pmplanhist_plan",
                        column: x => x.plan_id,
                        principalSchema: "mro",
                        principalTable: "pmplan",
                        principalColumn: "plan_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stock_out_line",
                schema: "mro",
                columns: table => new
                {
                    line_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    stock_out_id = table.Column<long>(type: "bigint", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_externalid = table.Column<string>(type: "text", nullable: true),
                    qty = table.Column<int>(type: "integer", nullable: false),
                    uom = table.Column<string>(type: "text", nullable: false),
                    slot_code = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stock_out_line", x => x.line_id);
                    table.ForeignKey(
                        name: "fk_stock_out_line_material",
                        column: x => x.material_id,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_stock_out_line_stock_out",
                        column: x => x.stock_out_id,
                        principalSchema: "mro",
                        principalTable: "stock_out_hdr",
                        principalColumn: "stock_out_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transferdetails",
                schema: "mro",
                columns: table => new
                {
                    transferdetail_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transferheaders_id = table.Column<long>(type: "bigint", nullable: false),
                    material_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fromslot_id = table.Column<int>(type: "integer", nullable: true),
                    toslot_id = table.Column<int>(type: "integer", nullable: true),
                    qty = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transferdetails", x => x.transferdetail_id);
                    table.ForeignKey(
                        name: "fk_transferdetails_header",
                        column: x => x.transferheaders_id,
                        principalSchema: "mro",
                        principalTable: "transferheaders",
                        principalColumn: "transferheaders_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_improvement_hdr_approved_by",
                schema: "mro",
                table: "improvement_hdr",
                column: "approved_by");

            migrationBuilder.CreateIndex(
                name: "ix_improvement_hdr_area_externalid",
                schema: "mro",
                table: "improvement_hdr",
                column: "area_externalid");

            migrationBuilder.CreateIndex(
                name: "IX_improvement_hdr_closed_by",
                schema: "mro",
                table: "improvement_hdr",
                column: "closed_by");

            migrationBuilder.CreateIndex(
                name: "IX_improvement_hdr_created_by",
                schema: "mro",
                table: "improvement_hdr",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_improvement_hdr_done_by",
                schema: "mro",
                table: "improvement_hdr",
                column: "done_by");

            migrationBuilder.CreateIndex(
                name: "ix_improvement_hdr_equipment_externalid",
                schema: "mro",
                table: "improvement_hdr",
                column: "equipment_externalid");

            migrationBuilder.CreateIndex(
                name: "ix_improvement_hdr_factory_status_created_desc",
                schema: "mro",
                table: "improvement_hdr",
                columns: new[] { "factory_externalid", "status", "created_at", "proposal_id" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_improvement_hdr_started_by",
                schema: "mro",
                table: "improvement_hdr",
                column: "started_by");

            migrationBuilder.CreateIndex(
                name: "ux_improvement_hdr_factory_proposal_external",
                schema: "mro",
                table: "improvement_hdr",
                columns: new[] { "factory_externalid", "proposal_external" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_imprhist_performed_by",
                schema: "mro",
                table: "improvementhistory",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "ix_imprhist_proposal_performed_desc",
                schema: "mro",
                table: "improvementhistory",
                columns: new[] { "proposal_id", "performed_at", "history_id" },
                descending: new[] { false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_movements_fromslot",
                schema: "mro",
                table: "movements",
                column: "fromslot_id");

            migrationBuilder.CreateIndex(
                name: "ix_movements_material_moved_desc",
                schema: "mro",
                table: "movements",
                columns: new[] { "material_id", "moved_at", "movement_id" },
                descending: new[] { false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_movements_toslot",
                schema: "mro",
                table: "movements",
                column: "toslot_id");

            migrationBuilder.CreateIndex(
                name: "ix_pmplan_equipment_externalid",
                schema: "mro",
                table: "pmplan",
                column: "equipment_externalid");

            migrationBuilder.CreateIndex(
                name: "ix_pmplan_factory_status_created_desc",
                schema: "mro",
                table: "pmplan",
                columns: new[] { "factory_externalid", "status", "created_at", "plan_id" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "ux_pmplan_factory_plan_external",
                schema: "mro",
                table: "pmplan",
                columns: new[] { "factory_externalid", "plan_externalid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pmplanhist_performed_by",
                schema: "mro",
                table: "pmplanhistory",
                column: "performed_by");

            migrationBuilder.CreateIndex(
                name: "ix_pmplanhist_plan_performed_desc",
                schema: "mro",
                table: "pmplanhistory",
                columns: new[] { "plan_id", "performed_at", "hist_id" },
                descending: new[] { false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_stock_out_hdr_created_by",
                schema: "mro",
                table: "stock_out_hdr",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_stock_out_hdr_factory_status_created_desc",
                schema: "mro",
                table: "stock_out_hdr",
                columns: new[] { "factory_id", "status", "CreatedAt", "stock_out_id" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_stock_out_hdr_posted_by",
                schema: "mro",
                table: "stock_out_hdr",
                column: "posted_by");

            migrationBuilder.CreateIndex(
                name: "IX_stock_out_hdr_source_id",
                schema: "mro",
                table: "stock_out_hdr",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "ux_stock_out_code_per_factory",
                schema: "mro",
                table: "stock_out_hdr",
                columns: new[] { "factory_id", "stock_out_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_stock_out_line_material_id",
                schema: "mro",
                table: "stock_out_line",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_stock_out_line_stock_out_id",
                schema: "mro",
                table: "stock_out_line",
                column: "stock_out_id");

            migrationBuilder.CreateIndex(
                name: "ix_transferdetails_fromslot",
                schema: "mro",
                table: "transferdetails",
                column: "fromslot_id");

            migrationBuilder.CreateIndex(
                name: "ix_transferdetails_header",
                schema: "mro",
                table: "transferdetails",
                column: "transferheaders_id");

            migrationBuilder.CreateIndex(
                name: "ix_transferdetails_material",
                schema: "mro",
                table: "transferdetails",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "ix_transferdetails_toslot",
                schema: "mro",
                table: "transferdetails",
                column: "toslot_id");

            migrationBuilder.CreateIndex(
                name: "ix_transferheaders_created_by",
                schema: "mro",
                table: "transferheaders",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_transferheaders_posted_created_desc",
                schema: "mro",
                table: "transferheaders",
                columns: new[] { "posted", "created_at", "transferheaders_id" },
                descending: new[] { false, true, true });

            migrationBuilder.CreateIndex(
                name: "ux_transferheaders_externalid",
                schema: "mro",
                table: "transferheaders",
                column: "transferheaders_externalid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_MfgProductionOrders_MfgProductionOrde~",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "MfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MfgProductionOrders_MerchandiseOrders_MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Formulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_MfgProductionOrders_MfgProductionOrde~",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK_MfgProductionOrders_MerchandiseOrders_MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropTable(
                name: "improvement_hdr",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "improvementhistory",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "movements",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "pmplanhistory",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "stock_out_line",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "transferdetails",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "pmplan",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "stock_out_hdr",
                schema: "mro");

            migrationBuilder.DropTable(
                name: "transferheaders",
                schema: "mro");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.RenameColumn(
                name: "RequiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "requiredDate");

            migrationBuilder.RenameColumn(
                name: "MfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "mfgProductionOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_MfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_mfgProductionOrderId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "updatedBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "totalQuantityRequest",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "externalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<Guid>(
                name: "createdBy",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "companyId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "bagType",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "requiredDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "merchandiseOrderExternalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "qualifiedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "rejectedQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "wasteQuantity",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedDate",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "updatedBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdDate",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "companyId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "isSelect",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isStandard",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "mfgProductionOrderExternalId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lotNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "stockId",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaLog",
                schema: "manufacturing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ManufacturingFormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    PerformedByNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    PerformedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaLog__Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Mf__ManufacturingFormulaLogs",
                        column: x => x.ManufacturingFormulaId,
                        principalSchema: "manufacturing",
                        principalTable: "ManufacturingFormulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mf__performedBy",
                        column: x => x.PerformedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_MerchandiseOrderExternalId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "merchandiseOrderExternalId");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_isselect_one_per_order",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "mfgProductionOrderId",
                unique: true,
                filter: "\"isSelect\" = TRUE AND \"isActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "ux_mfg_formula_isstandard_one_per_vu",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "VUFormulaId",
                unique: true,
                filter: "\"isStandard\" = TRUE AND \"isActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaLog_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaLog",
                column: "ManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaLog_PerformedBy",
                schema: "manufacturing",
                table: "ManufacturingFormulaLog",
                column: "PerformedBy");

            migrationBuilder.AddForeignKey(
                name: "FK__Mf__VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "VUFormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Mf__mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "mfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Mpo__merchandiseOrderId",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
