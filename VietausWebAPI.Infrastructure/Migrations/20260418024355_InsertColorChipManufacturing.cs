using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertColorChipManufacturing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "color_chip_manufacturing_records",
                schema: "manufacturing",
                columns: table => new
                {
                    color_chip_mfg_record_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    resin_type = table.Column<int>(type: "integer", nullable: false),
                    logo_type = table.Column<int>(type: "integer", nullable: false),
                    form_style = table.Column<int>(type: "integer", nullable: false),
                    mfg_production_order_id = table.Column<Guid>(type: "uuid", nullable: true),
                    manufacturing_formula_id = table.Column<Guid>(type: "uuid", nullable: true),
                    machine = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    resin = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    temperature_limit = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    size_text = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    pellet_weight_gram = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    net_weight_gram = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    electrostatic = table.Column<bool>(type: "boolean", nullable: true),
                    record_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    print_note = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_color_chip_manufacturing_records", x => x.color_chip_mfg_record_id);
                    table.ForeignKey(
                        name: "FK_color_chip_manufacturing_records_MfgProductionOrders_mfg_pr~",
                        column: x => x.mfg_production_order_id,
                        principalSchema: "manufacturing",
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_color_chip_manufacturing_records_manufacturing_formulas_man~",
                        column: x => x.manufacturing_formula_id,
                        principalSchema: "manufacturing",
                        principalTable: "manufacturing_formulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_manufacturing_records_manufacturing_formula_id",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records",
                column: "manufacturing_formula_id");

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_manufacturing_records_mfg_production_order_id",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records",
                column: "mfg_production_order_id");

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_color_chip_mfg_record_id_is_active",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records",
                columns: new[] { "color_chip_mfg_record_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_record_date_is_active",
                schema: "manufacturing",
                table: "color_chip_manufacturing_records",
                columns: new[] { "record_date", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "color_chip_manufacturing_records",
                schema: "manufacturing");
        }
    }
}
