using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInternalMessage3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalMessageReferences",
                schema: "InternalMail",
                columns: table => new
                {
                    InternalMessageReferenceId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    InternalMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedType = table.Column<int>(type: "integer", nullable: false),
                    RelatedId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelatedExternalId = table.Column<string>(type: "citext", nullable: true),
                    RelatedNameSnapshot = table.Column<string>(type: "citext", nullable: true),
                    SnapshotJson = table.Column<string>(type: "jsonb", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalMessageReferences_Id", x => x.InternalMessageReferenceId);
                    table.ForeignKey(
                        name: "FK_InternalMessageReferences_Message",
                        column: x => x.InternalMessageId,
                        principalSchema: "InternalMail",
                        principalTable: "InternalMessages",
                        principalColumn: "InternalMessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternalMessageReferences_Related",
                schema: "InternalMail",
                table: "InternalMessageReferences",
                columns: new[] { "RelatedType", "RelatedId" });

            migrationBuilder.CreateIndex(
                name: "UX_InternalMessageReferences_Message_Primary",
                schema: "InternalMail",
                table: "InternalMessageReferences",
                column: "InternalMessageId",
                unique: true,
                filter: "\"IsPrimary\" = true");

            migrationBuilder.CreateIndex(
                name: "UX_InternalMessageReferences_Message_Related",
                schema: "InternalMail",
                table: "InternalMessageReferences",
                columns: new[] { "InternalMessageId", "RelatedType", "RelatedId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternalMessageReferences",
                schema: "InternalMail");
        }
    }
}
