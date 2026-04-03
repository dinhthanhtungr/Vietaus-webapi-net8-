using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColourChipRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_chip_purpose_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_customer_external_id_snapshot_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_customer_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "ix_color_chip_records_product_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.RenameColumn(
                name: "ResinType",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "resin_type");

            migrationBuilder.RenameColumn(
                name: "chip_purpose",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "logo_type");

            migrationBuilder.AddColumn<int>(
                name: "form_style",
                schema: "SampleRequests",
                table: "color_chip_records",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_customer_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_color_chip_records_product_id",
                schema: "SampleRequests",
                table: "color_chip_records",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_customer_id",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropIndex(
                name: "IX_color_chip_records_product_id",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.DropColumn(
                name: "form_style",
                schema: "SampleRequests",
                table: "color_chip_records");

            migrationBuilder.RenameColumn(
                name: "resin_type",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "ResinType");

            migrationBuilder.RenameColumn(
                name: "logo_type",
                schema: "SampleRequests",
                table: "color_chip_records",
                newName: "chip_purpose");

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
                name: "ix_color_chip_records_product_id_is_active",
                schema: "SampleRequests",
                table: "color_chip_records",
                columns: new[] { "product_id", "is_active" });
        }
    }
}
