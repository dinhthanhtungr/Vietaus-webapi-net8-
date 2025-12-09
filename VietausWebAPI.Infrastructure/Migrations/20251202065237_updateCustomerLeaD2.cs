using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomerLeaD2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "CustomerNotes",
                schema: "crm",
                newName: "CustomerNotes",
                newSchema: "Customer");

            migrationBuilder.RenameTable(
                name: "CustomerClaims",
                schema: "crm",
                newName: "CustomerClaims",
                newSchema: "Customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "crm");

            migrationBuilder.RenameTable(
                name: "CustomerNotes",
                schema: "Customer",
                newName: "CustomerNotes",
                newSchema: "crm");

            migrationBuilder.RenameTable(
                name: "CustomerClaims",
                schema: "Customer",
                newName: "CustomerClaims",
                newSchema: "crm");
        }
    }
}
