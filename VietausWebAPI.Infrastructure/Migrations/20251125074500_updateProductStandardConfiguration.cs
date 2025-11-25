using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateProductStandardConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QCDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ProductT__3213E83F975320C1",
                table: "ProductTest");

            migrationBuilder.DropPrimaryKey(
                name: "PK__ProductS__3214EC0715B96228",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "Package",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "colourCode",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "customerExternalId",
                table: "ProductStandard");

            migrationBuilder.EnsureSchema(
                name: "devandga");

            migrationBuilder.RenameTable(
                name: "ProductTest",
                newName: "ProductTest",
                newSchema: "devandga");

            migrationBuilder.RenameTable(
                name: "ProductStandard",
                newName: "ProductStandard",
                newSchema: "devandga");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "ProductInspection",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Types",
                table: "ProductInspection",
                newName: "types");

            migrationBuilder.RenameColumn(
                name: "Shape",
                table: "ProductInspection",
                newName: "shape");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "ProductInspection",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "Moisture",
                table: "ProductInspection",
                newName: "moisture");

            migrationBuilder.RenameColumn(
                name: "Hardness",
                table: "ProductInspection",
                newName: "hardness");

            migrationBuilder.RenameColumn(
                name: "Elongation",
                table: "ProductInspection",
                newName: "elongation");

            migrationBuilder.RenameColumn(
                name: "Density",
                table: "ProductInspection",
                newName: "density");

            migrationBuilder.RenameColumn(
                name: "Antistatic",
                table: "ProductInspection",
                newName: "antistatic");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductInspection",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VisualCheck",
                table: "ProductInspection",
                newName: "visual_check");

            migrationBuilder.RenameColumn(
                name: "TensileStrength",
                table: "ProductInspection",
                newName: "tensile_strength");

            migrationBuilder.RenameColumn(
                name: "StorageCondition",
                table: "ProductInspection",
                newName: "storage_condition");

            migrationBuilder.RenameColumn(
                name: "ProductStandardId",
                table: "ProductInspection",
                newName: "product_standard_id");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "ProductInspection",
                newName: "product_name");

            migrationBuilder.RenameColumn(
                name: "ProductCode",
                table: "ProductInspection",
                newName: "product_code");

            migrationBuilder.RenameColumn(
                name: "ParticleSize",
                table: "ProductInspection",
                newName: "particle_size");

            migrationBuilder.RenameColumn(
                name: "PackingSpec",
                table: "ProductInspection",
                newName: "packing_spec");

            migrationBuilder.RenameColumn(
                name: "MigrationTest",
                table: "ProductInspection",
                newName: "migration_test");

            migrationBuilder.RenameColumn(
                name: "MeshType",
                table: "ProductInspection",
                newName: "mesh_type");

            migrationBuilder.RenameColumn(
                name: "ManufacturingDate",
                table: "ProductInspection",
                newName: "manufacturing_date");

            migrationBuilder.RenameColumn(
                name: "IsTensileStrengthPass",
                table: "ProductInspection",
                newName: "is_tensile_strength_pass");

            migrationBuilder.RenameColumn(
                name: "IsStorageConditionPass",
                table: "ProductInspection",
                newName: "is_storage_condition_pass");

            migrationBuilder.RenameColumn(
                name: "IsShapePass",
                table: "ProductInspection",
                newName: "is_shape_pass");

            migrationBuilder.RenameColumn(
                name: "IsParticleSizePass",
                table: "ProductInspection",
                newName: "is_particle_size_pass");

            migrationBuilder.RenameColumn(
                name: "IsPackingSpecPass",
                table: "ProductInspection",
                newName: "is_packing_spec_pass");

            migrationBuilder.RenameColumn(
                name: "IsMoisturePass",
                table: "ProductInspection",
                newName: "is_moisture_pass");

            migrationBuilder.RenameColumn(
                name: "IsMeshAttached",
                table: "ProductInspection",
                newName: "is_mesh_attached");

            migrationBuilder.RenameColumn(
                name: "IsImpactResistancePass",
                table: "ProductInspection",
                newName: "is_impact_resistance_pass");

            migrationBuilder.RenameColumn(
                name: "IsHardnessPass",
                table: "ProductInspection",
                newName: "is_hardness_pass");

            migrationBuilder.RenameColumn(
                name: "IsFlexuralStrengthPass",
                table: "ProductInspection",
                newName: "is_flexural_strength_pass");

            migrationBuilder.RenameColumn(
                name: "IsFlexuralModulusPass",
                table: "ProductInspection",
                newName: "is_flexural_modulus_pass");

            migrationBuilder.RenameColumn(
                name: "IsElongationPass",
                table: "ProductInspection",
                newName: "is_elongation_pass");

            migrationBuilder.RenameColumn(
                name: "IsDensityPass",
                table: "ProductInspection",
                newName: "is_density_pass");

            migrationBuilder.RenameColumn(
                name: "IsAntistaticPass",
                table: "ProductInspection",
                newName: "is_antistatic_pass");

            migrationBuilder.RenameColumn(
                name: "ImpactResistance",
                table: "ProductInspection",
                newName: "impact_resistance");

            migrationBuilder.RenameColumn(
                name: "FlexuralStrength",
                table: "ProductInspection",
                newName: "flexural_strength");

            migrationBuilder.RenameColumn(
                name: "FlexuralModulus",
                table: "ProductInspection",
                newName: "flexural_modulus");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "ProductInspection",
                newName: "external_id");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                table: "ProductInspection",
                newName: "expiry_date");

            migrationBuilder.RenameColumn(
                name: "DwellTime",
                table: "ProductInspection",
                newName: "dwell_time");

            migrationBuilder.RenameColumn(
                name: "DeliveryAccepted",
                table: "ProductInspection",
                newName: "delivery_accepted");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "ProductInspection",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "ProductInspection",
                newName: "create_date");

            migrationBuilder.RenameColumn(
                name: "ColorDeltaE",
                table: "ProductInspection",
                newName: "color_delta_e");

            migrationBuilder.RenameColumn(
                name: "BlackDots",
                table: "ProductInspection",
                newName: "black_dots");

            migrationBuilder.RenameColumn(
                name: "BatchId",
                table: "ProductInspection",
                newName: "batch_id");

            migrationBuilder.RenameColumn(
                name: "product_externalId",
                schema: "devandga",
                table: "ProductTest",
                newName: "product_externalid");

            migrationBuilder.RenameColumn(
                name: "product_customerExternalId",
                schema: "devandga",
                table: "ProductTest",
                newName: "product_customerexternalid");

            migrationBuilder.RenameColumn(
                name: "externalId",
                schema: "devandga",
                table: "ProductTest",
                newName: "externalid");

            migrationBuilder.RenameColumn(
                name: "ManufacturingDate",
                schema: "devandga",
                table: "ProductTest",
                newName: "manufacturingdate");

            migrationBuilder.RenameColumn(
                name: "ExpiryDate",
                schema: "devandga",
                table: "ProductTest",
                newName: "expirydate");

            migrationBuilder.RenameColumn(
                name: "TensileStrength",
                schema: "devandga",
                table: "ProductStandard",
                newName: "tensilestrength");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "devandga",
                table: "ProductStandard",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Shape",
                schema: "devandga",
                table: "ProductStandard",
                newName: "shape");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "devandga",
                table: "ProductStandard",
                newName: "productid");

            migrationBuilder.RenameColumn(
                name: "ProductExternalId",
                schema: "devandga",
                table: "ProductStandard",
                newName: "productexternalid");

            migrationBuilder.RenameColumn(
                name: "PelletSize",
                schema: "devandga",
                table: "ProductStandard",
                newName: "pelletsize");

            migrationBuilder.RenameColumn(
                name: "Moisture",
                schema: "devandga",
                table: "ProductStandard",
                newName: "moisture");

            migrationBuilder.RenameColumn(
                name: "MigrationTest",
                schema: "devandga",
                table: "ProductStandard",
                newName: "migrationtest");

            migrationBuilder.RenameColumn(
                name: "MeltIndex",
                schema: "devandga",
                table: "ProductStandard",
                newName: "meltindex");

            migrationBuilder.RenameColumn(
                name: "IzodImpactStrength",
                schema: "devandga",
                table: "ProductStandard",
                newName: "izodimpactstrength");

            migrationBuilder.RenameColumn(
                name: "Hardness",
                schema: "devandga",
                table: "ProductStandard",
                newName: "hardness");

            migrationBuilder.RenameColumn(
                name: "FlexuralStrength",
                schema: "devandga",
                table: "ProductStandard",
                newName: "flexuralstrength");

            migrationBuilder.RenameColumn(
                name: "FlexuralModulus",
                schema: "devandga",
                table: "ProductStandard",
                newName: "flexuralmodulus");

            migrationBuilder.RenameColumn(
                name: "ElongationAtBreak",
                schema: "devandga",
                table: "ProductStandard",
                newName: "elongationatbreak");

            migrationBuilder.RenameColumn(
                name: "DwellTime",
                schema: "devandga",
                table: "ProductStandard",
                newName: "dwelltime");

            migrationBuilder.RenameColumn(
                name: "Density",
                schema: "devandga",
                table: "ProductStandard",
                newName: "density");

            migrationBuilder.RenameColumn(
                name: "DeltaE",
                schema: "devandga",
                table: "ProductStandard",
                newName: "deltae");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "devandga",
                table: "ProductStandard",
                newName: "createddate");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                schema: "devandga",
                table: "ProductStandard",
                newName: "companyid");

            migrationBuilder.RenameColumn(
                name: "BlackDots",
                schema: "devandga",
                table: "ProductStandard",
                newName: "blackdots");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "devandga",
                table: "ProductStandard",
                newName: "id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "ProductInspection",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "color_name",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "step_of_product",
                schema: "manufacturing",
                table: "MfgProductionOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "product_weight",
                schema: "devandga",
                table: "ProductTest",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "product_id",
                schema: "devandga",
                table: "ProductTest",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "manufacturingdate",
                schema: "devandga",
                table: "ProductTest",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "expirydate",
                schema: "devandga",
                table: "ProductTest",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "devandga",
                table: "ProductTest",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "weight",
                schema: "devandga",
                table: "ProductStandard",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "productid",
                schema: "devandga",
                table: "ProductStandard",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "companyid",
                schema: "devandga",
                table: "ProductStandard",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "devandga",
                table: "ProductStandard",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                schema: "devandga",
                table: "ProductStandard",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTest",
                schema: "devandga",
                table: "ProductTest",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStandard",
                schema: "devandga",
                table: "ProductStandard",
                column: "id");

            migrationBuilder.CreateTable(
                name: "qc_pass_detail_history",
                schema: "devandga",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    qcdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    machineid = table.Column<string>(type: "citext", nullable: true),
                    batchno = table.Column<string>(type: "citext", nullable: true),
                    qcround = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    employeeid = table.Column<Guid>(type: "uuid", nullable: false),
                    statusqc = table.Column<string>(type: "citext", nullable: true),
                    diameter = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    diameterresult = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    particlesize = table.Column<string>(type: "citext", nullable: true),
                    particlesizeresult = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    equaltostandard = table.Column<string>(type: "citext", nullable: true),
                    colorcode = table.Column<string>(type: "citext", nullable: true),
                    deltae = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    moisture = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    moistureresult = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    mfr = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    mfrresult = table.Column<string>(type: "citext", nullable: true),
                    flexuralstrengthmpa = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    flexuralstrengthresult = table.Column<string>(type: "citext", nullable: true),
                    elongationmpa = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    elongationresult = table.Column<string>(type: "citext", nullable: true),
                    hardnessshored = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    hardnessresult = table.Column<string>(type: "citext", nullable: true),
                    densitysperm3 = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    densityresult = table.Column<string>(type: "citext", nullable: true),
                    tensilestrengthmpa = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    tensilestrengthresult = table.Column<string>(type: "citext", nullable: true),
                    flexmodulusmpa = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    flexmodulusresult = table.Column<string>(type: "citext", nullable: true),
                    impactkjperm2 = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    impactresult = table.Column<string>(type: "citext", nullable: true),
                    staticohm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    staticresult = table.Column<string>(type: "citext", nullable: true),
                    storagetempc = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    storagetempresult = table.Column<string>(type: "citext", nullable: true),
                    flaground = table.Column<bool>(type: "boolean", nullable: false),
                    flagblackdot = table.Column<bool>(type: "boolean", nullable: false),
                    flagswirl = table.Column<bool>(type: "boolean", nullable: false),
                    flagdust = table.Column<bool>(type: "boolean", nullable: false),
                    flagwrongcolor = table.Column<bool>(type: "boolean", nullable: false),
                    packingspeckg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    keepsample = table.Column<bool>(type: "boolean", nullable: false),
                    printsample = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_pass_detail_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "qc_pass_history",
                schema: "devandga",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    qcdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    machineid = table.Column<string>(type: "citext", nullable: true),
                    batchno = table.Column<string>(type: "citext", nullable: true),
                    qcround = table.Column<int>(type: "integer", nullable: false),
                    note = table.Column<string>(type: "text", nullable: true),
                    employeeid = table.Column<Guid>(type: "uuid", nullable: false),
                    statusqc = table.Column<string>(type: "citext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qc_pass_history", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_productinspection_batch_create_date",
                table: "ProductInspection",
                columns: new[] { "batch_id", "create_date" });

            migrationBuilder.CreateIndex(
                name: "ix_productinspection_create_date",
                table: "ProductInspection",
                column: "create_date");

            migrationBuilder.CreateIndex(
                name: "ix_productinspection_external_id",
                table: "ProductInspection",
                column: "external_id");

            migrationBuilder.CreateIndex(
                name: "ix_productinspection_product_create_date",
                table: "ProductInspection",
                columns: new[] { "product_code", "create_date" });

            migrationBuilder.CreateIndex(
                name: "ix_producttest_externalid_mfgdate",
                schema: "devandga",
                table: "ProductTest",
                columns: new[] { "product_externalid", "manufacturingdate" });

            migrationBuilder.CreateIndex(
                name: "ix_producttest_productid",
                schema: "devandga",
                table: "ProductTest",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandard_companyid",
                schema: "devandga",
                table: "ProductStandard",
                column: "companyid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandard_CreatedBy",
                schema: "devandga",
                table: "ProductStandard",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "ix_productstandard_createddate",
                schema: "devandga",
                table: "ProductStandard",
                column: "createddate");

            migrationBuilder.CreateIndex(
                name: "ix_productstandard_productid",
                schema: "devandga",
                table: "ProductStandard",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "ix_qcpassdetailhistory_batch_round",
                schema: "devandga",
                table: "qc_pass_detail_history",
                columns: new[] { "batchno", "qcround" });

            migrationBuilder.CreateIndex(
                name: "ix_qcpassdetailhistory_machine_qcdate",
                schema: "devandga",
                table: "qc_pass_detail_history",
                columns: new[] { "machineid", "qcdate" });

            migrationBuilder.CreateIndex(
                name: "ix_qcpassdetailhistory_qcdate",
                schema: "devandga",
                table: "qc_pass_detail_history",
                column: "qcdate");

            migrationBuilder.CreateIndex(
                name: "ix_qcpasshistory_batch_round",
                schema: "devandga",
                table: "qc_pass_history",
                columns: new[] { "batchno", "qcround" });

            migrationBuilder.CreateIndex(
                name: "ix_qcpasshistory_machine_qcdate",
                schema: "devandga",
                table: "qc_pass_history",
                columns: new[] { "machineid", "qcdate" });

            migrationBuilder.CreateIndex(
                name: "ix_qcpasshistory_qcdate",
                schema: "devandga",
                table: "qc_pass_history",
                column: "qcdate");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStandard_Company",
                schema: "devandga",
                table: "ProductStandard",
                column: "companyid",
                principalSchema: "company",
                principalTable: "Companies",
                principalColumn: "companyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductStandard_CreatedBy",
                schema: "devandga",
                table: "ProductStandard",
                column: "CreatedBy",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductStandard_Company",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductStandard_CreatedBy",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropTable(
                name: "qc_pass_detail_history",
                schema: "devandga");

            migrationBuilder.DropTable(
                name: "qc_pass_history",
                schema: "devandga");

            migrationBuilder.DropIndex(
                name: "ix_productinspection_batch_create_date",
                table: "ProductInspection");

            migrationBuilder.DropIndex(
                name: "ix_productinspection_create_date",
                table: "ProductInspection");

            migrationBuilder.DropIndex(
                name: "ix_productinspection_external_id",
                table: "ProductInspection");

            migrationBuilder.DropIndex(
                name: "ix_productinspection_product_create_date",
                table: "ProductInspection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTest",
                schema: "devandga",
                table: "ProductTest");

            migrationBuilder.DropIndex(
                name: "ix_producttest_externalid_mfgdate",
                schema: "devandga",
                table: "ProductTest");

            migrationBuilder.DropIndex(
                name: "ix_producttest_productid",
                schema: "devandga",
                table: "ProductTest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStandard",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropIndex(
                name: "IX_ProductStandard_companyid",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropIndex(
                name: "IX_ProductStandard_CreatedBy",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropIndex(
                name: "ix_productstandard_createddate",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropIndex(
                name: "ix_productstandard_productid",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.DropColumn(
                name: "color_name",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "step_of_product",
                schema: "manufacturing",
                table: "MfgProductionOrders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "devandga",
                table: "ProductStandard");

            migrationBuilder.RenameTable(
                name: "ProductTest",
                schema: "devandga",
                newName: "ProductTest");

            migrationBuilder.RenameTable(
                name: "ProductStandard",
                schema: "devandga",
                newName: "ProductStandard");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "ProductInspection",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "types",
                table: "ProductInspection",
                newName: "Types");

            migrationBuilder.RenameColumn(
                name: "shape",
                table: "ProductInspection",
                newName: "Shape");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "ProductInspection",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "moisture",
                table: "ProductInspection",
                newName: "Moisture");

            migrationBuilder.RenameColumn(
                name: "hardness",
                table: "ProductInspection",
                newName: "Hardness");

            migrationBuilder.RenameColumn(
                name: "elongation",
                table: "ProductInspection",
                newName: "Elongation");

            migrationBuilder.RenameColumn(
                name: "density",
                table: "ProductInspection",
                newName: "Density");

            migrationBuilder.RenameColumn(
                name: "antistatic",
                table: "ProductInspection",
                newName: "Antistatic");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ProductInspection",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "visual_check",
                table: "ProductInspection",
                newName: "VisualCheck");

            migrationBuilder.RenameColumn(
                name: "tensile_strength",
                table: "ProductInspection",
                newName: "TensileStrength");

            migrationBuilder.RenameColumn(
                name: "storage_condition",
                table: "ProductInspection",
                newName: "StorageCondition");

            migrationBuilder.RenameColumn(
                name: "product_standard_id",
                table: "ProductInspection",
                newName: "ProductStandardId");

            migrationBuilder.RenameColumn(
                name: "product_name",
                table: "ProductInspection",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "product_code",
                table: "ProductInspection",
                newName: "ProductCode");

            migrationBuilder.RenameColumn(
                name: "particle_size",
                table: "ProductInspection",
                newName: "ParticleSize");

            migrationBuilder.RenameColumn(
                name: "packing_spec",
                table: "ProductInspection",
                newName: "PackingSpec");

            migrationBuilder.RenameColumn(
                name: "migration_test",
                table: "ProductInspection",
                newName: "MigrationTest");

            migrationBuilder.RenameColumn(
                name: "mesh_type",
                table: "ProductInspection",
                newName: "MeshType");

            migrationBuilder.RenameColumn(
                name: "manufacturing_date",
                table: "ProductInspection",
                newName: "ManufacturingDate");

            migrationBuilder.RenameColumn(
                name: "is_tensile_strength_pass",
                table: "ProductInspection",
                newName: "IsTensileStrengthPass");

            migrationBuilder.RenameColumn(
                name: "is_storage_condition_pass",
                table: "ProductInspection",
                newName: "IsStorageConditionPass");

            migrationBuilder.RenameColumn(
                name: "is_shape_pass",
                table: "ProductInspection",
                newName: "IsShapePass");

            migrationBuilder.RenameColumn(
                name: "is_particle_size_pass",
                table: "ProductInspection",
                newName: "IsParticleSizePass");

            migrationBuilder.RenameColumn(
                name: "is_packing_spec_pass",
                table: "ProductInspection",
                newName: "IsPackingSpecPass");

            migrationBuilder.RenameColumn(
                name: "is_moisture_pass",
                table: "ProductInspection",
                newName: "IsMoisturePass");

            migrationBuilder.RenameColumn(
                name: "is_mesh_attached",
                table: "ProductInspection",
                newName: "IsMeshAttached");

            migrationBuilder.RenameColumn(
                name: "is_impact_resistance_pass",
                table: "ProductInspection",
                newName: "IsImpactResistancePass");

            migrationBuilder.RenameColumn(
                name: "is_hardness_pass",
                table: "ProductInspection",
                newName: "IsHardnessPass");

            migrationBuilder.RenameColumn(
                name: "is_flexural_strength_pass",
                table: "ProductInspection",
                newName: "IsFlexuralStrengthPass");

            migrationBuilder.RenameColumn(
                name: "is_flexural_modulus_pass",
                table: "ProductInspection",
                newName: "IsFlexuralModulusPass");

            migrationBuilder.RenameColumn(
                name: "is_elongation_pass",
                table: "ProductInspection",
                newName: "IsElongationPass");

            migrationBuilder.RenameColumn(
                name: "is_density_pass",
                table: "ProductInspection",
                newName: "IsDensityPass");

            migrationBuilder.RenameColumn(
                name: "is_antistatic_pass",
                table: "ProductInspection",
                newName: "IsAntistaticPass");

            migrationBuilder.RenameColumn(
                name: "impact_resistance",
                table: "ProductInspection",
                newName: "ImpactResistance");

            migrationBuilder.RenameColumn(
                name: "flexural_strength",
                table: "ProductInspection",
                newName: "FlexuralStrength");

            migrationBuilder.RenameColumn(
                name: "flexural_modulus",
                table: "ProductInspection",
                newName: "FlexuralModulus");

            migrationBuilder.RenameColumn(
                name: "external_id",
                table: "ProductInspection",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "expiry_date",
                table: "ProductInspection",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "dwell_time",
                table: "ProductInspection",
                newName: "DwellTime");

            migrationBuilder.RenameColumn(
                name: "delivery_accepted",
                table: "ProductInspection",
                newName: "DeliveryAccepted");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "ProductInspection",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "create_date",
                table: "ProductInspection",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "color_delta_e",
                table: "ProductInspection",
                newName: "ColorDeltaE");

            migrationBuilder.RenameColumn(
                name: "black_dots",
                table: "ProductInspection",
                newName: "BlackDots");

            migrationBuilder.RenameColumn(
                name: "batch_id",
                table: "ProductInspection",
                newName: "BatchId");

            migrationBuilder.RenameColumn(
                name: "product_externalid",
                table: "ProductTest",
                newName: "product_externalId");

            migrationBuilder.RenameColumn(
                name: "product_customerexternalid",
                table: "ProductTest",
                newName: "product_customerExternalId");

            migrationBuilder.RenameColumn(
                name: "manufacturingdate",
                table: "ProductTest",
                newName: "ManufacturingDate");

            migrationBuilder.RenameColumn(
                name: "externalid",
                table: "ProductTest",
                newName: "externalId");

            migrationBuilder.RenameColumn(
                name: "expirydate",
                table: "ProductTest",
                newName: "ExpiryDate");

            migrationBuilder.RenameColumn(
                name: "tensilestrength",
                table: "ProductStandard",
                newName: "TensileStrength");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "ProductStandard",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "shape",
                table: "ProductStandard",
                newName: "Shape");

            migrationBuilder.RenameColumn(
                name: "productid",
                table: "ProductStandard",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "productexternalid",
                table: "ProductStandard",
                newName: "ProductExternalId");

            migrationBuilder.RenameColumn(
                name: "pelletsize",
                table: "ProductStandard",
                newName: "PelletSize");

            migrationBuilder.RenameColumn(
                name: "moisture",
                table: "ProductStandard",
                newName: "Moisture");

            migrationBuilder.RenameColumn(
                name: "migrationtest",
                table: "ProductStandard",
                newName: "MigrationTest");

            migrationBuilder.RenameColumn(
                name: "meltindex",
                table: "ProductStandard",
                newName: "MeltIndex");

            migrationBuilder.RenameColumn(
                name: "izodimpactstrength",
                table: "ProductStandard",
                newName: "IzodImpactStrength");

            migrationBuilder.RenameColumn(
                name: "hardness",
                table: "ProductStandard",
                newName: "Hardness");

            migrationBuilder.RenameColumn(
                name: "flexuralstrength",
                table: "ProductStandard",
                newName: "FlexuralStrength");

            migrationBuilder.RenameColumn(
                name: "flexuralmodulus",
                table: "ProductStandard",
                newName: "FlexuralModulus");

            migrationBuilder.RenameColumn(
                name: "elongationatbreak",
                table: "ProductStandard",
                newName: "ElongationAtBreak");

            migrationBuilder.RenameColumn(
                name: "dwelltime",
                table: "ProductStandard",
                newName: "DwellTime");

            migrationBuilder.RenameColumn(
                name: "density",
                table: "ProductStandard",
                newName: "Density");

            migrationBuilder.RenameColumn(
                name: "deltae",
                table: "ProductStandard",
                newName: "DeltaE");

            migrationBuilder.RenameColumn(
                name: "createddate",
                table: "ProductStandard",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "companyid",
                table: "ProductStandard",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "blackdots",
                table: "ProductStandard",
                newName: "BlackDots");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ProductStandard",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductInspection",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<int>(
                name: "product_weight",
                table: "ProductTest",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "product_id",
                table: "ProductTest",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ManufacturingDate",
                table: "ProductTest",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "ProductTest",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "ProductTest",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<int>(
                name: "weight",
                table: "ProductStandard",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductStandard",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "ProductStandard",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProductStandard",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "ProductStandard",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Package",
                table: "ProductStandard",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "colourCode",
                table: "ProductStandard",
                type: "citext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "customerExternalId",
                table: "ProductStandard",
                type: "citext",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__ProductT__3213E83F975320C1",
                table: "ProductTest",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK__ProductS__3214EC0715B96228",
                table: "ProductStandard",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QCDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    BatchExternalId = table.Column<string>(type: "citext", nullable: true),
                    MachineExternalId = table.Column<string>(type: "citext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QCDetail__3214EC07937EA2C5", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QCDetail_ProductInspection",
                        column: x => x.BatchId,
                        principalTable: "ProductInspection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QCDetail_BatchId",
                table: "QCDetail",
                column: "BatchId",
                unique: true);
        }
    }
}
