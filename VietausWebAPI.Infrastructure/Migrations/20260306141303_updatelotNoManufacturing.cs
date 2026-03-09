using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelotNoManufacturing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LotNo",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "lot_no");

            migrationBuilder.AddColumn<string>(
                name: "LotNo",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LotNo",
                schema: "SampleRequests",
                table: "FormulaMaterialSnapshots");

            migrationBuilder.RenameColumn(
                name: "lot_no",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                newName: "LotNo");
        }
    }
}
