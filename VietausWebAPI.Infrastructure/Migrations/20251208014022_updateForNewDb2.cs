using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateForNewDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__Schedual__3214EC07A98DEC4E",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.AlterColumn<int>(
                name: "Idpk",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchedualMfg",
                schema: "Schedual",
                table: "SchedualMfg",
                column: "Idpk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchedualMfg",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.AlterColumn<int>(
                name: "Idpk",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK__Schedual__3214EC07A98DEC4E",
                schema: "Schedual",
                table: "SchedualMfg",
                column: "Id");
        }
    }
}
