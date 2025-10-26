using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewSchemas2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PurchaseOrdersSchedules",
                schema: "Others",
                newName: "PurchaseOrdersSchedules",
                newSchema: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Others");

            migrationBuilder.RenameTable(
                name: "PurchaseOrdersSchedules",
                schema: "Orders",
                newName: "PurchaseOrdersSchedules",
                newSchema: "Others");
        }
    }
}
