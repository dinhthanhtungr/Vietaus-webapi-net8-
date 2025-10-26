using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateWareHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WareHouseRequest",
                schema: "Warehouse",
                columns: table => new
                {
                    RequestCode = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    RequestedBy = table.Column<string>(type: "citext", nullable: false),
                    ReqStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<string>(type: "citext", nullable: false),
                    RequestName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReqType = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WareHouseRequest__3214EC07A98DEC4E", x => x.RequestCode);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseRequestDetail",
                schema: "Warehouse",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    RequestCode = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductCode = table.Column<string>(type: "text", nullable: true),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    LotNumber = table.Column<string>(type: "text", nullable: true),
                    WeightKg = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true),
                    BagNumber = table.Column<int>(type: "integer", nullable: true),
                    StockStatus = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WareHouseRequestDetail__3214EC07A98DEC4E", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_WareHouseRequestDetail_RequestCode",
                        column: x => x.RequestCode,
                        principalSchema: "Warehouse",
                        principalTable: "WareHouseRequest",
                        principalColumn: "RequestCode");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseRequestDetail_RequestCode",
                schema: "Warehouse",
                table: "WareHouseRequestDetail",
                column: "RequestCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WareHouseRequestDetail",
                schema: "Warehouse");

            migrationBuilder.DropTable(
                name: "WareHouseRequest",
                schema: "Warehouse");
        }
    }
}
