using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDeliveryOrderSoftDelete4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedIssueId",
                schema: "Warehouse",
                table: "WarehouseTempStock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LinkedIssueId",
                schema: "Warehouse",
                table: "WarehouseTempStock",
                type: "uuid",
                nullable: true);
        }
    }
}
