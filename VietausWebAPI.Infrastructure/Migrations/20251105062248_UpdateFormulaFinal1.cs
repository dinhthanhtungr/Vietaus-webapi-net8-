using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormulaFinal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductChangedHistory",
                schema: "SampleRequests");

            migrationBuilder.DropColumn(
                name: "SendByNameSnapshot",
                schema: "SampleRequests",
                table: "SampleRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendByNameSnapshot",
                schema: "SampleRequests",
                table: "SampleRequests",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductChangedHistory",
                schema: "SampleRequests",
                columns: table => new
                {
                    ProductChangedHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangeNote = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChangeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FieldChanged = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    NewValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OldValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductC__A793B6CA9FB36DED", x => x.ProductChangedHistoryId);
                    table.ForeignKey(
                        name: "FK_ProductHistory_ChangedBy",
                        column: x => x.ChangedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ProductHistory_Product",
                        column: x => x.ProductId,
                        principalSchema: "SampleRequests",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductChangedHistory_ChangedBy",
                schema: "SampleRequests",
                table: "ProductChangedHistory",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChangedHistory_ProductId",
                schema: "SampleRequests",
                table: "ProductChangedHistory",
                column: "ProductId");
        }
    }
}
