using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addAiSummaryGenerate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerInteractionAiSummaries",
                schema: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SummaryScope = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    Month = table.Column<int>(type: "integer", nullable: true),
                    PeriodFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PeriodTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InteractionCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PreviousSummary = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Summary = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CustomerNeed = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    CurrentStage = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    NextAction = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Risk = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Sentiment = table.Column<string>(type: "citext", maxLength: 50, nullable: false, defaultValue: ""),
                    SourceModel = table.Column<string>(type: "citext", maxLength: 100, nullable: false, defaultValue: ""),
                    PromptVersion = table.Column<string>(type: "citext", maxLength: 50, nullable: false, defaultValue: "v1"),
                    IsAiSuccess = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsAiSkipped = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AiErrorMessage = table.Column<string>(type: "text", nullable: true),
                    AiGeneratedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInteractionAiSummaries_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInteractionAiSummaries_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInteractionAiSummaries_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerInteractionAiSummaries_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "Customer",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInteractionAiSummaries_SaleEmployee",
                        column: x => x.SaleEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CustomerInteractionAiSummaries_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_AiStatus",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                columns: new[] { "IsAiSuccess", "IsAiSkipped", "AiGeneratedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_Company_Year_Month",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                columns: new[] { "CompanyId", "Year", "Month", "SummaryScope" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_CreatedBy",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_Customer_Period",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                columns: new[] { "CompanyId", "CustomerId", "SummaryScope", "PeriodFrom", "PeriodTo" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_CustomerId",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_Sale_Period",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                columns: new[] { "CompanyId", "SaleEmployeeId", "SummaryScope", "PeriodFrom", "PeriodTo" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_SaleEmployeeId",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                column: "SaleEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInteractionAiSummaries_UpdatedBy",
                schema: "Customer",
                table: "CustomerInteractionAiSummaries",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInteractionAiSummaries",
                schema: "Customer");
        }
    }
}
