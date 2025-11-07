using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewAttachment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrderDetails_AttachmentCollection_AttachmentColl~",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_MerchandiseOrderDetails_AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderDetails_AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "AttachmentCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_AttachmentCollection_AttachmentColl~",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
