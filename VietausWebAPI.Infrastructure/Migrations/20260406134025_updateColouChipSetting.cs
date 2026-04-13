using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateColouChipSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_Customer_CustomerId1",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_Customer_customer_id",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_Formulas_development_formula_id",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_manufacturing_formulas_manufacturing_for~",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_CustomerId1",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_dev_formula_external_id_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_development_formula_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_manufacturing_formula_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_mfg_formula_external_id_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_product_id",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_record_type_product_code_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "add_rate",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "color_name_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "customer_external_id_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "customer_name_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "development_formula_external_id_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "manufacturing_formula_external_id_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "product_name_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "temperature_max",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "temperature_min",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.RenameColumn(
                name: "manufacturing_formula_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "ManufacturingFormulaId");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "product_code_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "net_weight_gram");

            migrationBuilder.RenameColumn(
                name: "development_formula_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "FormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_color_chip_records_customer_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "IX_color_chip_records_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "machine",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "temperature_limit",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "color_chip_record_development_formulas",
                schema: "SampleRequests",
                columns: table => new
                {
                    color_chip_record_development_formula_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    color_chip_record_id = table.Column<Guid>(type: "uuid", nullable: false),
                    development_formula_id = table.Column<Guid>(type: "uuid", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_color_chip_record_development_formulas", x => x.color_chip_record_development_formula_id);
                    table.ForeignKey(
                        name: "FK_color_chip_record_development_formulas_Formulas_development~",
                        column: x => x.development_formula_id,
                        principalSchema: "SampleRequests",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_color_chip_record_development_formulas_color_chip_records_c~",
                        column: x => x.color_chip_record_id,
                        principalSchema: "SampleRequests",
                        principalTable: "color_chip_records",
                        principalColumn: "color_chip_record_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_company_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "company_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "ManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_product_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "product_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_record_date_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "record_date", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_record_type_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "record_type", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_ccrdf_color_chip_record_id",
                schema: "SampleRequests",
                table: "color_chip_record_development_formulas",
                column: "color_chip_record_id");

            migrationBuilder.CreateIndex(
                name: "ix_ccrdf_development_formula_id",
                schema: "SampleRequests",
                table: "color_chip_record_development_formulas",
                column: "development_formula_id");

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_Customer_CustomerId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_Formulas_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_manufacturing_formulas_ManufacturingForm~",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "ManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_Customer_CustomerId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_Formulas_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropForeignKey(
                name: "FK_color_chip_records_manufacturing_formulas_ManufacturingForm~",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropTable(
                name: "color_chip_record_development_formulas",
                schema: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_company_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_product_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_record_date_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_record_type_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "machine",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "temperature_limit",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.RenameColumn(
                name: "ManufacturingFormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "manufacturing_formula_id");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "net_weight_gram",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "product_code_snapshot");

            migrationBuilder.RenameColumn(
                name: "FormulaId",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "development_formula_id");

            migrationBuilder.RenameIndex(
                name: "IX_color_chip_records_CustomerId",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "IX_color_chip_records_customer_id");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId1",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "add_rate",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "color_name_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "customer_external_id_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "customer_name_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "development_formula_external_id_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "manufacturing_formula_external_id_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "product_name_snapshot",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "temperature_max",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "temperature_min",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_CustomerId1",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_dev_formula_external_id_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "development_formula_external_id_snapshot", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_development_formula_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "development_formula_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_manufacturing_formula_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "manufacturing_formula_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_mfg_formula_external_id_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "manufacturing_formula_external_id_snapshot", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_product_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_record_type_product_code_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "record_type", "product_code_snapshot", "is_active" });

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_Customer_CustomerId1",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "CustomerId1",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_Customer_customer_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "customer_id",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_Formulas_development_formula_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "development_formula_id",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_color_chip_records_manufacturing_formulas_manufacturing_for~",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "manufacturing_formula_id",
                principalSchema: "manufacturing",
                principalTable: "manufacturing_formulas",
                principalColumn: "manufacturingFormulaId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
