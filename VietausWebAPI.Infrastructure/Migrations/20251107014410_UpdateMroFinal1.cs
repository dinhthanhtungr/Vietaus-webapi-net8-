using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMroFinal1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slots_Rank",
                schema: "Manufacturing",
                table: "Slots");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Warehouse",
                schema: "Manufacturing",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "Ranks",
                schema: "Manufacturing");

            migrationBuilder.EnsureSchema(
                name: "mro");

            migrationBuilder.RenameTable(
                name: "Zones",
                schema: "Manufacturing",
                newName: "zones",
                newSchema: "mro");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                schema: "Manufacturing",
                newName: "warehouses",
                newSchema: "mro");

            migrationBuilder.RenameTable(
                name: "Slots",
                schema: "Manufacturing",
                newName: "slots",
                newSchema: "mro");

            migrationBuilder.RenameColumn(
                name: "ZoneName",
                schema: "mro",
                table: "zones",
                newName: "zone_name");

            migrationBuilder.RenameColumn(
                name: "ZoneExternalId",
                schema: "mro",
                table: "zones",
                newName: "zone_external_id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                schema: "mro",
                table: "zones",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "ZoneId",
                schema: "mro",
                table: "zones",
                newName: "zone_id");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_WarehouseId",
                schema: "mro",
                table: "zones",
                newName: "ix_zones_warehouse_id");

            migrationBuilder.RenameColumn(
                name: "WarehouseName",
                schema: "mro",
                table: "warehouses",
                newName: "warehouse_name");

            migrationBuilder.RenameColumn(
                name: "WarehouseExternalId",
                schema: "mro",
                table: "warehouses",
                newName: "warehouse_external_id");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                schema: "mro",
                table: "warehouses",
                newName: "warehouse_id");

            migrationBuilder.RenameColumn(
                name: "SlotName",
                schema: "mro",
                table: "slots",
                newName: "slot_name");

            migrationBuilder.RenameColumn(
                name: "SlotExternalId",
                schema: "mro",
                table: "slots",
                newName: "slot_external_id");

            migrationBuilder.RenameColumn(
                name: "CountToCapacity",
                schema: "mro",
                table: "slots",
                newName: "count_to_capacity");

            migrationBuilder.RenameColumn(
                name: "CapacityQty",
                schema: "mro",
                table: "slots",
                newName: "capacity_qty");

            migrationBuilder.RenameColumn(
                name: "SlotId",
                schema: "mro",
                table: "slots",
                newName: "slot_id");

            migrationBuilder.RenameColumn(
                name: "RankId",
                schema: "mro",
                table: "slots",
                newName: "rack_id");

            migrationBuilder.RenameIndex(
                name: "IX_Slots_RankId",
                schema: "mro",
                table: "slots",
                newName: "ix_slots_rack_id");

            migrationBuilder.AlterColumn<bool>(
                name: "count_to_capacity",
                schema: "mro",
                table: "slots",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<int>(
                name: "capacity_qty",
                schema: "mro",
                table: "slots",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "racks",
                schema: "mro",
                columns: table => new
                {
                    rack_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    zone_id = table.Column<int>(type: "integer", nullable: false),
                    rack_name = table.Column<string>(type: "text", nullable: false),
                    rack_external_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ranks__3214EC07A98DEC4E", x => x.rack_id);
                    table.ForeignKey(
                        name: "fk_racks_zone_id",
                        column: x => x.zone_id,
                        principalSchema: "mro",
                        principalTable: "zones",
                        principalColumn: "zone_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ux_zones_warehouse_external",
                schema: "mro",
                table: "zones",
                columns: new[] { "warehouse_id", "zone_external_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_warehouses_external_id",
                schema: "mro",
                table: "warehouses",
                column: "warehouse_external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_slots_rack_external",
                schema: "mro",
                table: "slots",
                columns: new[] { "rack_id", "slot_external_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_racks_zone_id",
                schema: "mro",
                table: "racks",
                column: "zone_id");

            migrationBuilder.CreateIndex(
                name: "ux_racks_zone_external",
                schema: "mro",
                table: "racks",
                columns: new[] { "zone_id", "rack_external_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_slots_rack_id",
                schema: "mro",
                table: "slots",
                column: "rack_id",
                principalSchema: "mro",
                principalTable: "racks",
                principalColumn: "rack_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_zones_warehouse_id",
                schema: "mro",
                table: "zones",
                column: "warehouse_id",
                principalSchema: "mro",
                principalTable: "warehouses",
                principalColumn: "warehouse_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_slots_rack_id",
                schema: "mro",
                table: "slots");

            migrationBuilder.DropForeignKey(
                name: "fk_zones_warehouse_id",
                schema: "mro",
                table: "zones");

            migrationBuilder.DropTable(
                name: "racks",
                schema: "mro");

            migrationBuilder.DropIndex(
                name: "ux_zones_warehouse_external",
                schema: "mro",
                table: "zones");

            migrationBuilder.DropIndex(
                name: "ix_warehouses_external_id",
                schema: "mro",
                table: "warehouses");

            migrationBuilder.DropIndex(
                name: "ux_slots_rack_external",
                schema: "mro",
                table: "slots");

            migrationBuilder.EnsureSchema(
                name: "Manufacturing");

            migrationBuilder.RenameTable(
                name: "zones",
                schema: "mro",
                newName: "Zones",
                newSchema: "Manufacturing");

            migrationBuilder.RenameTable(
                name: "warehouses",
                schema: "mro",
                newName: "Warehouses",
                newSchema: "Manufacturing");

            migrationBuilder.RenameTable(
                name: "slots",
                schema: "mro",
                newName: "Slots",
                newSchema: "Manufacturing");

            migrationBuilder.RenameColumn(
                name: "zone_name",
                schema: "Manufacturing",
                table: "Zones",
                newName: "ZoneName");

            migrationBuilder.RenameColumn(
                name: "zone_external_id",
                schema: "Manufacturing",
                table: "Zones",
                newName: "ZoneExternalId");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                schema: "Manufacturing",
                table: "Zones",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "zone_id",
                schema: "Manufacturing",
                table: "Zones",
                newName: "ZoneId");

            migrationBuilder.RenameIndex(
                name: "ix_zones_warehouse_id",
                schema: "Manufacturing",
                table: "Zones",
                newName: "IX_Zones_WarehouseId");

            migrationBuilder.RenameColumn(
                name: "warehouse_name",
                schema: "Manufacturing",
                table: "Warehouses",
                newName: "WarehouseName");

            migrationBuilder.RenameColumn(
                name: "warehouse_external_id",
                schema: "Manufacturing",
                table: "Warehouses",
                newName: "WarehouseExternalId");

            migrationBuilder.RenameColumn(
                name: "warehouse_id",
                schema: "Manufacturing",
                table: "Warehouses",
                newName: "WarehouseId");

            migrationBuilder.RenameColumn(
                name: "slot_name",
                schema: "Manufacturing",
                table: "Slots",
                newName: "SlotName");

            migrationBuilder.RenameColumn(
                name: "slot_external_id",
                schema: "Manufacturing",
                table: "Slots",
                newName: "SlotExternalId");

            migrationBuilder.RenameColumn(
                name: "count_to_capacity",
                schema: "Manufacturing",
                table: "Slots",
                newName: "CountToCapacity");

            migrationBuilder.RenameColumn(
                name: "capacity_qty",
                schema: "Manufacturing",
                table: "Slots",
                newName: "CapacityQty");

            migrationBuilder.RenameColumn(
                name: "slot_id",
                schema: "Manufacturing",
                table: "Slots",
                newName: "SlotId");

            migrationBuilder.RenameColumn(
                name: "rack_id",
                schema: "Manufacturing",
                table: "Slots",
                newName: "RankId");

            migrationBuilder.RenameIndex(
                name: "ix_slots_rack_id",
                schema: "Manufacturing",
                table: "Slots",
                newName: "IX_Slots_RankId");

            migrationBuilder.AlterColumn<bool>(
                name: "CountToCapacity",
                schema: "Manufacturing",
                table: "Slots",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "CapacityQty",
                schema: "Manufacturing",
                table: "Slots",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ranks",
                schema: "Manufacturing",
                columns: table => new
                {
                    RankId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ZoneId = table.Column<int>(type: "integer", nullable: false),
                    RankExternalId = table.Column<string>(type: "text", nullable: false),
                    RankName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ranks__3214EC07A98DEC4E", x => x.RankId);
                    table.ForeignKey(
                        name: "FK_Ranks_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalSchema: "Manufacturing",
                        principalTable: "Zones",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_ZoneId",
                schema: "Manufacturing",
                table: "Ranks",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Slots_Rank",
                schema: "Manufacturing",
                table: "Slots",
                column: "RankId",
                principalSchema: "Manufacturing",
                principalTable: "Ranks",
                principalColumn: "RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Warehouse",
                schema: "Manufacturing",
                table: "Zones",
                column: "WarehouseId",
                principalSchema: "Manufacturing",
                principalTable: "Warehouses",
                principalColumn: "WarehouseId");
        }
    }
}
