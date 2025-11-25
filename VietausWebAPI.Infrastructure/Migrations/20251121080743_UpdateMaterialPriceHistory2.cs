using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterialPriceHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceHistory",
                schema: "Material",
                columns: table => new
                {
                    PriceHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    materialsSuppliersId = table.Column<Guid>(type: "uuid", nullable: false),
                    oldPrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PriceHis__A927CACB4B3A2EAC", x => x.PriceHistoryId);
                    table.ForeignKey(
                        name: "FK_PriceHistory_Materials_CreatedBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceHistory_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Material",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "fk_pricehistory_materialsSuppliersId",
                        column: x => x.materialsSuppliersId,
                        principalSchema: "Material",
                        principalTable: "Materials_Suppliers",
                        principalColumn: "Materials_SuppliersId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pricehistory_createDate",
                schema: "Material",
                table: "PriceHistory",
                column: "createDate");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_CreatedBy",
                schema: "Material",
                table: "PriceHistory",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_MaterialId",
                schema: "Material",
                table: "PriceHistory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_MaterialsSuppliersId",
                schema: "Material",
                table: "PriceHistory",
                column: "materialsSuppliersId");

            migrationBuilder.CreateIndex(
                name: "ix_pricehistory_supplier_date",
                schema: "Material",
                table: "PriceHistory",
                columns: new[] { "materialsSuppliersId", "createDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_SupplierId",
                schema: "Material",
                table: "PriceHistory",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistory",
                schema: "Material");
        }
    }
}
