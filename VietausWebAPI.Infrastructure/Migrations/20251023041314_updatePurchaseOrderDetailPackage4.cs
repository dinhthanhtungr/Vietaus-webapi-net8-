using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderDetailPackage4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageType",
                schema: "Orders",
                table: "PurchaseOrderSnapshot");

            migrationBuilder.RenameColumn(
                name: "PaymentDays",
                schema: "Orders",
                table: "PurchaseOrderSnapshot",
                newName: "PaymentTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentTypes",
                schema: "Orders",
                table: "PurchaseOrderSnapshot",
                newName: "PaymentDays");

            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                schema: "Orders",
                table: "PurchaseOrderSnapshot",
                type: "text",
                nullable: true);
        }
    }
}
