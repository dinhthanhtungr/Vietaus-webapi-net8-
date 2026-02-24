using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomer22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                schema: "Customer",
                table: "Customer",
                newName: "RegistrationAddress");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationAddress1",
                schema: "Customer",
                table: "Customer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationAddress1",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "RegistrationAddress",
                schema: "Customer",
                table: "Customer",
                newName: "RegistrationNumber");
        }
    }
}
