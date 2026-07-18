using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class qoutationupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotations",
                schema: "Customer",
                columns: table => new
                {
                    QuotationId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "citext", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContactName = table.Column<string>(type: "citext", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "VND"),
                    ExchangeRate = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    QuotationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PaymentTerms = table.Column<string>(type: "text", nullable: true),
                    DeliveryTerms = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    PreviousQuotationId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.QuotationId);
                    table.ForeignKey(
                        name: "FK_Quotations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalSchema: "Customer",
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Quotations_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_Employees_SaleEmployeeId",
                        column: x => x.SaleEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotations_Employees_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Quotations_Quotations_PreviousQuotationId",
                        column: x => x.PreviousQuotationId,
                        principalSchema: "Customer",
                        principalTable: "Quotations",
                        principalColumn: "QuotationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuotationLines",
                schema: "Customer",
                columns: table => new
                {
                    QuotationLineId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    QuotationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductExternalIdSnapshot = table.Column<string>(type: "citext", nullable: false),
                    ProductNameSnapshot = table.Column<string>(type: "citext", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    Unit = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    DiscountPercent = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    TaxPercent = table.Column<decimal>(type: "numeric(8,4)", precision: 8, scale: 4, nullable: false),
                    LineTotal = table.Column<decimal>(type: "numeric(22,6)", precision: 22, scale: 6, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationLines", x => x.QuotationLineId);
                    table.ForeignKey(
                        name: "FK_QuotationLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SampleRequests",
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationLines_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalSchema: "Customer",
                        principalTable: "Quotations",
                        principalColumn: "QuotationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationStatusHistories",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    QuotationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatus = table.Column<int>(type: "integer", nullable: false),
                    ToStatus = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationStatusHistories_Employees_ChangedBy",
                        column: x => x.ChangedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationStatusHistories_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalSchema: "Customer",
                        principalTable: "Quotations",
                        principalColumn: "QuotationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLines_ProductId",
                schema: "Customer",
                table: "QuotationLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationLines_Quotation_SortOrder",
                schema: "Customer",
                table: "QuotationLines",
                columns: new[] { "QuotationId", "SortOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_Company_Customer_Date",
                schema: "Customer",
                table: "Quotations",
                columns: new[] { "CompanyId", "CustomerId", "QuotationDate" },
                descending: new[] { false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_Company_Sale_Status_Date",
                schema: "Customer",
                table: "Quotations",
                columns: new[] { "CompanyId", "SaleEmployeeId", "Status", "QuotationDate" },
                descending: new[] { false, false, false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_ContactId",
                schema: "Customer",
                table: "Quotations",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CreatedBy",
                schema: "Customer",
                table: "Quotations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_CustomerId",
                schema: "Customer",
                table: "Quotations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_PreviousQuotationId",
                schema: "Customer",
                table: "Quotations",
                column: "PreviousQuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_SaleEmployeeId",
                schema: "Customer",
                table: "Quotations",
                column: "SaleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_UpdatedBy",
                schema: "Customer",
                table: "Quotations",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UX_Quotations_Company_ExternalId_Version",
                schema: "Customer",
                table: "Quotations",
                columns: new[] { "CompanyId", "ExternalId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationStatusHistories_ChangedBy",
                schema: "Customer",
                table: "QuotationStatusHistories",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationStatusHistories_Quotation_Date",
                schema: "Customer",
                table: "QuotationStatusHistories",
                columns: new[] { "QuotationId", "ChangedDate" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationLines",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "QuotationStatusHistories",
                schema: "Customer");

            migrationBuilder.DropTable(
                name: "Quotations",
                schema: "Customer");
        }
    }
}
