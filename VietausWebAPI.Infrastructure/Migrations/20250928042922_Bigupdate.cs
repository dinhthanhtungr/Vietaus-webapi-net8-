using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Bigupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mf__checkBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "checkDate",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.RenameColumn(
                name: "checkNameSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "SourceVUExternalIdSnapshot");

            migrationBuilder.RenameColumn(
                name: "checkBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "SourceVUFormulaId");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_checkBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_SourceVUFormulaId");

            migrationBuilder.AlterColumn<Guid>(
                name: "updatedBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "isStandard",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isSelect",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "createdBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceManufacturingExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ManufacturingFormulaLog",
                schema: "manufacturing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ManufacturingFormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    PerformedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedByNameSnapshot = table.Column<string>(type: "text", nullable: true),
                    PerformedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ManufacturingFormulaLog__Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Mf__ManufacturingFormulaLogs",
                        column: x => x.ManufacturingFormulaId,
                        principalSchema: "manufacturing",
                        principalTable: "ManufacturingFormulas",
                        principalColumn: "manufacturingFormulaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Mf__performedBy",
                        column: x => x.PerformedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulas_SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "SourceManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaLog_ManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaLog",
                column: "ManufacturingFormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturingFormulaLog_PerformedBy",
                schema: "manufacturing",
                table: "ManufacturingFormulaLog",
                column: "PerformedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK__Mf__SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "SourceManufacturingFormulaId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulas",
                principalColumn: "manufacturingFormulaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__Mf__SourceVUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "SourceVUFormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManufacturingFormulas_Employees_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK__Mf__SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropForeignKey(
                name: "FK__Mf__SourceVUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropTable(
                name: "ManufacturingFormulaLog",
                schema: "manufacturing");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturingFormulas_SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "SourceManufacturingExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "SourceManufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "SourceType",
                schema: "manufacturing",
                table: "ManufacturingFormulas");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.RenameColumn(
                name: "SourceVUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "checkBy");

            migrationBuilder.RenameColumn(
                name: "SourceVUExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "checkNameSnapshot");

            migrationBuilder.RenameIndex(
                name: "IX_ManufacturingFormulas_SourceVUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                newName: "IX_ManufacturingFormulas_checkBy");

            migrationBuilder.AlterColumn<Guid>(
                name: "updatedBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "mfgProductionOrderId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<bool>(
                name: "isStandard",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isSelect",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "createdBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "VUFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "checkDate",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Mf__checkBy",
                schema: "manufacturing",
                table: "ManufacturingFormulas",
                column: "checkBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
