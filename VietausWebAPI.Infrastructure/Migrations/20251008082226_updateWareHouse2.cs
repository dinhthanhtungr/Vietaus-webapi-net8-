using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateWareHouse2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WareHouseRequestDetail");

            migrationBuilder.RenameTable(
                name: "WareHouseRequestDetail",
                schema: "Warehouse",
                newName: "WarehouseRequestDetail",
                newSchema: "Warehouse");

            migrationBuilder.RenameTable(
                name: "WareHouseRequest",
                schema: "Warehouse",
                newName: "WarehouseRequest",
                newSchema: "Warehouse");

            migrationBuilder.RenameIndex(
                name: "IX_WareHouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                newName: "IX_WarehouseRequestDetail_RequestCode");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "RequestCode",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.RenameTable(
                name: "WarehouseRequestDetail",
                schema: "Warehouse",
                newName: "WareHouseRequestDetail",
                newSchema: "Warehouse");

            migrationBuilder.RenameTable(
                name: "WarehouseRequest",
                schema: "Warehouse",
                newName: "WareHouseRequest",
                newSchema: "Warehouse");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WareHouseRequestDetail",
                newName: "IX_WareHouseRequestDetail_RequestCode");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WareHouseRequestDetail",
                column: "RequestCode",
                principalSchema: "Warehouse",
                principalTable: "WareHouseRequest",
                principalColumn: "RequestCode");
        }
    }
}
