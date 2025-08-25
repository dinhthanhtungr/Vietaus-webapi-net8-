using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_requestTestSample_for_samplerequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestTestSampleDate",
                schema: "labs",
                table: "SampleRequests",
                type: "timestamptz",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestTestSampleDate",
                schema: "labs",
                table: "SampleRequests");
        }
    }
}
