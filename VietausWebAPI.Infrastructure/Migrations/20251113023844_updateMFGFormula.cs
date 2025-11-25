using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMFGFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaVersions",
                schema: "manufacturing",
                columns: table => new
                {
                    manufacturingFormulaVersionId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ManufacturingFormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    versionNo = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "citext", nullable: false, defaultValue: "Draft"),
                    effectiveFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    effectiveTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaVersions__manufacturingFormulaVersionId", x => x.manufacturingFormulaVersionId);
                    table.ForeignKey(
                        name: "fk_mf_versions_formula",
                        column: x => x.ManufacturingFormulaId,
                        principalSchema: "manufacturing",
                        principalTable: "manufacturing_formulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaVersionItems",
                schema: "manufacturing",
                columns: table => new
                {
                    manufacturingFormulaVersionItemId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    manufacturingFormulaVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: false),
                    unitPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    totalPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                    materialNameSnapshot = table.Column<string>(type: "text", nullable: false),
                    materialExternalIdSnapshot = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mf_version_items", x => x.manufacturingFormulaVersionItemId);
                    table.ForeignKey(
                        name: "fk_mf_version_items_version",
                        column: x => x.manufacturingFormulaVersionId,
                        principalSchema: "manufacturing",
                        principalTable: "ManufacturingFormulaVersions",
                        principalColumn: "manufacturingFormulaVersionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_production_select_versions_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "ManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_product_standard_formulas_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "ManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "ix_mf_version_items_version",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "manufacturingFormulaVersionId");

            migrationBuilder.CreateIndex(
                name: "ux_mf_version_items_version_material",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                columns: new[] { "manufacturingFormulaVersionId", "materialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_mf_versions_period",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersions",
                columns: new[] { "ManufacturingFormulaId", "effectiveFrom", "effectiveTo" });

            migrationBuilder.CreateIndex(
                name: "ix_mf_versions_status",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ux_mf_versions_formula_versionno",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersions",
                columns: new[] { "ManufacturingFormulaId", "versionNo" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_product_standard_formulas_manufacturing_formulas_Manufactur~",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "ManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId");

            migrationBuilder.AddForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_production_select_versions_manufacturing_formulas_Manufactu~",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "ManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId");

            migrationBuilder.AddForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_standard_formulas_manufacturing_formulas_Manufactur~",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_production_select_versions_manufacturing_formulas_Manufactu~",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulaVersionItems",
                schema: "manufacturing");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulaVersions",
                schema: "manufacturing");

            migrationBuilder.DropIndex(
                name: "IX_production_select_versions_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropIndex(
                name: "IX_product_standard_formulas_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropColumn(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropColumn(
                name: "ManufacturingFormulaId",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.AddForeignKey(
                name: "fk_psf_manufacturing_formula",
                schema: "manufacturing",
                table: "product_standard_formulas",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_psv_manufacturing_formula",
                schema: "manufacturing",
                table: "production_select_versions",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
