using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_MaterialId_SupplierId_IsActive",
                schema: "inventory",
                table: "PriceHistory",
                columns: new[] { "MaterialId", "SupplierId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceHistory_MaterialId_SupplierId_IsActive",
                schema: "inventory",
                table: "PriceHistory");
        }
    }
}
