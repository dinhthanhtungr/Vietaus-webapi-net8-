using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "qc_pass_history",
                schema: "devandga",
                newName: "qc_pass_history",
                newSchema: "devandqa");

            migrationBuilder.RenameTable(
                name: "qc_pass_detail_history",
                schema: "devandga",
                newName: "qc_pass_detail_history",
                newSchema: "devandqa");

            migrationBuilder.RenameTable(
                name: "ProductTest",
                schema: "devandga",
                newName: "ProductTest",
                newSchema: "devandqa");

            migrationBuilder.RenameTable(
                name: "ProductStandard",
                schema: "devandga",
                newName: "ProductStandard",
                newSchema: "devandqa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "devandga");

            migrationBuilder.RenameTable(
                name: "qc_pass_history",
                schema: "devandqa",
                newName: "qc_pass_history",
                newSchema: "devandga");

            migrationBuilder.RenameTable(
                name: "qc_pass_detail_history",
                schema: "devandqa",
                newName: "qc_pass_detail_history",
                newSchema: "devandga");

            migrationBuilder.RenameTable(
                name: "ProductTest",
                schema: "devandqa",
                newName: "ProductTest",
                newSchema: "devandga");

            migrationBuilder.RenameTable(
                name: "ProductStandard",
                schema: "devandqa",
                newName: "ProductStandard",
                newSchema: "devandga");
        }
    }
}
