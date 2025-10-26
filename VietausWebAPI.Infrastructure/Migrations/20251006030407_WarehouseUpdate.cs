using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WarehouseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Warehouse");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "WarehouseShelfStock",
                schema: "Warehouse",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SlotCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BatchNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LotKey = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    QtyKg = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: false),
                    Bags = table.Column<int>(type: "integer", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CompanyId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WarehouseShelfStock__3214EC07A98DEC4E", x => x.SlotId);
                    table.ForeignKey(
                        name: "FK_WarehouseShelfStock_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseShelfStock_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_WarehouseShelfStock_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_WarehouseShelfStock_Employees_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSnapshotSet",
                schema: "Warehouse",
                columns: table => new
                {
                    SnapshotSetId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaCode = table.Column<string>(type: "citext", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WarehouseSnapshotSet__3214EC07A98DEC4E", x => x.SnapshotSetId);
                    table.ForeignKey(
                        name: "FK_WarehouseSnapshotSet_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseSnapshotSet_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_WarehouseSnapshotSet_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseSnapshotSet_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseTempStock",
                schema: "Warehouse",
                columns: table => new
                {
                    TempId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    TempType = table.Column<int>(type: "integer", nullable: false),
                    SnapshotSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaCode = table.Column<string>(type: "citext", nullable: false),
                    VaLineCode = table.Column<string>(type: "citext", nullable: true),
                    Code = table.Column<string>(type: "citext", nullable: false),
                    LotKey = table.Column<string>(type: "citext", nullable: true),
                    QtyStock = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true),
                    QtyRequest = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true),
                    ReserveStatus = table.Column<int>(type: "integer", nullable: true),
                    LinkedIssueId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WarehouseTempStock__3214EC07A98DEC4E", x => x.TempId);
                    table.ForeignKey(
                        name: "FK_WarehouseTempStock_Employees_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseTempStock_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_WarehouseTempStock_WarehouseSnapshotSet_SnapshotSetId",
                        column: x => x.SnapshotSetId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseSnapshotSet",
                        principalColumn: "SnapshotSetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_CompanyId_Code",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                columns: new[] { "CompanyId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_CompanyId_Code_LotKey",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                columns: new[] { "CompanyId", "Code", "LotKey" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseShelfStock_UpdatedBy",
                schema: "Warehouse",
                table: "WarehouseShelfStock",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_CompanyId_VaCode_createdDate",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                columns: new[] { "CompanyId", "VaCode", "createdDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_CompanyId1",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSnapshotSet_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseSnapshotSet",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_CompanyId_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                columns: new[] { "CompanyId", "SnapshotSetId" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_CompanyId_TempType_Code_LotKey",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                columns: new[] { "CompanyId", "TempType", "Code", "LotKey" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_CompanyId_VaCode",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                columns: new[] { "CompanyId", "VaCode" });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_EmployeeId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseTempStock_SnapshotSetId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                column: "SnapshotSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WarehouseShelfStock",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "WarehouseTempStock",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "WarehouseSnapshotSet",
                schema: "Warehouse");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }
    }
}
