using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurnchaseAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrderDocuments",
                schema: "Orders",
                columns: table => new
                {
                    PurchaseOrderDocumentId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DocumentType = table.Column<int>(type: "integer", nullable: false, defaultValue: 99),
                    AttachmentCollectionId = table.Column<Guid>(type: "uuid", nullable: true),
                    DocumentCode = table.Column<string>(type: "citext", nullable: true),
                    DocumentName = table.Column<string>(type: "citext", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsReceived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    VerifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDocuments_PurchaseOrderDocumentId", x => x.PurchaseOrderDocumentId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDocuments_AttachmentCollection",
                        column: x => x.AttachmentCollectionId,
                        principalSchema: "Attachment",
                        principalTable: "AttachmentCollection",
                        principalColumn: "AttachmentCollectionID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDocuments_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDocuments_PurchaseOrders",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Orders",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDocuments_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDocuments_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDocuments_AttachmentCollectionId",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                column: "AttachmentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDocuments_CreatedBy",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDocuments_PO_DocumentType_IsActive",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                columns: new[] { "PurchaseOrderId", "DocumentType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDocuments_PurchaseOrderId",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDocuments_UpdatedBy",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDocuments_VerifiedBy",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                column: "VerifiedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderDocuments",
                schema: "Orders");
        }
    }
}
