using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormulaAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormulaMaterials_Supplier",
                schema: "labs",
                table: "FormulaMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropForeignKey(
                name: "FK_Formulas_VerifiedBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropIndex(
                name: "IX_Formulas_VerifiedBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "SentBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "SentDate",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "VerifiedBy",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "VerifiedDate",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                schema: "labs",
                table: "FormulaMaterials");

            migrationBuilder.RenameColumn(
                name: "SelectedSupplierId",
                schema: "labs",
                table: "FormulaMaterials",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "LotNo",
                schema: "labs",
                table: "FormulaMaterials",
                newName: "MaterialCodeSnapshot");

            migrationBuilder.RenameIndex(
                name: "IX_FormulaMaterials_SelectedSupplierId",
                schema: "labs",
                table: "FormulaMaterials",
                newName: "IX_FormulaMaterials_CategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "labs",
                table: "Formulas",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Draft");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "labs",
                table: "FormulaMaterials",
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

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "labs",
                table: "FormulaMaterials",
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

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "labs",
                table: "FormulaMaterials",
                type: "numeric(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialNameSnapshot",
                schema: "labs",
                table: "FormulaMaterials",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "labs",
                table: "FormulaMaterials",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormulaStatusLog",
                schema: "labs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    OldStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    NewStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    CreateNameSnapShot = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
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
                        principalSchema: "labs",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStatusLog_CreatedAt",
                schema: "labs",
                table: "FormulaStatusLog",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStatusLog_CreatedBy",
                schema: "labs",
                table: "FormulaStatusLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaStatusLog_FormulaId",
                schema: "labs",
                table: "FormulaStatusLog",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormulaMaterials_Category",
                schema: "labs",
                table: "FormulaMaterials",
                column: "CategoryId",
                principalSchema: "inventory",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormulaMaterials_Category",
                schema: "labs",
                table: "FormulaMaterials");

            migrationBuilder.DropTable(
                name: "FormulaStatusLog",
                schema: "labs");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "labs",
                table: "Formulas");

            migrationBuilder.DropColumn(
                name: "MaterialNameSnapshot",
                schema: "labs",
                table: "FormulaMaterials");

            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "labs",
                table: "FormulaMaterials");

            migrationBuilder.RenameColumn(
                name: "MaterialCodeSnapshot",
                schema: "labs",
                table: "FormulaMaterials",
                newName: "LotNo");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                schema: "labs",
                table: "FormulaMaterials",
                newName: "SelectedSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_FormulaMaterials_CategoryId",
                schema: "labs",
                table: "FormulaMaterials",
                newName: "IX_FormulaMaterials_SelectedSupplierId");

            migrationBuilder.AddColumn<Guid>(
                name: "SentBy",
                schema: "labs",
                table: "Formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                schema: "labs",
                table: "Formulas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VerifiedBy",
                schema: "labs",
                table: "Formulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedDate",
                schema: "labs",
                table: "Formulas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                schema: "labs",
                table: "FormulaMaterials",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                schema: "labs",
                table: "FormulaMaterials",
                type: "numeric(16,2)",
                precision: 16,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)",
                oldPrecision: 16,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                schema: "labs",
                table: "FormulaMaterials",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                schema: "labs",
                table: "FormulaMaterials",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "SentBy");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_VerifiedBy",
                schema: "labs",
                table: "Formulas",
                column: "VerifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_FormulaMaterials_Supplier",
                schema: "labs",
                table: "FormulaMaterials",
                column: "SelectedSupplierId",
                principalSchema: "inventory",
                principalTable: "Suppliers",
                principalColumn: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "SentBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Formulas_VerifiedBy",
                schema: "labs",
                table: "Formulas",
                column: "VerifiedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
