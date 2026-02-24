using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatepproductstandart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "externalid",
                schema: "devandqa",
                table: "ProductStandard",
                type: "citext",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_productstandard_externalid",
                schema: "devandqa",
                table: "ProductStandard",
                column: "externalid");

            migrationBuilder.CreateIndex(
                name: "ix_productstandard_productexternalid",
                schema: "devandqa",
                table: "ProductStandard",
                column: "productexternalid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_productstandard_externalid",
                schema: "devandqa",
                table: "ProductStandard");

            migrationBuilder.DropIndex(
                name: "ix_productstandard_productexternalid",
                schema: "devandqa",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "externalid",
                schema: "devandqa",
                table: "ProductStandard");
        }
    }
}
