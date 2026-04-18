using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCOA2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsIntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "is_intrinsic_viscosity");

            migrationBuilder.RenameColumn(
                name: "IntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "intrinsic_viscosity");

            migrationBuilder.AlterColumn<string>(
                name: "intrinsic_viscosity",
                schema: "devandqa",
                table: "ProductInspection",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_intrinsic_viscosity",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "IsIntrinsicViscosity");

            migrationBuilder.RenameColumn(
                name: "intrinsic_viscosity",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "IntrinsicViscosity");

            migrationBuilder.AlterColumn<string>(
                name: "IntrinsicViscosity",
                schema: "devandqa",
                table: "ProductInspection",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);
        }
    }
}
