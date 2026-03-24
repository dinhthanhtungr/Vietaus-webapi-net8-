using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateIndexExternalIdDeliveryOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_DeliveryOrders_ExternalId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                column: "ExternalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_DeliveryOrders_ExternalId",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");
        }
    }
}
