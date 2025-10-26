using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMerchandiseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormulaExternalId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                newName: "UnitPriceAgreed");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPaid",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(3)",
                unicode: false,
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerByNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManagerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpectedQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ProductExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RealQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RecommendedUnitPrice",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "FormulaId",
                principalSchema: "labs",
                principalTable: "Formulas",
                principalColumn: "FormulaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_MerchandiseOrderDetails_FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "Currency",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "CustomerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "CustomerNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "ManagerByNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "ManagerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "BaseCostSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "FormulaExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "RealQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "RecommendedUnitPrice",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "UnitPriceAgreed",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                newName: "Price");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPaid",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<double>(
                name: "ExpectedQuantity",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormulaExternalId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
