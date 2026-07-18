using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMaterialAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Materials_AttachmentCollection",
                schema: "Material",
                table: "Materials",
                column: "AttachmentCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_AttachmentCollection",
                schema: "Material",
                table: "Materials",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_AttachmentCollection",
                schema: "Material",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_AttachmentCollection",
                schema: "Material",
                table: "Materials");
        }
    }
}
