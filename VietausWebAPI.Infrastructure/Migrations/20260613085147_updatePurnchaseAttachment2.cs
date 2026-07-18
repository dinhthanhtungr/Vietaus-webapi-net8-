using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePurnchaseAttachment2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 6,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 99);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DocumentType",
                schema: "Orders",
                table: "PurchaseOrderDocuments",
                type: "integer",
                nullable: false,
                defaultValue: 99,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 6);
        }
    }
}
