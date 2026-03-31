using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateOperationHistoryMd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "ix_op_history_externalid",
                schema: "shiftreports",
                table: "OperationHistory_MD",
                column: "externalid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_op_history_externalid",
                schema: "shiftreports",
                table: "OperationHistory_MD");
        }
    }
}
