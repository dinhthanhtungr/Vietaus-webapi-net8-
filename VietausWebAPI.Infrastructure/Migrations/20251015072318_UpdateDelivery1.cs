using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDelivery1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.RenameColumn(
                name: "ManufacturingFormulaExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "LotNoList");

            migrationBuilder.AddColumn<bool>(
                name: "IsAttach",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAttach",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.RenameColumn(
                name: "LotNoList",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "ManufacturingFormulaExternalIdSnapShot");

            migrationBuilder.AddColumn<string>(
                name: "LocationNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "text",
                nullable: true);
        }
    }
}
