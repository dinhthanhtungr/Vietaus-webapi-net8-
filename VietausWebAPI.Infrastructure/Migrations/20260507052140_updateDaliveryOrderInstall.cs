using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDaliveryOrderInstall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneSnapshot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentDeadline",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeliveryDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryTripId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryWindowFrom",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryWindowTo",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveredSuccessfully",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDeliveryDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityNumber",
                schema: "DeliveryOrder",
                table: "DelivererInfor",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LicenseExpiryDate",
                schema: "DeliveryOrder",
                table: "DelivererInfor",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                schema: "DeliveryOrder",
                table: "DelivererInfor",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryProofOfDeliveries",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DeliveryOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ReceiverPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SignatureUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    PhotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric(18,8)", precision: 18, scale: 8, nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric(18,8)", precision: 18, scale: 8, nullable: true),
                    ConfirmedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ConfirmedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryProofOfDeliveries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeliveryProofOfDeliveries_DeliveryOrder",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryVehicles",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    PlateNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VehicleType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MaxLoadKg = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true),
                    MaxVolumeM3 = table.Column<decimal>(type: "numeric(18,3)", precision: 18, scale: 3, nullable: true),
                    IsInternal = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    OwnerName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryVehicles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTrips",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "citext", nullable: false),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    TripDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PlannedStartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PlannedEndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualStartTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: true),
                    DriverId = table.Column<Guid>(type: "uuid", nullable: true),
                    DispatcherId = table.Column<Guid>(type: "uuid", nullable: true),
                    RouteCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTrips", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeliveryTrips_Driver",
                        column: x => x.DriverId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DelivererInfor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DeliveryTrips_Vehicle",
                        column: x => x.VehicleId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryVehicles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryExpenses",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DeliveryTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpenseType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EvidenceUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryExpenses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeliveryExpenses_DeliveryTrip",
                        column: x => x.DeliveryTripId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryTrips",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStops",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    DeliveryTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    SequenceNo = table.Column<int>(type: "integer", nullable: false),
                    StopType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    ContactName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlannedArrivalTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PlannedDepartureTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualArrivalTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualDepartureTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    FailureReason = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStops", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DeliveryStops_DeliveryOrder",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DeliveryStops_DeliveryTrip",
                        column: x => x.DeliveryTripId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryTrips",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTripOrders",
                schema: "DeliveryOrder",
                columns: table => new
                {
                    DeliveryTripId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    StopSequence = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTripOrders", x => new { x.DeliveryTripId, x.DeliveryOrderId });
                    table.ForeignKey(
                        name: "FK_DeliveryTripOrders_DeliveryOrder",
                        column: x => x.DeliveryOrderId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryOrders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryTripOrders_DeliveryTrip",
                        column: x => x.DeliveryTripId,
                        principalSchema: "DeliveryOrder",
                        principalTable: "DeliveryTrips",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_CompanyId_CreatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                columns: new[] { "CompanyId", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_CompanyId_Status_CreatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                columns: new[] { "CompanyId", "Status", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOrders_DeliveryTripId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "DeliveryTripId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryExpenses_CreatedBy",
                schema: "DeliveryOrder",
                table: "DeliveryExpenses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryExpenses_DeliveryTripId",
                schema: "DeliveryOrder",
                table: "DeliveryExpenses",
                column: "DeliveryTripId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProofOfDeliveries_ConfirmedBy",
                schema: "DeliveryOrder",
                table: "DeliveryProofOfDeliveries",
                column: "ConfirmedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryProofOfDeliveries_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryProofOfDeliveries",
                column: "DeliveryOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStops_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryStops",
                column: "DeliveryOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStops_DeliveryTripId",
                schema: "DeliveryOrder",
                table: "DeliveryStops",
                column: "DeliveryTripId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryStops_DeliveryTripId_SequenceNo",
                schema: "DeliveryOrder",
                table: "DeliveryStops",
                columns: new[] { "DeliveryTripId", "SequenceNo" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTripOrders_DeliveryOrderId",
                schema: "DeliveryOrder",
                table: "DeliveryTripOrders",
                column: "DeliveryOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTripOrders_DeliveryTripId_StopSequence",
                schema: "DeliveryOrder",
                table: "DeliveryTripOrders",
                columns: new[] { "DeliveryTripId", "StopSequence" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrips_CompanyId",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrips_CompanyId_Status_TripDate",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                columns: new[] { "CompanyId", "Status", "TripDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrips_DispatcherId",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                column: "DispatcherId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrips_DriverId",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrips_TripDate",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                column: "TripDate");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTrips_VehicleId",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "UX_DeliveryTrips_ExternalId",
                schema: "DeliveryOrder",
                table: "DeliveryTrips",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryVehicles_CompanyId",
                schema: "DeliveryOrder",
                table: "DeliveryVehicles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryVehicles_CompanyId_IsActive",
                schema: "DeliveryOrder",
                table: "DeliveryVehicles",
                columns: new[] { "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryVehicles_PlateNumber",
                schema: "DeliveryOrder",
                table: "DeliveryVehicles",
                column: "PlateNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOrders_DeliveryTrip",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "DeliveryTripId",
                principalSchema: "DeliveryOrder",
                principalTable: "DeliveryTrips",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOrders_DeliveryTrip",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropTable(
                name: "DeliveryExpenses",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryProofOfDeliveries",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryStops",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryTripOrders",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryTrips",
                schema: "DeliveryOrder");

            migrationBuilder.DropTable(
                name: "DeliveryVehicles",
                schema: "DeliveryOrder");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_CompanyId_CreatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_CompanyId_Status_CreatedDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOrders_DeliveryTripId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "ActualDeliveryDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryTripId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryWindowFrom",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryWindowTo",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "IsDeliveredSuccessfully",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "PlannedDeliveryDate",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.DropColumn(
                name: "IdentityNumber",
                schema: "DeliveryOrder",
                table: "DelivererInfor");

            migrationBuilder.DropColumn(
                name: "LicenseExpiryDate",
                schema: "DeliveryOrder",
                table: "DelivererInfor");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                schema: "DeliveryOrder",
                table: "DelivererInfor");

            migrationBuilder.AlterColumn<string>(
                name: "TaxNumber",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Receiver",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneSnapshot",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentType",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentDeadline",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
