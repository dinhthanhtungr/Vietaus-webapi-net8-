using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateQCInput2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQC_WarehouseRequestDetail",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "ix_qcinputbyqc_warehouserequestdetailid",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestDetailId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.AddColumn<int>(
                name: "WarehouseRequestDetailDetailId",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "voucherdetailid",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbyqc_voucherdetailid",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "voucherdetailid");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQC_WarehouseRequestDetailDetailId",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "WarehouseRequestDetailDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQC_WarehouseRequestDetail_WarehouseRequestDetailDe~",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "WarehouseRequestDetailDetailId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequestDetail",
                principalColumn: "detailId");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQC_WarehouseVoucherDetail",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "voucherdetailid",
                principalSchema: "Warehouse",
                principalTable: "WarehouseVoucherDetails",
                principalColumn: "voucherDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQC_WarehouseRequestDetail_WarehouseRequestDetailDe~",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQC_WarehouseVoucherDetail",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "ix_qcinputbyqc_voucherdetailid",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "IX_QCInputByQC_WarehouseRequestDetailDetailId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestDetailDetailId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "voucherdetailid",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.AddColumn<int>(
                name: "WarehouseRequestDetailId",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbyqc_warehouserequestdetailid",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "WarehouseRequestDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQC_WarehouseRequestDetail",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "WarehouseRequestDetailId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequestDetail",
                principalColumn: "detailId");
        }
    }
}
