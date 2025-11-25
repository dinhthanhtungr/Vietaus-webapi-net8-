using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_group_tariff_map_groups_GroupId1",
                schema: "energy",
                table: "group_tariff_map");

            migrationBuilder.DropForeignKey(
                name: "FK_group_tariff_map_tariffs_TariffId1",
                schema: "energy",
                table: "group_tariff_map");

            migrationBuilder.DropIndex(
                name: "IX_group_tariff_map_GroupId1",
                schema: "energy",
                table: "group_tariff_map");

            migrationBuilder.DropIndex(
                name: "IX_group_tariff_map_TariffId1",
                schema: "energy",
                table: "group_tariff_map");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                schema: "energy",
                table: "group_tariff_map");

            migrationBuilder.DropColumn(
                name: "TariffId1",
                schema: "energy",
                table: "group_tariff_map");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "GroupId1",
                schema: "energy",
                table: "group_tariff_map",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TariffId1",
                schema: "energy",
                table: "group_tariff_map",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_tariff_map_GroupId1",
                schema: "energy",
                table: "group_tariff_map",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_group_tariff_map_TariffId1",
                schema: "energy",
                table: "group_tariff_map",
                column: "TariffId1");

            migrationBuilder.AddForeignKey(
                name: "FK_group_tariff_map_groups_GroupId1",
                schema: "energy",
                table: "group_tariff_map",
                column: "GroupId1",
                principalSchema: "energy",
                principalTable: "groups",
                principalColumn: "group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_group_tariff_map_tariffs_TariffId1",
                schema: "energy",
                table: "group_tariff_map",
                column: "TariffId1",
                principalSchema: "energy",
                principalTable: "tariffs",
                principalColumn: "tariff_id");
        }
    }
}
