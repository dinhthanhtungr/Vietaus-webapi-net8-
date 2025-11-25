using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewManufacuringTable9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_areas_AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropForeignKey(
                name: "FK_incident_hdr_equipment_EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropIndex(
                name: "IX_incident_hdr_EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr");

            migrationBuilder.DropColumn(
                name: "EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr",
                column: "AreaMROAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_incident_hdr_EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr",
                column: "EquipmentMROEquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_areas_AreaMROAreaId",
                schema: "mro",
                table: "incident_hdr",
                column: "AreaMROAreaId",
                principalSchema: "mro",
                principalTable: "areas",
                principalColumn: "area_id");

            migrationBuilder.AddForeignKey(
                name: "FK_incident_hdr_equipment_EquipmentMROEquipmentId",
                schema: "mro",
                table: "incident_hdr",
                column: "EquipmentMROEquipmentId",
                principalSchema: "mro",
                principalTable: "equipment",
                principalColumn: "equipment_id");
        }
    }
}
