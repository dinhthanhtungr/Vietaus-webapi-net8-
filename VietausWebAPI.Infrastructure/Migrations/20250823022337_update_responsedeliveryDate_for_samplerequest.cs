using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_responsedeliveryDate_for_samplerequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResponseDeliveryDate",
                schema: "labs",
                table: "SampleRequests",
                type: "timestamptz",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseDeliveryDate",
                schema: "labs",
                table: "SampleRequests");
        }
    }
}
