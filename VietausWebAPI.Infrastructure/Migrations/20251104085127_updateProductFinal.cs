using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Category",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Company",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "RohsStandard",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ReturnSample",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductUsage",
                schema: "SampleRequests",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRecycle",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "FoodSafety",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeltaE",
                schema: "SampleRequests",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "SampleRequests",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "SampleRequests",
                table: "Products",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Application",
                schema: "SampleRequests",
                table: "Products",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReachStandard",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "PartName",
                schema: "hr",
                table: "Parts",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalID",
                schema: "hr",
                table: "Parts",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Company_IsActive_CreatedDateDesc",
                schema: "SampleRequests",
                table: "Products",
                columns: new[] { "CompanyId", "IsActive", "CreatedDate", "ProductId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "UX_Products_Company_Code",
                schema: "SampleRequests",
                table: "Products",
                columns: new[] { "CompanyId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Products_Company_ColourCode",
                schema: "SampleRequests",
                table: "Products",
                columns: new[] { "CompanyId", "ColourCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Category",
                schema: "SampleRequests",
                table: "Products",
                column: "CategoryId",
                principalSchema: "Material",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Company",
                schema: "SampleRequests",
                table: "Products",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Category",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Company",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Company_IsActive_CreatedDateDesc",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UX_Products_Company_Code",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "UX_Products_Company_ColourCode",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReachStandard",
                schema: "SampleRequests",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "RohsStandard",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "ReturnSample",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductUsage",
                schema: "SampleRequests",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRecycle",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "FoodSafety",
                schema: "SampleRequests",
                table: "Products",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "DeltaE",
                schema: "SampleRequests",
                table: "Products",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "SampleRequests",
                table: "Products",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "SampleRequests",
                table: "Products",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Application",
                schema: "SampleRequests",
                table: "Products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartName",
                schema: "hr",
                table: "Parts",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalID",
                schema: "hr",
                table: "Parts",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Category",
                schema: "SampleRequests",
                table: "Products",
                column: "CategoryId",
                principalSchema: "Material",
                principalTable: "Categories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Company",
                schema: "SampleRequests",
                table: "Products",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }
    }
}
