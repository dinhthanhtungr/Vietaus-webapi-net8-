using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
