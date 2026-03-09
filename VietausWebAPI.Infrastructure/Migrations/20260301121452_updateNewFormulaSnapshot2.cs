using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewFormulaSnapshot2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "FormulaMaterialSnapshots",
                schema: "SampleRequestSchema",
                newName: "FormulaMaterialSnapshots",
                newSchema: "SampleRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SampleRequestSchema");

            migrationBuilder.RenameTable(
                name: "FormulaMaterialSnapshots",
                schema: "SampleRequests",
                newName: "FormulaMaterialSnapshots",
                newSchema: "SampleRequestSchema");
        }
    }
}
