using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatepunchaseOrderIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "isActive");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                newName: "isActive");

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<Guid>(
                name: "attachmentcollectionid",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<bool>(
                name: "isActive",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "isActive",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                newName: "IsActive");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "attachmentcollectionid",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Orders",
                table: "PurchaseOrderDetails",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);
        }
    }
}
