using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrderDetail_MfgProductionOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WareHouseRequest__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrderDetail_MfgProductionOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "RequestedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "ExportExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "MfgProductionOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.RenameColumn(
                name: "ExportId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "WarehouseRequestId");

            migrationBuilder.RenameColumn(
                name: "MfgProductionOrderExternalId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "ManufacturingFormulaExternalIdSnapShot");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightKg",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StockStatus",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LotNumber",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BagNumber",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReqType",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReqStatus",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "WarehouseRequestCodeSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WareHouseRequest__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_CompanyId",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequest_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequest_Company",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequest_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequestDetail_RequestId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "RequestId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequest_Company",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequest_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRequestDetail_RequestId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WareHouseRequest__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequest_CompanyId",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseRequest_CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "RequestId",
                schema: "Warehouse",
                table: "WarehouseRequestDetail");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Warehouse",
                table: "WarehouseRequest");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestCodeSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "LocationExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.DropColumn(
                name: "ProductNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.RenameColumn(
                name: "WarehouseRequestId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                newName: "ExportId");

            migrationBuilder.RenameColumn(
                name: "ManufacturingFormulaExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "MfgProductionOrderExternalId");

            migrationBuilder.AlterColumn<decimal>(
                name: "WeightKg",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "numeric(18,3)",
                precision: 18,
                scale: 3,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,3)",
                oldPrecision: 18,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "StockStatus",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LotNumber",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "BagNumber",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Guid>(
                name: "RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "ReqType",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ReqStatus",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AddColumn<string>(
                name: "CreatedDate",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "citext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestedBy",
                schema: "Warehouse",
                table: "WarehouseRequest",
                type: "citext",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExportExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MfgProductionOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WareHouseRequest__3214EC07A98DEC4E",
                schema: "Warehouse",
                table: "WarehouseRequest",
                column: "RequestCode");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "RequestCode");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrderDetail_MfgProductionOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MfgProductionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrderDetail_MfgProductionOrder",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                column: "MfgProductionOrderId",
                principalSchema: "manufacturing",
                principalTable: "MfgProductionOrders",
                principalColumn: "mfgProductionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WarehouseRequestDetail",
                column: "RequestCode",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequest",
                principalColumn: "RequestCode");
        }
    }
}
