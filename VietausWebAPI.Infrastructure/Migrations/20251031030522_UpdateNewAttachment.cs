using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNewAttachment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Attachment");

            migrationBuilder.EnsureSchema(
                name: "Manufacturing");

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AttachmentCollection",
                schema: "Attachment",
                columns: table => new
                {
                    AttachmentCollectionID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AttachmentCollection__B3C4DAF6D3F2E2B3", x => x.AttachmentCollectionID);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                schema: "Manufacturing",
                columns: table => new
                {
                    WarehouseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    WarehouseName = table.Column<string>(type: "text", nullable: false),
                    WarehouseExternalId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Warehouses__3214EC07A98DEC4E", x => x.WarehouseId);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentModel",
                schema: "Attachment",
                columns: table => new
                {
                    AttachmentModelID = table.Column<Guid>(type: "uuid", nullable: false),
                    AttachmentCollectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    StoragePath = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ContentHash = table.Column<string>(type: "text", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AttachmentModel__B3C4DAF6D3F2E2B3", x => x.AttachmentModelID);
                    table.ForeignKey(
                        name: "FK_AttachmentModel_AttachmentCollection_AttachmentCollectionId",
                        column: x => x.AttachmentCollectionId,
                        principalSchema: "Attachment",
                        principalTable: "AttachmentCollection",
                        principalColumn: "AttachmentCollectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttachmentModel_CreatedBy",
                        column: x => x.CreateBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_AttachmentModel_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                schema: "Manufacturing",
                columns: table => new
                {
                    ZoneId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    ZoneName = table.Column<string>(type: "text", nullable: false),
                    ZoneExternalId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Zone__3214EC07A98DEC4E", x => x.ZoneId);
                    table.ForeignKey(
                        name: "FK_Zones_Warehouse",
                        column: x => x.WarehouseId,
                        principalSchema: "Manufacturing",
                        principalTable: "Warehouses",
                        principalColumn: "WarehouseId");
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                schema: "Manufacturing",
                columns: table => new
                {
                    RankId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ZoneId = table.Column<int>(type: "integer", nullable: false),
                    RankName = table.Column<string>(type: "text", nullable: false),
                    RankExternalId = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Slots",
                schema: "Manufacturing",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    RankId = table.Column<int>(type: "integer", nullable: false),
                    SlotName = table.Column<string>(type: "text", nullable: false),
                    SlotExternalId = table.Column<string>(type: "text", nullable: false),
                    CapacityQty = table.Column<int>(type: "integer", nullable: false),
                    CountToCapacity = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Slots__3214EC07A98DEC4E", x => x.SlotId);
                    table.ForeignKey(
                        name: "FK_Slots_Rank",
                        column: x => x.RankId,
                        principalSchema: "Manufacturing",
                        principalTable: "Ranks",
                        principalColumn: "RankId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_AttachmentCollection",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "AttachmentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderDetails_AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "AttachmentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentModel_AttachmentCollectionId",
                schema: "Attachment",
                table: "AttachmentModel",
                column: "AttachmentCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentModel_CreateBy",
                schema: "Attachment",
                table: "AttachmentModel",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentModel_EmployeeId",
                schema: "Attachment",
                table: "AttachmentModel",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_ZoneId",
                schema: "Manufacturing",
                table: "Ranks",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_RankId",
                schema: "Manufacturing",
                table: "Slots",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_WarehouseId",
                schema: "Manufacturing",
                table: "Zones",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrderDetails_AttachmentCollection_AttachmentColl~",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "AttachmentCollectionId",
                principalSchema: "Attachment",
                principalTable: "AttachmentCollection",
                principalColumn: "AttachmentCollectionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_AttachmentCollection_AttachmentCollection~",
                schema: "Orders",
                table: "MerchandiseOrders",
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
                name: "FK_MerchandiseOrderDetails_AttachmentCollection_AttachmentColl~",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_AttachmentCollection_AttachmentCollection~",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropTable(
                name: "AttachmentModel",
                schema: "Attachment");

            migrationBuilder.DropTable(
                name: "Slots",
                schema: "Manufacturing");

            migrationBuilder.DropTable(
                name: "AttachmentCollection",
                schema: "Attachment");

            migrationBuilder.DropTable(
                name: "Ranks",
                schema: "Manufacturing");

            migrationBuilder.DropTable(
                name: "Zones",
                schema: "Manufacturing");

            migrationBuilder.DropTable(
                name: "Warehouses",
                schema: "Manufacturing");

            migrationBuilder.DropIndex(
                name: "IX_Order_AttachmentCollection",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "IX_MerchandiseOrderDetails_AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");

            migrationBuilder.DropColumn(
                name: "AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropColumn(
                name: "AttachmentCollectionId",
                schema: "Orders",
                table: "MerchandiseOrderDetails");
        }
    }
}
