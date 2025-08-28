using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexTestMaterialSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Materials_Suppliers_MaterialId_IsPreferred",
                schema: "inventory",
                table: "Materials_Suppliers",
                columns: new[] { "MaterialId", "IsPreferred" },
                unique: true,
                filter: "\"IsPreferred\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materials_Suppliers_MaterialId_IsPreferred",
                schema: "inventory",
                table: "Materials_Suppliers");
        }
    }
}
