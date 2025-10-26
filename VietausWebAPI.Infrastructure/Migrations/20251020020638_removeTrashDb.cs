using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeTrashDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalHistory_Material_data");

            migrationBuilder.DropTable(
                name: "InventoryReceipts_Material_data");

            migrationBuilder.DropTable(
                name: "Machines_Common_data");

            migrationBuilder.DropTable(
                name: "MaterialsSuppliers_material_data");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails_material_data");

            migrationBuilder.DropTable(
                name: "PurchaseOrderStatusHistory_material_data");

            migrationBuilder.DropTable(
                name: "Groups_Common_data");

            migrationBuilder.DropTable(
                name: "PriceHistory_material_data");

            migrationBuilder.DropTable(
                name: "RequestDetail_Material_data");

            migrationBuilder.DropTable(
                name: "PurchaseOrders_material_data");

            migrationBuilder.DropTable(
                name: "Materials_material_data");

            migrationBuilder.DropTable(
                name: "SupplyRequests_Material_data");

            migrationBuilder.DropTable(
                name: "Suppliers_material_data");

            migrationBuilder.DropTable(
                name: "MaterialGroups_Material_data");

            migrationBuilder.DropTable(
                name: "Employees_Common_data");

            migrationBuilder.DropTable(
                name: "ApprovalLevels_Common_data");

            migrationBuilder.DropTable(
                name: "Parts_Common_data");

            migrationBuilder.DropColumn(
                name: "LinkedIssueId",
                schema: "Warehouse",
                table: "WarehouseTempStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkedIssueId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApprovalLevels_Common_data",
                columns: table => new
                {
                    LevelID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "citext", nullable: true),
                    LevelName = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Approval__09F03C061CA120B7", x => x.LevelID);
                });

            migrationBuilder.CreateTable(
                name: "Groups_Common_data",
                columns: table => new
                {
                    GroupID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "citext", nullable: true),
                    GroupName = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Groups__149AF30A6FD59030", x => x.GroupID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialGroups_Material_data",
                columns: table => new
                {
                    MaterialGroupID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Detail = table.Column<string>(type: "citext", nullable: true),
                    externalId = table.Column<string>(type: "citext", nullable: true),
                    MaterialGroupName = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__E20265FD37B632C7", x => x.MaterialGroupID);
                });

            migrationBuilder.CreateTable(
                name: "Parts_Common_data",
                columns: table => new
                {
                    PartID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    PartName = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Parts__7C3F0D3012CD1E19", x => x.PartID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers_material_data",
                columns: table => new
                {
                    supplierId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    externalId = table.Column<string>(type: "citext", nullable: true),
                    Name = table.Column<string>(type: "citext", nullable: true),
                    phone = table.Column<string>(type: "citext", nullable: true),
                    regNo = table.Column<string>(type: "citext", nullable: true),
                    taxNo = table.Column<string>(type: "citext", nullable: true),
                    website = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__DB8E62ED69C94CF3", x => x.supplierId);
                });

            migrationBuilder.CreateTable(
                name: "Employees_Common_data",
                columns: table => new
                {
                    EmployeeID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    LevelID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PartID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Address = table.Column<string>(type: "citext", nullable: true),
                    DateHired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Email = table.Column<string>(type: "citext", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FullName = table.Column<string>(type: "citext", nullable: false),
                    Gender = table.Column<string>(type: "citext", nullable: true),
                    Identifier = table.Column<string>(type: "citext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "citext", nullable: true),
                    Status = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__7AD04FF184E31508", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK__Employees__Level__693CA210",
                        column: x => x.LevelID,
                        principalTable: "ApprovalLevels_Common_data",
                        principalColumn: "LevelID");
                    table.ForeignKey(
                        name: "FK__Employees__PartI__68487DD7",
                        column: x => x.PartID,
                        principalTable: "Parts_Common_data",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "Machines_Common_data",
                columns: table => new
                {
                    MachineID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    GroupID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PartID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "citext", nullable: true),
                    Factory = table.Column<string>(type: "citext", nullable: true, defaultValueSql: "'Tam Phước'::character varying"),
                    GroupMachine = table.Column<string>(type: "citext", nullable: true),
                    MachineName = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Machines__5A97603FADC7D2ED", x => x.MachineID);
                    table.ForeignKey(
                        name: "FK__Machines__GroupI__6477ECF3",
                        column: x => x.GroupID,
                        principalTable: "Groups_Common_data",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK__Machines__PartID__656C112C",
                        column: x => x.PartID,
                        principalTable: "Parts_Common_data",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "Materials_material_data",
                columns: table => new
                {
                    materialId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    EmployeeID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    MaterialGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    externalId = table.Column<string>(type: "citext", nullable: true),
                    Name = table.Column<string>(type: "citext", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    Unit = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__99B653FDEEB02A00", x => x.materialId);
                    table.ForeignKey(
                        name: "FK__Materials__Emplo__6DCC4D03",
                        column: x => x.EmployeeID,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Materials__Mater__6CD828CA",
                        column: x => x.MaterialGroupId,
                        principalTable: "MaterialGroups_Material_data",
                        principalColumn: "MaterialGroupID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders_material_data",
                columns: table => new
                {
                    POID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    EmployeeId = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContactName = table.Column<string>(type: "citext", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "citext", nullable: true),
                    DeliveryContact = table.Column<string>(type: "citext", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    InvoiceNote = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "citext", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Packaging = table.Column<string>(type: "citext", nullable: true),
                    PaymentTerm = table.Column<string>(type: "citext", nullable: true),
                    POCode = table.Column<string>(type: "citext", nullable: true),
                    RequiredDocuments = table.Column<string>(type: "citext", nullable: true),
                    RequiredDocuments_Eng = table.Column<string>(type: "citext", nullable: true),
                    status = table.Column<string>(type: "citext", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    VendorAddress = table.Column<string>(type: "citext", nullable: true),
                    VendorPhone = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__5F02A2F4EB754C0F", x => x.POID);
                    table.ForeignKey(
                        name: "FK__PurchaseO__Emplo__320C68B7",
                        column: x => x.EmployeeId,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__PurchaseO__Suppl__33008CF0",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers_material_data",
                        principalColumn: "supplierId");
                });

            migrationBuilder.CreateTable(
                name: "SupplyRequests_Material_data",
                columns: table => new
                {
                    RequestID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    EmployeeID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Note = table.Column<string>(type: "citext", nullable: true),
                    NoteCancel = table.Column<string>(type: "citext", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RequestStatus = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupplyRe__33A8519AEED4B83E", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK__SupplyReq__Emplo__6C190EBB",
                        column: x => x.EmployeeID,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory_material_data",
                columns: table => new
                {
                    priceHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    supplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    updatedBy = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    currency = table.Column<string>(type: "citext", nullable: true),
                    newPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    oldPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    reason = table.Column<string>(type: "citext", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PriceHis__77D1486CB71DF1F1", x => x.priceHistoryId);
                    table.ForeignKey(
                        name: "FK__PriceHist__mater__793DFFAF",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK__PriceHist__suppl__2D47B39A",
                        column: x => x.supplierId,
                        principalTable: "Suppliers_material_data",
                        principalColumn: "supplierId");
                    table.ForeignKey(
                        name: "FK__PriceHist__updat__2E3BD7D3",
                        column: x => x.updatedBy,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderStatusHistory_material_data",
                columns: table => new
                {
                    StatusHistoryId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    POID = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmployeeId = table.Column<string>(type: "citext", nullable: true),
                    note = table.Column<string>(type: "citext", nullable: true),
                    StatusFrom = table.Column<string>(type: "citext", nullable: true),
                    StatusTo = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__DB9734917B73E8D7", x => x.StatusHistoryId);
                    table.ForeignKey(
                        name: "FK__PurchaseOr__POID__34E8D562",
                        column: x => x.POID,
                        principalTable: "PurchaseOrders_material_data",
                        principalColumn: "POID");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalHistory_Material_data",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    RequestID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Note = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Approval__3214EC27EC140479", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_Request",
                        column: x => x.RequestID,
                        principalTable: "SupplyRequests_Material_data",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "RequestDetail_Material_data",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Note = table.Column<string>(type: "citext", nullable: true),
                    PurchasedQuantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    ReceivedQuantity = table.Column<int>(type: "integer", nullable: true),
                    RequestedQuantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RequestD__135C314D44B7445B", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_RequestDetail_Material",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK_RequestDetail_Request",
                        column: x => x.RequestID,
                        principalTable: "SupplyRequests_Material_data",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "MaterialsSuppliers_material_data",
                columns: table => new
                {
                    Materials_SuppliersId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    materialId = table.Column<Guid>(type: "uuid", nullable: true),
                    priceHistoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    supplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    currency = table.Column<string>(type: "citext", nullable: true),
                    currentPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    isPreferred = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    minDeliveryDays = table.Column<int>(type: "integer", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__4F13EDBB69530C3D", x => x.Materials_SuppliersId);
                    table.ForeignKey(
                        name: "FK__Materials__mater__7FEAFD3E",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK__Materials__price__2A6B46EF",
                        column: x => x.priceHistoryId,
                        principalTable: "PriceHistory_material_data",
                        principalColumn: "priceHistoryId");
                    table.ForeignKey(
                        name: "FK__Materials__suppl__2B5F6B28",
                        column: x => x.supplierId,
                        principalTable: "Suppliers_material_data",
                        principalColumn: "supplierId");
                });

            migrationBuilder.CreateTable(
                name: "InventoryReceipts_Material_data",
                columns: table => new
                {
                    ReceiptID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetailID = table.Column<int>(type: "integer", nullable: true),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestID = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    ExportedQty = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "citext", nullable: true),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReceiptQty = table.Column<int>(type: "integer", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventor__CC08C400857F0DFA", x => x.ReceiptID);
                    table.ForeignKey(
                        name: "FK_InventoryReceipts_Material",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK_InventoryReceipts_Request",
                        column: x => x.RequestID,
                        principalTable: "SupplyRequests_Material_data",
                        principalColumn: "RequestID");
                    table.ForeignKey(
                        name: "FK_PO_Detail_InventoryReceipt",
                        column: x => x.DetailID,
                        principalTable: "RequestDetail_Material_data",
                        principalColumn: "DetailID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails_material_data",
                columns: table => new
                {
                    PODetailId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DetailID = table.Column<int>(type: "integer", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    POID = table.Column<Guid>(type: "uuid", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    note = table.Column<string>(type: "citext", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__4EB47B3EB8D4CA48", x => x.PODetailId);
                    table.ForeignKey(
                        name: "FK_PO_Detail_RequestDetail",
                        column: x => x.DetailID,
                        principalTable: "RequestDetail_Material_data",
                        principalColumn: "DetailID");
                    table.ForeignKey(
                        name: "FK__PurchaseO__Mater__10216507",
                        column: x => x.MaterialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK__PurchaseOr__POID__30242045",
                        column: x => x.POID,
                        principalTable: "PurchaseOrders_material_data",
                        principalColumn: "POID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_Material_data_EmployeeID",
                table: "ApprovalHistory_Material_data",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_Material_data_RequestID",
                table: "ApprovalHistory_Material_data",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Common_data_LevelID",
                table: "Employees_Common_data",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Common_data_PartID",
                table: "Employees_Common_data",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_Material_data_DetailID",
                table: "InventoryReceipts_Material_data",
                column: "DetailID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_Material_data_materialId",
                table: "InventoryReceipts_Material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_Material_data_RequestID",
                table: "InventoryReceipts_Material_data",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Common_data_GroupID",
                table: "Machines_Common_data",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Common_data_PartID",
                table: "Machines_Common_data",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_material_data_EmployeeID",
                table: "Materials_material_data",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_material_data_MaterialGroupId",
                table: "Materials_material_data",
                column: "MaterialGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSuppliers_material_data_materialId",
                table: "MaterialsSuppliers_material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSuppliers_material_data_priceHistoryId",
                table: "MaterialsSuppliers_material_data",
                column: "priceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSuppliers_material_data_supplierId",
                table: "MaterialsSuppliers_material_data",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_material_data_materialId",
                table: "PriceHistory_material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_material_data_supplierId",
                table: "PriceHistory_material_data",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_material_data_updatedBy",
                table: "PriceHistory_material_data",
                column: "updatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_material_data_DetailID",
                table: "PurchaseOrderDetails_material_data",
                column: "DetailID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_material_data_MaterialId",
                table: "PurchaseOrderDetails_material_data",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_material_data_POID",
                table: "PurchaseOrderDetails_material_data",
                column: "POID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_material_data_EmployeeId",
                table: "PurchaseOrders_material_data",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_material_data_SupplierId",
                table: "PurchaseOrders_material_data",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderStatusHistory_material_data_POID",
                table: "PurchaseOrderStatusHistory_material_data",
                column: "POID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_Material_data_materialId",
                table: "RequestDetail_Material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_Material_data_RequestID",
                table: "RequestDetail_Material_data",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_Material_data_EmployeeID",
                table: "SupplyRequests_Material_data",
                column: "EmployeeID");
        }
    }
}
