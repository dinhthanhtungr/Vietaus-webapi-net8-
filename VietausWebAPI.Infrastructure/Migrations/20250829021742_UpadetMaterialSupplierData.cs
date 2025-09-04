using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpadetMaterialSupplierData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                schema: "inventory",
                table: "Materials_Suppliers",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "inventory",
                table: "Materials_Suppliers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Suppliers_CreatedBy",
                schema: "inventory",
                table: "Materials_Suppliers",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialsSuppliers_CreatedBy",
                schema: "inventory",
                table: "Materials_Suppliers",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialsSuppliers_CreatedBy",
                schema: "inventory",
                table: "Materials_Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_Materials_Suppliers_CreatedBy",
                schema: "inventory",
                table: "Materials_Suppliers");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                schema: "inventory",
                table: "Materials_Suppliers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "inventory",
                table: "Materials_Suppliers");
        }
    }
}
