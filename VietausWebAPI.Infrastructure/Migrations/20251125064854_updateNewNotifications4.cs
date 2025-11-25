using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewNotifications4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "labeltemplates",
                schema: "printcell",
                newName: "labeltemplates",
                newSchema: "printect");

            migrationBuilder.RenameTable(
                name: "labelelements",
                schema: "printcell",
                newName: "labelelements",
                newSchema: "printect");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "printcell");

            migrationBuilder.RenameTable(
                name: "labeltemplates",
                schema: "printect",
                newName: "labeltemplates",
                newSchema: "printcell");

            migrationBuilder.RenameTable(
                name: "labelelements",
                schema: "printect",
                newName: "labelelements",
                newSchema: "printcell");
        }
    }
}
