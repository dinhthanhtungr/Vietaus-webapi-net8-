using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewTableSampleRequestImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SampleRequestImages",
                schema: "labs",
                columns: table => new
                {
                    SampleRequestImageId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SampleRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FileType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
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
                        principalSchema: "labs",
                        principalTable: "SampleRequests",
                        principalColumn: "SampleRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequestImages_SampleRequestId",
                schema: "labs",
                table: "SampleRequestImages",
                column: "SampleRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SampleRequestImages",
                schema: "labs");
        }
    }
}
