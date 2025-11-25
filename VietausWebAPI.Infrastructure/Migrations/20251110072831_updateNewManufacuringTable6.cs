using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_transferheaders_posted_by",
                schema: "mro",
                table: "transferheaders",
                column: "posted_by");

            migrationBuilder.AddForeignKey(
                name: "fk_transferheaders_posted_by",
                schema: "mro",
                table: "transferheaders",
                column: "posted_by",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transferheaders_posted_by",
                schema: "mro",
                table: "transferheaders");

            migrationBuilder.DropIndex(
                name: "IX_transferheaders_posted_by",
                schema: "mro",
                table: "transferheaders");
        }
    }
}
