using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMaterialPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceHistory",
                schema: "Material");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceHistory",
                schema: "Material",
                columns: table => new
                {
                    PriceHistoryId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    NewPrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    OldPrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PriceHis__A927CACB4B3A2EAC", x => x.PriceHistoryId);
                    table.ForeignKey(
                        name: "FK_PriceHistory_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Material",
                        column: x => x.MaterialId,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Supplier",
                        column: x => x.SupplierId,
                        principalSchema: "Material",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_CreatedBy",
                schema: "Material",
                table: "PriceHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_EmployeeId",
                schema: "Material",
                table: "PriceHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_MaterialId",
                schema: "Material",
                table: "PriceHistory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_SupplierId",
                schema: "Material",
                table: "PriceHistory",
                column: "SupplierId");
        }
    }
}
