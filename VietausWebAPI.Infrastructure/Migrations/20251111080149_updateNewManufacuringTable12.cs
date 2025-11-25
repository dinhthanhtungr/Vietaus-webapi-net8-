using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_equipment_Companies_CompanyId",
                schema: "mro",
                table: "equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_equipment_Parts_PartId1",
                schema: "mro",
                table: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_equipment_CompanyId",
                schema: "mro",
                table: "equipment");

            migrationBuilder.DropIndex(
                name: "IX_equipment_PartId1",
                schema: "mro",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "mro",
                table: "equipment");

            migrationBuilder.DropColumn(
                name: "PartId1",
                schema: "mro",
                table: "equipment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "mro",
                table: "equipment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PartId1",
                schema: "mro",
                table: "equipment",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_equipment_CompanyId",
                schema: "mro",
                table: "equipment",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_PartId1",
                schema: "mro",
                table: "equipment",
                column: "PartId1");

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_Companies_CompanyId",
                schema: "mro",
                table: "equipment",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_equipment_Parts_PartId1",
                schema: "mro",
                table: "equipment",
                column: "PartId1",
                principalSchema: "hr",
                principalTable: "Parts",
                principalColumn: "PartID");
        }
    }
}
