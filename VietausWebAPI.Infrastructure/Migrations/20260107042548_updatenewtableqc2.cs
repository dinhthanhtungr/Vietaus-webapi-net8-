using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByWarehouse_Customer",
                schema: "devandqa",
                table: "QCInputByWarehouse");

            migrationBuilder.DropIndex(
                name: "ix_qcinputbywarehouse_customerid",
                schema: "devandqa",
                table: "QCInputByWarehouse");

            migrationBuilder.DropColumn(
                name: "customerid",
                schema: "devandqa",
                table: "QCInputByWarehouse");

            migrationBuilder.RenameColumn(
                name: "customernamesnapshot",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                newName: "csnamesnapshot");

            migrationBuilder.RenameColumn(
                name: "customerexternalidsnapshot",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                newName: "csexternalidsnapshot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "csnamesnapshot",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                newName: "customernamesnapshot");

            migrationBuilder.RenameColumn(
                name: "csexternalidsnapshot",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                newName: "customerexternalidsnapshot");

            migrationBuilder.AddColumn<Guid>(
                name: "customerid",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbywarehouse_customerid",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "customerid");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByWarehouse_Customer",
                schema: "devandqa",
                table: "QCInputByWarehouse",
                column: "customerid",
                principalSchema: "Customer",
                principalTable: "Customer",
                principalColumn: "CustomerId");
        }
    }
}
