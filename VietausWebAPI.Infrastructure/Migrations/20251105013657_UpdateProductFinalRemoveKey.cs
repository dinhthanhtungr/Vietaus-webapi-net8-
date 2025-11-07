using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductFinalRemoveKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Products_Company_Code",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UX_Products_Company_ColourCode",
                schema: "SampleRequests",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_Products_Company_Code",
                schema: "SampleRequests",
                table: "Products",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Products_Company_ColourCode",
                schema: "SampleRequests",
                table: "Products",
                columns: new[] { "CompanyId", "ColourCode" },
                unique: true);
        }
    }
}
