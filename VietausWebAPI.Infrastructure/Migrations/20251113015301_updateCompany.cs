using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "company",
                table: "Companies",
                newName: "updatedDate");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                schema: "company",
                table: "Companies",
                newName: "updatedBy");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "company",
                table: "Companies",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "company",
                table: "Companies",
                newName: "createdDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "company",
                table: "Companies",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "company",
                table: "Companies",
                newName: "companyId");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "company",
                table: "Companies",
                newName: "companyExternalId");

            migrationBuilder.AlterColumn<string>(
                name: "companyExternalId",
                schema: "company",
                table: "Companies",
                type: "citext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                schema: "company",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country",
                schema: "company",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                schema: "company",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                schema: "company",
                table: "Companies",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                schema: "company",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "zipCode",
                schema: "company",
                table: "Companies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "country",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "email",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "isActive",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "phone",
                schema: "company",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "zipCode",
                schema: "company",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "updatedDate",
                schema: "company",
                table: "Companies",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "updatedBy",
                schema: "company",
                table: "Companies",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "name",
                schema: "company",
                table: "Companies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                schema: "company",
                table: "Companies",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                schema: "company",
                table: "Companies",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "companyId",
                schema: "company",
                table: "Companies",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "companyExternalId",
                schema: "company",
                table: "Companies",
                newName: "Code");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "company",
                table: "Companies",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext");
        }
    }
}
