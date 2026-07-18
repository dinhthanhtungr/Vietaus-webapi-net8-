using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateMerchadiseDeliveryPausedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrders_DeliveryPausedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "DeliveryPausedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchandiseOrders_DeliveryPausedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "DeliveryPausedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchandiseOrders_DeliveryPausedBy",
                schema: "Orders",
                table: "MerchandiseOrders");

            migrationBuilder.DropIndex(
                name: "IX_MerchandiseOrders_DeliveryPausedBy",
                schema: "Orders",
                table: "MerchandiseOrders");
        }
    }
}
