using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Customer",
                schema: "Customer",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customer",
                schema: "Customer",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCustomerTransfer_Customer",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Customer",
                schema: "Customer",
                table: "Address",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Customer",
                schema: "Customer",
                table: "Contacts",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCustomerTransfer_Customer",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Customer",
                schema: "Customer",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Customer",
                schema: "Customer",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCustomerTransfer_Customer",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Customer",
                schema: "Customer",
                table: "Address",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Customer",
                schema: "Customer",
                table: "Contacts",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCustomerTransfer_Customer",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
