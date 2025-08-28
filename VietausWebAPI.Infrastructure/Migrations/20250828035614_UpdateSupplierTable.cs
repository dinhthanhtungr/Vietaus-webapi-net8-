using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSupplierTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Application",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Group",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RegNo",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "TaxNo",
                schema: "inventory",
                table: "Suppliers",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "inventory",
                table: "Suppliers",
                newName: "SupplierName");

            migrationBuilder.AddColumn<string>(
                name: "FaxNumber",
                schema: "inventory",
                table: "Suppliers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueDate",
                schema: "inventory",
                table: "Suppliers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IssuedPlace",
                schema: "inventory",
                table: "Suppliers",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                schema: "inventory",
                table: "Suppliers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TaxNumber",
                schema: "inventory",
                table: "Suppliers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "inventory",
                table: "Suppliers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "inventory",
                table: "Suppliers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                schema: "inventory",
                table: "SupplierContacts",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_UpdatedBy",
                schema: "inventory",
                table: "Suppliers",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_UpdatedBy",
                schema: "inventory",
                table: "Suppliers",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_UpdatedBy",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_UpdatedBy",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "FaxNumber",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IssueDate",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IssuedPlace",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Priority",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "TaxNumber",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "inventory",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                schema: "inventory",
                table: "SupplierContacts");

            migrationBuilder.RenameColumn(
                name: "SupplierName",
                schema: "inventory",
                table: "Suppliers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                schema: "inventory",
                table: "Suppliers",
                newName: "TaxNo");

            migrationBuilder.AddColumn<string>(
                name: "Application",
                schema: "inventory",
                table: "Suppliers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                schema: "inventory",
                table: "Suppliers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "inventory",
                table: "Suppliers",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegNo",
                schema: "inventory",
                table: "Suppliers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "inventory",
                table: "Suppliers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
