using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderDetai5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                newName: "UnitPriceAgreed");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                newName: "TotalPriceAgreed");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RealQuantity",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RequestQuantity",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "RealQuantity",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "RequestQuantity",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "UnitPriceAgreed",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "TotalPriceAgreed",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                newName: "Price");
        }
    }
}
