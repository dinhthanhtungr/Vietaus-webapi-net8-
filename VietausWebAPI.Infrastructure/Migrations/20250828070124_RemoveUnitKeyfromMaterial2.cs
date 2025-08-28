using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnitKeyfromMaterial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Units_UnitId",
                schema: "inventory",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_UnitId",
                schema: "inventory",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "UnitId",
                schema: "inventory",
                table: "Materials");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                schema: "inventory",
                table: "Materials",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                schema: "inventory",
                table: "Materials");

            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                schema: "inventory",
                table: "Materials",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UnitId",
                schema: "inventory",
                table: "Materials",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Units_UnitId",
                schema: "inventory",
                table: "Materials",
                column: "UnitId",
                principalSchema: "inventory",
                principalTable: "Units",
                principalColumn: "UnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
