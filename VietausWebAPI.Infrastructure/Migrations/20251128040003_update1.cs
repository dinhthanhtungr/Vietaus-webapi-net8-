using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CreatedBy",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_UpdatedBy",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Parts",
                schema: "hr",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CreatedBy",
                schema: "company",
                table: "Companies",
                column: "createdBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_UpdatedBy",
                schema: "company",
                table: "Companies",
                column: "updatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies",
                schema: "hr",
                table: "Employees",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "companyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Parts",
                schema: "hr",
                table: "Employees",
                column: "PartID",
                principalSchema: "hr",
                principalTable: "Parts",
                principalColumn: "PartID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CreatedBy",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_UpdatedBy",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Companies",
                schema: "hr",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Parts",
                schema: "hr",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CreatedBy",
                schema: "company",
                table: "Companies",
                column: "createdBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_UpdatedBy",
                schema: "company",
                table: "Companies",
                column: "updatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Companies",
                schema: "hr",
                table: "Employees",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "companyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Parts",
                schema: "hr",
                table: "Employees",
                column: "PartID",
                principalSchema: "hr",
                principalTable: "Parts",
                principalColumn: "PartID");
        }
    }
}
