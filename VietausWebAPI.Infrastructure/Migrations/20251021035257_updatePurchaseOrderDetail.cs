using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "citext",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialExternalIDSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialNameSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "citext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaterialExternalIDSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "MaterialNameSnapshot",
                schema: "Orders",
                table: "PurchaseOrderDetails");

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExternalId",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);
        }
    }
}
