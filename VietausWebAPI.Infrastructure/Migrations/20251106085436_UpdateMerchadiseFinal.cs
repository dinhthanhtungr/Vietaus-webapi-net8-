using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMerchadiseFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_AttachmentCollection_AttachmentCollection~",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_Customer",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderst_Company",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderst_CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderst_ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderst_UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropTable(
                name: "MerchandiseOrderLogs",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "MerchandiseOrderSchedules",
                schema: "Orders");

            migrationBuilder.DropColumn(
                name: "ProductionType",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.RenameColumn(
                name: "createDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                schema: "Customer",
                table: "Contacts",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ContactID",
                schema: "Customer",
                table: "Contacts",
                newName: "ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_CustomerID",
                schema: "Customer",
                table: "Contacts",
                newName: "IX_Contacts_CustomerId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                schema: "Customer",
                table: "Address",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "AddressID",
                schema: "Customer",
                table: "Address",
                newName: "AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CustomerID",
                schema: "Customer",
                table: "Address",
                newName: "IX_Address_CustomerId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldDefaultValueSql: "timezone('utc', now())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingMethod",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PONo",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ManagerByNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPaid",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(10)",
                unicode: false,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3)",
                oldUnicode: false,
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PackageWeight",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryRequestDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BagType",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "Address",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_Customer_IsPrimary",
                schema: "Customer",
                table: "Address",
                columns: new[] { "CustomerId", "IsPrimary" });

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_AttachmentCollection",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_Company",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_Customer",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "ManagerById",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_AttachmentCollection",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_Company",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_Customer",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "IX_Address_Customer_IsPrimary",
                schema: "Customer",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                newName: "createDate");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "Customer",
                table: "Contacts",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "ContactId",
                schema: "Customer",
                table: "Contacts",
                newName: "ContactID");

            migrationBuilder.RenameIndex(
                name: "IX_Contacts_CustomerId",
                schema: "Customer",
                table: "Contacts",
                newName: "IX_Contacts_CustomerID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "Customer",
                table: "Address",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                schema: "Customer",
                table: "Address",
                newName: "AddressID");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CustomerId",
                schema: "Customer",
                table: "Address",
                newName: "IX_Address_CustomerID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createDate",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "timestamp without time zone",
                nullable: true,
                defaultValueSql: "timezone('utc', now())",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingMethod",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PONo",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerByNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPaid",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryAddress",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomerId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "character varying(3)",
                unicode: false,
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldUnicode: false,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNameSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "ProductExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "PackageWeight",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "FormulaExternalIdSnapshot",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryRequestDate",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "BagType",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ProductionType",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "Address",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                name: "MerchandiseOrderLogs",
                schema: "Orders",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uuid", nullable: false),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MerchandiseOrderLogs__LogId", x => x.LogId);
                    table.ForeignKey(
                        name: "FK__MerchandiseOrderLogs__MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__MerchandiseOrderLogs__createdBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MerchandiseOrderSchedules",
                schema: "Orders",
                columns: table => new
                {
                    MerchandiseOrderScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Merchand__B49D545E8C296524", x => x.MerchandiseOrderScheduleId);
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderSchedules_MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderLogs_createdBy",
                schema: "Orders",
                table: "MerchandiseOrderLogs",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderLogs_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderLogs",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderSchedules_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderSchedules",
                column: "MerchandiseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_AttachmentCollection_AttachmentCollection~",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_Customer",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderst_Company",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderst_CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderst_ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "ManagerById",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderst_UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
