using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerFinal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CreatedBy",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_UpdatedBy",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK__Groups__CompanyI__131DCD43",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInGroup_Groups",
                schema: "company",
                table: "MemberInGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInGroup_Profile",
                schema: "company",
                table: "MemberInGroup");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "company",
                table: "Groups",
                type: "citext",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "company",
                table: "Groups",
                type: "citext",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "UX_MemberInGroup_Group_Profile_Active",
                schema: "company",
                table: "MemberInGroup",
                columns: new[] { "GroupId", "Profile" },
                unique: true,
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "UX_Groups_Company_ExternalId",
                schema: "company",
                table: "Groups",
                columns: new[] { "CompanyId", "ExternalId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Company",
                schema: "company",
                table: "Groups",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CreatedBy",
                schema: "company",
                table: "Groups",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_UpdatedBy",
                schema: "company",
                table: "Groups",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInGroup_Group",
                schema: "company",
                table: "MemberInGroup",
                column: "GroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInGroup_Profile",
                schema: "company",
                table: "MemberInGroup",
                column: "Profile",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Company",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_CreatedBy",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_UpdatedBy",
                schema: "company",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInGroup_Group",
                schema: "company",
                table: "MemberInGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_MemberInGroup_Profile",
                schema: "company",
                table: "MemberInGroup");

            migrationBuilder.DropIndex(
                name: "UX_MemberInGroup_Group_Profile_Active",
                schema: "company",
                table: "MemberInGroup");

            migrationBuilder.DropIndex(
                name: "UX_Groups_Company_ExternalId",
                schema: "company",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "company",
                table: "Groups",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalId",
                schema: "company",
                table: "Groups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_CreatedBy",
                schema: "company",
                table: "Groups",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_UpdatedBy",
                schema: "company",
                table: "Groups",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK__Groups__CompanyI__131DCD43",
                schema: "company",
                table: "Groups",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInGroup_Groups",
                schema: "company",
                table: "MemberInGroup",
                column: "GroupId",
                principalSchema: "company",
                principalTable: "Groups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberInGroup_Profile",
                schema: "company",
                table: "MemberInGroup",
                column: "Profile",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
