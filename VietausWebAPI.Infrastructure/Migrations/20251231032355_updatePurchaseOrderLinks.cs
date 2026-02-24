using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurchaseOrderLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SupplyRequestId",
                schema: "Orders",
                table: "PurchaseOrderLink",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLink_SupplyRequestId",
                schema: "Orders",
                table: "PurchaseOrderLink",
                column: "SupplyRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLink_SupplyRequests_SupplyRequestId",
                schema: "Orders",
                table: "PurchaseOrderLink",
                column: "SupplyRequestId",
                principalSchema: "SupplyRequest",
                principalTable: "SupplyRequests",
                principalColumn: "requestid",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLink_SupplyRequests_SupplyRequestId",
                schema: "Orders",
                table: "PurchaseOrderLink");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLink_SupplyRequestId",
                schema: "Orders",
                table: "PurchaseOrderLink");

            migrationBuilder.DropColumn(
                name: "SupplyRequestId",
                schema: "Orders",
                table: "PurchaseOrderLink");
        }
    }
}
