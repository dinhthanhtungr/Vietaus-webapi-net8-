using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSampleRequestFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_CreatedBy",
                schema: "company",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_UpdatedBy",
                schema: "company",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK__Branches__Compan__0C70CFB4",
                schema: "company",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_AttachmentCollection_AttachmentCollectionId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Company",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Customer",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Formula",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Manager",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Product",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SampleRe__6F83B553A1A0D2A8",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_CompanyId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_SendBy1",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "Branch",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "Image",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.RenameColumn(
                name: "Comment",
                schema: "SampleRequests",
                table: "SampleRequests",
                newName: "SaleComment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RequestType",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Package",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalComment",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "SendDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "company",
                table: "Branches",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                schema: "company",
                table: "Branches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "company",
                table: "Branches",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "company",
                table: "Branches",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "company",
                table: "Branches",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "company",
                table: "Branches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "company",
                table: "Branches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "company",
                table: "Branches",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SampleRequests",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "SampleRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_Company_CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                columns: new[] { "CompanyId", "CreatedBy" });

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_Company_Customer",
                schema: "SampleRequests",
                table: "SampleRequests",
                columns: new[] { "CompanyId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_Company_IsActive_CreatedDateDesc",
                schema: "SampleRequests",
                table: "SampleRequests",
                columns: new[] { "CompanyId", "IsActive", "CreatedDate", "SampleRequestId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_Company_Product",
                schema: "SampleRequests",
                table: "SampleRequests",
                columns: new[] { "CompanyId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_Company_Status_CreatedDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                columns: new[] { "CompanyId", "IsActive", "Status", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "UX_SampleRequests_Company_ExternalId",
                schema: "SampleRequests",
                table: "SampleRequests",
                columns: new[] { "CompanyId", "IsActive", "ExternalId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_CreatedBy",
                schema: "company",
                table: "Branches",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UpdatedBy",
                schema: "company",
                table: "Branches",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Branches__Compan__0C70CFB4",
                schema: "company",
                table: "Branches",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_AttachmentCollection",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Branch",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "BranchId",
                principalSchema: "company",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Company",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Customer",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Formula",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Manager",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "ManagerBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Product",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "SendBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_CreatedBy",
                schema: "company",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_UpdatedBy",
                schema: "company",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK__Branches__Compan__0C70CFB4",
                schema: "company",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_AttachmentCollection",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Branch",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Company",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Customer",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Formula",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Manager",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_Product",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SampleRequests",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_BranchId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_Company_CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_Company_Customer",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_Company_IsActive_CreatedDateDesc",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_Company_Product",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_Company_Status_CreatedDate",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "UX_SampleRequests_Company_ExternalId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "SendDate",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.RenameColumn(
                name: "SaleComment",
                schema: "SampleRequests",
                table: "SampleRequests",
                newName: "Comment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldDefaultValue: "New");

            migrationBuilder.AlterColumn<string>(
                name: "RequestType",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Package",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalComment",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Branch",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "company",
                table: "Branches",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                schema: "company",
                table: "Branches",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "company",
                table: "Branches",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "company",
                table: "Branches",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "company",
                table: "Branches",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "company",
                table: "Branches",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "company",
                table: "Branches",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "company",
                table: "Branches",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK__SampleRe__6F83B553A1A0D2A8",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "SampleRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_CompanyId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_SendBy1",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "ManagerBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_CreatedBy",
                schema: "company",
                table: "Branches",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_UpdatedBy",
                schema: "company",
                table: "Branches",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK__Branches__Compan__0C70CFB4",
                schema: "company",
                table: "Branches",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_AttachmentCollection_AttachmentCollectionId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Company",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_CreatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Customer",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Formula",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "FormulaId",
                principalSchema: "SampleRequests",
                principalTable: "Formulas",
                principalColumn: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Manager",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "ManagerBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_Product",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "ProductId",
                principalSchema: "SampleRequests",
                principalTable: "Products",
                principalColumn: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_SendBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "SendBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_UpdatedBy",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
