using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateManufacturingvuFORMULA1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManufacturingVUFormula",
                schema: "SampleRequests",
                columns: table => new
                {
                    manufacturingvuformulaid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    formulaid = table.Column<Guid>(type: "uuid", nullable: false),
                    totalproductionquantity = table.Column<decimal>(type: "numeric", nullable: true),
                    numofbatches = table.Column<int>(type: "integer", nullable: true),
                    labnote = table.Column<string>(type: "citext", nullable: true),
                    requirement = table.Column<string>(type: "citext", nullable: true),
                    qccheck = table.Column<string>(type: "citext", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    createdby = table.Column<Guid>(type: "uuid", nullable: false),
                    updateddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedby = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturingVUFormula", x => x.manufacturingvuformulaid);
                    table.ForeignKey(
                        name: "FK_ManufacturingVUFormula_CreatedBy",
                        column: x => x.createdby,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ManufacturingVUFormula_Formula",
                        column: x => x.formulaid,
                        principalSchema: "SampleRequests",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId");
                    table.ForeignKey(
                        name: "FK_ManufacturingVUFormula_UpdatedBy",
                        column: x => x.updatedby,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingVUFormula_createdby",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "createdby");

            migrationBuilder.CreateIndex(
                name: "ix_manufacturingvuformula_createddate",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "createddate");

            migrationBuilder.CreateIndex(
                name: "ix_manufacturingvuformula_formulaid",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "formulaid");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingVUFormula_updatedby",
                schema: "SampleRequests",
                table: "ManufacturingVUFormula",
                column: "updatedby");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturingVUFormula",
                schema: "SampleRequests");
        }
    }
}
