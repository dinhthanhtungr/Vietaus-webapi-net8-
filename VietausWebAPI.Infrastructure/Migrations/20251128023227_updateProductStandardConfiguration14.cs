using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartId",
                schema: "company",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_PartId",
                schema: "company",
                table: "Groups",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Part",
                schema: "company",
                table: "Groups",
                column: "PartId",
                principalSchema: "hr",
                principalTable: "Parts",
                principalColumn: "PartID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Part",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_PartId",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PartId",
                schema: "company",
                table: "Groups");
        }
    }
}
