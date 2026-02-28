using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingFormulavu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingVUFormula_CreatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingVUFormula_Formula",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingVUFormula_UpdatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.EnsureSchema(
                name: "SampleRequestSchema");

            migrationBuilder.AddColumn<int>(
                name: "status",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                type: "integer",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.CreateTable(
                name: "FormulaMaterialSnapshots",
                schema: "SampleRequestSchema",
                columns: table => new
                {
                    FormulaMaterialSnapshotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturingVUFormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(16,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(16,2)", nullable: false),
                    itemType = table.Column<int>(type: "integer", nullable: false),
                    MaterialNameSnapshot = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MaterialExternalIdSnapshot = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ManufacturingVUFormulaId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormulaMaterialSnapshots", x => x.FormulaMaterialSnapshotId);
                    table.ForeignKey(
                        name: "FK_FormulaMaterialSnapshots_ManufacturingVUFormula_Manufacturi~",
                        column: x => x.ManufacturingVUFormulaId1,
                        principalSchema: "SampleRequests",
                        principalTable: "ManufacturingVUFormula",
                        principalColumn: "manufacturingvuformulaid");
                    table.ForeignKey(
                        name: "FK_ManufacturingVUFormula_FormulaMaterialSnapshots",
                        column: x => x.ManufacturingVUFormulaId,
                        principalSchema: "SampleRequests",
                        principalTable: "ManufacturingVUFormula",
                        principalColumn: "manufacturingvuformulaid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormulaMaterialSnapshots_ManufacturingVUFormulaId",
                schema: "SampleRequestSchema",
                table: "FormulaMaterialSnapshots",
                column: "ManufacturingVUFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaMaterialSnapshots_ManufacturingVUFormulaId1",
                schema: "SampleRequestSchema",
                table: "FormulaMaterialSnapshots",
                column: "ManufacturingVUFormulaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingVUFormula_CreatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "createdby",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingVUFormula_Formula",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "formulaid",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingVUFormula_UpdatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "updatedby",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingVUFormula_CreatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingVUFormula_Formula",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingVUFormula_UpdatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.DropTable(
                name: "FormulaMaterialSnapshots",
                schema: "SampleRequestSchema");

            migrationBuilder.DropColumn(
                name: "status",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingVUFormula_CreatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "createdby",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingVUFormula_Formula",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "formulaid",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingVUFormula_UpdatedBy",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "updatedby",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
