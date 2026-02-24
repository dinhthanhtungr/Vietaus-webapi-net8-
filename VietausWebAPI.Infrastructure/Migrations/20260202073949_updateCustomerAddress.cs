using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationAddress1",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNumber",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationNumber",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.AddColumn<string>(
                name: "RegistrationAddress1",
                schema: "Customer",
                table: "Customer",
                type: "text",
                nullable: true);
        }
    }
}
