using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStockTypeStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // (Tùy chọn) đảm bảo có citext cho bước Down
            migrationBuilder.Sql("""CREATE EXTENSION IF NOT EXISTS citext;""");

            migrationBuilder.Sql("""
                ALTER TABLE "Warehouse"."WarehouseShelfStock"
                ALTER COLUMN "StockType" TYPE integer USING
                  CASE
                    WHEN "StockType" IS NULL THEN 0
                    WHEN ("StockType")::text ~ '^[0-9]+$' THEN ("StockType")::integer
                    WHEN UPPER(("StockType")::text) = 'RAWMATERIAL'  THEN 0
                    WHEN UPPER(("StockType")::text) = 'SEMIFINISHED' THEN 1
                    WHEN UPPER(("StockType")::text) = 'FINISHEDGOOD' THEN 2
                    WHEN UPPER(("StockType")::text) = 'WIP'          THEN 3
                    ELSE 0
                  END;

                ALTER TABLE "Warehouse"."WarehouseShelfStock"
                ALTER COLUMN "StockType" SET NOT NULL,
                ALTER COLUMN "StockType" SET DEFAULT 0;
            """);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            // Đổi lại integer -> citext phải có USING, và drop default trước
            migrationBuilder.Sql("""
                ALTER TABLE "Warehouse"."WarehouseShelfStock"
                ALTER COLUMN "StockType" DROP DEFAULT;

                ALTER TABLE "Warehouse"."WarehouseShelfStock"
                ALTER COLUMN "StockType" TYPE citext
                USING ("StockType")::text::citext;

                ALTER TABLE "Warehouse"."WarehouseShelfStock"
                ALTER COLUMN "StockType" DROP NOT NULL;
            """);
        }
    }
}
