using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateqcinput3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "attachmentlasterror",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "attachmentstatus",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "integer",
                nullable: false,
                defaultValueSql: "0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachmentlasterror",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "attachmentstatus",
                schema: "devandqa",
                table: "QCInputByQC");
        }
    }
}
