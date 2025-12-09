using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatecustomerclaim2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_Customer",
                schema: "Customer",
                table: "CustomerNotes");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_Customer",
                schema: "Customer",
                table: "CustomerNotes",
                column: "customerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerNotes_Customer",
                schema: "Customer",
                table: "CustomerNotes");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerNotes_Customer",
                schema: "Customer",
                table: "CustomerNotes",
                column: "customerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
