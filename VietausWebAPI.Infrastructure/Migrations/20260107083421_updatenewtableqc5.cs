using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatenewtableqc5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QCInputByQCs",
                columns: table => new
                {
                    QCInputByQCId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttachmentCollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseRequestDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionMethod = table.Column<string>(type: "text", nullable: true),
                    IsCOAProvided = table.Column<bool>(type: "boolean", nullable: true),
                    IsMSDSTDSProvided = table.Column<bool>(type: "boolean", nullable: true),
                    IsMetalDetectionRequired = table.Column<bool>(type: "boolean", nullable: true),
                    ImportWarehouseType = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestDetailDetailId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QCInputByQCs", x => x.QCInputByQCId);
                    table.ForeignKey(
                        name: "FK_QCInputByQCs_AttachmentCollection_AttachmentCollectionId",
                        column: x => x.AttachmentCollectionId,
                        principalSchema: "Attachment",
                        principalTable: "AttachmentCollection",
                        principalColumn: "AttachmentCollectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QCInputByQCs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_QCInputByQCs_WarehouseRequestDetail_RequestDetailDetailId",
                        column: x => x.RequestDetailDetailId,
                        principalSchema: "Warehouse",
                        principalTable: "WarehouseRequestDetail",
                        principalColumn: "detailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQCs_AttachmentCollectionId",
                table: "QCInputByQCs",
                column: "AttachmentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQCs_EmployeeId",
                table: "QCInputByQCs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_QCInputByQCs_RequestDetailDetailId",
                table: "QCInputByQCs",
                column: "RequestDetailDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QCInputByQCs");
        }
    }
}
