using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryOrderDetail1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocationExternalIdSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "LocationNameSnapShot");

            migrationBuilder.AddColumn<string>(
                name: "PONo",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                type: "citext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PONo",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail");

            migrationBuilder.RenameColumn(
                name: "LocationNameSnapShot",
                schema: "DeliveryOrder",
                table: "DeliveryOrderDetail",
                newName: "LocationExternalIdSnapShot");
        }
    }
}
