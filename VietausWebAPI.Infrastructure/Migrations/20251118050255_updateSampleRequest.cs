using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSampleRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Branch",
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

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Branches_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "BranchId",
                principalSchema: "company",
                principalTable: "Branches",
                principalColumn: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Branches_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests");

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
                name: "FK_SampleRequests_Branch",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "BranchId",
                principalSchema: "company",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
