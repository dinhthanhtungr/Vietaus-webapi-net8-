using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomer2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CreatedBy",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_UpdatedBy",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK__Customer__Compan__1E8F7FEF",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Company",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_CreatedBy",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_EmployeeID",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_updatedBy",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_Company",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_CreatedBy",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_FromEmployeeId",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_FromGroupId",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_ToEmployeeId",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_ToGroupId",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCustomerTransfer_CustomerID",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCustomerTransfer_LogID",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DetailCu__3214EC273B3F47A0",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropIndex(
                name: "IX_DetailCustomerTransfer_LogID",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTransferLog_companyId",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAssignment_companyId",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropColumn(
                name: "ID",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropColumn(
                name: "Product",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "LogID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                newName: "LogId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCustomerTransfer_CustomerID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                newName: "IX_DetailCustomerTransfer_CustomerId");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "ID",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerTransferLog_createdBy",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "IX_CustomerTransferLog_CreatedBy");

            migrationBuilder.RenameColumn(
                name: "updatedDate",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "updatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "GroupID",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "CustomerID",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "ID",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_updatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_EmployeeID",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_CustomerID",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_createdBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_UpdatedBy",
                schema: "Customer",
                table: "Customer",
                newName: "IX_Customers_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_CreatedBy",
                schema: "Customer",
                table: "Customer",
                newName: "IX_Customers_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_CompanyId",
                schema: "Customer",
                table: "Customer",
                newName: "IX_Customers_CompanyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "Customer",
                table: "CustomerTransferLog",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "Customer",
                table: "CustomerTransferLog",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedDate",
                schema: "Customer",
                table: "CustomerAssignment",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "Customer",
                table: "CustomerAssignment",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "CustomerAssignment",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "Customer",
                type: "boolean",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "Customer",
                table: "Customer",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "Customer",
                table: "Customer",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "Customer",
                table: "Customer",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "Address",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailCustomerTransfer",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                columns: new[] { "LogId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_Company_CreatedDateDesc",
                schema: "Customer",
                table: "CustomerTransferLog",
                columns: new[] { "CompanyId", "CreatedDate", "Id" },
                descending: new[] { false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_Company_FromEmp_CreatedDateDesc",
                schema: "Customer",
                table: "CustomerTransferLog",
                columns: new[] { "CompanyId", "FromEmployeeId", "CreatedDate", "Id" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_Company_ToEmp_CreatedDateDesc",
                schema: "Customer",
                table: "CustomerTransferLog",
                columns: new[] { "CompanyId", "ToEmployeeId", "CreatedDate", "Id" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_CustAssign_Company_Active_Emp_UpdatedDesc",
                schema: "Customer",
                table: "CustomerAssignment",
                columns: new[] { "CompanyId", "IsActive", "EmployeeId", "UpdatedDate", "Id" },
                descending: new[] { false, false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_CustAssign_Company_Active_Group_UpdatedDesc",
                schema: "Customer",
                table: "CustomerAssignment",
                columns: new[] { "CompanyId", "IsActive", "GroupId", "UpdatedDate", "Id" },
                descending: new[] { false, false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "IX_CustAssign_Company_Customer_Active",
                schema: "Customer",
                table: "CustomerAssignment",
                columns: new[] { "CompanyId", "CustomerId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_CustAssign_Company_Customer_Group_Active",
                schema: "Customer",
                table: "CustomerAssignment",
                columns: new[] { "CompanyId", "CustomerId", "GroupId" },
                unique: true,
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Company_IsActive_CreatedDateDesc",
                schema: "Customer",
                table: "Customer",
                columns: new[] { "CompanyId", "IsActive", "CreatedDate", "CustomerId" },
                descending: new[] { false, false, true, true });

            migrationBuilder.CreateIndex(
                name: "UX_Customers_Company_ExternalId",
                schema: "Customer",
                table: "Customer",
                columns: new[] { "CompanyId", "ExternalId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Company",
                schema: "Customer",
                table: "Customer",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CreatedBy",
                schema: "Customer",
                table: "Customer",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_UpdatedBy",
                schema: "Customer",
                table: "Customer",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Company",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_CreatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Employee",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_UpdatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_Company",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_CreatedBy",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_FromEmployee",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "FromEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_FromGroup",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "FromGroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_ToEmployee",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "ToEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_ToGroup",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "ToGroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCustomerTransfer_Customer",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "CustomerId",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCustomerTransfer_Log",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "LogId",
                principalSchema: "Customer",
                principalTable: "CustomerTransferLog",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Company",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CreatedBy",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_UpdatedBy",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Company",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_CreatedBy",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_Employee",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAssignment_UpdatedBy",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_Company",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_CreatedBy",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_FromEmployee",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_FromGroup",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_ToEmployee",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerTransferLog_ToGroup",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCustomerTransfer_Customer",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_DetailCustomerTransfer_Log",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailCustomerTransfer",
                schema: "Customer",
                table: "DetailCustomerTransfer");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTransferLog_Company_CreatedDateDesc",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTransferLog_Company_FromEmp_CreatedDateDesc",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropIndex(
                name: "IX_CustomerTransferLog_Company_ToEmp_CreatedDateDesc",
                schema: "Customer",
                table: "CustomerTransferLog");

            migrationBuilder.DropIndex(
                name: "IX_CustAssign_Company_Active_Emp_UpdatedDesc",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropIndex(
                name: "IX_CustAssign_Company_Active_Group_UpdatedDesc",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropIndex(
                name: "IX_CustAssign_Company_Customer_Active",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropIndex(
                name: "UX_CustAssign_Company_Customer_Group_Active",
                schema: "Customer",
                table: "CustomerAssignment");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Company_IsActive_CreatedDateDesc",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "UX_Customers_Company_ExternalId",
                schema: "Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Customer",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "LogId",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                newName: "LogID");

            migrationBuilder.RenameIndex(
                name: "IX_DetailCustomerTransfer_CustomerId",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                newName: "IX_DetailCustomerTransfer_CustomerID");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "createdDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerTransferLog_CreatedBy",
                schema: "Customer",
                table: "CustomerTransferLog",
                newName: "IX_CustomerTransferLog_createdBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "updatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "GroupID");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "EmployeeID");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "CustomerID");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "createdDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_UpdatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_updatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_EmployeeId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_EmployeeID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_CustomerId",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_CustomerID");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerAssignment_CreatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                newName: "IX_CustomerAssignment_createdBy");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_UpdatedBy",
                schema: "Customer",
                table: "Customer",
                newName: "IX_Customer_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_CreatedBy",
                schema: "Customer",
                table: "Customer",
                newName: "IX_Customer_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_CompanyId",
                schema: "Customer",
                table: "Customer",
                newName: "IX_Customer_CompanyId");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                schema: "Customer",
                table: "CustomerTransferLog",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdDate",
                schema: "Customer",
                table: "CustomerTransferLog",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updatedDate",
                schema: "Customer",
                table: "CustomerAssignment",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "CustomerAssignment",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdDate",
                schema: "Customer",
                table: "CustomerAssignment",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                schema: "Customer",
                table: "Customer",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "Customer",
                table: "Customer",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "Customer",
                table: "Customer",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                schema: "Customer",
                table: "Customer",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                schema: "Customer",
                table: "Customer",
                type: "citext",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__DetailCu__3214EC273B3F47A0",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_DetailCustomerTransfer_LogID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "LogID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_companyId",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_companyId",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "companyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CreatedBy",
                schema: "Customer",
                table: "Customer",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_UpdatedBy",
                schema: "Customer",
                table: "Customer",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK__Customer__Compan__1E8F7FEF",
                schema: "Customer",
                table: "Customer",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Company",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "companyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_CreatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "createdBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_Customer",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "CustomerID",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_EmployeeID",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "EmployeeID",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAssignment_updatedBy",
                schema: "Customer",
                table: "CustomerAssignment",
                column: "updatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_Company",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "companyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_CreatedBy",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "createdBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_FromEmployeeId",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "FromEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_FromGroupId",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "FromGroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_ToEmployeeId",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "ToEmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerTransferLog_ToGroupId",
                schema: "Customer",
                table: "CustomerTransferLog",
                column: "ToGroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCustomerTransfer_CustomerID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "CustomerID",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailCustomerTransfer_LogID",
                schema: "Customer",
                table: "DetailCustomerTransfer",
                column: "LogID",
                principalSchema: "Customer",
                principalTable: "CustomerTransferLog",
                principalColumn: "ID");
        }
    }
}
