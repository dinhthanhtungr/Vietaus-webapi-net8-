using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Mfm__manufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropIndex(
                name: "ux_psv_current_per_order",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropIndex(
                name: "ux_psf_company_product_current",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropIndex(
                name: "UX_MfgOrderPOs_Detail_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs");

            migrationBuilder.DropIndex(
                name: "UX_MfgOrderPOs_MfgOrder_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs");

            migrationBuilder.DropIndex(
                name: "ux_mfm_formula_material_unique_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ProductI__3214EC072F6AD3E1",
                table: "ProductInspection");

            migrationBuilder.DropColumn(
                name: "ColorCode",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ColorName",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductAddRate",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductExpiryType",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductMaxTemp",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductRohsStandard",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ProductWeight",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "ReachStandard",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "VerifyBatches",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.EnsureSchema(
                name: "devandqa");

            migrationBuilder.RenameTable(
                name: "ProductInspection",
                newName: "ProductInspection",
                newSchema: "devandqa");

            migrationBuilder.RenameColumn(
                name: "MFR",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "mfr");

            migrationBuilder.RenameColumn(
                name: "Defect_Moist",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "defect_moist");

            migrationBuilder.RenameColumn(
                name: "Defect_Impurity",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "defect_impurity");

            migrationBuilder.RenameColumn(
                name: "Defect_Dusty",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "defect_dusty");

            migrationBuilder.RenameColumn(
                name: "IsMFRPass",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "is_mfrpass");

            migrationBuilder.RenameColumn(
                name: "IsColorDeltaEPass",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "is_color_delta_epass");

            migrationBuilder.RenameColumn(
                name: "Defect_WrongColor",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "defect_wrong_color");

            migrationBuilder.RenameColumn(
                name: "Defect_ShortFiber",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "defect_short_fiber");

            migrationBuilder.RenameColumn(
                name: "Defect_BlackDot",
                schema: "devandqa",
                table: "ProductInspection",
                newName: "defect_black_dot");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryPlanDate",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QcNote",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldDefaultValue: "New");

            migrationBuilder.AlterColumn<string>(
                name: "notes",
                schema: "devandqa",
                table: "ProductInspection",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "citext",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInspection",
                schema: "devandqa",
                table: "ProductInspection",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ux_psv_current_per_order",
                schema: "manufacturing",
                table: "production_select_versions",
                columns: new[] { "company_id", "mfg_production_order_id" });

            migrationBuilder.CreateIndex(
                name: "ux_psf_company_product_current",
                schema: "manufacturing",
                table: "product_standard_formulas",
                columns: new[] { "company_id", "product_id" });

            migrationBuilder.CreateIndex(
                name: "UX_MfgOrderPOs_Detail_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                columns: new[] { "MerchandiseOrderDetailId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "UX_MfgOrderPOs_MfgOrder_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                columns: new[] { "MfgProductionOrderId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "ux_mfm_formula_material_unique_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                columns: new[] { "manufacturing_formula_id", "material_id", "category_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_mf_version_items_version",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "manufacturingFormulaVersionId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_mf_version_items_version",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems");

            migrationBuilder.DropIndex(
                name: "ux_psv_current_per_order",
                schema: "manufacturing",
                table: "production_select_versions");

            migrationBuilder.DropIndex(
                name: "ux_psf_company_product_current",
                schema: "manufacturing",
                table: "product_standard_formulas");

            migrationBuilder.DropIndex(
                name: "UX_MfgOrderPOs_Detail_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs");

            migrationBuilder.DropIndex(
                name: "UX_MfgOrderPOs_MfgOrder_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs");

            migrationBuilder.DropIndex(
                name: "ux_mfm_formula_material_unique_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInspection",
                schema: "devandqa",
                table: "ProductInspection");

            migrationBuilder.DropColumn(
                name: "DeliveryPlanDate",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.DropColumn(
                name: "QcNote",
                schema: "Schedual",
                table: "SchedualMfg");

            migrationBuilder.RenameTable(
                name: "ProductInspection",
                schema: "devandqa",
                newName: "ProductInspection");

            migrationBuilder.RenameColumn(
                name: "mfr",
                table: "ProductInspection",
                newName: "MFR");

            migrationBuilder.RenameColumn(
                name: "defect_moist",
                table: "ProductInspection",
                newName: "Defect_Moist");

            migrationBuilder.RenameColumn(
                name: "defect_impurity",
                table: "ProductInspection",
                newName: "Defect_Impurity");

            migrationBuilder.RenameColumn(
                name: "defect_dusty",
                table: "ProductInspection",
                newName: "Defect_Dusty");

            migrationBuilder.RenameColumn(
                name: "is_mfrpass",
                table: "ProductInspection",
                newName: "IsMFRPass");

            migrationBuilder.RenameColumn(
                name: "is_color_delta_epass",
                table: "ProductInspection",
                newName: "IsColorDeltaEPass");

            migrationBuilder.RenameColumn(
                name: "defect_wrong_color",
                table: "ProductInspection",
                newName: "Defect_WrongColor");

            migrationBuilder.RenameColumn(
                name: "defect_short_fiber",
                table: "ProductInspection",
                newName: "Defect_ShortFiber");

            migrationBuilder.RenameColumn(
                name: "defect_black_dot",
                table: "ProductInspection",
                newName: "Defect_BlackDot");

            migrationBuilder.AddColumn<string>(
                name: "ColorCode",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ColorName",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductAddRate",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductExpiryType",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductMaxTemp",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ProductRohsStandard",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ProductWeight",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReachStandard",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifyBatches",
                schema: "Schedual",
                table: "SchedualMfg",
                type: "citext",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "citext");

            migrationBuilder.AlterColumn<string>(
                name: "notes",
                table: "ProductInspection",
                type: "citext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__ProductI__3214EC072F6AD3E1",
                table: "ProductInspection",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ux_psv_current_per_order",
                schema: "manufacturing",
                table: "production_select_versions",
                columns: new[] { "company_id", "mfg_production_order_id" },
                unique: true,
                filter: "\"valid_to\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "ux_psf_company_product_current",
                schema: "manufacturing",
                table: "product_standard_formulas",
                columns: new[] { "company_id", "product_id" },
                unique: true,
                filter: "\"valid_to\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "UX_MfgOrderPOs_Detail_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                columns: new[] { "MerchandiseOrderDetailId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "UX_MfgOrderPOs_MfgOrder_Active",
                schema: "manufacturing",
                table: "MfgOrderPOs",
                columns: new[] { "MfgProductionOrderId", "IsActive" },
                unique: true,
                filter: "\"IsActive\" = TRUE");

            migrationBuilder.CreateIndex(
                name: "ux_mfm_formula_material_unique_active",
                schema: "manufacturing",
                table: "ManufacturingFormulaMaterials",
                columns: new[] { "manufacturing_formula_id", "material_id", "category_id" },
                unique: true,
                filter: "\"is_active\" = TRUE");

            migrationBuilder.AddForeignKey(
                name: "FK__Mfm__manufacturingFormulaId",
                schema: "manufacturing",
                table: "ManufacturingFormulaVersionItems",
                column: "manufacturingFormulaVersionId",
                principalSchema: "manufacturing",
                principalTable: "ManufacturingFormulaVersions",
                principalColumn: "manufacturingFormulaVersionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
