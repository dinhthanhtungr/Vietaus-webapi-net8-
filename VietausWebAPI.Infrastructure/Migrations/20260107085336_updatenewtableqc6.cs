using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQCs_AttachmentCollection_AttachmentCollectionId",
                table: "QCInputByQCs");

            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQCs_Employees_EmployeeId",
                table: "QCInputByQCs");

            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQCs_WarehouseRequestDetail_RequestDetailDetailId",
                table: "QCInputByQCs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QCInputByQCs",
                table: "QCInputByQCs");

            migrationBuilder.DropIndex(
                name: "IX_QCInputByQCs_EmployeeId",
                table: "QCInputByQCs");

            migrationBuilder.DropIndex(
                name: "IX_QCInputByQCs_RequestDetailDetailId",
                table: "QCInputByQCs");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "QCInputByQCs");

            migrationBuilder.DropColumn(
                name: "RequestDetailDetailId",
                table: "QCInputByQCs");

            migrationBuilder.DropColumn(
                name: "WarehouseRequestDetailId",
                table: "QCInputByQCs");

            migrationBuilder.RenameTable(
                name: "QCInputByQCs",
                newName: "QCInputByQC",
                newSchema: "devandqa");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "IsMetalDetectionRequired",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "ismetaldetectionrequired");

            migrationBuilder.RenameColumn(
                name: "IsMSDSTDSProvided",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "ismsdstdsprovided");

            migrationBuilder.RenameColumn(
                name: "IsCOAProvided",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "iscoaprovided");

            migrationBuilder.RenameColumn(
                name: "InspectionMethod",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "inspectionmethod");

            migrationBuilder.RenameColumn(
                name: "ImportWarehouseType",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "importwarehousetype");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "createddate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "AttachmentCollectionId",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "attachmentcollectionid");

            migrationBuilder.RenameColumn(
                name: "QCInputByQCId",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "qcinputbyqcid");

            migrationBuilder.RenameIndex(
                name: "IX_QCInputByQCs_AttachmentCollectionId",
                schema: "devandqa",
                table: "QCInputByQC",
                newName: "IX_QCInputByQC_attachmentcollectionid");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "inspectionmethod",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "qcinputbyqcid",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "CSExternalId",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CSName",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialExternalId",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialName",
                schema: "devandqa",
                table: "QCInputByQC",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QCInputByQC",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "qcinputbyqcid");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQC_createdby",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "createdby");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbyqc_createddate",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "createddate");

            migrationBuilder.CreateIndex(
                name: "ix_qcinputbyqc_importwarehousetype",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "importwarehousetype");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQC_AttachmentCollection",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "attachmentcollectionid",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQC_Employee",
                schema: "devandqa",
                table: "QCInputByQC",
                column: "createdby",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQC_AttachmentCollection",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropForeignKey(
                name: "FK_QCInputByQC_Employee",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QCInputByQC",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "IX_QCInputByQC_createdby",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "ix_qcinputbyqc_createddate",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropIndex(
                name: "ix_qcinputbyqc_importwarehousetype",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "CSExternalId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "CSName",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "MaterialExternalId",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.DropColumn(
                name: "MaterialName",
                schema: "devandqa",
                table: "QCInputByQC");

            migrationBuilder.RenameTable(
                name: "QCInputByQC",
                schema: "devandqa",
                newName: "QCInputByQCs");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "QCInputByQCs",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "ismsdstdsprovided",
                table: "QCInputByQCs",
                newName: "IsMSDSTDSProvided");

            migrationBuilder.RenameColumn(
                name: "ismetaldetectionrequired",
                table: "QCInputByQCs",
                newName: "IsMetalDetectionRequired");

            migrationBuilder.RenameColumn(
                name: "iscoaprovided",
                table: "QCInputByQCs",
                newName: "IsCOAProvided");

            migrationBuilder.RenameColumn(
                name: "inspectionmethod",
                table: "QCInputByQCs",
                newName: "InspectionMethod");

            migrationBuilder.RenameColumn(
                name: "importwarehousetype",
                table: "QCInputByQCs",
                newName: "ImportWarehouseType");

            migrationBuilder.RenameColumn(
                name: "createddate",
                table: "QCInputByQCs",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "createdby",
                table: "QCInputByQCs",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "attachmentcollectionid",
                table: "QCInputByQCs",
                newName: "AttachmentCollectionId");

            migrationBuilder.RenameColumn(
                name: "qcinputbyqcid",
                table: "QCInputByQCs",
                newName: "QCInputByQCId");

            migrationBuilder.RenameIndex(
                name: "IX_QCInputByQC_attachmentcollectionid",
                table: "QCInputByQCs",
                newName: "IX_QCInputByQCs_AttachmentCollectionId");

            migrationBuilder.AlterColumn<string>(
                name: "Note",
                table: "QCInputByQCs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InspectionMethod",
                table: "QCInputByQCs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QCInputByQCId",
                table: "QCInputByQCs",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "QCInputByQCs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestDetailDetailId",
                table: "QCInputByQCs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseRequestDetailId",
                table: "QCInputByQCs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_QCInputByQCs",
                table: "QCInputByQCs",
                column: "QCInputByQCId");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQCs_EmployeeId",
                table: "QCInputByQCs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQCs_RequestDetailDetailId",
                table: "QCInputByQCs",
                column: "RequestDetailDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQCs_AttachmentCollection_AttachmentCollectionId",
                table: "QCInputByQCs",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQCs_Employees_EmployeeId",
                table: "QCInputByQCs",
                column: "EmployeeId",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_QCInputByQCs_WarehouseRequestDetail_RequestDetailDetailId",
                table: "QCInputByQCs",
                column: "RequestDetailDetailId",
                principalSchema: "Warehouse",
                principalTable: "WarehouseRequestDetail",
                principalColumn: "detailId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
