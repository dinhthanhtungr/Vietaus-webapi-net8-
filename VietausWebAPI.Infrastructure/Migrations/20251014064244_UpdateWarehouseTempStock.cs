using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWarehouseTempStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtyStock",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.DropColumn(
                name: "VaLineCode",
                schema: "Warehouse",
                table: "WarehouseTempStock");

            migrationBuilder.AlterColumn<decimal>(
                name: "RealQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QtyStock",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VaLineCode",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "citext",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RealQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
