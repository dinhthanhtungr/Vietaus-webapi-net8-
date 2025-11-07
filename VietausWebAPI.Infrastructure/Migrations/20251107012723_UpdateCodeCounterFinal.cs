using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCodeCounterFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_ProductId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_MerchandiseOrderDetails_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                newName: "IX_MO_Details_OrderId");

            migrationBuilder.AlterColumn<decimal>(
                name: "RealQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PackageWeight",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "code_counters",
                schema: "Audit",
                columns: table => new
                {
                    prefix = table.Column<string>(type: "text", nullable: false),
                    ymd = table.Column<int>(type: "integer", nullable: false),
                    last_value = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_code_counters_prefix_ymd", x => new { x.prefix, x.ymd });
                });

            migrationBuilder.CreateIndex(
                name: "IX_MO_Customer_Active_CreateDateDesc",
                schema: "Orders",
                table: "MerchandiseOrders",
                columns: new[] { "CustomerId", "CreateDate", "MerchandiseOrderId" },
                descending: new[] { false, true, true },
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_MO_Detail_Order_Active",
                schema: "Orders",
                table: "MerchandiseOrders",
                columns: new[] { "MerchandiseOrderId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_MO_Tenant_Active_CreateDateDesc",
                schema: "Orders",
                table: "MerchandiseOrders",
                columns: new[] { "CompanyId", "CreateDate", "MerchandiseOrderId" },
                descending: new[] { false, true, true },
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "UX_MO_Company_ExternalId",
                schema: "Orders",
                table: "MerchandiseOrders",
                columns: new[] { "CompanyId", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MO_Details_Order_Formula",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                columns: new[] { "MerchandiseOrderId", "FormulaId" });

            migrationBuilder.CreateIndex(
                name: "IX_MO_Details_Order_Product",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                columns: new[] { "MerchandiseOrderId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_MO_Details_Order_Status_DateDesc",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                columns: new[] { "MerchandiseOrderId", "IsActive", "Status", "ExpectedDeliveryDate", "MerchandiseOrderDetailId" },
                descending: new[] { false, false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "ix_code_counters_prefix",
                schema: "Audit",
                table: "code_counters",
                column: "prefix");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_Product",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_Product",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropTable(
                name: "code_counters",
                schema: "Audit");

            migrationBuilder.DropIndex(
                name: "IX_MO_Customer_Active_CreateDateDesc",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "IX_MO_Detail_Order_Active",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "IX_MO_Tenant_Active_CreateDateDesc",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "UX_MO_Company_ExternalId",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "IX_MO_Details_Order_Formula",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_MO_Details_Order_Product",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_MO_Details_Order_Status_DateDesc",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_MO_Details_OrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                newName: "IX_MerchandiseOrderDetails_MerchandiseOrderId");

            migrationBuilder.AlterColumn<decimal>(
                name: "RealQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PackageWeight",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "MerchandiseOrderId",
                principalSchema: "Orders",
                principalTable: "MerchandiseOrders",
                principalColumn: "MerchandiseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_ProductId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
