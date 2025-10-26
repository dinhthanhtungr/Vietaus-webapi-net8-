using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDeliveryOrderTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DeliveryOrders",
                schema: "DeliveryOrderDetail",
                newName: "DeliveryOrderDetail",
                newSchema: "DeliveryOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DeliveryOrderDetail");

            migrationBuilder.RenameTable(
                name: "DeliveryOrderDetail",
                schema: "DeliveryOrder",
                newName: "DeliveryOrders",
                newSchema: "DeliveryOrderDetail");
        }
    }
}
