using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSupplier1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierAddress_Supplier",
                schema: "Material",
                table: "SupplierAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierContact_Supplier",
                schema: "Material",
                table: "SupplierContacts");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierAddress_Supplier",
                schema: "Material",
                table: "SupplierAddresses",
                column: "SupplierId",
                principalSchema: "Material",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierContact_Supplier",
                schema: "Material",
                table: "SupplierContacts",
                column: "SupplierId",
                principalSchema: "Material",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierAddress_Supplier",
                schema: "Material",
                table: "SupplierAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierContact_Supplier",
                schema: "Material",
                table: "SupplierContacts");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierAddress_Supplier",
                schema: "Material",
                table: "SupplierAddresses",
                column: "SupplierId",
                principalSchema: "Material",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierContact_Supplier",
                schema: "Material",
                table: "SupplierContacts",
                column: "SupplierId",
                principalSchema: "Material",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");
        }
    }
}
