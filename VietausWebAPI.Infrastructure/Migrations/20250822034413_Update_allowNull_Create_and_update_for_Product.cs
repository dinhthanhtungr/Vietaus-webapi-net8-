using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_allowNull_Create_and_update_for_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CreatedBy",
                schema: "labs",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UpdatedBy",
                schema: "labs",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CreatedBy",
                schema: "labs",
                table: "Products",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UpdatedBy",
                schema: "labs",
                table: "Products",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CreatedBy",
                schema: "labs",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UpdatedBy",
                schema: "labs",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CreatedBy",
                schema: "labs",
                table: "Products",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UpdatedBy",
                schema: "labs",
                table: "Products",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
