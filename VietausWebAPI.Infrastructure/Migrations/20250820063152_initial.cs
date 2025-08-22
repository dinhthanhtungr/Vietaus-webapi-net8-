using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sales");

            migrationBuilder.EnsureSchema(
                name: "SupplyRequest");

            migrationBuilder.EnsureSchema(
                name: "company");

            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.EnsureSchema(
                name: "hr");

            migrationBuilder.EnsureSchema(
                name: "labs");

            migrationBuilder.EnsureSchema(
                name: "Orders");

            migrationBuilder.CreateTable(
                name: "ApprovalLevels_Common_data",
                columns: table => new
                {
                    LevelID = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: false),
                    LevelName = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Approval__09F03C061CA120B7", x => x.LevelID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    personName = table.Column<string>(type: "text", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpirationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups_Common_data",
                columns: table => new
                {
                    GroupID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    GroupName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Groups__149AF30A6FD59030", x => x.GroupID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialGroups",
                columns: table => new
                {
                    MaterialGroupID = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    MaterialGroupName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__E20265FD68C7B626", x => x.MaterialGroupID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialGroups_Material_data",
                columns: table => new
                {
                    MaterialGroupID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    externalId = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    MaterialGroupName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Detail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__E20265FD37B632C7", x => x.MaterialGroupID);
                });

            migrationBuilder.CreateTable(
                name: "MfgProductionOrdersPlan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    requirement = table.Column<string>(type: "text", nullable: true),
                    expiryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    externalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    product_externalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    product_expiryType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    product_package = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    product_rohsStandard = table.Column<bool>(type: "boolean", nullable: true),
                    product_recycleRate = table.Column<double>(type: "double precision", nullable: true),
                    product_weight = table.Column<double>(type: "double precision", nullable: true),
                    product_customerExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    product_maxTemp = table.Column<double>(type: "double precision", nullable: true),
                    product_addRate = table.Column<double>(type: "double precision", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MfgProdu__3213E83F2188AAF5", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                schema: "hr",
                columns: table => new
                {
                    PartID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalID = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    PartName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Parts__7C3F0D30F786F0A7", x => x.PartID);
                });

            migrationBuilder.CreateTable(
                name: "Parts_Common_data",
                columns: table => new
                {
                    PartID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    PartName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Parts__7C3F0D3012CD1E19", x => x.PartID);
                });

            migrationBuilder.CreateTable(
                name: "ProductInspection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    ProductCode = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    ManufacturingDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Shape = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    IsShapePass = table.Column<bool>(type: "boolean", nullable: true),
                    ParticleSize = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    IsParticleSizePass = table.Column<bool>(type: "boolean", nullable: true),
                    PackingSpec = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    IsPackingSpecPass = table.Column<bool>(type: "boolean", nullable: true),
                    VisualCheck = table.Column<bool>(type: "boolean", nullable: true),
                    ColorDeltaE = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsColorDeltaEPass = table.Column<bool>(type: "boolean", nullable: true),
                    Moisture = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsMoisturePass = table.Column<bool>(type: "boolean", nullable: true),
                    MFR = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsMFRPass = table.Column<bool>(type: "boolean", nullable: true),
                    FlexuralStrength = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsFlexuralStrengthPass = table.Column<bool>(type: "boolean", nullable: true),
                    Elongation = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsElongationPass = table.Column<bool>(type: "boolean", nullable: true),
                    Hardness = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsHardnessPass = table.Column<bool>(type: "boolean", nullable: true),
                    Density = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsDensityPass = table.Column<bool>(type: "boolean", nullable: true),
                    TensileStrength = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsTensileStrengthPass = table.Column<bool>(type: "boolean", nullable: true),
                    FlexuralModulus = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsFlexuralModulusPass = table.Column<bool>(type: "boolean", nullable: true),
                    ImpactResistance = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsImpactResistancePass = table.Column<bool>(type: "boolean", nullable: true),
                    Antistatic = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    IsAntistaticPass = table.Column<bool>(type: "boolean", nullable: true),
                    StorageCondition = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    IsStorageConditionPass = table.Column<bool>(type: "boolean", nullable: true),
                    DwellTime = table.Column<bool>(type: "boolean", nullable: true),
                    BlackDots = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    MigrationTest = table.Column<bool>(type: "boolean", nullable: true),
                    Defect_Impurity = table.Column<bool>(type: "boolean", nullable: true),
                    Defect_BlackDot = table.Column<bool>(type: "boolean", nullable: true),
                    Defect_ShortFiber = table.Column<bool>(type: "boolean", nullable: true),
                    Defect_Moist = table.Column<bool>(type: "boolean", nullable: true),
                    Defect_Dusty = table.Column<bool>(type: "boolean", nullable: true),
                    DeliveryAccepted = table.Column<bool>(type: "boolean", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProductName = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    ProductStandardId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Types = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Defect_WrongColor = table.Column<bool>(type: "boolean", nullable: true),
                    MeshType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsMeshAttached = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductI__3214EC072F6AD3E1", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStandard",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ProductExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    DeltaE = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    PelletSize = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Moisture = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Density = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    MeltIndex = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    TensileStrength = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    ElongationAtBreak = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    FlexuralStrength = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    FlexuralModulus = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    IzodImpactStrength = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Hardness = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    DwellTime = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    BlackDots = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    MigrationTest = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Package = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    customerExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    colourCode = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Shape = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductS__3214EC0715B96228", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTest",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    externalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    product_externalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    product_customerExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ManufacturingDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    product_package = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    product_weight = table.Column<int>(type: "integer", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductT__3213E83F975320C1", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SchedualMfg",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    MachineId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Number = table.Column<int>(type: "integer", nullable: true),
                    ColorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ColorCode = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    CustomerExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomerRequiredDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    VerifyBatches = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    BagType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ExpectedCompletionDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    PlanDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    QCStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Schedual__3214EC07A98DEC4E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers_material_data",
                columns: table => new
                {
                    supplierId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    externalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    regNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    taxNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    website = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__DB8E62ED69C94CF3", x => x.supplierId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "hr",
                columns: table => new
                {
                    EmployeeID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Identifier = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PartID = table.Column<Guid>(type: "uuid", nullable: true),
                    LevelID = table.Column<Guid>(type: "uuid", nullable: true),
                    DateHired = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__7AD04FF1C1895B9F", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK_Employees_Parts",
                        column: x => x.PartID,
                        principalSchema: "hr",
                        principalTable: "Parts",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "Employees_Common_data",
                columns: table => new
                {
                    EmployeeID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    FullName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Identifier = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PartID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    LevelID = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    DateHired = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__7AD04FF184E31508", x => x.EmployeeID);
                    table.ForeignKey(
                        name: "FK__Employees__Level__693CA210",
                        column: x => x.LevelID,
                        principalTable: "ApprovalLevels_Common_data",
                        principalColumn: "LevelID");
                    table.ForeignKey(
                        name: "FK__Employees__PartI__68487DD7",
                        column: x => x.PartID,
                        principalTable: "Parts_Common_data",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "Machines_Common_data",
                columns: table => new
                {
                    MachineID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    MachineName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GroupID = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PartID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    GroupMachine = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Factory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "Tam Phước")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Machines__5A97603FADC7D2ED", x => x.MachineID);
                    table.ForeignKey(
                        name: "FK__Machines__GroupI__6477ECF3",
                        column: x => x.GroupID,
                        principalTable: "Groups_Common_data",
                        principalColumn: "GroupID");
                    table.ForeignKey(
                        name: "FK__Machines__PartID__656C112C",
                        column: x => x.PartID,
                        principalTable: "Parts_Common_data",
                        principalColumn: "PartID");
                });

            migrationBuilder.CreateTable(
                name: "QCDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    BatchExternalId = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    MachineExternalId = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "company",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Companie__2D971CAC11C45530", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_Companies_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Companies_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                schema: "inventory",
                columns: table => new
                {
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Symbol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Units__44F5ECB5E080D698", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Materials_material_data",
                columns: table => new
                {
                    materialId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    externalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    EmployeeID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    MaterialGroupId = table.Column<Guid>(type: "uuid", nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__99B653FDEEB02A00", x => x.materialId);
                    table.ForeignKey(
                        name: "FK__Materials__Emplo__6DCC4D03",
                        column: x => x.EmployeeID,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Materials__Mater__6CD828CA",
                        column: x => x.MaterialGroupId,
                        principalTable: "MaterialGroups_Material_data",
                        principalColumn: "MaterialGroupID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders_material_data",
                columns: table => new
                {
                    POID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    POCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    EmployeeId = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ContactName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VendorAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    VendorPhone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    InvoiceNote = table.Column<string>(type: "text", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeliveryContact = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Packaging = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PaymentTerm = table.Column<string>(type: "text", nullable: true),
                    RequiredDocuments = table.Column<string>(type: "text", nullable: true),
                    RequiredDocuments_Eng = table.Column<string>(type: "text", nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__5F02A2F4EB754C0F", x => x.POID);
                    table.ForeignKey(
                        name: "FK__PurchaseO__Emplo__320C68B7",
                        column: x => x.EmployeeId,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__PurchaseO__Suppl__33008CF0",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers_material_data",
                        principalColumn: "supplierId");
                });

            migrationBuilder.CreateTable(
                name: "SupplyRequests_Material_data",
                columns: table => new
                {
                    RequestID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    EmployeeID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    RequestStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    NoteCancel = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupplyRe__33A8519AEED4B83E", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK__SupplyReq__Emplo__6C190EBB",
                        column: x => x.EmployeeID,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "company",
                columns: table => new
                {
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Code = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Branches__A1682FC5D195FBDD", x => x.BranchId);
                    table.ForeignKey(
                        name: "FK_Branches_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Branches_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Branches__Compan__0C70CFB4",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "inventory",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Types = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__19093A0B0352530F", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "sales",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CustomerGroup = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ApplicationName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    TaxNumber = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    IssuedPlace = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FaxNumber = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Product = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64D875977EBB", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Groups_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Groups_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Customer__Compan__1E8F7FEF",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "company",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    GroupType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExternalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Groups__149AF36A3DC9A844", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Groups_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK__Groups__CompanyI__131DCD43",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "inventory",
                columns: table => new
                {
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Group = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Application = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RegNo = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    TaxNo = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "character varying(500)", unicode: false, maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__4BE666B4029CD1B8", x => x.SupplierId);
                    table.ForeignKey(
                        name: "FK_Suppliers_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Suppliers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "SupplyRequests",
                schema: "SupplyRequest",
                columns: table => new
                {
                    RequestID = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestStatus = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    RequestSourceType = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CancelNote = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SupplyRe__33A8519A7A78FE1B", x => x.RequestID);
                    table.ForeignKey(
                        name: "FK_SupplyRequest_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_SupplyRequest_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_SupplyRequest_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory_material_data",
                columns: table => new
                {
                    priceHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    supplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    oldPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    newPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    updatedBy = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PriceHis__77D1486CB71DF1F1", x => x.priceHistoryId);
                    table.ForeignKey(
                        name: "FK__PriceHist__mater__793DFFAF",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK__PriceHist__suppl__2D47B39A",
                        column: x => x.supplierId,
                        principalTable: "Suppliers_material_data",
                        principalColumn: "supplierId");
                    table.ForeignKey(
                        name: "FK__PriceHist__updat__2E3BD7D3",
                        column: x => x.updatedBy,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderStatusHistory_material_data",
                columns: table => new
                {
                    StatusHistoryId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    POID = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusFrom = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StatusTo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__DB9734917B73E8D7", x => x.StatusHistoryId);
                    table.ForeignKey(
                        name: "FK__PurchaseOr__POID__34E8D562",
                        column: x => x.POID,
                        principalTable: "PurchaseOrders_material_data",
                        principalColumn: "POID");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalHistory_Material_data",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    EmployeeID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Approval__3214EC27EC140479", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_Employee",
                        column: x => x.EmployeeID,
                        principalTable: "Employees_Common_data",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_Request",
                        column: x => x.RequestID,
                        principalTable: "SupplyRequests_Material_data",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "RequestDetail_Material_data",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    RequestedQuantity = table.Column<int>(type: "integer", nullable: true),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    PurchasedQuantity = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    ReceivedQuantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RequestD__135C314D44B7445B", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_RequestDetail_Material",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK_RequestDetail_Request",
                        column: x => x.RequestID,
                        principalTable: "SupplyRequests_Material_data",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                schema: "inventory",
                columns: table => new
                {
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomCode = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    UnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Package = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MinQuantity = table.Column<double>(type: "double precision", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    Barcode = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    ImagePath = table.Column<string>(type: "character varying(300)", unicode: false, maxLength: 300, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__C50610F7C355BA5C", x => x.MaterialId);
                    table.ForeignKey(
                        name: "FK_Materials_Category",
                        column: x => x.CategoryId,
                        principalSchema: "inventory",
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Materials_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Materials_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Materials_Unit",
                        column: x => x.UnitId,
                        principalSchema: "inventory",
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_Materials_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "labs",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ColourCode = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ColourName = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    Additive = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UsageRate = table.Column<double>(type: "double precision", nullable: true),
                    DeltaE = table.Column<double>(type: "double precision", nullable: true),
                    Requirement = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExpiryType = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    StorageCondition = table.Column<string>(type: "character varying(200)", unicode: false, maxLength: 200, nullable: true),
                    LabComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ProductType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Procedure = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecycleRate = table.Column<double>(type: "double precision", nullable: true),
                    TaicalRate = table.Column<double>(type: "double precision", nullable: true),
                    Application = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ProductUsage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PolymerMatchedIn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EndUser = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FoodSafety = table.Column<bool>(type: "boolean", nullable: true),
                    RohsStandard = table.Column<bool>(type: "boolean", nullable: true),
                    MaxTemp = table.Column<double>(type: "double precision", nullable: true),
                    WeatherResistance = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    LightCondition = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    VisualTest = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ReturnSample = table.Column<bool>(type: "boolean", nullable: true),
                    OtherComment = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Weight = table.Column<double>(type: "double precision", nullable: true),
                    Unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__B40CC6CD344F4294", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Category",
                        column: x => x.CategoryId,
                        principalSchema: "inventory",
                        principalTable: "Categories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_Products_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Products_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Products_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                schema: "sales",
                columns: table => new
                {
                    AddressID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerID = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressLine = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Address__091C2A1BCCA306B0", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_Customer",
                        column: x => x.CustomerID,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                schema: "sales",
                columns: table => new
                {
                    ContactID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerID = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Contacts__5C6625BB570D4F62", x => x.ContactID);
                    table.ForeignKey(
                        name: "FK_Contacts_Customer",
                        column: x => x.CustomerID,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerAssignment",
                schema: "sales",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomerID = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupID = table.Column<Guid>(type: "uuid", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false),
                    updatedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__3214EC279397A842", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerAssignment_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_CustomerAssignment_CreatedBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_CustomerAssignment_Customer",
                        column: x => x.CustomerID,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_CustomerAssignment_EmployeeID",
                        column: x => x.EmployeeID,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_CustomerAssignment_updatedBy",
                        column: x => x.updatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "MerchandiseOrders",
                schema: "Orders",
                columns: table => new
                {
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManagerById = table.Column<Guid>(type: "uuid", nullable: true),
                    Receiver = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DeliveryAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PaymentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ShippingMethod = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Merchand__D0AB7E7AFDA62167", x => x.MerchandiseOrderId);
                    table.ForeignKey(
                        name: "FK_MerchandiseOrders_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderst_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderst_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderst_ManagerById",
                        column: x => x.ManagerById,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderst_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "CustomerTransferLog",
                schema: "sales",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FromEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdBy = table.Column<Guid>(type: "uuid", nullable: false),
                    companyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__3214EC276977D605", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CustomerTransferLog_Company",
                        column: x => x.companyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_CustomerTransferLog_CreatedBy",
                        column: x => x.createdBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_CustomerTransferLog_FromEmployeeId",
                        column: x => x.FromEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_CustomerTransferLog_FromGroupId",
                        column: x => x.FromGroupId,
                        principalSchema: "company",
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_CustomerTransferLog_ToEmployeeId",
                        column: x => x.ToEmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_CustomerTransferLog_ToGroupId",
                        column: x => x.ToGroupId,
                        principalSchema: "company",
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "MemberInGroup",
                schema: "company",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    Profile = table.Column<Guid>(type: "uuid", nullable: true),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MemberIn__0CF04B189ED315D5", x => x.MemberId);
                    table.ForeignKey(
                        name: "FK_MemberInGroup_Groups",
                        column: x => x.GroupId,
                        principalSchema: "company",
                        principalTable: "Groups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK_MemberInGroup_Profile",
                        column: x => x.Profile,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "inventory",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    RequestSourceType = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    RequestSourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(16,2)", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PackageType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PaymentDays = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VAT = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__036BACA44B53DA7A", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "inventory",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "SupplierAddresses",
                schema: "inventory",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressLine = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    District = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__091C2AFB0B8862E7", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_SupplierAddress_Supplier",
                        column: x => x.SupplierId,
                        principalSchema: "inventory",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "SupplierContacts",
                schema: "inventory",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Supplier__5C66259B7A3571DD", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_SupplierContact_Supplier",
                        column: x => x.SupplierId,
                        principalSchema: "inventory",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalHistory",
                schema: "SupplyRequest",
                columns: table => new
                {
                    ApprovalID = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestID = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    EmployeeID = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Approval__328477D4B3FEB4F1", x => x.ApprovalID);
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_EmployeeID",
                        column: x => x.EmployeeID,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ApprovalHistory_RequestID",
                        column: x => x.RequestID,
                        principalSchema: "SupplyRequest",
                        principalTable: "SupplyRequests",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "MaterialsSuppliers_material_data",
                columns: table => new
                {
                    Materials_SuppliersId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    materialId = table.Column<Guid>(type: "uuid", nullable: true),
                    supplierId = table.Column<Guid>(type: "uuid", nullable: true),
                    minDeliveryDays = table.Column<int>(type: "integer", nullable: true),
                    currentPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    updatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    isPreferred = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    priceHistoryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__4F13EDBB69530C3D", x => x.Materials_SuppliersId);
                    table.ForeignKey(
                        name: "FK__Materials__mater__7FEAFD3E",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK__Materials__price__2A6B46EF",
                        column: x => x.priceHistoryId,
                        principalTable: "PriceHistory_material_data",
                        principalColumn: "priceHistoryId");
                    table.ForeignKey(
                        name: "FK__Materials__suppl__2B5F6B28",
                        column: x => x.supplierId,
                        principalTable: "Suppliers_material_data",
                        principalColumn: "supplierId");
                });

            migrationBuilder.CreateTable(
                name: "InventoryReceipts_Material_data",
                columns: table => new
                {
                    ReceiptID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestID = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    materialId = table.Column<Guid>(type: "uuid", nullable: false),
                    DetailID = table.Column<int>(type: "integer", nullable: true),
                    ReceiptQty = table.Column<int>(type: "integer", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    ExportedQty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Inventor__CC08C400857F0DFA", x => x.ReceiptID);
                    table.ForeignKey(
                        name: "FK_InventoryReceipts_Material",
                        column: x => x.materialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK_InventoryReceipts_Request",
                        column: x => x.RequestID,
                        principalTable: "SupplyRequests_Material_data",
                        principalColumn: "RequestID");
                    table.ForeignKey(
                        name: "FK_PO_Detail_InventoryReceipt",
                        column: x => x.DetailID,
                        principalTable: "RequestDetail_Material_data",
                        principalColumn: "DetailID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails_material_data",
                columns: table => new
                {
                    PODetailId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    POID = table.Column<Guid>(type: "uuid", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    DetailID = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__4EB47B3EB8D4CA48", x => x.PODetailId);
                    table.ForeignKey(
                        name: "FK_PO_Detail_RequestDetail",
                        column: x => x.DetailID,
                        principalTable: "RequestDetail_Material_data",
                        principalColumn: "DetailID");
                    table.ForeignKey(
                        name: "FK__PurchaseO__Mater__10216507",
                        column: x => x.MaterialId,
                        principalTable: "Materials_material_data",
                        principalColumn: "materialId");
                    table.ForeignKey(
                        name: "FK__PurchaseOr__POID__30242045",
                        column: x => x.POID,
                        principalTable: "PurchaseOrders_material_data",
                        principalColumn: "POID");
                });

            migrationBuilder.CreateTable(
                name: "Materials_Suppliers",
                schema: "inventory",
                columns: table => new
                {
                    Materials_SuppliersId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinDeliveryDays = table.Column<int>(type: "integer", nullable: true),
                    CurrentPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    LastPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPreferred = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Material__4F13EDBB73A34869", x => x.Materials_SuppliersId);
                    table.ForeignKey(
                        name: "FK_MaterialsSuppliers_Material",
                        column: x => x.MaterialId,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_MaterialsSuppliers_Supplier",
                        column: x => x.SupplierId,
                        principalSchema: "inventory",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_MaterialsSuppliers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                schema: "inventory",
                columns: table => new
                {
                    PriceHistoryId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uuid", nullable: false),
                    OldPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    NewPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PriceHis__A927CACB4B3A2EAC", x => x.PriceHistoryId);
                    table.ForeignKey(
                        name: "FK_PriceHistory_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Material",
                        column: x => x.MaterialId,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Supplier",
                        column: x => x.SupplierId,
                        principalSchema: "inventory",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_PriceHistory_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "RequestDetail",
                schema: "SupplyRequest",
                columns: table => new
                {
                    DetailID = table.Column<int>(type: "integer", nullable: false),
                    RequestID = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestedQuantity = table.Column<int>(type: "integer", nullable: true),
                    MaterialID = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestStatus = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RequestD__135C314D66D34B2A", x => x.DetailID);
                    table.ForeignKey(
                        name: "FK_SupplyRequest_MaterialId",
                        column: x => x.MaterialID,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_SupplyRequest_RequestID",
                        column: x => x.RequestID,
                        principalSchema: "SupplyRequest",
                        principalTable: "SupplyRequests",
                        principalColumn: "RequestID");
                });

            migrationBuilder.CreateTable(
                name: "Formulas",
                schema: "labs",
                columns: table => new
                {
                    FormulaId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SentDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    SentBy = table.Column<Guid>(type: "uuid", nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    VerifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(16,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Formulas__227429A55C6F1195", x => x.FormulaId);
                    table.ForeignKey(
                        name: "FK_Formulas_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_Formulas_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Formulas_Product",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_Formulas_SentBy",
                        column: x => x.SentBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Formulas_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_Formulas_VerifiedBy",
                        column: x => x.VerifiedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "PriceHistory",
                schema: "labs",
                columns: table => new
                {
                    PriceHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(16,2)", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", unicode: false, maxLength: 10, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PriceHis__A927CACB8B375541", x => x.PriceHistoryId);
                    table.ForeignKey(
                        name: "FK_PriceHistory_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PriceHistory_Product",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_PriceHistory_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "ProductChangedHistory",
                schema: "labs",
                columns: table => new
                {
                    ProductChangedHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    FieldChanged = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    OldValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    NewValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChangeNote = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChangeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductC__A793B6CA9FB36DED", x => x.ProductChangedHistoryId);
                    table.ForeignKey(
                        name: "FK_ProductHistory_ChangedBy",
                        column: x => x.ChangedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ProductHistory_Product",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductStandards",
                schema: "labs",
                columns: table => new
                {
                    ProductStandardId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeltaE = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    PelletSize = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Moisture = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Density = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    MeltIndex = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    TensileStrength = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    ElongationAtBreak = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    FlexuralStrength = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    FlexuralModulus = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    IzodImpactStrength = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Hardness = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    DwellTime = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    BlackDots = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    MigrationTest = table.Column<string>(type: "character varying(100)", unicode: false, maxLength: 100, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Package = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    CustomerExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ColourCode = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Shape = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductS__A405C52429CBAE28", x => x.ProductStandardId);
                    table.ForeignKey(
                        name: "FK_ProductStandards_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_ProductStandards_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_ProductStandards_Product",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_ProductStandards_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "MerchandiseOrderDetails",
                schema: "Orders",
                columns: table => new
                {
                    MerchandiseOrderDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    FormulaExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    ExpectedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    BagType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Merchand__FE0FB3FF67BDE750", x => x.MerchandiseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderDetails_MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId");
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderDetails_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "MerchandiseOrderSchedules",
                schema: "Orders",
                columns: table => new
                {
                    MerchandiseOrderScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchandiseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Merchand__B49D545E8C296524", x => x.MerchandiseOrderScheduleId);
                    table.ForeignKey(
                        name: "FK_MerchandiseOrderSchedules_MerchandiseOrderId",
                        column: x => x.MerchandiseOrderId,
                        principalSchema: "Orders",
                        principalTable: "MerchandiseOrders",
                        principalColumn: "MerchandiseOrderId");
                });

            migrationBuilder.CreateTable(
                name: "DetailCustomerTransfer",
                schema: "sales",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerID = table.Column<Guid>(type: "uuid", nullable: false),
                    LogID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DetailCu__3214EC273B3F47A0", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DetailCustomerTransfer_CustomerID",
                        column: x => x.CustomerID,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_DetailCustomerTransfer_LogID",
                        column: x => x.LogID,
                        principalSchema: "sales",
                        principalTable: "CustomerTransferLog",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                schema: "inventory",
                columns: table => new
                {
                    PurchaseOrderDetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uuid", nullable: true),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<double>(type: "double precision", nullable: true),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__5026B698A94EFE68", x => x.PurchaseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "inventory",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrdersSchedules",
                schema: "inventory",
                columns: table => new
                {
                    PurchaseOrdersScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Quantity = table.Column<double>(type: "double precision", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__658C1532BFBEF664", x => x.PurchaseOrdersScheduleId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrdersSchedules_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "inventory",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderStatusHistory",
                schema: "inventory",
                columns: table => new
                {
                    StatusHistoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusFrom = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    StatusTo = table.Column<string>(type: "character varying(16)", unicode: false, maxLength: 16, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ChangedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Purchase__DB973491370CD24F", x => x.StatusHistoryId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderStatusHistory_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderStatusHistory_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "inventory",
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId");
                });

            migrationBuilder.CreateTable(
                name: "FormulaMaterials",
                schema: "labs",
                columns: table => new
                {
                    FormulaMaterialId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    FormulaId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaterialType = table.Column<string>(type: "character varying(20)", unicode: false, maxLength: 20, nullable: true),
                    Quantity = table.Column<double>(type: "double precision", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "numeric(16,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "numeric(16,2)", nullable: true),
                    LotNo = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    SelectedSupplierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FormulaM__0315C60A1F19742A", x => x.FormulaMaterialId);
                    table.ForeignKey(
                        name: "FK_FormulaMaterials_Formula",
                        column: x => x.FormulaId,
                        principalSchema: "labs",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId");
                    table.ForeignKey(
                        name: "FK_FormulaMaterials_Material",
                        column: x => x.MaterialId,
                        principalSchema: "inventory",
                        principalTable: "Materials",
                        principalColumn: "MaterialId");
                    table.ForeignKey(
                        name: "FK_FormulaMaterials_Supplier",
                        column: x => x.SelectedSupplierId,
                        principalSchema: "inventory",
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "SampleRequests",
                schema: "labs",
                columns: table => new
                {
                    SampleRequestId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ExternalId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    RealDeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    RealPriceQuoteDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    ExpectedPriceQuoteDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    AdditionalComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RequestType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExpectedQuantity = table.Column<double>(type: "double precision", nullable: true),
                    ExpectedPrice = table.Column<decimal>(type: "numeric(18,4)", nullable: true),
                    SampleQuantity = table.Column<double>(type: "double precision", nullable: true),
                    OtherComment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    InfoType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FormulaId = table.Column<Guid>(type: "uuid", nullable: true),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Branch = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Package = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SampleRe__6F83B553A1A0D2A8", x => x.SampleRequestId);
                    table.ForeignKey(
                        name: "FK_SampleRequests_Company",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_SampleRequests_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_SampleRequests_Customer",
                        column: x => x.CustomerId,
                        principalSchema: "sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_SampleRequests_Formula",
                        column: x => x.FormulaId,
                        principalSchema: "labs",
                        principalTable: "Formulas",
                        principalColumn: "FormulaId");
                    table.ForeignKey(
                        name: "FK_SampleRequests_Manager",
                        column: x => x.ManagerBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                    table.ForeignKey(
                        name: "FK_SampleRequests_Product",
                        column: x => x.ProductId,
                        principalSchema: "labs",
                        principalTable: "Products",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK_SampleRequests_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "hr",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CustomerID",
                schema: "sales",
                table: "Address",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_EmployeeID",
                schema: "SupplyRequest",
                table: "ApprovalHistory",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_RequestID",
                schema: "SupplyRequest",
                table: "ApprovalHistory",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_Material_data_EmployeeID",
                table: "ApprovalHistory_Material_data",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistory_Material_data_RequestID",
                table: "ApprovalHistory_Material_data",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyId",
                schema: "company",
                table: "Branches",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedBy",
                schema: "company",
                table: "Branches",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_UpdatedBy",
                schema: "company",
                table: "Branches",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CompanyId",
                schema: "inventory",
                table: "Categories",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CreatedBy",
                schema: "company",
                table: "Companies",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_UpdatedBy",
                schema: "company",
                table: "Companies",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CustomerID",
                schema: "sales",
                table: "Contacts",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CompanyId",
                schema: "sales",
                table: "Customer",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedBy",
                schema: "sales",
                table: "Customer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UpdatedBy",
                schema: "sales",
                table: "Customer",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_companyId",
                schema: "sales",
                table: "CustomerAssignment",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_createdBy",
                schema: "sales",
                table: "CustomerAssignment",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_CustomerID",
                schema: "sales",
                table: "CustomerAssignment",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_EmployeeID",
                schema: "sales",
                table: "CustomerAssignment",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAssignment_updatedBy",
                schema: "sales",
                table: "CustomerAssignment",
                column: "updatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_companyId",
                schema: "sales",
                table: "CustomerTransferLog",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_createdBy",
                schema: "sales",
                table: "CustomerTransferLog",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_FromEmployeeId",
                schema: "sales",
                table: "CustomerTransferLog",
                column: "FromEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_FromGroupId",
                schema: "sales",
                table: "CustomerTransferLog",
                column: "FromGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_ToEmployeeId",
                schema: "sales",
                table: "CustomerTransferLog",
                column: "ToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerTransferLog_ToGroupId",
                schema: "sales",
                table: "CustomerTransferLog",
                column: "ToGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailCustomerTransfer_CustomerID",
                schema: "sales",
                table: "DetailCustomerTransfer",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_DetailCustomerTransfer_LogID",
                schema: "sales",
                table: "DetailCustomerTransfer",
                column: "LogID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PartID",
                schema: "hr",
                table: "Employees",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Common_data_LevelID",
                table: "Employees_Common_data",
                column: "LevelID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Common_data_PartID",
                table: "Employees_Common_data",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaMaterials_FormulaId",
                schema: "labs",
                table: "FormulaMaterials",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaMaterials_MaterialId",
                schema: "labs",
                table: "FormulaMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_FormulaMaterials_SelectedSupplierId",
                schema: "labs",
                table: "FormulaMaterials",
                column: "SelectedSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_CompanyId",
                schema: "labs",
                table: "Formulas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_CreatedBy",
                schema: "labs",
                table: "Formulas",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_ProductId",
                schema: "labs",
                table: "Formulas",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_SentBy",
                schema: "labs",
                table: "Formulas",
                column: "SentBy");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_UpdatedBy",
                schema: "labs",
                table: "Formulas",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Formulas_VerifiedBy",
                schema: "labs",
                table: "Formulas",
                column: "VerifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CompanyId",
                schema: "company",
                table: "Groups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatedBy",
                schema: "company",
                table: "Groups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UpdatedBy",
                schema: "company",
                table: "Groups",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_Material_data_DetailID",
                table: "InventoryReceipts_Material_data",
                column: "DetailID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_Material_data_materialId",
                table: "InventoryReceipts_Material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipts_Material_data_RequestID",
                table: "InventoryReceipts_Material_data",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Common_data_GroupID",
                table: "Machines_Common_data",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_Machines_Common_data_PartID",
                table: "Machines_Common_data",
                column: "PartID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CategoryId",
                schema: "inventory",
                table: "Materials",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CompanyId",
                schema: "inventory",
                table: "Materials",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CreatedBy",
                schema: "inventory",
                table: "Materials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UnitId",
                schema: "inventory",
                table: "Materials",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UpdatedBy",
                schema: "inventory",
                table: "Materials",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_material_data_EmployeeID",
                table: "Materials_material_data",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_material_data_MaterialGroupId",
                table: "Materials_material_data",
                column: "MaterialGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Suppliers_MaterialId",
                schema: "inventory",
                table: "Materials_Suppliers",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Suppliers_SupplierId",
                schema: "inventory",
                table: "Materials_Suppliers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Suppliers_UpdatedBy",
                schema: "inventory",
                table: "Materials_Suppliers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSuppliers_material_data_materialId",
                table: "MaterialsSuppliers_material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSuppliers_material_data_priceHistoryId",
                table: "MaterialsSuppliers_material_data",
                column: "priceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsSuppliers_material_data_supplierId",
                table: "MaterialsSuppliers_material_data",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInGroup_GroupId",
                schema: "company",
                table: "MemberInGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInGroup_Profile",
                schema: "company",
                table: "MemberInGroup",
                column: "Profile");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderDetails_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderDetails_ProductId",
                schema: "Orders",
                table: "MerchandiseOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrders_CompanyId",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrders_CreatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrders_CustomerId",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrders_ManagerById",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "ManagerById");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrders_UpdatedBy",
                schema: "Orders",
                table: "MerchandiseOrders",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MerchandiseOrderSchedules_MerchandiseOrderId",
                schema: "Orders",
                table: "MerchandiseOrderSchedules",
                column: "MerchandiseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_CreatedBy",
                schema: "inventory",
                table: "PriceHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_MaterialId",
                schema: "inventory",
                table: "PriceHistory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_SupplierId",
                schema: "inventory",
                table: "PriceHistory",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_UpdatedBy",
                schema: "inventory",
                table: "PriceHistory",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_CreatedBy1",
                schema: "labs",
                table: "PriceHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_ProductId",
                schema: "labs",
                table: "PriceHistory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_UpdatedBy1",
                schema: "labs",
                table: "PriceHistory",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_material_data_materialId",
                table: "PriceHistory_material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_material_data_supplierId",
                table: "PriceHistory_material_data",
                column: "supplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceHistory_material_data_updatedBy",
                table: "PriceHistory_material_data",
                column: "updatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChangedHistory_ChangedBy",
                schema: "labs",
                table: "ProductChangedHistory",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChangedHistory_ProductId",
                schema: "labs",
                table: "ProductChangedHistory",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "labs",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CompanyId",
                schema: "labs",
                table: "Products",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedBy",
                schema: "labs",
                table: "Products",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UpdatedBy",
                schema: "labs",
                table: "Products",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandards_CompanyId",
                schema: "labs",
                table: "ProductStandards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandards_CreatedBy",
                schema: "labs",
                table: "ProductStandards",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandards_ProductId",
                schema: "labs",
                table: "ProductStandards",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStandards_UpdatedBy",
                schema: "labs",
                table: "ProductStandards",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_MaterialId",
                schema: "inventory",
                table: "PurchaseOrderDetails",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderId",
                schema: "inventory",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_material_data_DetailID",
                table: "PurchaseOrderDetails_material_data",
                column: "DetailID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_material_data_MaterialId",
                table: "PurchaseOrderDetails_material_data",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_material_data_POID",
                table: "PurchaseOrderDetails_material_data",
                column: "POID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CompanyId",
                schema: "inventory",
                table: "PurchaseOrders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CreatedBy",
                schema: "inventory",
                table: "PurchaseOrders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierId",
                schema: "inventory",
                table: "PurchaseOrders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_UpdatedBy",
                schema: "inventory",
                table: "PurchaseOrders",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_material_data_EmployeeId",
                table: "PurchaseOrders_material_data",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_material_data_SupplierId",
                table: "PurchaseOrders_material_data",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrdersSchedules_PurchaseOrderId",
                schema: "inventory",
                table: "PurchaseOrdersSchedules",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderStatusHistory_EmployeeId",
                schema: "inventory",
                table: "PurchaseOrderStatusHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderStatusHistory_PurchaseOrderId",
                schema: "inventory",
                table: "PurchaseOrderStatusHistory",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderStatusHistory_material_data_POID",
                table: "PurchaseOrderStatusHistory_material_data",
                column: "POID");

            migrationBuilder.CreateIndex(
                name: "IX_QCDetail_BatchId",
                table: "QCDetail",
                column: "BatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_MaterialID",
                schema: "SupplyRequest",
                table: "RequestDetail",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_RequestID",
                schema: "SupplyRequest",
                table: "RequestDetail",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_Material_data_materialId",
                table: "RequestDetail_Material_data",
                column: "materialId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDetail_Material_data_RequestID",
                table: "RequestDetail_Material_data",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_CompanyId",
                schema: "labs",
                table: "SampleRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_CreatedBy",
                schema: "labs",
                table: "SampleRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_CustomerId",
                schema: "labs",
                table: "SampleRequests",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_FormulaId",
                schema: "labs",
                table: "SampleRequests",
                column: "FormulaId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_ManagerBy",
                schema: "labs",
                table: "SampleRequests",
                column: "ManagerBy");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_ProductId",
                schema: "labs",
                table: "SampleRequests",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleRequests_UpdatedBy",
                schema: "labs",
                table: "SampleRequests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierAddresses_SupplierId",
                schema: "inventory",
                table: "SupplierAddresses",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierContacts_SupplierId",
                schema: "inventory",
                table: "SupplierContacts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CompanyId",
                schema: "inventory",
                table: "Suppliers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatedBy",
                schema: "inventory",
                table: "Suppliers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_CompanyId",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_CreatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_UpdatedBy",
                schema: "SupplyRequest",
                table: "SupplyRequests",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyRequests_Material_data_EmployeeID",
                table: "SupplyRequests_Material_data",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Units_CreatedBy",
                schema: "inventory",
                table: "Units",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "ApprovalHistory",
                schema: "SupplyRequest");

            migrationBuilder.DropTable(
                name: "ApprovalHistory_Material_data");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "company");

            migrationBuilder.DropTable(
                name: "Contacts",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "CustomerAssignment",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "DetailCustomerTransfer",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "FormulaMaterials",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "InventoryReceipts_Material_data");

            migrationBuilder.DropTable(
                name: "Machines_Common_data");

            migrationBuilder.DropTable(
                name: "MaterialGroups");

            migrationBuilder.DropTable(
                name: "Materials_Suppliers",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "MaterialsSuppliers_material_data");

            migrationBuilder.DropTable(
                name: "MemberInGroup",
                schema: "company");

            migrationBuilder.DropTable(
                name: "MerchandiseOrderDetails",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "MerchandiseOrderSchedules",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "MfgProductionOrdersPlan");

            migrationBuilder.DropTable(
                name: "PriceHistory",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "PriceHistory",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "ProductChangedHistory",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "ProductStandard");

            migrationBuilder.DropTable(
                name: "ProductStandards",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "ProductTest");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails_material_data");

            migrationBuilder.DropTable(
                name: "PurchaseOrdersSchedules",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "PurchaseOrderStatusHistory",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "PurchaseOrderStatusHistory_material_data");

            migrationBuilder.DropTable(
                name: "QCDetail");

            migrationBuilder.DropTable(
                name: "RequestDetail",
                schema: "SupplyRequest");

            migrationBuilder.DropTable(
                name: "SampleRequests",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "SchedualMfg");

            migrationBuilder.DropTable(
                name: "SupplierAddresses",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "SupplierContacts",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CustomerTransferLog",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "Groups_Common_data");

            migrationBuilder.DropTable(
                name: "PriceHistory_material_data");

            migrationBuilder.DropTable(
                name: "MerchandiseOrders",
                schema: "Orders");

            migrationBuilder.DropTable(
                name: "RequestDetail_Material_data");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "PurchaseOrders_material_data");

            migrationBuilder.DropTable(
                name: "ProductInspection");

            migrationBuilder.DropTable(
                name: "Materials",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "SupplyRequests",
                schema: "SupplyRequest");

            migrationBuilder.DropTable(
                name: "Formulas",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "company");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "Materials_material_data");

            migrationBuilder.DropTable(
                name: "SupplyRequests_Material_data");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Suppliers_material_data");

            migrationBuilder.DropTable(
                name: "Units",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "labs");

            migrationBuilder.DropTable(
                name: "MaterialGroups_Material_data");

            migrationBuilder.DropTable(
                name: "Employees_Common_data");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ApprovalLevels_Common_data");

            migrationBuilder.DropTable(
                name: "Parts_Common_data");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "company");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "hr");

            migrationBuilder.DropTable(
                name: "Parts",
                schema: "hr");
        }
    }
}
