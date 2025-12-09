using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSampleRequestId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttachmentCollection_Materials_MaterialId",
                schema: "Attachment",
                table: "AttachmentCollection");

            migrationBuilder.DropIndex(
                name: "IX_AttachmentCollection_MaterialId",
                schema: "Attachment",
                table: "AttachmentCollection");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                schema: "Attachment",
                table: "AttachmentCollection");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MaterialId",
                schema: "Attachment",
                table: "AttachmentCollection",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentCollection_MaterialId",
                schema: "Attachment",
                table: "AttachmentCollection",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttachmentCollection_Materials_MaterialId",
                schema: "Attachment",
                table: "AttachmentCollection",
                column: "MaterialId",
                principalSchema: "Material",
                principalTable: "Materials",
                principalColumn: "MaterialId");
        }
    }
}
