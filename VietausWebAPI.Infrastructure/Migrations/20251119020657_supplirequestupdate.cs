using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class supplirequestupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyRequest_Company",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyRequest_CreatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyRequest_UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropTable(
                name: "ApprovalHistory",
                schema: "SupplyRequest");

            migrationBuilder.DropTable(
                name: "RequestDetail",
                schema: "SupplyRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK__SupplyRe__33A8519A7A78FE1B",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropIndex(
                name: "IX_SupplyRequests_CompanyId",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropIndex(
                name: "IX_SupplyRequests_UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropColumn(
                name: "CancelNote",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropColumn(
                name: "RequestSourceType",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.RenameColumn(
                name: "RequestStatus",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "requeststatus");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "ExternalID",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "externalid");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "createdby");

            migrationBuilder.RenameColumn(
                name: "RequestID",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "requestid");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "created_date");

            migrationBuilder.RenameIndex(
                name: "IX_SupplyRequests_CreatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "ix_supply_requests_createdby");

            migrationBuilder.AlterColumn<string>(
                name: "requeststatus",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "externalid",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "createdby",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "requestid",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_supply_requests",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "requestid");

            migrationBuilder.CreateTable(
                name: "SupplyRequestDetails",
                schema: "SupplyRequest",
                columns: table => new
                {
                    detailid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    requestid = table.Column<Guid>(type: "uuid", nullable: false),
                    materialid = table.Column<Guid>(type: "uuid", nullable: false),
                    requestedquantity = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supply_request_details", x => x.detailid);
                    table.ForeignKey(
                        name: "fk_sr_details_material",
                        column: x => x.materialid,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sr_details_request",
                        column: x => x.requestid,
                        principalSchema: "SupplyRequest",
                        principalTable: "SupplyRequests",
                        principalColumn: "requestid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_supply_requests_status_createddate",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                columns: new[] { "requeststatus", "created_date" });

            migrationBuilder.CreateIndex(
                name: "ux_supply_requests_externalid",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "externalid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sr_details_materialid",
                schema: "SupplyRequest",
                table: "SupplyRequestDetails",
                column: "materialid");

            migrationBuilder.CreateIndex(
                name: "ix_sr_details_requestid",
                schema: "SupplyRequest",
                table: "SupplyRequestDetails",
                column: "requestid");

            migrationBuilder.CreateIndex(
                name: "ux_sr_details_request_material",
                schema: "SupplyRequest",
                table: "SupplyRequestDetails",
                columns: new[] { "requestid", "materialid" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_supply_requests_createdby",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "createdby",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_supply_requests_createdby",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropTable(
                name: "SupplyRequestDetails",
                schema: "SupplyRequest");

            migrationBuilder.DropPrimaryKey(
                name: "pk_supply_requests",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropIndex(
                name: "ix_supply_requests_status_createddate",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropIndex(
                name: "ux_supply_requests_externalid",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.DropColumn(
                name: "isactive",
                schema: "SupplyRequest",
                table: "SupplyRequests");

            migrationBuilder.RenameColumn(
                name: "requeststatus",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "RequestStatus");

            migrationBuilder.RenameColumn(
                name: "note",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "externalid",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "ExternalID");

            migrationBuilder.RenameColumn(
                name: "createdby",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "requestid",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "RequestID");

            migrationBuilder.RenameColumn(
                name: "created_date",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "ix_supply_requests_createdby",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                newName: "IX_SupplyRequests_CreatedBy");

            migrationBuilder.AlterColumn<string>(
                name: "RequestStatus",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalID",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestID",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "CancelNote",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestSourceType",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__SupplyRe__33A8519A7A78FE1B",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "RequestID");

            migrationBuilder.CreateTable(
                name: "ApprovalHistory",
                schema: "SupplyRequest",
                columns: table => new
                {
                    ApprovalID = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestID = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Approval__328477D4B3FEB4F1", x => x.ApprovalID);
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_EmployeeID",
                        column: x => x.EmployeeID,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_RequestID",
                        column: x => x.RequestID,
                        principalSchema: "SupplyRequest",
                        principalTable: "SupplyRequests",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "RequestDetail",
                schema: "SupplyRequest",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "integer", nullable: false),
                    MaterialID = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestID = table.Column<Guid>(type: "uuid", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    RequestStatus = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    RequestedQuantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RequestD__135C314D66D34B2A", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_SupplyRequest_MaterialId",
                        column: x => x.MaterialID,
                        principalSchema: "Material",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_SupplyRequest_RequestID",
                        column: x => x.RequestID,
                        principalSchema: "SupplyRequest",
                        principalTable: "SupplyRequests",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_CompanyId",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_EmployeeID",
                schema: "SupplyRequest",
                table: "ApprovalHistory",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_RequestID",
                schema: "SupplyRequest",
                table: "ApprovalHistory",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_MaterialID",
                schema: "SupplyRequest",
                table: "RequestDetail",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_RequestID",
                schema: "SupplyRequest",
                table: "RequestDetail",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyRequest_Company",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "CompanyId",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "companyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyRequest_CreatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyRequest_UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "UpdatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
