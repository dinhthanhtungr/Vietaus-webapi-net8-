using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnitKeyfromMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Unit",
                schema: "inventory",
                table: "Materials");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Units_UnitId",
                schema: "inventory",
                table: "Materials");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Unit",
                schema: "inventory",
                table: "Materials",
                column: "UnitId",
                principalSchema: "inventory",
                principalTable: "Units",
                principalColumn: "UnitId");
        }
    }
}
