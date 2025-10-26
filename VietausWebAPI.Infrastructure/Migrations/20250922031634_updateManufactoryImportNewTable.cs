using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufactoryImportNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MfgProductionOrders",
                columns: table => new
                {
                    mfgProductionOrderId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    externalId = table.Column<string>(type: "citext", nullable: true),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    merchandiseOrderExternalId = table.Column<string>(type: "citext", nullable: true),
                    productionType = table.Column<string>(type: "citext", nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    productExternalIdSnapshot = table.Column<string>(type: "citext", nullable: true),
                    productNameSnapshot = table.Column<string>(type: "citext", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    customerNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    customerExternalIdSnapshot = table.Column<string>(type: "text", nullable: true),
                    ManufacturingFormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    formulaExternalIdSnapshot = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    companyId = table.Column<Guid>(type: "uuid", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    totalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    manufacturingDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    expectedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    requiredDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    totalQuantity = table.Column<int>(type: "integer", nullable: true),
                    numOfBatches = table.Column<int>(type: "integer", nullable: true),
                    quantityPerBatch = table.Column<int>(type: "integer", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    requirement = table.Column<string>(type: "text", nullable: true),
                    bagType = table.Column<string>(type: "text", nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "timezone('utc', now())"),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MfgProductionOrders__MfgProductionOrderId", x => x.mfgProductionOrderId);
                    table.ForeignKey(
                        name: "FK_MfgProductionOrders_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_MfgProductionOrders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_MfgProductionOrders_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Mpo__companyId",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mpo__createdBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mpo__customerId",
                        column: x => x.CustomerId,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mpo__merchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mpo__productId",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mpo__updatedBy",
                        column: x => x.updatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulas",
                columns: table => new
                {
                    manufacturingFormulaId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    mfgProductionOrderExternalId = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    mfgProductionOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true),
                    totalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    checkBy = table.Column<Guid>(type: "uuid", nullable: true),
                    checkNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    checkDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    isSelect = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    companyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId2 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulas__manufacturingFormulaId", x => x.manufacturingFormulaId);
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulas_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulas_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulas_Employees_EmployeeId2",
                        column: x => x.EmployeeId2,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Mf__checkBy",
                        column: x => x.checkBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mf__companyId",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mf__createdBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mf__mfgProductionOrderId",
                        column: x => x.mfgProductionOrderId,
                        principalTable: "MfgProductionOrders",
                        principalColumn: "mfgProductionOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mf__updatedBy",
                        column: x => x.updatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaMaterials",
                columns: table => new
                {
                    manufacturingFormulaMaterialId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    manufacturingFormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    categoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,6)", precision: 18, scale: 6, nullable: true),
                    unit = table.Column<string>(type: "text", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: true),
                    lotNo = table.Column<string>(type: "text", nullable: true),
                    stockId = table.Column<Guid>(type: "uuid", nullable: true),
                    materialNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    materialExternalIdSnapshot = table.Column<string>(type: "text", nullable: true),
                    CategoryId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    MaterialId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaMaterials__manufacturingFormulaMaterialId", x => x.manufacturingFormulaMaterialId);
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulaMaterials_Categories_CategoryId1",
                        column: x => x.CategoryId1,
                        principalSchema: "inventory",
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_ManufacturingFormulaMaterials_Materials_MaterialId1",
                        column: x => x.MaterialId1,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK__Mfm__categoryId",
                        column: x => x.categoryId,
                        principalSchema: "inventory",
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mfm__manufacturingFormulaId",
                        column: x => x.manufacturingFormulaId,
                        principalTable: "ManufacturingFormulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Mfm__materialId",
                        column: x => x.materialId,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_categoryId",
                table: "ManufacturingFormulaMaterials",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_CategoryId1",
                table: "ManufacturingFormulaMaterials",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_manufacturingFormulaId",
                table: "ManufacturingFormulaMaterials",
                column: "manufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_materialId",
                table: "ManufacturingFormulaMaterials",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaMaterials_MaterialId1",
                table: "ManufacturingFormulaMaterials",
                column: "MaterialId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_checkBy",
                table: "ManufacturingFormulas",
                column: "checkBy");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_companyId",
                table: "ManufacturingFormulas",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_CompanyId",
                table: "ManufacturingFormulas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_createdBy",
                table: "ManufacturingFormulas",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                table: "ManufacturingFormulas",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId1",
                table: "ManufacturingFormulas",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId2",
                table: "ManufacturingFormulas",
                column: "EmployeeId2");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_mfgProductionOrderId",
                table: "ManufacturingFormulas",
                column: "mfgProductionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_updatedBy",
                table: "ManufacturingFormulas",
                column: "updatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_companyId",
                table: "MfgProductionOrders",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_CompanyId1",
                table: "MfgProductionOrders",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_createdBy",
                table: "MfgProductionOrders",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_customerExternalIdSnapshot",
                table: "MfgProductionOrders",
                column: "customerExternalIdSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_CustomerId",
                table: "MfgProductionOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_EmployeeId",
                table: "MfgProductionOrders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_EmployeeId1",
                table: "MfgProductionOrders",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_MerchandiseOrderExternalId",
                table: "MfgProductionOrders",
                column: "merchandiseOrderExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_MerchandiseOrderId",
                table: "MfgProductionOrders",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_productExternalIdSnapshot",
                table: "MfgProductionOrders",
                column: "productExternalIdSnapshot");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_ProductId",
                table: "MfgProductionOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_status",
                table: "MfgProductionOrders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_status_expectedDate",
                table: "MfgProductionOrders",
                columns: new[] { "status", "expectedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_status_requiredDate",
                table: "MfgProductionOrders",
                columns: new[] { "status", "requiredDate" });

            migrationBuilder.CreateIndex(
                name: "IX_MfgProductionOrders_updatedBy",
                table: "MfgProductionOrders",
                column: "updatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturingFormulaMaterials");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulas");

            migrationBuilder.DropTable(
                name: "MfgProductionOrders");
        }
    }
}
