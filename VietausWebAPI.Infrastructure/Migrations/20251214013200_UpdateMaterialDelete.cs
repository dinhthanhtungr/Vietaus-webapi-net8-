using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterialDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialsSuppliers_Material",
                schema: "Material",
                table: "Materials_Suppliers");

            migrationBuilder.DropForeignKey(
                name: "fk_pricehistory_materialsSuppliersId",
                schema: "Material",
                table: "PriceHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialsSuppliers_Material",
                schema: "Material",
                table: "Materials_Suppliers",
                column: "MaterialId",
                principalSchema: "Material",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_pricehistory_materialsSuppliersId",
                schema: "Material",
                table: "PriceHistory",
                column: "materialsSuppliersId",
                principalSchema: "Material",
                principalTable: "Materials_Suppliers",
                principalColumn: "Materials_SuppliersId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialsSuppliers_Material",
                schema: "Material",
                table: "Materials_Suppliers");

            migrationBuilder.DropForeignKey(
                name: "fk_pricehistory_materialsSuppliersId",
                schema: "Material",
                table: "PriceHistory");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialsSuppliers_Material",
                schema: "Material",
                table: "Materials_Suppliers",
                column: "MaterialId",
                principalSchema: "Material",
                principalTable: "Materials",
                principalColumn: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "fk_pricehistory_materialsSuppliersId",
                schema: "Material",
                table: "PriceHistory",
                column: "materialsSuppliersId",
                principalSchema: "Material",
                principalTable: "Materials_Suppliers",
                principalColumn: "Materials_SuppliersId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
