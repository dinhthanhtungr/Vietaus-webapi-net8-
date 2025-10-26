using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewSchemas3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Schedual");

            migrationBuilder.RenameTable(
                name: "SchedualMfg",
                newName: "SchedualMfg",
                newSchema: "Schedual");

            migrationBuilder.AddColumn<Guid>(
                name: "MfgProductionOrderId",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductAddRate",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCustomerExternalId",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductExpiryType",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductExternalId",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductMaxTemp",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductRecycleRate",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProductRohsStandard",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductWeight",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MfgProductionOrderId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductAddRate",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductCustomerExternalId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductExpiryType",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductExternalId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductMaxTemp",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductName",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductRecycleRate",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductRohsStandard",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductWeight",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.RenameTable(
                name: "SchedualMfg",
                schema: "Schedual",
                newName: "SchedualMfg");
        }
    }
}
