using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatebucketForSampleRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleRequestImages",
                schema: "SampleRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentCollectionId",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_AttachmentCollection",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "AttachmentCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SampleRequests_AttachmentCollection_AttachmentCollectionId",
                schema: "SampleRequests",
                table: "SampleRequests",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SampleRequests_AttachmentCollection_AttachmentCollectionId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropIndex(
                name: "IX_SampleRequests_AttachmentCollection",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "AttachmentCollectionId",
                schema: "SampleRequests",
                table: "SampleRequests");

            migrationBuilder.CreateTable(
                name: "SampleRequestImages",
                schema: "SampleRequests",
                columns: table => new
                {
                    SampleRequestImageId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SampleRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FileUrl = table.Column<string>(type: "text", nullable: false),
                    IsCover = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SampleRequestImage__3214EC07A98DEC4E", x => x.SampleRequestImageId);
                    table.ForeignKey(
                        name: "FK_SampleRequestImages_SampleRequests_SampleRequestId",
                        column: x => x.SampleRequestId,
                        principalSchema: "SampleRequests",
                        principalTable: "SampleRequests",
                        principalColumn: "SampleRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequestImages_SampleRequestId",
                schema: "SampleRequests",
                table: "SampleRequestImages",
                column: "SampleRequestId");
        }
    }
}
