using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class uapdateMerchandiseOrderImportExpectedDeliveryDate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OrderAttachment",
                newName: "OrderAttachments",
                newSchema: "Orders");

            migrationBuilder.RenameIndex(
                name: "IX_OrderAttachment_MerchandiseOrderId",
                schema: "Orders",
                table: "OrderAttachments",
                newName: "IX_OrderAttachments_MerchandiseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderAttachment_CreateBy",
                schema: "Orders",
                table: "OrderAttachments",
                newName: "IX_OrderAttachments_CreateBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OrderAttachments",
                schema: "Orders",
                newName: "OrderAttachment");

            migrationBuilder.RenameIndex(
                name: "IX_OrderAttachments_MerchandiseOrderId",
                table: "OrderAttachment",
                newName: "IX_OrderAttachment_MerchandiseOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderAttachments_CreateBy",
                table: "OrderAttachment",
                newName: "IX_OrderAttachment_CreateBy");
        }
    }
}
