using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormulaFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Company",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_CreatedBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Product",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_UpdatedBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "IX_Formulas_SentBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropTable(
                name: "FormulaStatusLog",
                schema: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_SentBy1",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "CheckNameSnapshot",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "SentByNameSnapshot",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "SampleRequests",
                table: "Formulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "SampleRequests",
                table: "Formulas",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelect",
                schema: "SampleRequests",
                table: "Formulas",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "SampleRequests",
                table: "Formulas",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "SampleRequests",
                table: "Formulas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductId_Company_IsActive_CreatedDateDesc",
                schema: "SampleRequests",
                table: "Formulas",
                columns: new[] { "CompanyId", "IsActive", "CreatedDate", "ProductId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CheckBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Company",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CreatedBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Product",
                schema: "SampleRequests",
                table: "Formulas",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_SentBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "SentBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_UpdatedBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Company",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_CreatedBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_Product",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_SentBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_UpdatedBy",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_ProductId_Company_IsActive_CreatedDateDesc",
                schema: "SampleRequests",
                table: "Formulas");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "SampleRequests",
                table: "Formulas",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "SampleRequests",
                table: "Formulas",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "SampleRequests",
                table: "Formulas",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelect",
                schema: "SampleRequests",
                table: "Formulas",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "SampleRequests",
                table: "Formulas",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "SampleRequests",
                table: "Formulas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CheckNameSnapshot",
                schema: "SampleRequests",
                table: "Formulas",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SentByNameSnapshot",
                schema: "SampleRequests",
                table: "Formulas",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "SampleRequests",
                table: "FormulaMaterials",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                name: "FormulaStatusLog",
                schema: "SampleRequests",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    FormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateNameSnapShot = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
                    NewStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    OldStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaStatusLog", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_FormulaStatusLog_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_FormulaStatusLog_Formula",
                        column: x => x.FormulaId,
                        principalSchema: "SampleRequests",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_SentBy1",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CheckBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStatusLog_CreatedAt",
                schema: "SampleRequests",
                table: "FormulaStatusLog",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStatusLog_CreatedBy",
                schema: "SampleRequests",
                table: "FormulaStatusLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStatusLog_FormulaId",
                schema: "SampleRequests",
                table: "FormulaStatusLog",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CheckBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CheckBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Company",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_CreatedBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_Product",
                schema: "SampleRequests",
                table: "Formulas",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_UpdatedBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "IX_Formulas_SentBy",
                schema: "SampleRequests",
                table: "Formulas",
                column: "SentBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
