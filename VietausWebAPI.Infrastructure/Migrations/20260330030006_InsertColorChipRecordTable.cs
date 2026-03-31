using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InsertColorChipRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "color_chip_records",
                schema: "SampleRequests",
                columns: table => new
                {
                    color_chip_record_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    record_type = table.Column<int>(type: "integer", nullable: false),
                    chip_purpose = table.Column<int>(type: "integer", nullable: false),
                    ResinType = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_code_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    product_name_snapshot = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    color_name_snapshot = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    manufacturing_formula_id = table.Column<Guid>(type: "uuid", nullable: true),
                    manufacturing_formula_external_id_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    development_formula_id = table.Column<Guid>(type: "uuid", nullable: true),
                    development_formula_external_id_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    attachment_collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    record_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_external_id_snapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    customer_name_snapshot = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    add_rate = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    resin = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    temperature_min = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    temperature_max = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    size_text = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    pellet_weight_gram = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    anti_static_info = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    note = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    print_note = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CustomerId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_color_chip_records", x => x.color_chip_record_id);
                    table.ForeignKey(
                        name: "FK_color_chip_records_AttachmentCollection_attachment_collecti~",
                        column: x => x.attachment_collection_id,
                        principalSchema: "Attachment",
                        principalTable: "AttachmentCollection",
                        principalColumn: "AttachmentCollectionID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_color_chip_records_Customer_CustomerId1",
                        column: x => x.CustomerId1,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_color_chip_records_Customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_color_chip_records_Formulas_development_formula_id",
                        column: x => x.development_formula_id,
                        principalSchema: "SampleRequests",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_color_chip_records_Products_product_id",
                        column: x => x.product_id,
                        principalSchema: "SampleRequests",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_color_chip_records_manufacturing_formulas_manufacturing_for~",
                        column: x => x.manufacturing_formula_id,
                        principalSchema: "manufacturing",
                        principalTable: "manufacturing_formulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_attachment_collection_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "attachment_collection_id");

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_chip_purpose_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "chip_purpose", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_customer_external_id_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "customer_external_id_snapshot", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_customer_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "customer_id", "is_active" });

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
                name: "ix_color_chip_records_product_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "product_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "ix_color_chip_records_record_type_product_code_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "record_type", "product_code_snapshot", "is_active" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "color_chip_records",
                schema: "SampleRequests");
        }
    }
}
