using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatecustomerclaim1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerClaims_Customer",
                schema: "Customer",
                table: "CustomerClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerClaims_Customer",
                schema: "Customer",
                table: "CustomerClaims",
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
                name: "FK_CustomerClaims_Customer",
                schema: "Customer",
                table: "CustomerClaims");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerClaims_Customer",
                schema: "Customer",
                table: "CustomerClaims",
                column: "customerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
