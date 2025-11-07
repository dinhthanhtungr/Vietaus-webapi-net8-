using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_GroupId",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Group",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "GroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Group",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAssignment_GroupId",
                schema: "Customer",
                table: "CustomerAssignment");
        }
    }
}
