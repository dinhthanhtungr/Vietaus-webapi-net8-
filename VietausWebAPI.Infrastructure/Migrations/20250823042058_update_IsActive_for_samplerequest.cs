using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_IsActive_for_samplerequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "labs",
                table: "SampleRequests",
                type: "boolean",
                nullable: true,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "labs",
                table: "SampleRequests");
        }
    }
}
