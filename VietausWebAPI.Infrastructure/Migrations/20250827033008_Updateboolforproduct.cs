using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updateboolforproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        ALTER TABLE ""labs"".""Products""
        ALTER COLUMN ""StorageCondition"" TYPE boolean
        USING (
          CASE
            WHEN ""StorageCondition"" IN ('1','true','t','yes','y','on','Yêu cầu ca SX và QC thực hiện') THEN TRUE
            WHEN btrim(coalesce(""StorageCondition"", '')) = '' THEN NULL
            ELSE FALSE
          END
        );
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StorageCondition",
                schema: "labs",
                table: "Products",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);
        }
    }
}
