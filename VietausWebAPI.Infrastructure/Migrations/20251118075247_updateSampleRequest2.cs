using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSampleRequest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Branches_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "company");

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "BranchId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "companyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "company",
                columns: table => new
                {
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Branches__A1682FC5D195FBDD", x => x.BranchId);
                    table.ForeignKey(
                        name: "FK_Branches_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Branches_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Branches__Compan__0C70CFB4",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "companyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyId",
                schema: "company",
                table: "Branches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedBy",
                schema: "company",
                table: "Branches",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UpdatedBy",
                schema: "company",
                table: "Branches",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Branches_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "BranchId",
                principalSchema: "company",
                principalTable: "Branches",
                principalColumn: "BranchId");
        }
    }
}
