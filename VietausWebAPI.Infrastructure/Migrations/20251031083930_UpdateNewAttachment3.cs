using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewAttachment3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderAttachments",
                schema: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderAttachments",
                schema: "Orders",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: true),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: false),
                    StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderAttachment__B49D545E8C296524", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_OrderAttachment_CreatedBy",
                        column: x => x.CreateBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_OrderAttachment_MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderAttachments_CreateBy",
                schema: "Orders",
                table: "OrderAttachments",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAttachments_MerchandiseOrderId",
                schema: "Orders",
                table: "OrderAttachments",
                column: "MerchandiseOrderId");
        }
    }
}
