
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using QuestPDF.Infrastructure;
using System.Net;
using System.Reflection.Metadata;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Core.Domain.Entities.AuditSchema;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
using VietausWebAPI.Core.Domain.Entities.DeliverySchema;
using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;
using VietausWebAPI.Core.Domain.Entities.EnergyScheme;
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.MaterialSchema;
using VietausWebAPI.Core.Domain.Entities.MROSchema;
using VietausWebAPI.Core.Domain.Entities.OrderSchema;
using VietausWebAPI.Core.Domain.Entities.SampleRequestSchema;
using VietausWebAPI.Core.Domain.Entities.SupplyRequestSchema;
using VietausWebAPI.Core.Domain.Entities.WarehouseSchema;
using VietausWebAPI.Core.Domain.Enums.Manufacturings;




//using System.Text.RegularExpressions;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Infrastructure.Helpers.IdCounter;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext
{
    // Scaffold-DbContext "Server=DESKTOP-BL5L5IM;Database=VietausDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -context ApplicationDbContext


    //Scaffold-DbContext "Host=Localhost;Port=5432;Database=VietausDb;Username=postgres;Password=qazwsxedc123@" 

    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<IdCounter> IdCounters { get; set; } = default!;

        public virtual DbSet<AttachmentCollection> AttachmentCollections { get; set; }
        public virtual DbSet<AttachmentModel> AttachmentModels { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Company> Companies { get; set; }


        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerAssignment> CustomerAssignments { get; set; }

        public virtual DbSet<CustomerTransferLog> CustomerTransferLogs { get; set; }

        public virtual DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public virtual DbSet<DeliveryOrderPO> DeliveryOrderPOs { get; set; }

        public virtual DbSet<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }

        public virtual DbSet<DelivererInfor> DelivererInfors { get; set; }

        public virtual DbSet<Deliverer> Deliverers { get; set; } 

        public virtual DbSet<DetailCustomerTransfer> DetailCustomerTransfers { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Formula> Formulas { get; set; }

        public virtual DbSet<FormulaMaterial> FormulaMaterials { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<MemberInGroup> MemberInGroups { get; set; }

        public virtual DbSet<MfgProductionOrdersPlan> MfgProductionOrdersPlans { get; set; }

        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        //public virtual DbSet<ProductInspection> ProductInspections { get; set; }

        //public virtual DbSet<ProductStandard> ProductStandards { get; set; }

        //public virtual DbSet<ProductTest> ProductTests { get; set; }
        //public virtual DbSet<Qcdetail> Qcdetails { get; set; }
        public virtual DbSet<SampleRequest> SampleRequests { get; set; }




        // ======================================================================== SupplyRequest Module ========================================================================
        //public virtual DbSet<SupplyRequest> SupplyRequests { get; set; }
        //public virtual DbSet<SupplyRequestDetail> SupplyRequestDetails { get; set; }

        // ======================================================================== ManufacturingSchema Module ======================================================================== 
        //public virtual DbSet<MfgProductionOrder> MfgProductionOrders { get; set; }
        //public virtual DbSet<ManufacturingFormulaMaterial> ManufacturingFormulaMaterials { get; set; }
        //public virtual DbSet<ManufacturingFormula> ManufacturingFormulas { get; set; }
        //public virtual DbSet<ManufacturingFormulaVersion> ManufacturingFormulaVersions { get; set; }
        //public virtual DbSet<ManufacturingFormulaVersionItem> ManufacturingFormulaVersionItems { get; set; }
        //public virtual DbSet<ProductionSelectVersion> ProductionSelectVersions { get; set; }
        //public virtual DbSet<ProductStandardFormula> ProductStandardFormulas { get; set; }
        //public virtual DbSet<MfgOrderPO> MfgOrderPOs { get; set; }
        //public virtual DbSet<SchedualMfg> SchedualMfgs { get; set; }

        // ======================================================================== MRO Module ======================================================================== 

        //public virtual DbSet<AreaMRO> AreaMROs { get; set; }
        //public virtual DbSet<WarehouseMRO> WarehouseMROs { get; set; }
        //public virtual DbSet<ZoneMRO> ZoneMROs { get; set; }
        //public virtual DbSet<RackMRO> RackMROs { get; set; }
        //public virtual DbSet<SlotMRO> SlotMROs { get; set; }
        //public virtual DbSet<EquipmentMRO> EquipmentMROs { get; set; }
        //public virtual DbSet<EquipmentDetailMRO> EquipmentDetailMROs { get; set; }
        //public virtual DbSet<EquipmentTypeMRO> EquipmentTypeMROs { get; set; }
        //public virtual DbSet<EquipmentSpecMRO> EquipmentSpecMROs { get; set; }
        //public virtual DbSet<IncidentHeaderMRO> IncidentHeaderMROs { get; set; }
        //public virtual DbSet<IncidentLineMRO> IncidentLineMROs { get; set; }
        //public virtual DbSet<StockOutHeaderMRO> StockOutHeaderMROs { get; set; }
        //public virtual DbSet<StockOutLineMRO> StockOutLineMROs { get; set; }
        //public virtual DbSet<ImprovementHdrMRO> ImprovementHdrMROs { get; set; }
        //public virtual DbSet<ImprovementHistoryMRO> ImprovementHistoryMROs { get; set; }
        //public virtual DbSet<PmPlanMRO> PmPlanMROs { get; set; }
        //public virtual DbSet<PmPlanHistoryMRO> PmPlanHistoryMROs { get; set; }
        //public virtual DbSet<MovementMRO> MovementMROs { get; set; }
        //public virtual DbSet<TransferHeaderMRO> TransferHeaderMROs { get; set; }
        //public virtual DbSet<TransferDetailMRO> TransferDetailMROs { get; set; }

        // ======================================================================== Audit Module ======================================================================== 



        //// ======================================================================== Energy Module ======================================================================== 
        //public virtual DbSet<EnergyGroupTariffMap> EnergyGroupTariffMaps { get; set; }
        //public virtual DbSet<EnergyTouCalendar> EnergyTouCalendars { get; set; }
        //public virtual DbSet<EnergyTouException> EnergyTouExceptions { get; set; }
        //public virtual DbSet<EnergyGroup> EnergyGroups { get; set; }
        //public virtual DbSet<EnergyMeter> EnergyMeters { get; set; }
        //public virtual DbSet<EnergyTariff> EnergyTariffs { get; set; }
        //public virtual DbSet<EnergyTariffVersion> EnergyTariffVersions { get; set; }
        //public virtual DbSet<EnergyTariffBandRate> EnergyTariffBandRates { get; set; }
        //public virtual DbSet<EnergyRegisterSnapshot> EnergyRegisterSnapshots { get; set; }
        //public virtual DbSet<EnergyMeterGroupHistory> EnergyMeterGroupHistories { get; set; }
        //public virtual DbSet<EnergyMeterCommConfig> EnergyMeterCommConfigs { get; set; }
        //public virtual DbSet<EnergyTouWindow> EnergyTouWindows { get; set; }
        //public virtual DbSet<EnergyReadingsHourly> EnergyReadingsHourlies { get; set; }
        //public virtual DbSet<EnergyReadingsHourlyVn> EnergyReadingsHourlyVns { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
            AppContext.SetSwitch("Npgsql.EnableParameterLogging", true); // log tham số Npgsql
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
            builder.Properties<DateTime?>().HaveColumnType("timestamp without time zone");
        }




        //49
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


            // Đếm số
            modelBuilder.Entity<IdCounter>(e =>
            {
                e.ToTable("IdCounters", "public");
                e.HasKey(x => new { x.CompanyId, x.Prefix, x.Period });
            });


            // Thiết lập khóa chính
            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Thiết lập quan hệ với ApplicationUser
            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            // Thiết lập quan hệ với ApplicationRole
            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);


            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Employee)
                .WithMany(e => e.ApplicationUsers)   // <— CHỈ RÕ NAVIGATION PHÍA Employee
                .HasForeignKey(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.EmployeeId);

            modelBuilder.Entity<AttachmentModel>(entity =>
            {
                entity.HasKey(e => e.AttachmentId).HasName("PK__AttachmentModel__B3C4DAF6D3F2E2B3");
                entity.ToTable("AttachmentModel", "Attachment");

                entity.Property(e => e.AttachmentId)
                    .HasColumnName("AttachmentModelID")
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator<GuidV7Generator>();

                entity.HasOne(d => d.CreatedByNavigation).WithMany()
                    .HasForeignKey(d => d.CreateBy)
                    .HasConstraintName("FK_AttachmentModel_CreatedBy");
            });

            modelBuilder.Entity<AttachmentCollection>(entity =>
            {
                entity.HasKey(e => e.AttachmentCollectionId).HasName("PK__AttachmentCollection__B3C4DAF6D3F2E2B3");
                entity.ToTable("AttachmentCollection", "Attachment");

                entity.Property(e => e.AttachmentCollectionId)
                    .HasColumnName("AttachmentCollectionID")
                    .ValueGeneratedOnAdd()
                    .HasValueGenerator<GuidV7Generator>();
            });



            // Thiết lập khóa chính
            modelBuilder.Entity<Address>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1BCCA306B0");

                // Tên bảng và schema
                entity.ToTable("Address", "Customer");

                // ===== Columns =====
                entity.Property(e => e.AddressId)
                      .HasColumnName("AddressId")  // Đặt tên chuẩn
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.AddressLine).HasColumnName("AddressLine")
                      .HasColumnType("citext"); // Không phân biệt hoa thường

                entity.Property(e => e.City).HasColumnName("City")
                      .HasColumnType("citext");

                entity.Property(e => e.Country).HasColumnName("Country")
                      .HasColumnType("citext");

                entity.Property(e => e.CustomerId)
                      .HasColumnName("CustomerId");

                entity.Property(e => e.District).HasColumnName("District")
                      .HasColumnType("citext");

                entity.Property(e => e.IsPrimary).HasColumnName("IsPrimary")
                      .HasDefaultValue(false); 

                entity.Property(e => e.PostalCode).HasColumnName("PostalCode")
                      .HasColumnType("citext");

                entity.Property(e => e.Province).HasColumnName("Province")
                      .HasColumnType("citext");

                entity.Property(e => e.IsActive).HasColumnName("IsActive")
                      .HasDefaultValue(true); 

                // ===== Indexes =====
                // Index cho CustomerId để tìm kiếm nhanh các địa chỉ theo khách hàng
                entity.HasIndex(e => e.CustomerId).HasDatabaseName("IX_Address_CustomerId");

                // Index cho IsPrimary, để lọc nhanh địa chỉ chính của khách hàng
                entity.HasIndex(e => new { e.CustomerId, e.IsPrimary }).HasDatabaseName("IX_Address_Customer_IsPrimary");

                // ===== Relationships =====
                // Quan hệ với bảng Customer
                entity.HasOne(d => d.Customer)
                      .WithMany(p => p.Addresses)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.ClientSetNull) // Nếu khách hàng bị xóa thì không xóa địa chỉ, chỉ set null
                      .HasConstraintName("FK_Address_Customer");
            });

            //modelBuilder.Entity<ApprovalHistory>(entity =>
            //{
            //    entity.HasKey(e => e.ApprovalId).HasName("PK__Approval__328477D4B3FEB4F1");

            //    entity.ToTable("ApprovalHistory", "SupplyRequest");

            //    entity.HasIndex(e => e.EmployeeId, "IX_ApprovalHistory_EmployeeID");

            //    entity.HasIndex(e => e.RequestId, "IX_ApprovalHistory_RequestID");

            //    entity.Property(e => e.ApprovalId)
            //        .ValueGeneratedNever()
            //        .HasColumnName("ApprovalID");
            //    entity.Property(e => e.ApprovalStatus).HasMaxLength(16);
            //    entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            //    entity.Property(e => e.RequestId).HasColumnName("RequestID");

            //    entity.HasOne(d => d.Employee).WithMany(p => p.ApprovalHistories)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .HasConstraintName("FK_ApprovalHistory_EmployeeID");

            //    entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistories)
            //        .HasForeignKey(d => d.RequestId)
            //        .HasConstraintName("FK_ApprovalHistory_RequestID");
            //});



            //modelBuilder.Entity<ApprovalHistoryMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC27EC140479");

            //    entity.ToTable("ApprovalHistory_Material_data");

            //    entity.HasIndex(e => e.EmployeeId, "IX_ApprovalHistory_Material_data_EmployeeID");

            //    entity.HasIndex(e => e.RequestId, "IX_ApprovalHistory_Material_data_RequestID");

            //    entity.Property(e => e.Id).HasColumnName("ID");
            //    entity.Property(e => e.EmployeeId)
            //        .HasMaxLength(16)
            //        .HasColumnName("EmployeeID");
            //    entity.Property(e => e.Note).HasColumnType("citext");
            //    entity.Property(e => e.RequestId)
            //        .HasMaxLength(16)
            //        .HasColumnName("RequestID");

            //    entity.HasOne(d => d.Employee).WithMany(p => p.ApprovalHistoryMaterialData)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ApprovalHistory_Employee");

            //    entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistoryMaterialData)
            //        .HasForeignKey(d => d.RequestId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ApprovalHistory_Request");
            //});

            //modelBuilder.Entity<ApprovalLevelsCommonDatum>(entity =>
            //{
            //    entity.HasKey(e => e.LevelId).HasName("PK__Approval__09F03C061CA120B7");

            //    entity.ToTable("ApprovalLevels_Common_data");

            //    entity.Property(e => e.LevelId)
            //        .HasMaxLength(10)
            //        .HasColumnName("LevelID");
            //    entity.Property(e => e.Description).HasColumnType("citext");
            //    entity.Property(e => e.LevelName).HasColumnType("citext");
            //});

            //modelBuilder.Entity<Branch>(entity =>
            //{
            //    entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FC5D195FBDD");

            //    entity.ToTable("Branches", "company");

            //    entity.HasIndex(e => e.CompanyId, "IX_Branches_CompanyId");

            //    entity.HasIndex(e => e.CreatedBy, "IX_Branches_CreatedBy");

            //    entity.HasIndex(e => e.UpdatedBy, "IX_Branches_UpdatedBy");

            //    entity.Property(e => e.BranchId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.Code).HasMaxLength(50);
            //    entity.Property(e => e.IsActive).HasDefaultValue(true);
            //    entity.Property(e => e.Name).HasMaxLength(200);

            //    entity.HasOne(d => d.Company).WithMany(p => p.Branches)
            //        .HasForeignKey(d => d.CompanyId)
            //        .HasConstraintName("FK__Branches__Compan__0C70CFB4");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchCreatedByNavigations)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_Branches_CreatedBy");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BranchUpdatedByNavigations)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_Branches_UpdatedBy");
            //});

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B0352530F");

                entity.ToTable("Categories", "Material");

                entity.HasIndex(e => e.CompanyId, "IX_Categories_CompanyId");

                entity.Property(e => e.CategoryId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Types).HasMaxLength(50);

                entity.HasOne(d => d.Company).WithMany(p => p.Categories)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categories_Company");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CAC11C45530");

                entity.ToTable("Companies", "company");

                entity.HasIndex(e => e.CreatedBy, "IX_Companies_CreatedBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_Companies_UpdatedBy");

                entity.Property(e => e.CompanyId).HasDefaultValueSql("gen_random_uuid()").HasColumnName("companyId");

                entity.Property(e => e.Code)
                      .HasColumnName("companyExternalId")
                      .HasColumnType("citext")   // dùng citext để so sánh không phân biệt hoa/thường
                      .IsRequired();

                entity.Property(e => e.Name).HasMaxLength(200).HasColumnName("name");

                entity.Property(e => e.Address)
                      .HasColumnName("address")
                      .HasColumnType("text");

                entity.Property(e => e.Country)
                      .HasColumnName("country")
                      .HasColumnType("text");

                entity.Property(e => e.ZipCode)
                      .HasColumnName("zipCode")
                      .HasColumnType("text");

                entity.Property(e => e.Phone)
                      .HasColumnName("phone")
                      .HasColumnType("text");

                entity.Property(e => e.Email)
                      .HasColumnName("email")
                      .HasColumnType("text");

                entity.Property(e => e.IsActive)
                      .HasColumnName("isActive")
                      .HasDefaultValue(true);

                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
                entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CompanyCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Companies_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CompanyUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Companies_UpdatedBy");
            });


            modelBuilder.Entity<Contact>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.ContactId).HasName("PK__Contacts__5C6625BB570D4F62");

                // Tên bảng và schema
                entity.ToTable("Contacts", "Customer");

                // ===== Columns =====
                entity.Property(e => e.ContactId).HasColumnName("ContactId")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");

                entity.Property(e => e.Email).HasColumnName("Email")
                      .HasColumnType("citext"); // Không phân biệt hoa thường

                entity.Property(e => e.FirstName).HasColumnName("FirstName")
                      .HasColumnType("citext");

                entity.Property(e => e.Gender).HasColumnName("Gender")
                      .HasColumnType("citext");

                entity.Property(e => e.LastName).HasColumnName("LastName")
                      .HasColumnType("citext");

                entity.Property(e => e.Phone).HasColumnName("Phone")
                      .HasColumnType("citext");

                // ===== Indexes =====
                entity.HasIndex(e => e.CustomerId).HasDatabaseName("IX_Contacts_CustomerId");

                // ===== Relationships =====
                entity.HasOne(d => d.Customer)
                      .WithMany(p => p.Contacts)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.ClientSetNull)  // Khi khách hàng bị xóa, chỉ set null mà không xoá liên hệ
                      .HasConstraintName("FK_Contacts_Customer");
            });


            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D875977EBB");

                entity.ToTable("Customer", "Customer");

                // ===== Columns (đặt HasColumnName y như property) =====
                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerId")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ExternalId).HasColumnName("ExternalId").HasColumnType("citext").IsRequired();
                entity.Property(e => e.CustomerName).HasColumnName("CustomerName").HasColumnType("citext").IsRequired();
                entity.Property(e => e.CustomerGroup).HasColumnName("CustomerGroup").HasColumnType("citext");
                entity.Property(e => e.ApplicationName).HasColumnName("ApplicationName").HasColumnType("citext");
                entity.Property(e => e.RegistrationNumber).HasColumnName("RegistrationNumber").HasColumnType("citext");
                entity.Property(e => e.TaxNumber).HasColumnName("TaxNumber").HasColumnType("citext");
                entity.Property(e => e.Phone).HasColumnName("Phone").HasColumnType("citext");
                entity.Property(e => e.Website).HasColumnName("Website").HasColumnType("citext");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
                entity.Property(e => e.IssueDate).HasColumnName("IssueDate");
                entity.Property(e => e.IssuedPlace).HasColumnName("IssuedPlace").HasColumnType("citext");
                entity.Property(e => e.FaxNumber).HasColumnName("FaxNumber").HasColumnType("citext");

                entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                // ===== Indexes =====
                entity.HasIndex(e => e.CompanyId).HasDatabaseName("IX_Customers_CompanyId");
                entity.HasIndex(e => e.CreatedBy).HasDatabaseName("IX_Customers_CreatedBy");
                entity.HasIndex(e => e.UpdatedBy).HasDatabaseName("IX_Customers_UpdatedBy");

                // Duy nhất theo tenant (ExternalId là mã ngoài của KH)
                entity.HasIndex(e => new { e.CompanyId, e.ExternalId })
                      .IsUnique()
                      .HasDatabaseName("UX_Customers_Company_ExternalId");

                // List/paging trong tenant (CreatedDate DESC)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.CreatedDate, e.CustomerId })
                      .IsDescending(false, false, true, true) // EF Core 8+
                      .HasDatabaseName("IX_Customers_Company_IsActive_CreatedDateDesc");

                // ===== Relationships =====
                entity.HasOne(d => d.Company).WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Customers_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict) // CreatedBy non-nullable
                    .HasConstraintName("FK_Customers_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CustomerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.SetNull)  // UpdatedBy nullable
                    .HasConstraintName("FK_Customers_UpdatedBy");
            });

            modelBuilder.Entity<CustomerAssignment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC279397A842");

                entity.ToTable("CustomerAssignment", "Customer");

                // ===== Columns (đặt HasColumnName y như property) =====
                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId");
                entity.Property(e => e.GroupId).HasColumnName("GroupId");
                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.IsActive).HasColumnName("IsActive")
                      .HasDefaultValue(true);

                // ===== Indexes =====
                // List nhanh theo nhân viên trong tenant, phân trang ổn định (UpdatedDate/Id DESC)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.EmployeeId, e.UpdatedDate, e.Id })
                      .IsDescending(false, false, false, true, true)
                      .HasDatabaseName("IX_CustAssign_Company_Active_Emp_UpdatedDesc");

                // List nhanh theo nhóm (nếu có màn hình theo Group)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.GroupId, e.UpdatedDate, e.Id })
                      .IsDescending(false, false, false, true, true)
                      .HasDatabaseName("IX_CustAssign_Company_Active_Group_UpdatedDesc");

                // Tra cứu theo khách trong tenant
                entity.HasIndex(e => new { e.CompanyId, e.CustomerId, e.IsActive })
                      .HasDatabaseName("IX_CustAssign_Company_Customer_Active");

                // ===== RÀNG BUỘC NGHIỆP VỤ QUAN TRỌNG =====
                // Mỗi khách CHỈ CÓ 1 phân công đang active trong 1 Company
                //entity.HasIndex(e => new { e.CompanyId, e.CustomerId })
                //      .IsUnique()
                //      .HasFilter("\"IsActive\" = TRUE") // partial unique index (PostgreSQL)
                //      .HasDatabaseName("UX_CustAssign_Company_Customer_Active");

                // Nếu bạn muốn "mỗi khách chỉ có 1 phân công active cho MỖI GROUP"
                // thì thay bằng:
                entity.HasIndex(e => new { e.CompanyId, e.CustomerId, e.GroupId })
                      .IsUnique()
                      .HasFilter("\"IsActive\" = TRUE")
                      .HasDatabaseName("UX_CustAssign_Company_Customer_Group_Active");

                // ===== Relationships (non-nullable => Restrict) =====
                entity.HasOne(d => d.Group).WithMany(p => p.CustomerAssignments)
                      .HasForeignKey(d => d.GroupId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerAssignment_Group");

                entity.HasOne(d => d.Company).WithMany(p => p.CustomerAssignments)
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerAssignment_Company");

                entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAssignments)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerAssignment_Customer");

                entity.HasOne(d => d.Employee).WithMany(p => p.CustomerAssignmentEmployees)
                      .HasForeignKey(d => d.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerAssignment_Employee");

                // CreatedBy/UpdatedBy là non-nullable Guid => Restrict (không SetNull)
                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerAssignmentCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerAssignment_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CustomerAssignmentUpdatedByNavigations)
                      .HasForeignKey(d => d.UpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerAssignment_UpdatedBy");
            });

            modelBuilder.Entity<CustomerTransferLog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC276977D605");

                entity.ToTable("CustomerTransferLog", "Customer");

                // Columns (đặt đúng tên property)
                entity.Property(e => e.Id)
                      .HasColumnName("Id")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.FromEmployeeId).HasColumnName("FromEmployeeId");
                entity.Property(e => e.ToEmployeeId).HasColumnName("ToEmployeeId");
                entity.Property(e => e.FromGroupId).HasColumnName("FromGroupId");
                entity.Property(e => e.ToGroupId).HasColumnName("ToGroupId");
                entity.Property(e => e.Note).HasColumnName("Note").HasColumnType("text"); // Note dùng text là hợp lí
                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");

                // Indexes (phân trang & tra cứu theo nhân viên)
                entity.HasIndex(e => new { e.CompanyId, e.CreatedDate, e.Id })
                      .IsDescending(false, true, true)
                      .HasDatabaseName("IX_CustomerTransferLog_Company_CreatedDateDesc");

                entity.HasIndex(e => new { e.CompanyId, e.FromEmployeeId, e.CreatedDate, e.Id })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("IX_CustomerTransferLog_Company_FromEmp_CreatedDateDesc");

                entity.HasIndex(e => new { e.CompanyId, e.ToEmployeeId, e.CreatedDate, e.Id })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("IX_CustomerTransferLog_Company_ToEmp_CreatedDateDesc");

                // Relationships (non-nullable => Restrict; header là cha của detail)
                entity.HasOne(d => d.Company).WithMany(p => p.CustomerTransferLogs)
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerTransferLog_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerTransferLogCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerTransferLog_CreatedBy");

                entity.HasOne(d => d.FromEmployee).WithMany(p => p.CustomerTransferLogFromEmployees)
                      .HasForeignKey(d => d.FromEmployeeId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerTransferLog_FromEmployee");

                entity.HasOne(d => d.ToEmployee).WithMany(p => p.CustomerTransferLogToEmployees)
                      .HasForeignKey(d => d.ToEmployeeId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerTransferLog_ToEmployee");

                entity.HasOne(d => d.FromGroup).WithMany(p => p.CustomerTransferLogFromGroups)
                      .HasForeignKey(d => d.FromGroupId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerTransferLog_FromGroup");

                entity.HasOne(d => d.ToGroup).WithMany(p => p.CustomerTransferLogToGroups)
                      .HasForeignKey(d => d.ToGroupId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_CustomerTransferLog_ToGroup");

            });


            modelBuilder.Entity<Deliverer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Deliverer__3214EC27D1AFC3E3");


                entity.HasIndex(e => e.DelivererInforId, "IX_Deliverer_DelivererInforId");
                entity.HasIndex(e => e.DeliveryOrderId, "IX_Deliverer_DeliveryOrderId");


                entity.ToTable("Deliverer", "DeliveryOrder");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ID");

                // FK -> DelivererInfor
                entity.HasOne(d => d.DelivererInfor)
                      .WithMany()
                      .HasForeignKey(d => d.DelivererInforId)
                      .OnDelete(DeleteBehavior.Restrict) // hoặc Cascade; tránh ClientSetNull khi FK non-null
                      .HasConstraintName("FK_Deliverer_DelivererInforId");

                // FK -> DeliveryOrder
                entity.HasOne(d => d.DeliveryOrder)
                      .WithMany(p => p.Deliverers)
                      .HasForeignKey(d => d.DeliveryOrderId)
                      .OnDelete(DeleteBehavior.Restrict) // hoặc Cascade
                      .HasConstraintName("FK_Deliverer_DeliveryOrderId");

            });

            modelBuilder.Entity<DelivererInfor>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DelivererInfor__3214EC27B1E3D8F1");
                entity.ToTable("DelivererInfor", "DeliveryOrder");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ID");

            });

            modelBuilder.Entity<DeliveryOrder>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DeliveryOrder__A2F6B5D8D1C1E3E3");
                entity.ToTable("DeliveryOrders", "DeliveryOrder");

                entity.Property(e => e.Id)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("ID");


                // Nếu bạn muốn Status, Note... cũng mapping tên khác thì thêm tương tự:
                entity.Property(e => e.Status)
                    .HasMaxLength(64)
                    .IsRequired()
                    .HasColumnName("Status");

                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("ExternalId");

                entity.Property(e => e.CustomerExternalIdSnapShot)
                    .HasColumnType("citext")
                    .HasColumnName("CustomerExternalIdSnapShot");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("IsActive");

                entity.Property(e => e.HasPrinted)
                    .HasDefaultValue(false)
                    .HasColumnName("HasPrinted");

                entity.Property(e => e.DeliveryPrice)
                    .HasColumnType("numeric(18,2)")
                    .HasColumnName("DeliveryPrice");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("CreatedDate");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("UpdatedDate");

                entity.HasIndex(e => e.CompanyId, "IX_DeliveryOrders_CompanyId");
                entity.HasIndex(e => e.CreatedBy, "IX_DeliveryOrders_CreatedBy");
                entity.HasIndex(e => e.UpdatedBy, "IX_DeliveryOrders_UpdatedBy");
                entity.HasIndex(e => e.CustomerId, "IX_DeliveryOrders_CustomerId");
                entity.Property(x => x.ExternalId).HasColumnType("citext");
                //entity.Property(x => x.MerchandiseOrderExternalIdSnapShot).HasColumnType("citext");
                entity.Property(x => x.CustomerExternalIdSnapShot).HasColumnType("citext");
                entity.Property(x => x.IsActive).HasDefaultValue(true);

                entity.HasOne(d => d.Customer).WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_Customer");

                entity.HasOne(d => d.Company).WithMany()
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(d => d.DeliveryOrderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(d => d.DeliveryOrderUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_UpdatedBy");
            });

            modelBuilder.Entity<DeliveryOrderDetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DeliveryOrderDetail__A2F6B5D8D1C1E3E3");
                entity.ToTable("DeliveryOrderDetail", "DeliveryOrder");

                entity.Property(e => e.Id)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("ID");

                entity.HasIndex(x => x.DeliveryOrderId, "IX_DeliveryOrderDetail_DeliveryOrderId");
                entity.HasIndex(x => x.ProductId, "IX_DeliveryOrderDetail_ProductId");
                entity.HasIndex(x => x.MerchandiseOrderDetailId, "IX_DeliveryOrderDetail_MerchandiseOrderDetailId");

                // ===== Columns & types =====
                entity.Property(x => x.ProductExternalIdSnapShot)
                    .HasColumnType("citext");

                entity.Property(x => x.ProductNameSnapShot)
                    .HasMaxLength(256);

                entity.Property(x => x.LotNoList)
                    .HasColumnType("citext");

                entity.Property(x => x.PONo)
                    .HasColumnType("citext");

                // DECIMAL(18,3) cho số lượng
                entity.Property(x => x.Quantity)
                    .HasPrecision(18, 3);

                entity.Property(x => x.NumOfBags);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                // Model default là false → để DB default false luôn cho đồng bộ
                entity.Property(x => x.IsAttach)
                    .HasDefaultValue(false);

                // ===== Relationships =====

                // Detail thuộc 1 DeliveryOrder
                entity.HasOne(d => d.DeliveryOrder)
                    .WithMany(o => o.Details)
                    .HasForeignKey(d => d.DeliveryOrderId)
                    .OnDelete(DeleteBehavior.Cascade) // hoặc ClientSetNull tùy business
                    .HasConstraintName("FK_DeliveryOrderDetail_DeliveryOrder");

                // OPTIONAL: link tới MerchandiseOrderDetail (nếu không phải hàng attach ngoài danh mục)
                entity.HasOne(d => d.MerchandiseOrderDetail)
                    .WithMany(m => m.DeliveryOrderDetails)
                    .HasForeignKey(d => d.MerchandiseOrderDetailId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_DeliveryOrderDetail_MerchandiseOrderDetail");

                // OPTIONAL: Product (nếu có map thẳng sản phẩm)
                entity.HasOne(d => d.Product)
                    .WithMany(d => d.DeliveryOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_DeliveryOrderDetail_Product");



            });

            modelBuilder.Entity<DeliveryOrderPO>(entity =>
            {
                entity.ToTable("DeliveryOrderPO", "DeliveryOrder");
                entity.HasKey(x => new { x.DeliveryOrderId, x.MerchandiseOrderId });

                entity.HasOne(x => x.DeliveryOrder)
                    .WithMany(o => o.DeliveryOrderPOs)
                    .HasForeignKey(x => x.DeliveryOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.MerchandiseOrder)
                    .WithMany(mo => mo.DeliveryOrderPOs)
                    .HasForeignKey(x => x.MerchandiseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<DetailCustomerTransfer>(entity =>
            {
                entity.HasKey(e => new { e.LogId, e.CustomerId }).HasName("PK_DetailCustomerTransfer");

                entity.ToTable("DetailCustomerTransfer", "Customer");

                entity.Property(e => e.LogId).HasColumnName("LogId");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");

                entity.HasIndex(e => e.CustomerId).HasDatabaseName("IX_DetailCustomerTransfer_CustomerId");

                entity.HasOne(d => d.Log).WithMany(p => p.DetailCustomerTransfers)
                      .HasForeignKey(d => d.LogId)
                      .OnDelete(DeleteBehavior.Cascade)   // xoá header => xoá detail
                      .HasConstraintName("FK_DetailCustomerTransfer_Log");

                entity.HasOne(d => d.Customer).WithMany(p => p.DetailCustomerTransfers)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict)  // giữ lịch sử, không cho xoá KH khi đã có log
                      .HasConstraintName("FK_DetailCustomerTransfer_Customer");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1C1895B9F");

                entity.ToTable("Employees", "hr");

                entity.HasIndex(e => e.PartId, "IX_Employees_PartID");
                entity.HasIndex(e => e.CompanyId, "IX_Employees_CompanyId");

                entity.Property(e => e.EmployeeId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Address).HasColumnType("citext");
                entity.Property(e => e.Email).HasColumnType("citext");
                entity.Property(e => e.ExternalId).HasColumnType("citext");
                entity.Property(e => e.FullName).HasColumnType("citext");
                entity.Property(e => e.Gender).HasColumnType("citext");
                entity.Property(e => e.Identifier).HasColumnType("citext");
                entity.Property(e => e.LevelId).HasColumnName("LevelID");
                entity.Property(e => e.PartId).HasColumnName("PartID");
                entity.Property(e => e.PhoneNumber).HasColumnType("citext");
                entity.Property(e => e.Status).HasColumnType("citext");

                entity.HasOne(d => d.Part).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PartId)
                    .HasConstraintName("FK_Employees_Parts");

                entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Employees_Companies");
            });


            //modelBuilder.Entity<EmployeesCommonDatum>(entity =>
            //{
            //    entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF184E31508");

            //    entity.ToTable("Employees_Common_data");

            //    entity.HasIndex(e => e.LevelId, "IX_Employees_Common_data_LevelID");

            //    entity.HasIndex(e => e.PartId, "IX_Employees_Common_data_PartID");

            //    entity.Property(e => e.EmployeeId)
            //        .HasMaxLength(16)
            //        .HasColumnName("EmployeeID");
            //    entity.Property(e => e.Address).HasColumnType("citext");
            //    entity.Property(e => e.Email).HasColumnType("citext");
            //    entity.Property(e => e.FullName).HasColumnType("citext");
            //    entity.Property(e => e.Gender).HasColumnType("citext");
            //    entity.Property(e => e.Identifier).HasColumnType("citext");
            //    entity.Property(e => e.LevelId)
            //        .HasMaxLength(10)
            //        .HasColumnName("LevelID");
            //    entity.Property(e => e.PartId)
            //        .HasMaxLength(16)
            //        .HasColumnName("PartID");
            //    entity.Property(e => e.PhoneNumber).HasColumnType("citext");
            //    entity.Property(e => e.Status).HasColumnType("citext");

            //    entity.HasOne(d => d.Level).WithMany(p => p.EmployeesCommonData)
            //        .HasForeignKey(d => d.LevelId)
            //        .HasConstraintName("FK__Employees__Level__693CA210");

            //    entity.HasOne(d => d.Part).WithMany(p => p.EmployeesCommonData)
            //        .HasForeignKey(d => d.PartId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__Employees__PartI__68487DD7");
            //});



            //modelBuilder.Entity<EventLog>(entity =>
            //{
            //    entity.HasKey(e => e.EventId).HasName("PK__EventLogs__227429A55C6F1195");

            //    entity.ToTable("EventLogs", "Audit");

            //    entity.Property(e => e.EventId)
            //      .HasDefaultValueSql("gen_random_uuid()");

            //    entity.HasIndex(e => e.CompanyId, "IX_EventLogs_CompanyId");
            //    entity.HasIndex(e => e.EmployeeID, "IX_EventLogs_CreatedBy");

            //    entity.HasIndex(x => x.SourceId).HasDatabaseName("IX_EventLog_SourceId");
            //    entity.HasIndex(x => x.SourceCode).HasDatabaseName("IX_EventLog_SourceCode");
            //    entity.HasIndex(x => x.EventType).HasDatabaseName("IX_EventLog_EventType");
            //    entity.HasIndex(x => x.Status).HasDatabaseName("IX_EventLog_Status");


            //    // Composite cho các case lọc kết hợp
            //    entity.HasIndex(x => new { x.SourceId, x.EventType }).HasDatabaseName("IX_EventLog_SourceId_EventType");
                
            //    entity.Property(e => e.IsActive).HasDefaultValue(true);
            //    //entity.Property(e => e.IsCustomerSelect).HasDefaultValue(false);
            //    entity.Property(e => e.Status)
            //          .HasMaxLength(32)
            //          .HasDefaultValue("Draft");


            //    entity.HasOne(d => d.Company).WithMany(p => p.EventLogs)
            //        .HasForeignKey(d => d.CompanyId)
            //        .HasConstraintName("FK_EventLogs_Company");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EventLogs)
            //        .HasForeignKey(d => d.EmployeeID)
            //        .HasConstraintName("FK_EventLogs_CreatedBy");

            //    entity.HasOne(d => d.Part).WithMany(p => p.EventLogs)
            //        .HasForeignKey(d => d.DepartmentId)
            //        .HasConstraintName("FK_EventLogs_DepartmentId");


            //});


            modelBuilder.Entity<Formula>(entity =>
            {
                entity.ToTable("Formulas", "SampleRequests");
                entity.HasKey(e => e.FormulaId).HasName("PK__Formulas__227429A55C6F1195");

                // ===== Columns (đặt HasColumnName y như property) =====
                entity.Property(e => e.FormulaId)
                    .HasColumnName("FormulaId")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ExternalId).HasColumnName("ExternalId").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();

                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.Status).HasColumnName("Status").HasMaxLength(32).HasDefaultValue("Draft");

                entity.Property(e => e.CheckBy).HasColumnName("CheckBy");
                entity.Property(e => e.CheckDate).HasColumnName("CheckDate");

                entity.Property(e => e.SentBy).HasColumnName("SentBy");
                entity.Property(e => e.SentDate).HasColumnName("SentDate");

                entity.Property(e => e.TotalPrice).HasColumnName("TotalPrice").HasPrecision(16, 2);
                entity.Property(e => e.IsSelect).HasColumnName("IsSelect").HasDefaultValue(false);
                entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                entity.Property(e => e.Note).HasColumnName("Note").HasColumnType("text");

                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");

                // ===== Indexes =====
                entity.HasIndex(e => e.CompanyId).HasDatabaseName("IX_Formulas_CompanyId");
                entity.HasIndex(e => e.ProductId).HasDatabaseName("IX_Formulas_ProductId");
                entity.HasIndex(e => e.CreatedBy).HasDatabaseName("IX_Formulas_CreatedBy");
                entity.HasIndex(e => e.UpdatedBy).HasDatabaseName("IX_Formulas_UpdatedBy");
                entity.HasIndex(e => e.CheckBy).HasDatabaseName("IX_Formulas_CheckBy");
                entity.HasIndex(e => e.SentBy).HasDatabaseName("IX_Formulas_SentBy"); // ✅ sửa nhầm cũ (trước ghi CheckBy)


                // List/paging mặc định trong tenant (CreatedDate DESC)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.CreatedDate, e.ProductId })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("IX_ProductId_Company_IsActive_CreatedDateDesc");

    
                // ===== Relationships =====
                entity.HasOne(d => d.Company).WithMany(p => p.Formulas)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Formulas_Company");

                entity.HasOne(d => d.Product).WithMany(p => p.Formulas)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict) // ProductId là non-nullable ⇒ không SetNull
                    .HasConstraintName("FK_Formulas_Product");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FormulaCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull)   // CreatedBy nullable
                    .HasConstraintName("FK_Formulas_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FormulaUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.SetNull)   // UpdatedBy nullable
                    .HasConstraintName("FK_Formulas_UpdatedBy");

                entity.HasOne(d => d.CheckByNavigation).WithMany(p => p.FormulaCheckByNavigations)
                    .HasForeignKey(d => d.CheckBy)
                    .OnDelete(DeleteBehavior.SetNull)   // CheckBy nullable
                    .HasConstraintName("FK_Formulas_CheckBy");

                entity.HasOne(d => d.SentByNavigation).WithMany(p => p.FormulaSentByNavigations)
                    .HasForeignKey(d => d.SentBy)
                    .OnDelete(DeleteBehavior.SetNull)   // SentBy nullable
                    .HasConstraintName("FK_Formulas_SentBy"); // ✅ sửa tên constraint (trước ghi nhầm IX_)
            });



            //modelBuilder.Entity<FormulaStatusLog>(entity =>
            //{
            //    entity.HasKey(e => e.LogId).HasName("PK_FormulaStatusLog");
            //    entity.ToTable("FormulaStatusLog", "SampleRequests");

            //    // Index như yêu cầu
            //    entity.HasIndex(e => e.FormulaId, "IX_FormulaStatusLog_FormulaId");
            //    entity.HasIndex(e => e.CreatedDate, "IX_FormulaStatusLog_CreatedAt");

            //    entity.Property(e => e.LogId)
            //          .HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.OldStatus).HasMaxLength(32);
            //    entity.Property(e => e.NewStatus).HasMaxLength(32);
            //    entity.Property(e => e.CreateNameSnapShot).HasMaxLength(200);
            //    entity.Property(e => e.CreatedDate)
            //          .HasDefaultValueSql("now()"); // mặc định thời điểm ghi log

            //    entity.HasOne(d => d.Formula).WithMany(p => p.StatusLogs)
            //          .HasForeignKey(d => d.FormulaId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("FK_FormulaStatusLog_Formula");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FormulaStatusLogCreatedByNavigations)
            //          .HasForeignKey(d => d.CreatedBy)
            //          .HasConstraintName("FK_FormulaStatusLog_CreatedBy");

            //    // (Khuyến nghị) hợp lệ hoá trạng thái
            //    //entity.ToTable(tb => tb.HasCheckConstraint(
            //    //    "CK_FormulaStatusLog_Status",
            //    //    "(OldStatus IS NULL OR OldStatus IN ('Draft','Sent','Verified','Rejected','Archived')) " +
            //    //    "AND (NewStatus IS NULL OR NewStatus IN ('Draft','Sent','Verified','Rejected','Archived'))"
            //    //));
            //});


            modelBuilder.Entity<FormulaMaterial>(entity =>
            {
                entity.HasKey(e => e.FormulaMaterialId).HasName("PK__FormulaM__0315C60A1F19742A");

                entity.ToTable("FormulaMaterials", "SampleRequests");

                entity.HasIndex(e => e.FormulaId, "IX_FormulaMaterials_FormulaId");

                entity.HasIndex(e => e.MaterialId, "IX_FormulaMaterials_MaterialId");

                entity.Property(e => e.FormulaMaterialId).HasDefaultValueSql("gen_random_uuid()");


                entity.Property(e => e.Quantity).HasPrecision(18, 6);
                entity.Property(e => e.UnitPrice).HasPrecision(16, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(16, 2);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.MaterialNameSnapshot).HasMaxLength(200);
                entity.Property(e => e.MaterialExternalIdSnapshot).HasMaxLength(50);
                entity.Property(e => e.Unit).HasMaxLength(32);

                entity.HasOne(d => d.Formula).WithMany(p => p.FormulaMaterials)
                    .HasForeignKey(d => d.FormulaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormulaMaterials_Formula");

                entity.HasOne(d => d.Formula).WithMany(p => p.FormulaMaterials)
                    .HasForeignKey(d => d.FormulaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormulaMaterials_Formula");

                entity.HasOne(d => d.Material).WithMany(p => p.FormulaMaterials)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormulaMaterials_Material");

                entity.HasOne(d => d.Category).WithMany(p => p.FormulaMaterials)
                      .HasForeignKey(d => d.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_FormulaMaterials_Category");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36A3DC9A844");

                entity.ToTable("Groups", "company");

                // ===== Columns =====
                entity.Property(e => e.GroupId)
                      .HasColumnName("GroupId")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.GroupType).HasColumnName("GroupType").HasMaxLength(100);
                entity.Property(e => e.ExternalId).HasColumnName("ExternalId").HasColumnType("citext").HasMaxLength(50);
                entity.Property(e => e.Name).HasColumnName("Name").HasColumnType("citext").HasMaxLength(200);

                entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

                entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");

                // ===== Indexes =====
                entity.HasIndex(e => e.CompanyId).HasDatabaseName("IX_Groups_CompanyId");
                entity.HasIndex(e => e.CreatedBy).HasDatabaseName("IX_Groups_CreatedBy");
                entity.HasIndex(e => e.UpdatedBy).HasDatabaseName("IX_Groups_UpdatedBy");

                // (tuỳ chọn) Duy nhất theo tenant cho mã nhóm
                entity.HasIndex(e => new { e.CompanyId, e.ExternalId })
                      .IsUnique()
                      .HasDatabaseName("UX_Groups_Company_ExternalId");

                // (tuỳ chọn) Duy nhất theo tenant cho tên nhóm (citext → không phân biệt hoa/thường)
                // entity.HasIndex(e => new { e.CompanyId, e.Name })
                //       .IsUnique()
                //       .HasDatabaseName("UX_Groups_Company_Name");

                // ===== Relationships =====
                entity.HasOne(d => d.Company).WithMany(p => p.Groups)
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_Groups_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GroupCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Groups_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.GroupUpdatedByNavigations)
                      .HasForeignKey(d => d.UpdatedBy)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("FK_Groups_UpdatedBy");
            });

            //modelBuilder.Entity<GroupsCommonDatum>(entity =>
            //{
            //    entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF30A6FD59030");

            //    entity.ToTable("Groups_Common_data");

            //    entity.Property(e => e.GroupId)
            //        .HasMaxLength(10)
            //        .HasColumnName("GroupID");
            //    entity.Property(e => e.Description).HasColumnType("citext");
            //    entity.Property(e => e.GroupName).HasColumnType("citext");
            //});

            //modelBuilder.Entity<InventoryReceiptsMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C400857F0DFA");

            //    entity.ToTable("InventoryReceipts_Material_data");

            //    entity.HasIndex(e => e.DetailId, "IX_InventoryReceipts_Material_data_DetailID");

            //    entity.HasIndex(e => e.RequestId, "IX_InventoryReceipts_Material_data_RequestID");

            //    entity.HasIndex(e => e.MaterialId, "IX_InventoryReceipts_Material_data_materialId");

            //    entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
            //    entity.Property(e => e.DetailId).HasColumnName("DetailID");
            //    entity.Property(e => e.MaterialId).HasColumnName("materialId");
            //    entity.Property(e => e.Note).HasColumnType("citext");
            //    entity.Property(e => e.RequestId)
            //        .HasMaxLength(16)
            //        .HasColumnName("RequestID");
            //    entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
            //    entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            //    entity.HasOne(d => d.Detail).WithMany(p => p.InventoryReceiptsMaterialData)
            //        .HasForeignKey(d => d.DetailId)
            //        .HasConstraintName("FK_PO_Detail_InventoryReceipt");

            //    entity.HasOne(d => d.Material).WithMany(p => p.InventoryReceiptsMaterialData)
            //        .HasForeignKey(d => d.MaterialId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_InventoryReceipts_Material");

            //    entity.HasOne(d => d.Request).WithMany(p => p.InventoryReceiptsMaterialData)
            //        .HasForeignKey(d => d.RequestId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_InventoryReceipts_Request");
            //});

            //modelBuilder.Entity<MachinesCommonDatum>(entity =>
            //{
            //    entity.HasKey(e => e.MachineId).HasName("PK__Machines__5A97603FADC7D2ED");

            //    entity.ToTable("Machines_Common_data");

            //    entity.HasIndex(e => e.GroupId, "IX_Machines_Common_data_GroupID");

            //    entity.HasIndex(e => e.PartId, "IX_Machines_Common_data_PartID");

            //    entity.Property(e => e.MachineId)
            //        .HasMaxLength(16)
            //        .HasColumnName("MachineID");
            //    entity.Property(e => e.Description).HasColumnType("citext");
            //    entity.Property(e => e.Factory)
            //        .HasDefaultValueSql("'Tam Phước'::character varying")
            //        .HasColumnType("citext");
            //    entity.Property(e => e.GroupId)
            //        .HasMaxLength(10)
            //        .HasColumnName("GroupID");
            //    entity.Property(e => e.GroupMachine).HasColumnType("citext");
            //    entity.Property(e => e.MachineName).HasColumnType("citext");
            //    entity.Property(e => e.PartId)
            //        .HasMaxLength(16)
            //        .HasColumnName("PartID");

            //    entity.HasOne(d => d.Group).WithMany(p => p.MachinesCommonData)
            //        .HasForeignKey(d => d.GroupId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__Machines__GroupI__6477ECF3");

            //    entity.HasOne(d => d.Part).WithMany(p => p.MachinesCommonData)
            //        .HasForeignKey(d => d.PartId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__Machines__PartID__656C112C");
            //});

            //modelBuilder.Entity<Material>(entity =>
            //{
            //    entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F7C355BA5C");

            //    entity.ToTable("Materials", "Material");

            //    entity.HasIndex(e => e.CategoryId, "IX_Materials_CategoryId");

            //    entity.HasIndex(e => e.CompanyId, "IX_Materials_CompanyId");

            //    entity.HasIndex(e => e.CreatedBy, "IX_Materials_CreatedBy");

            //    //entity.HasIndex(e => e.UnitId, "IX_Materials_UnitId");

            //    entity.HasIndex(e => e.UpdatedBy, "IX_Materials_UpdatedBy");

            //    entity.Property(e => e.MaterialId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.Barcode).HasMaxLength(16);
            //    entity.Property(e => e.Comment).HasMaxLength(500);
            //    entity.Property(e => e.CustomCode).HasMaxLength(50);
            //    entity.Property(e => e.ExternalId).HasMaxLength(50);
            //    entity.Property(e => e.IsActive).HasDefaultValue(true);
            //    entity.Property(e => e.Name).HasMaxLength(200);
            //    entity.Property(e => e.Package).HasMaxLength(100);

            //    entity.HasOne(d => d.Category).WithMany(p => p.Materials)
            //        .HasForeignKey(d => d.CategoryId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Materials_Category");

            //    entity.HasOne(d => d.Company).WithMany(p => p.Materials)
            //        .HasForeignKey(d => d.CompanyId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_Materials_Company");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaterialCreatedByNavigations)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_Materials_CreatedBy");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaterialUpdatedByNavigations)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_Materials_UpdatedBy");
            //});

            //modelBuilder.Entity<MaterialGroup>(entity =>
            //{
            //    entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD68C7B626");

            //    entity.Property(e => e.MaterialGroupId)
            //        .HasMaxLength(50)
            //        .HasColumnName("MaterialGroupID");
            //    entity.Property(e => e.MaterialGroupName).HasColumnType("citext");
            //});

            //modelBuilder.Entity<MaterialGroupsMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD37B632C7");

            //    entity.ToTable("MaterialGroups_Material_data");

            //    entity.Property(e => e.MaterialGroupId)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("MaterialGroupID");
            //    entity.Property(e => e.Detail).HasColumnType("citext");
            //    entity.Property(e => e.ExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("externalId");
            //    entity.Property(e => e.MaterialGroupName).HasColumnType("citext");
            //});

            //modelBuilder.Entity<MaterialsMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.MaterialId).HasName("PK__Material__99B653FDEEB02A00");

            //    entity.ToTable("Materials_material_data");

            //    entity.HasIndex(e => e.EmployeeId, "IX_Materials_material_data_EmployeeID");

            //    entity.HasIndex(e => e.MaterialGroupId, "IX_Materials_material_data_MaterialGroupId");

            //    entity.Property(e => e.MaterialId)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("materialId");
            //    entity.Property(e => e.EmployeeId)
            //        .HasMaxLength(16)
            //        .HasColumnName("EmployeeID");
            //    entity.Property(e => e.ExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("externalId");
            //    entity.Property(e => e.Name).HasColumnType("citext");
            //    entity.Property(e => e.Unit).HasColumnType("citext");

            //    entity.HasOne(d => d.Employee).WithMany(p => p.MaterialsMaterialData)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .HasConstraintName("FK__Materials__Emplo__6DCC4D03");

            //    entity.HasOne(d => d.MaterialGroup).WithMany(p => p.MaterialsMaterialData)
            //        .HasForeignKey(d => d.MaterialGroupId)
            //        .HasConstraintName("FK__Materials__Mater__6CD828CA");
            //});

            //modelBuilder.Entity<MaterialsSupplier>(entity =>
            //{
            //    entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB73A34869");

            //    entity.ToTable("Materials_Suppliers", "Material");

            //    entity.HasIndex(e => e.MaterialId, "IX_Materials_Suppliers_MaterialId");

            //    entity.HasIndex(e => e.SupplierId, "IX_Materials_Suppliers_SupplierId");

            //    entity.HasIndex(e => e.UpdatedBy, "IX_Materials_Suppliers_UpdatedBy");

            //    entity.Property(e => e.MaterialsSuppliersId)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("Materials_SuppliersId");
            //    entity.Property(e => e.Currency).HasMaxLength(10);
            //    entity.Property(e => e.CurrentPrice).HasPrecision(18, 4);
            //    entity.Property(e => e.IsPreferred).HasDefaultValue(false);
            //    entity.Property(e => e.IsActive).HasDefaultValue(true);

            //    entity.HasOne(d => d.Material).WithMany(p => p.MaterialsSuppliers)
            //        .HasForeignKey(d => d.MaterialId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_MaterialsSuppliers_Material");

            //    entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialsSuppliers)
            //        .HasForeignKey(d => d.SupplierId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_MaterialsSuppliers_Supplier");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaterialsSupplierCreatedByNavigations)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_MaterialsSuppliers_CreatedBy");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaterialsSupplierUpdatedByNavigations)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_MaterialsSuppliers_UpdatedBy");
            //});

            //// 1 supplier preferred duy nhất cho mỗi material (PostgreSQL partial index)
            //modelBuilder.Entity<MaterialsSupplier>()
            //    .HasIndex(x => new { x.MaterialId, x.IsPreferred })
            //    .HasFilter("\"IsPreferred\" = TRUE")
            //    .IsUnique();

            //modelBuilder.Entity<MaterialsSuppliersMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB69530C3D");

            //    entity.ToTable("MaterialsSuppliers_material_data");

            //    entity.HasIndex(e => e.MaterialId, "IX_MaterialsSuppliers_material_data_materialId");

            //    entity.HasIndex(e => e.PriceHistoryId, "IX_MaterialsSuppliers_material_data_priceHistoryId");

            //    entity.HasIndex(e => e.SupplierId, "IX_MaterialsSuppliers_material_data_supplierId");

            //    entity.Property(e => e.MaterialsSuppliersId)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("Materials_SuppliersId");
            //    entity.Property(e => e.Currency)
            //        .HasColumnType("citext")
            //        .HasColumnName("currency");
            //    entity.Property(e => e.CurrentPrice)
            //        .HasPrecision(18, 2)
            //        .HasColumnName("currentPrice");
            //    entity.Property(e => e.IsPreferred)
            //        .HasDefaultValue(false)
            //        .HasColumnName("isPreferred");
            //    entity.Property(e => e.MaterialId).HasColumnName("materialId");
            //    entity.Property(e => e.MinDeliveryDays).HasColumnName("minDeliveryDays");
            //    entity.Property(e => e.PriceHistoryId).HasColumnName("priceHistoryId");
            //    entity.Property(e => e.SupplierId).HasColumnName("supplierId");
            //    entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");

            //    entity.HasOne(d => d.Material).WithMany(p => p.MaterialsSuppliersMaterialData)
            //        .HasForeignKey(d => d.MaterialId)
            //        .HasConstraintName("FK__Materials__mater__7FEAFD3E");

            //    entity.HasOne(d => d.PriceHistory).WithMany(p => p.MaterialsSuppliersMaterialData)
            //        .HasForeignKey(d => d.PriceHistoryId)
            //        .HasConstraintName("FK__Materials__price__2A6B46EF");

            //    entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialsSuppliersMaterialData)
            //        .HasForeignKey(d => d.SupplierId)
            //        .HasConstraintName("FK__Materials__suppl__2B5F6B28");
            //});



            //modelBuilder.Entity<MfgProductionOrderLog>(entity =>
            //{
            //    entity.ToTable("MfgProductionOrderLogs", "manufacturing");

            //    entity.HasKey(e => e.LogId)
            //          .HasName("PK__MfgProductionOrderLogs__LogId");


            //    entity.Property(e => e.CreatedDate).HasDefaultValueSql("timezone('utc', now())").HasColumnName("createDate");

            //    entity.Property(e => e.CreatedBy).HasColumnName("createdBy");


            //    entity.HasOne(d => d.MfgProductionOrder).WithMany(p => p.ManufacturingOrderLogs)
            //          .HasForeignKey(d => d.MfgProductionOrderId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK__MfgProductionOrderLogs__MfgProductionOrderId");


            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MfgProductionOrderLogCreatedByNavigations)
            //          .HasForeignKey(d => d.CreatedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK__MfgProductionOrderLogs__createdBy");
            //});





       

            //modelBuilder.Entity<ManufacturingFormulaLog>(entity =>
            //{
            //    // ===== TABLE & PK =====
            //    entity.ToTable("ManufacturingFormulaLog", "manufacturing");
            //    entity.HasKey(e => e.Id)
            //          .HasName("PK__ManufacturingFormulaLog__Id");

            //    entity.Property(e => e.Id)
            //          .HasDefaultValueSql("gen_random_uuid()")
            //          .HasColumnName("Id");

            //    // ===== INDEXES (tuỳ nhu cầu lọc) =====
            //    entity.HasIndex(e => e.ManufacturingFormulaId, "IX_ManufacturingFormulaLog_ManufacturingFormulaId");


            //    entity.Property(e => e.PerformedDate).HasColumnName("PerformedDate");

            //    // ===== RELATIONSHIPS (FK names) =====
            //    entity.HasOne(d => d.ManufacturingFormula).WithMany(p => p.ManufacturingFormulaLogs)
            //          .HasForeignKey(d => d.ManufacturingFormulaId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK__Mf__ManufacturingFormulaLogs");

            //    entity.HasOne(d => d.PerformedByNavigation).WithMany(p => p.PerformedByNavigations)
            //          .HasForeignKey(d => d.PerformedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK__Mf__performedBy");

            //});


        



            modelBuilder.Entity<MemberInGroup>(entity =>
            {
                entity.HasKey(e => e.MemberId).HasName("PK__MemberIn__0CF04B189ED315D5");

                entity.ToTable("MemberInGroup", "company");

                // ===== Columns =====
                entity.Property(e => e.MemberId)
                      .HasColumnName("MemberId")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.IsAdmin).HasColumnName("IsAdmin").HasDefaultValue(false);
                entity.Property(e => e.Profile).HasColumnName("Profile");     // EmployeeId (nullable)
                entity.Property(e => e.GroupId).HasColumnName("GroupId");
                entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                // ===== Indexes =====
                entity.HasIndex(e => e.GroupId).HasDatabaseName("IX_MemberInGroup_GroupId");
                entity.HasIndex(e => e.Profile).HasDatabaseName("IX_MemberInGroup_Profile");

                // Chống trùng một nhân sự trong cùng group (chỉ tính bản IsActive = TRUE)
                entity.HasIndex(e => new { e.GroupId, e.Profile })
                      .IsUnique()
                      .HasFilter("\"IsActive\" = TRUE")
                      .HasDatabaseName("UX_MemberInGroup_Group_Profile_Active");

                // (tuỳ chọn) Nếu muốn MỖI NHÓM chỉ có 1 admin active:
                // entity.HasIndex(e => e.GroupId)
                //       .IsUnique()
                //       .HasFilter("\"IsActive\" = TRUE AND \"IsAdmin\" = TRUE")
                //       .HasDatabaseName("UX_MemberInGroup_Group_SingleAdmin_Active");

                // ===== Relationships =====
                entity.HasOne(d => d.Group).WithMany(p => p.MemberInGroups)
                      .HasForeignKey(d => d.GroupId)
                      .OnDelete(DeleteBehavior.Cascade)   // Xoá group → xoá thành viên
                      .HasConstraintName("FK_MemberInGroup_Group");

                entity.HasOne(d => d.ProfileNavigation).WithMany(p => p.MemberInGroups)
                      .HasForeignKey(d => d.Profile)
                      .OnDelete(DeleteBehavior.SetNull)   // Xoá employee → giữ bản ghi, set null
                      .HasConstraintName("FK_MemberInGroup_Profile");
            });




            modelBuilder.Entity<MfgProductionOrdersPlan>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__MfgProdu__3213E83F2188AAF5");

                entity.ToTable("MfgProductionOrdersPlan");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
                entity.Property(e => e.ExpiryDate).HasColumnName("expiryDate");
                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("externalId");
                entity.Property(e => e.ProductAddRate).HasColumnName("product_addRate");
                entity.Property(e => e.ProductCustomerExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("product_customerExternalId");
                entity.Property(e => e.ProductExpiryType)
                    .HasColumnType("citext")
                    .HasColumnName("product_expiryType");
                entity.Property(e => e.ProductExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("product_externalId");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.ProductMaxTemp).HasColumnName("product_maxTemp");
                entity.Property(e => e.ProductName)
                    .HasColumnType("citext")
                    .HasColumnName("product_name");
                entity.Property(e => e.ProductPackage)
                    .HasColumnType("citext")
                    .HasColumnName("product_package");
                entity.Property(e => e.ProductRecycleRate).HasColumnName("product_recycleRate");
                entity.Property(e => e.ProductRohsStandard).HasColumnName("product_rohsStandard");
                entity.Property(e => e.ProductWeight).HasColumnName("product_weight");
                entity.Property(e => e.Requirement)
                    .HasColumnType("citext")
                    .HasColumnName("requirement");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D30F786F0A7");

                entity.ToTable("Parts", "hr");

                entity.Property(e => e.PartId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PartID");
                entity.Property(e => e.Description).HasColumnType("citext");
                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("ExternalID");
                entity.Property(e => e.PartName).HasColumnType("citext");
            });

            //modelBuilder.Entity<PartsCommonDatum>(entity =>
            //{
            //    entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D3012CD1E19");

            //    entity.ToTable("Parts_Common_data");

            //    entity.Property(e => e.PartId)
            //        .HasMaxLength(16)
            //        .HasColumnName("PartID");
            //    entity.Property(e => e.PartName).HasColumnType("citext");
            //});

            //modelBuilder.Entity<PriceHistory>(entity =>
            //{
            //    entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__A927CACB4B3A2EAC");

            //    entity.ToTable("PriceHistory", "Material");



            //    // Columns
            //    entity.Property(e => e.MaterialsSuppliersId)
            //          .HasColumnName("materialsSuppliersId")
            //          .IsRequired();

            //    entity.Property(e => e.OldPrice)
            //          .HasPrecision(18, 4)
            //          .HasColumnName("oldPrice");

            //    entity.Property(e => e.Currency)
            //          .HasMaxLength(10)
            //          .HasColumnName("currency");

            //    entity.Property(e => e.CreateDate)
            //          .HasColumnName("createDate");

            //    entity.Property(e => e.CreatedBy)
            //          .HasColumnName("createdBy");

            //    // Indexes
            //    entity.HasIndex(e => e.CreatedBy, "IX_PriceHistory_CreatedBy");
            //    entity.HasIndex(e => e.MaterialsSuppliersId, "IX_PriceHistory_MaterialsSuppliersId");

            //    entity.HasIndex(e => e.CreateDate)
            //          .HasDatabaseName("ix_pricehistory_createDate");

            //    // (khuyến nghị) thường truy vấn theo nhà cung cấp + thời gian
            //    entity.HasIndex(e => new { e.MaterialsSuppliersId, e.CreateDate })
            //          .HasDatabaseName("ix_pricehistory_supplier_date");

            //    // FKs
            //    entity.HasOne(d => d.CreatedByNavigation)
            //          .WithMany(p => p.PriceHistoryCreatedByNavigations)
            //          .HasForeignKey(d => d.CreatedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK_PriceHistory_Materials_CreatedBy");

            //    entity.HasOne(d => d.MaterialsSuppliers)
            //          .WithMany(p => p.PriceHistories) // nếu MaterialsSupplier có ICollection<PriceHistory> thì thay .WithMany(p => p.PriceHistories)
            //          .HasForeignKey(d => d.MaterialsSuppliersId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_pricehistory_materialsSuppliersId");


            //});

            //PriceHistory: chỉ 1 bản ghi IsActive = true cho (MaterialId, SupplierId):
            //modelBuilder.Entity<PriceHistory>()
            //    .HasIndex(x => new { x.MaterialId, x.SupplierId, x.IsActive })
            //    .HasFilter("\"IsActive\" = TRUE")
            //    .IsUnique();    

            //modelBuilder.Entity<PriceHistoryMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__77D1486CB71DF1F1");

            //    entity.ToTable("PriceHistory_material_data");

            //    entity.HasIndex(e => e.MaterialId, "IX_PriceHistory_material_data_materialId");

            //    entity.HasIndex(e => e.SupplierId, "IX_PriceHistory_material_data_supplierId");

            //    entity.HasIndex(e => e.UpdatedBy, "IX_PriceHistory_material_data_updatedBy");

            //    entity.Property(e => e.PriceHistoryId)
            //        .ValueGeneratedNever()
            //        .HasColumnName("priceHistoryId");
            //    entity.Property(e => e.Currency)
            //        .HasColumnType("citext")
            //        .HasColumnName("currency");
            //    entity.Property(e => e.MaterialId).HasColumnName("materialId");
            //    entity.Property(e => e.NewPrice)
            //        .HasPrecision(18, 2)
            //        .HasColumnName("newPrice");
            //    entity.Property(e => e.OldPrice)
            //        .HasPrecision(18, 2)
            //        .HasColumnName("oldPrice");
            //    entity.Property(e => e.Reason)
            //        .HasColumnType("citext")
            //        .HasColumnName("reason");
            //    entity.Property(e => e.SupplierId).HasColumnName("supplierId");
            //    entity.Property(e => e.UpdatedBy)
            //        .HasMaxLength(16)
            //        .HasColumnName("updatedBy");
            //    entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");

            //    entity.HasOne(d => d.Material).WithMany(p => p.PriceHistoryMaterialData)
            //        .HasForeignKey(d => d.MaterialId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__PriceHist__mater__793DFFAF");

            //    entity.HasOne(d => d.Supplier).WithMany(p => p.PriceHistoryMaterialData)
            //        .HasForeignKey(d => d.SupplierId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__PriceHist__suppl__2D47B39A");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PriceHistoryMaterialData)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK__PriceHist__updat__2E3BD7D3");
            //});

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products", "SampleRequests");
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD344F4294");

                // ==== Columns (đặt HasColumnName y như property) ====
                entity.Property(e => e.ProductId)
                    .HasColumnName("ProductId")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ColourCode).HasColumnName("ColourCode").HasColumnType("citext");
                entity.Property(e => e.Name).HasColumnName("Name").HasColumnType("citext");
                entity.Property(e => e.ColourName).HasColumnName("ColourName").HasMaxLength(100);
                entity.Property(e => e.Additive).HasColumnName("Additive").HasMaxLength(200);
                entity.Property(e => e.UsageRate).HasColumnName("UsageRate");
                entity.Property(e => e.DeltaE).HasColumnName("DeltaE");
                entity.Property(e => e.Requirement).HasColumnName("Requirement").HasMaxLength(500);
                entity.Property(e => e.ExpiryType).HasColumnName("ExpiryType").HasMaxLength(100);
                entity.Property(e => e.StorageCondition).HasColumnName("StorageCondition");
                entity.Property(e => e.LabComment).HasColumnName("LabComment").HasMaxLength(500);
                //entity.Property(e => e.ProductType).HasColumnName("ProductType").HasMaxLength(100);
                entity.Property(e => e.Procedure).HasColumnName("Procedure").HasMaxLength(100);
                entity.Property(e => e.RecycleRate).HasColumnName("RecycleRate");
                entity.Property(e => e.TaicalRate).HasColumnName("TaicalRate");
                entity.Property(e => e.Application).HasColumnName("Application");
                entity.Property(e => e.ProductUsage).HasColumnName("ProductUsage");
                entity.Property(e => e.PolymerMatchedIn).HasColumnName("PolymerMatchedIn").HasMaxLength(100);
                entity.Property(e => e.Code).HasColumnName("Code").HasMaxLength(100);
                entity.Property(e => e.EndUser).HasColumnName("EndUser").HasMaxLength(100);

                entity.Property(e => e.FoodSafety).HasColumnName("FoodSafety").HasDefaultValue(false);
                entity.Property(e => e.RohsStandard).HasColumnName("RohsStandard").HasDefaultValue(false);
                entity.Property(e => e.ReachStandard).HasColumnName("ReachStandard").HasDefaultValue(false);
                entity.Property(e => e.ReturnSample).HasColumnName("ReturnSample").HasDefaultValue(false);
                entity.Property(e => e.IsRecycle).HasColumnName("IsRecycle").HasDefaultValue(false);

                entity.Property(e => e.MaxTemp).HasColumnName("MaxTemp");
                entity.Property(e => e.WeatherResistance).HasColumnName("WeatherResistance").HasMaxLength(100);
                entity.Property(e => e.LightCondition).HasColumnName("LightCondition").HasMaxLength(100);
                entity.Property(e => e.VisualTest).HasColumnName("VisualTest").HasMaxLength(100);

                entity.Property(e => e.OtherComment).HasColumnName("OtherComment");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
                entity.Property(e => e.Weight).HasColumnName("Weight");
                entity.Property(e => e.Unit).HasColumnName("Unit").HasMaxLength(50);


                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
                entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                // ==== Indexes ====
                entity.HasIndex(e => e.CompanyId).HasDatabaseName("IX_Products_CompanyId");
                entity.HasIndex(e => e.CategoryId).HasDatabaseName("IX_Products_CategoryId");
                entity.HasIndex(e => e.CreatedBy).HasDatabaseName("IX_Products_CreatedBy");
                entity.HasIndex(e => e.UpdatedBy).HasDatabaseName("IX_Products_UpdatedBy");


                // List/paging mặc định trong tenant (khớp CreatedDate desc)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.CreatedDate, e.ProductId })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("IX_Products_Company_IsActive_CreatedDateDesc");

                // ==== Relationships ====
                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Products_Category");

                entity.HasOne(d => d.Company).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull) // CreatedBy là non-nullable
                    .HasConstraintName("FK_Products_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.SetNull) // UpdatedBy nullable
                    .HasConstraintName("FK_Products_UpdatedBy");

            });

            //modelBuilder.Entity<ProductChangedHistory>(entity =>
            //{
            //    entity.HasKey(e => e.ProductChangedHistoryId).HasName("PK__ProductC__A793B6CA9FB36DED");

            //    entity.ToTable("ProductChangedHistory", "SampleRequests");

            //    entity.HasIndex(e => e.ChangedBy, "IX_ProductChangedHistory_ChangedBy");

            //    entity.HasIndex(e => e.ProductId, "IX_ProductChangedHistory_ProductId");

            //    entity.Property(e => e.ChangeNote).HasMaxLength(500);
            //    entity.Property(e => e.ChangeType).HasMaxLength(100);
            //    entity.Property(e => e.FieldChanged).HasMaxLength(100);
            //    entity.Property(e => e.NewValue).HasMaxLength(500);
            //    entity.Property(e => e.OldValue).HasMaxLength(500);

            //    entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.ProductChangedHistories)
            //        .HasForeignKey(d => d.ChangedBy)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ProductHistory_ChangedBy");

            //    entity.HasOne(d => d.Product).WithMany(p => p.ProductChangedHistories)
            //        .HasForeignKey(d => d.ProductId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_ProductHistory_Product");
            //});

            //modelBuilder.Entity<ProductInspection>(entity =>
            //{
            //    entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC072F6AD3E1");

            //    entity.ToTable("ProductInspection");

            //    entity.Property(e => e.Id).ValueGeneratedNever();
            //    entity.Property(e => e.Antistatic).HasColumnType("citext");
            //    entity.Property(e => e.BatchId).HasColumnType("citext");
            //    entity.Property(e => e.BlackDots).HasColumnType("citext");
            //    entity.Property(e => e.ColorDeltaE).HasColumnType("citext");
            //    entity.Property(e => e.CreatedBy).HasColumnType("citext");
            //    entity.Property(e => e.DefectBlackDot).HasColumnName("Defect_BlackDot");
            //    entity.Property(e => e.DefectDusty).HasColumnName("Defect_Dusty");
            //    entity.Property(e => e.DefectImpurity).HasColumnName("Defect_Impurity");
            //    entity.Property(e => e.DefectMoist).HasColumnName("Defect_Moist");
            //    entity.Property(e => e.DefectShortFiber).HasColumnName("Defect_ShortFiber");
            //    entity.Property(e => e.DefectWrongColor).HasColumnName("Defect_WrongColor");
            //    entity.Property(e => e.Density).HasColumnType("citext");
            //    entity.Property(e => e.Elongation).HasColumnType("citext");
            //    entity.Property(e => e.ExternalId).HasColumnType("citext");
            //    entity.Property(e => e.FlexuralModulus).HasColumnType("citext");
            //    entity.Property(e => e.FlexuralStrength).HasColumnType("citext");
            //    entity.Property(e => e.Hardness).HasColumnType("citext");
            //    entity.Property(e => e.ImpactResistance).HasColumnType("citext");
            //    entity.Property(e => e.IsColorDeltaEpass).HasColumnName("IsColorDeltaEPass");
            //    entity.Property(e => e.IsMfrpass).HasColumnName("IsMFRPass");
            //    entity.Property(e => e.MeshType).HasColumnType("citext");
            //    entity.Property(e => e.Mfr)
            //        .HasColumnType("citext")
            //        .HasColumnName("MFR");
            //    entity.Property(e => e.Moisture).HasColumnType("citext");
            //    entity.Property(e => e.Notes).HasColumnType("citext");
            //    entity.Property(e => e.PackingSpec).HasColumnType("citext");
            //    entity.Property(e => e.ParticleSize).HasColumnType("citext");
            //    entity.Property(e => e.ProductCode).HasColumnType("citext");
            //    entity.Property(e => e.ProductName).HasColumnType("citext");
            //    entity.Property(e => e.Shape).HasColumnType("citext");
            //    entity.Property(e => e.StorageCondition).HasColumnType("citext");
            //    entity.Property(e => e.TensileStrength).HasColumnType("citext");
            //    entity.Property(e => e.Types).HasColumnType("citext");
            //});

            //modelBuilder.Entity<ProductStandard>(entity =>
            //{
            //    entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC0715B96228");

            //    entity.ToTable("ProductStandard");

            //    entity.Property(e => e.Id).ValueGeneratedNever();
            //    entity.Property(e => e.BlackDots).HasColumnType("citext");
            //    entity.Property(e => e.ColourCode)
            //        .HasColumnType("citext")
            //        .HasColumnName("colourCode");
            //    entity.Property(e => e.CustomerExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("customerExternalId");
            //    entity.Property(e => e.DeltaE).HasColumnType("citext");
            //    entity.Property(e => e.Density).HasColumnType("citext");
            //    entity.Property(e => e.DwellTime).HasColumnType("citext");
            //    entity.Property(e => e.ElongationAtBreak).HasColumnType("citext");
            //    entity.Property(e => e.ExternalId).HasColumnType("citext");
            //    entity.Property(e => e.FlexuralModulus).HasColumnType("citext");
            //    entity.Property(e => e.FlexuralStrength).HasColumnType("citext");
            //    entity.Property(e => e.Hardness).HasColumnType("citext");
            //    entity.Property(e => e.IzodImpactStrength).HasColumnType("citext");
            //    entity.Property(e => e.MeltIndex).HasColumnType("citext");
            //    entity.Property(e => e.MigrationTest).HasColumnType("citext");
            //    entity.Property(e => e.Moisture).HasColumnType("citext");
            //    entity.Property(e => e.Package).HasColumnType("citext");
            //    entity.Property(e => e.PelletSize).HasColumnType("citext");
            //    entity.Property(e => e.ProductExternalId).HasColumnType("citext");
            //    entity.Property(e => e.Shape).HasColumnType("citext");
            //    entity.Property(e => e.Status).HasColumnType("citext");
            //    entity.Property(e => e.TensileStrength).HasColumnType("citext");
            //    entity.Property(e => e.Weight).HasColumnName("weight");
            //});

            //modelBuilder.Entity<ProductTest>(entity =>
            //{
            //    entity.HasKey(e => e.Id).HasName("PK__ProductT__3213E83F975320C1");

            //    entity.ToTable("ProductTest");

            //    entity.Property(e => e.Id)
            //        .ValueGeneratedNever()
            //        .HasColumnName("id");
            //    entity.Property(e => e.ExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("externalId");
            //    entity.Property(e => e.ProductCustomerExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("product_customerExternalId");
            //    entity.Property(e => e.ProductExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("product_externalId");
            //    entity.Property(e => e.ProductId).HasColumnName("product_id");
            //    entity.Property(e => e.ProductName)
            //        .HasColumnType("citext")
            //        .HasColumnName("product_name");
            //    entity.Property(e => e.ProductPackage)
            //        .HasColumnType("citext")
            //        .HasColumnName("product_package");
            //    entity.Property(e => e.ProductWeight).HasColumnName("product_weight");
            //});

            //modelBuilder.Entity<Qcdetail>(entity =>
            //{
            //    entity.HasKey(e => e.Id).HasName("PK__QCDetail__3214EC07937EA2C5");

            //    entity.ToTable("QCDetail");

            //    entity.HasIndex(e => e.BatchId, "IX_QCDetail_BatchId").IsUnique();

            //    entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.BatchExternalId).HasColumnType("citext");
            //    entity.Property(e => e.MachineExternalId).HasColumnType("citext");

            //    entity.HasOne(d => d.Batch).WithOne(p => p.Qcdetails)
            //        .HasForeignKey<Qcdetail>(d => d.BatchId)
            //        .OnDelete(DeleteBehavior.Cascade)
            //        .HasConstraintName("FK_QCDetail_ProductInspection");
            //});

            //modelBuilder.Entity<RequestDetail>(entity =>
            //{
            //    entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D66D34B2A");

            //    entity.ToTable("RequestDetail", "SupplyRequest");

            //    entity.HasIndex(e => e.MaterialId, "IX_RequestDetail_MaterialID");

            //    entity.HasIndex(e => e.RequestId, "IX_RequestDetail_RequestID");

            //    entity.Property(e => e.DetailId)
            //        .ValueGeneratedNever()
            //        .HasColumnName("DetailID");
            //    entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
            //    entity.Property(e => e.RequestId).HasColumnName("RequestID");
            //    entity.Property(e => e.RequestStatus).HasMaxLength(16);

            //    entity.HasOne(d => d.Material).WithMany(p => p.RequestDetails)
            //        .HasForeignKey(d => d.MaterialId)
            //        .HasConstraintName("FK_SupplyRequest_MaterialId");

            //    entity.HasOne(d => d.Request).WithMany(p => p.RequestDetails)
            //        .HasForeignKey(d => d.RequestId)
            //        .HasConstraintName("FK_SupplyRequest_RequestID");
            //});



            //modelBuilder.Entity<RequestDetailMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D44B7445B");

            //    entity.ToTable("RequestDetail_Material_data");

            //    entity.HasIndex(e => e.RequestId, "IX_RequestDetail_Material_data_RequestID");

            //    entity.HasIndex(e => e.MaterialId, "IX_RequestDetail_Material_data_materialId");

            //    entity.Property(e => e.DetailId).HasColumnName("DetailID");
            //    entity.Property(e => e.MaterialId).HasColumnName("materialId");
            //    entity.Property(e => e.Note).HasColumnType("citext");
            //    entity.Property(e => e.PurchasedQuantity).HasDefaultValue(0);
            //    entity.Property(e => e.RequestId)
            //        .HasMaxLength(16)
            //        .HasColumnName("RequestID");

            //    entity.HasOne(d => d.Material).WithMany(p => p.RequestDetailMaterialData)
            //        .HasForeignKey(d => d.MaterialId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_RequestDetail_Material");

            //    entity.HasOne(d => d.Request).WithMany(p => p.RequestDetailMaterialData)
            //        .HasForeignKey(d => d.RequestId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_RequestDetail_Request");
            //});

            modelBuilder.Entity<SampleRequest>(entity =>
            {
                // Table + PK
                entity.ToTable("SampleRequests", "SampleRequests");
                entity.HasKey(e => e.SampleRequestId).HasName("PK_SampleRequests");

                // ==== Columns (HasColumnName cho tất cả) ====
                entity.Property(e => e.SampleRequestId).HasColumnName("SampleRequestId")
                     .ValueGeneratedOnAdd()
                     .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ExternalId).HasColumnName("ExternalId")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");

                entity.Property(e => e.ManagerBy).HasColumnName("ManagerBy");

                entity.Property(e => e.ProductId).HasColumnName("ProductId");

                entity.Property(e => e.AttachmentCollectionId).HasColumnName("AttachmentCollectionId");

                entity.Property(e => e.RealDeliveryDate).HasColumnName("RealDeliveryDate");

                entity.Property(e => e.ExpectedDeliveryDate).HasColumnName("ExpectedDeliveryDate");

                entity.Property(e => e.RequestDeliveryDate).HasColumnName("RequestDeliveryDate");

                entity.Property(e => e.RequestTestSampleDate).HasColumnName("RequestTestSampleDate");

                entity.Property(e => e.ResponseDeliveryDate).HasColumnName("ResponseDeliveryDate");

                entity.Property(e => e.RealPriceQuoteDate).HasColumnName("RealPriceQuoteDate");

                entity.Property(e => e.ExpectedPriceQuoteDate).HasColumnName("ExpectedPriceQuoteDate");

                entity.Property(e => e.RequestType).HasColumnName("RequestType")
                    .HasMaxLength(100);

                entity.Property(e => e.ExpectedQuantity).HasColumnName("ExpectedQuantity");

                entity.Property(e => e.ExpectedPrice).HasColumnName("ExpectedPrice")
                    .HasPrecision(18, 4);

                entity.Property(e => e.SampleQuantity).HasColumnName("SampleQuantity");

                entity.Property(e => e.OtherComment).HasColumnName("OtherComment")
                    .HasMaxLength(500);

                entity.Property(e => e.InfoType).HasColumnName("InfoType")
                    .HasMaxLength(100);

                entity.Property(e => e.FormulaId).HasColumnName("FormulaId");

                entity.Property(e => e.SaleComment).HasColumnName("SaleComment");
                entity.Property(e => e.AdditionalComment).HasColumnName("AdditionalComment");

                entity.Property(e => e.CustomerProductCode).HasColumnName("CustomerProductCode")
                    .HasMaxLength(100);
                entity.Property(e => e.Status).HasColumnName("Status")
                    .HasMaxLength(100)
                    .HasDefaultValue("New");
                entity.Property(e => e.Package).HasColumnName("Package")
                    .HasMaxLength(100);


                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.SendBy).HasColumnName("SendBy");
                ////entity.Property(e => e.SendByNameSnapshot).HasColumnName("SendByNameSnapshot")
                //    .HasMaxLength(255);
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.CompanyId).HasColumnName("CompanyId");
                entity.Property(e => e.IsActive).HasColumnName("IsActive")
                    .HasDefaultValue(true);

                // ==== Indexes ====
                //entity.HasIndex(e => e.CompanyId).HasDatabaseName("IX_SampleRequests_CompanyId");
                //entity.HasIndex(e => e.BranchId).HasDatabaseName("IX_SampleRequests_BranchId");
                entity.HasIndex(e => e.FormulaId).HasDatabaseName("IX_SampleRequests_FormulaId");
                entity.HasIndex(e => e.ManagerBy).HasDatabaseName("IX_SampleRequests_ManagerBy");
                entity.HasIndex(e => e.SendBy).HasDatabaseName("IX_SampleRequests_SendBy");
                entity.HasIndex(e => e.UpdatedBy).HasDatabaseName("IX_SampleRequests_UpdatedBy");
                entity.HasIndex(o => o.AttachmentCollectionId).HasDatabaseName("IX_SampleRequests_AttachmentCollection");

                // Trang list mặc định: Company + Active + sort theo thời gian (ổn định, ít tốn CPU sort)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.CreatedDate, e.SampleRequestId })
                      .IsDescending(false, false, true, true) // EF Core 8+: CreatedDate, SampleRequestId DESC
                      .HasDatabaseName("IX_SampleRequests_Company_IsActive_CreatedDateDesc");


                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.Status, e.CreatedDate })
                      .HasDatabaseName("IX_SampleRequests_Company_Status_CreatedDate");
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.ExternalId })
                      .IsUnique()
                      .HasDatabaseName("UX_SampleRequests_Company_ExternalId");

                entity.HasIndex(e => new { e.CompanyId, e.ProductId })
                      .HasDatabaseName("IX_SampleRequests_Company_Product");

                entity.HasIndex(e => new { e.CompanyId, e.CustomerId })
                      .HasDatabaseName("IX_SampleRequests_Company_Customer");

                entity.HasIndex(e => new { e.CompanyId, e.CreatedBy })
                      .HasDatabaseName("IX_SampleRequests_Company_CreatedBy");

                // ==== Relationships ====
                entity.HasOne(d => d.AttachmentCollection)
                    .WithMany()
                    .HasForeignKey(d => d.AttachmentCollectionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_AttachmentCollection");

                entity.HasOne(d => d.Company).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_Company");

                entity.HasOne(d => d.Branch).WithMany(p => p.SampleRequestBranchs)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_BranchId");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SampleRequestCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_CreatedBy");

                entity.HasOne(d => d.Customer).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_Customer");

                entity.HasOne(d => d.Formula).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.FormulaId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SampleRequests_Formula");

                entity.HasOne(d => d.ManagerByNavigation).WithMany(p => p.SampleRequestManagerByNavigations)
                    .HasForeignKey(d => d.ManagerBy)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_Manager");

                entity.HasOne(d => d.Product).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_Product");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SampleRequestUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_UpdatedBy");

                entity.HasOne(d => d.SendByNavigation).WithMany(p => p.SampleRequestSendByNavigations)
                    .HasForeignKey(d => d.SendBy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_SampleRequests_SendBy");
            });



            //modelBuilder.Entity<Supplier>(entity =>
            //{
            //    entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4029CD1B8");

            //    entity.ToTable("Suppliers", "Material");

            //    entity.HasIndex(e => e.CompanyId, "IX_Suppliers_CompanyId");

            //    entity.HasIndex(e => e.CreatedBy, "IX_Suppliers_CreatedBy");

            //    entity.Property(e => e.SupplierId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.ExternalId).HasMaxLength(50);
            //    entity.Property(e => e.IssuedPlace).HasColumnType("citext");
            //    entity.Property(e => e.SupplierName).HasMaxLength(200);
            //    entity.Property(e => e.Note).HasMaxLength(500);
            //    entity.Property(e => e.Phone).HasMaxLength(20);
            //    entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
            //    entity.Property(e => e.Website).HasMaxLength(200);
            //    entity.Property(e => e.IsActive).HasDefaultValue(true);

            //    entity.HasOne(d => d.Company).WithMany(p => p.Suppliers)
            //        .HasForeignKey(d => d.CompanyId)
            //        .HasConstraintName("FK_Suppliers_Company");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierCreatedByNavigations)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_Suppliers_CreatedBy");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SupplierUpdatedByNavigations)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_Supplier_UpdatedBy");
            //});


            //modelBuilder.Entity<SupplierAddress>(entity =>
            //{
            //    entity.HasKey(e => e.AddressId).HasName("PK__Supplier__091C2AFB0B8862E7");

            //    entity.ToTable("SupplierAddresses", "Material");

            //    entity.HasIndex(e => e.SupplierId, "IX_SupplierAddresses_SupplierId");

            //    entity.Property(e => e.AddressId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.AddressLine).HasMaxLength(250);
            //    entity.Property(e => e.City).HasMaxLength(100);
            //    entity.Property(e => e.Country).HasMaxLength(100);
            //    entity.Property(e => e.District).HasMaxLength(100);
            //    entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            //    entity.Property(e => e.PostalCode).HasMaxLength(20);
            //    entity.Property(e => e.Province).HasMaxLength(100);

            //    entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAddresses)
            //        .HasForeignKey(d => d.SupplierId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_SupplierAddress_Supplier");
            //});

            //modelBuilder.Entity<SupplierContact>(entity =>
            //{
            //    entity.HasKey(e => e.ContactId).HasName("PK__Supplier__5C66259B7A3571DD");

            //    entity.ToTable("SupplierContacts", "Material");

            //    entity.HasIndex(e => e.SupplierId, "IX_SupplierContacts_SupplierId");

            //    entity.Property(e => e.ContactId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.Email).HasMaxLength(100);
            //    entity.Property(e => e.FirstName).HasMaxLength(100);
            //    entity.Property(e => e.Gender).HasMaxLength(20);
            //    entity.Property(e => e.LastName).HasMaxLength(100);
            //    entity.Property(e => e.Phone).HasMaxLength(20);

            //    entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierContacts)
            //        .HasForeignKey(d => d.SupplierId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_SupplierContact_Supplier");
            //});

            //modelBuilder.Entity<SuppliersMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__DB8E62ED69C94CF3");

            //    entity.ToTable("Suppliers_material_data");

            //    entity.Property(e => e.SupplierId)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("supplierId");
            //    entity.Property(e => e.ExternalId)
            //        .HasColumnType("citext")
            //        .HasColumnName("externalId");
            //    entity.Property(e => e.Name).HasColumnType("citext");
            //    entity.Property(e => e.Phone)
            //        .HasColumnType("citext")
            //        .HasColumnName("phone");
            //    entity.Property(e => e.RegNo)
            //        .HasColumnType("citext")
            //        .HasColumnName("regNo");
            //    entity.Property(e => e.TaxNo)
            //        .HasColumnType("citext")
            //        .HasColumnName("taxNo");
            //    entity.Property(e => e.Website)
            //        .HasColumnType("citext")
            //        .HasColumnName("website");
            //});

            //modelBuilder.Entity<SupplyRequest>(entity =>
            //{
            //    entity.HasKey(e => e.RequestId).HasName("PK__SupplyRe__33A8519A7A78FE1B");

            //    entity.ToTable("SupplyRequests", "SupplyRequest");

            //    entity.HasIndex(e => e.CompanyId, "IX_SupplyRequests_CompanyId");

            //    entity.HasIndex(e => e.CreatedBy, "IX_SupplyRequests_CreatedBy");

            //    entity.HasIndex(e => e.UpdatedBy, "IX_SupplyRequests_UpdatedBy");

            //    entity.Property(e => e.RequestId)
            //        .ValueGeneratedNever()
            //        .HasColumnName("RequestID");
            //    entity.Property(e => e.ExternalId)
            //        .HasMaxLength(16)
            //        .HasColumnName("ExternalID");
            //    entity.Property(e => e.RequestSourceType).HasMaxLength(16);
            //    entity.Property(e => e.RequestStatus).HasMaxLength(16);

            //    entity.HasOne(d => d.Company).WithMany(p => p.SupplyRequests)
            //        .HasForeignKey(d => d.CompanyId)
            //        .HasConstraintName("FK_SupplyRequest_Company");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplyRequestCreatedByNavigations)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_SupplyRequest_CreatedBy");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SupplyRequestUpdatedByNavigations)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_SupplyRequest_UpdatedBy");
            //});

            //modelBuilder.Entity<SupplyRequestsMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.RequestId).HasName("PK__SupplyRe__33A8519AEED4B83E");

            //    entity.ToTable("SupplyRequests_Material_data");

            //    entity.HasIndex(e => e.EmployeeId, "IX_SupplyRequests_Material_data_EmployeeID");

            //    entity.Property(e => e.RequestId)
            //        .HasMaxLength(16)
            //        .HasColumnName("RequestID");
            //    entity.Property(e => e.EmployeeId)
            //        .HasMaxLength(16)
            //        .HasColumnName("EmployeeID");
            //    entity.Property(e => e.Note).HasColumnType("citext");
            //    entity.Property(e => e.NoteCancel).HasColumnType("citext");
            //    entity.Property(e => e.RequestStatus).HasColumnType("citext");

            //    entity.HasOne(d => d.Employee).WithMany(p => p.SupplyRequestsMaterialData)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__SupplyReq__Emplo__6C190EBB");
            //});

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.UnitId).HasName("PK__Units__44F5ECB5E080D698");

                entity.ToTable("Units", "Material");

                entity.HasIndex(e => e.CreatedBy, "IX_Units_CreatedBy");

                entity.Property(e => e.UnitId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Symbol).HasMaxLength(20);

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Units)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Units_CreatedBy");
            });

            //modelBuilder.Entity<WarehouseShelfStock>(entity =>
            //{
            //    entity.HasKey(e => e.SlotId).HasName("PK__WarehouseShelfStock__3214EC07A98DEC4E");
            //    entity.Property(x => x.SlotId).UseIdentityAlwaysColumn();

            //    entity.ToTable("WarehouseShelfStock", "Warehouse");


            //    entity.Property(x => x.SlotCode).HasMaxLength(50).IsRequired();
            //    entity.Property(x => x.Code).HasColumnType("citext").IsRequired();
            //    entity.Property(x => x.BatchNo).HasMaxLength(50);
            //    entity.Property(x => x.LotKey).HasColumnType("citext");
            //    entity.Property(x => x.StockType).HasConversion<int>();
            //    entity.Property(x => x.QtyKg).HasPrecision(18, 3);

            //    entity.HasOne(d => d.Company).WithMany(p => p.WarehouseShelfStocks)
            //        .HasForeignKey(d => d.CompanyId)
            //        .HasConstraintName("FK_WarehouseShelfStock_Company");


            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WarehouseShelfStockUpdatedByNavigations)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_WarehouseShelfStock_UpdatedBy");

            //    entity.HasIndex(x => new { x.CompanyId, x.Code });
            //    entity.HasIndex(x => new { x.CompanyId, x.Code, x.LotKey });

            //});

            //modelBuilder.Entity<WarehouseRequest>(entity =>
            //{
            //    entity.HasKey(e => e.RequestId).HasName("PK__WareHouseRequest__3214EC07A98DEC4E");
               
            //    entity.Property(x => x.RequestId).UseIdentityAlwaysColumn();


            //    entity.ToTable("WarehouseRequest", "Warehouse");


            //    entity.Property(x => x.RequestName).IsRequired();
            //    entity.Property(x => x.CreatedBy).IsRequired();
            //    entity.Property(x => x.ReqStatus);
                
            //    entity.Property(x => x.ReqType);

            //    entity.HasOne(d => d.Company).WithMany(p => p.WarehouseRequests)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasForeignKey(d => d.CompanyId)
            //        .HasConstraintName("FK_WarehouseRequest_Company");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WarehouseRequestCreatedByNavigations)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_WarehouseRequest_CreatedBy");

            //    entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WarehouseRequestUpdatedByNavigations)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasForeignKey(d => d.UpdatedBy)
            //        .HasConstraintName("FK_WarehouseRequest_UpdatedBy");

            //});

            //modelBuilder.Entity<WarehouseRequestDetail>(entity =>
            //{
            //    entity.HasKey(e => e.DetailId).HasName("PK__WareHouseRequestDetail__3214EC07A98DEC4E");
            //    entity.Property(x => x.DetailId).UseIdentityAlwaysColumn();

            //    entity.HasIndex(e => e.RequestId, "IX_WarehouseRequestDetail_RequestCode");
            //    entity.ToTable("WarehouseRequestDetail", "Warehouse");


            //    entity.Property(x => x.WeightKg).HasPrecision(18, 3);

            //    entity.HasOne(d => d.WarehouseRequest)
            //        .WithMany(p => p.WarehouseRequestDetails)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasForeignKey(d => d.RequestId)
            //        .HasConstraintName("FK_WarehouseRequestDetail_RequestId");

            //});

           
            //modelBuilder.Entity<WarehouseTempStock>(entity =>
            //{
            //    entity.HasKey(e => e.TempId).HasName("PK__WarehouseTempStock__3214EC07A98DEC4E");

            //    entity.ToTable("WarehouseTempStock", "Warehouse");
            //    entity.Property(x => x.TempId).UseIdentityAlwaysColumn(); // Postgres identity;


            //    entity.Property(x => x.Code).HasColumnType("citext").IsRequired();
            //    entity.Property(x => x.VaCode).HasColumnType("citext").IsRequired();
            //    //entity.Property(x => x.VaLineCode).HasColumnType("citext");
            //    entity.Property(x => x.LotKey).HasColumnType("citext");

            //    //entity.Property(x => x.QtyStock).HasPrecision(18, 3);
            //    entity.Property(x => x.QtyRequest).HasPrecision(18, 3);


            //    // Enum = int (mặc định của EF) -> KHÔNG dùng HasConversion<string>()
            //    //entity.Property(x => x.TempType).IsRequired();
            //    entity.Property(x => x.ReserveStatus);


            //    entity.HasOne(x => x.CreatedByNavigation)
            //        .WithMany()
            //        .HasForeignKey(x => x.CreatedBy)
            //        .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WarehouseTempStockCreatedByNavigations)
            //        .HasForeignKey(d => d.CreatedBy)
            //        .HasConstraintName("FK_WarehouseTempStock_CreatedBy");

            //    //entity.HasIndex(x => new { x.CompanyId, x.TempType, x.Code, x.LotKey });
            //    entity.HasIndex(x => new { x.CompanyId, x.VaCode });
            //    //entity.HasIndex(x => new { x.CompanyId, x.SnapshotSetId });
            //});


            // ======================================================================== ManufacturingSchema Module ======================================================================== 
            modelBuilder.Entity<MfgProductionOrder>(entity =>
            {
                entity.ToTable("MfgProductionOrders", "manufacturing");

                entity.HasKey(e => e.MfgProductionOrderId)
                      .HasName("PK__MfgProductionOrders__MfgProductionOrderId");

                entity.Property(e => e.MfgProductionOrderId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("mfgProductionOrderId");

                // ===== COLUMNS =====
                entity.Property(e => e.ExternalId)
                      .HasColumnName("external_id")
                      .HasColumnType("citext")
                      .IsRequired();

                //entity.Property(e => e.ProductionType)
                //      .HasColumnName("production_type")
                //      .HasColumnType("citext");

                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.ProductExternalIdSnapshot).HasColumnName("product_externalid_snapshot").HasColumnType("citext");
                entity.Property(e => e.ProductNameSnapshot).HasColumnName("product_name_snapshot").HasColumnType("citext");
                entity.Property(e => e.ColorName).HasColumnName("color_name").HasColumnType("citext");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.CustomerNameSnapshot).HasColumnName("customer_name_snapshot").HasColumnType("citext");
                entity.Property(e => e.CustomerExternalIdSnapshot).HasColumnName("customer_externalid_snapshot").HasColumnType("citext");

                entity.Property(e => e.FormulaId).HasColumnName("formula_id");
                entity.Property(e => e.FormulaExternalIdSnapshot).HasColumnName("formula_externalid_snapshot").HasColumnType("citext");

                entity.Property(e => e.ManufacturingDate).HasColumnName("manufacturing_date");
                entity.Property(e => e.ExpectedDate).HasColumnName("expected_date");
                entity.Property(e => e.RequiredDate).HasColumnName("required_date");

                entity.Property(e => e.TotalQuantityRequest).HasColumnName("total_quantity_request");
                entity.Property(e => e.TotalQuantity).HasColumnName("total_quantity");
                entity.Property(e => e.NumOfBatches).HasColumnName("num_of_batches");

                entity.Property(e => e.UnitPriceAgreed)
                      .HasColumnName("unit_price_agreed")
                      .HasPrecision(18, 2);

                entity.Property(e => e.Status)
                      .HasColumnName("status")
                      .HasColumnType("citext")
                      .HasDefaultValue(ManufacturingProductOrder.New.ToString());

                entity.Property(e => e.LabNote).HasColumnName("lab_note");
                entity.Property(e => e.Requirement).HasColumnName("requirement");
                entity.Property(e => e.PlpuNote).HasColumnName("plpu_note");
                entity.Property(e => e.BagType).HasColumnName("bag_type");
               
                entity.Property(e => e.QcCheck).HasColumnName("qc_check");
                entity.Property(e => e.StepOfProduct).HasColumnName("step_of_product");

                entity.Property(e => e.IsActive)
                      .HasColumnName("is_active")
                      .HasDefaultValue(true)
                      .IsRequired();

                entity.Property(e => e.CompanyId).HasColumnName("company_id");

                entity.Property(e => e.CreatedDate)
                      .HasColumnName("created_date");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.UpdatedDate)
                      .HasColumnName("updated_date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");


                // ===== INDEXES =====
                // Mã đơn duy nhất trong tenant
                entity.HasIndex(e => new { e.CompanyId, e.ExternalId })
                      .IsUnique()
                      .HasDatabaseName("ux_mpo_company_externalid");

                // Paging ổn định trong tenant (CreatedDate DESC, tie-breaker theo PK)
                entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.CreatedDate, e.MfgProductionOrderId })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("ix_mpo_company_active_createddesc");

                // Lọc theo trạng thái + ngày kế hoạch (tenant-aware)
                entity.HasIndex(e => new { e.CompanyId, e.Status, e.ExpectedDate, e.MfgProductionOrderId })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("ix_mpo_company_status_expecteddesc");

                entity.HasIndex(e => new { e.CompanyId, e.Status, e.RequiredDate, e.MfgProductionOrderId })
                      .IsDescending(false, false, true, true)
                      .HasDatabaseName("ix_mpo_company_status_requireddesc");

                // Lọc theo sản phẩm trong tenant
                entity.HasIndex(e => new { e.CompanyId, e.ProductId, e.IsActive })
                      .HasDatabaseName("ix_mpo_company_product_active");


                // ===== RELATIONSHIPS =====
                entity.HasOne(d => d.Product).WithMany(p => p.MfgProductionOrders)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mpo__productId");

                entity.HasOne(d => d.Customer).WithMany(p => p.MfgProductionOrders)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mpo__customerId");

                entity.HasOne(d => d.Company).WithMany(p => p.MfgProductionOrders)
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mpo__companyId");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MfgProductionOrderCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mpo__createdBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MfgProductionOrderUpdatedByNavigations)
                      .HasForeignKey(d => d.UpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mpo__updatedBy");
            });

            modelBuilder.Entity<ManufacturingFormulaMaterial>(entity =>
            {
                // ===== TABLE & PK =====
                entity.ToTable("ManufacturingFormulaMaterials", "manufacturing");

                entity.HasKey(e => e.ManufacturingFormulaMaterialId)
                      .HasName("PK__ManufacturingFormulaMaterials__manufacturingFormulaMaterialId");

                entity.Property(e => e.ManufacturingFormulaMaterialId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("manufacturingFormulaMaterialId");

                // ===== COLUMNS =====
                entity.Property(x => x.ManufacturingFormulaId).HasColumnName("manufacturing_formula_id");
                entity.Property(x => x.MaterialId).HasColumnName("material_id");
                entity.Property(x => x.CategoryId).HasColumnName("category_id");

                entity.Property(x => x.Quantity)
                      .HasColumnName("quantity")
                      .HasPrecision(18, 6);

                entity.Property(x => x.UnitPrice)
                      .HasColumnName("unit_price")
                      .HasPrecision(16, 2);

                entity.Property(x => x.TotalPrice)
                      .HasColumnName("total_price")
                      .HasPrecision(16, 2);

                entity.Property(x => x.Unit)
                      .HasColumnName("unit"); // dùng "citext" nếu muốn case-insensitive

                entity.Property(x => x.MaterialNameSnapshot)
                      .HasColumnName("material_name_snapshot"); // có thể "citext" nếu cần

                entity.Property(x => x.MaterialExternalIdSnapshot)
                      .HasColumnName("material_externalid_snapshot"); // có thể "citext" nếu cần

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active")
                      .HasDefaultValue(true)
                      .IsRequired();
                // ===== INDEXES =====
                // Lấy nhanh danh sách vật liệu theo công thức (BOM)
                entity.HasIndex(x => x.ManufacturingFormulaId)
                      .HasDatabaseName("ix_mfm_formula_id");

                // Tra theo vật liệu trong một công thức (thường dùng khi hợp nhất dòng)
                entity.HasIndex(x => new { x.ManufacturingFormulaId, x.MaterialId })
                      .HasDatabaseName("ix_mfm_formula_material");

                // Chặn trùng dòng active trong cùng công thức cho cùng (Material, Category)
                entity.HasIndex(x => new { x.ManufacturingFormulaId, x.MaterialId, x.CategoryId })
                      .IsUnique()
                      .HasFilter("\"is_active\" = TRUE")   // chỉ khắt khe với dòng còn hiệu lực
                      .HasDatabaseName("ux_mfm_formula_material_unique_active");


                // ===== RELATIONSHIPS =====
                entity.HasOne(x => x.ManufacturingFormula)
                      .WithMany(f => f.ManufacturingFormulaMaterials)
                      .HasForeignKey(x => x.ManufacturingFormulaId)
                      .OnDelete(DeleteBehavior.Cascade) // xoá công thức => xoá vật liệu con
                      .HasConstraintName("FK__Mfm__manufacturingFormulaId");

                // ManufacturingFormulaMaterial mapping
                entity.HasOne(x => x.Category)
                      .WithMany(c => c.ManufacturingFormulaMaterials)   // <-- nếu CHƯA có collection, đổi .WithMany()
                      .HasForeignKey(x => x.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mfm__categoryId");

                entity.HasOne(x => x.Material)
                      .WithMany(m => m.ManufacturingFormulaMaterials)   // <-- nếu CHƯA có collection, đổi .WithMany()
                      .HasForeignKey(x => x.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mfm__materialId");
            });

            modelBuilder.Entity<ManufacturingFormulaVersion>(entity =>
            {
                // ===== TABLE & PK =====
                entity.ToTable("ManufacturingFormulaVersions", "manufacturing");

                entity.HasKey(e => e.ManufacturingFormulaVersionId)
                      .HasName("PK__ManufacturingFormulaVersions__manufacturingFormulaVersionId");

                entity.Property(e => e.ManufacturingFormulaVersionId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("manufacturingFormulaVersionId");

                // ===== COLUMNS =====
                entity.Property(x => x.VersionNo)
                      .HasColumnName("versionNo")
                      .IsRequired();

                entity.Property(x => x.Status)
                      .HasColumnName("status")
                      .HasColumnType("citext")
                      .HasDefaultValue("Draft");

                entity.Property(x => x.EffectiveFrom)
                      .HasColumnName("effectiveFrom");

                entity.Property(x => x.EffectiveTo)
                      .HasColumnName("effectiveTo");

                entity.Property(x => x.Note).HasColumnName("note");

                // Mỗi formula chỉ có 1 VersionNo duy nhất
                entity.HasIndex(x => new { x.ManufacturingFormulaId, x.VersionNo })
                      .IsUnique()
                      .HasDatabaseName("ux_mf_versions_formula_versionno");

                entity.HasIndex(x => x.Status).HasDatabaseName("ix_mf_versions_status");
                entity.HasIndex(x => new { x.ManufacturingFormulaId, x.EffectiveFrom, x.EffectiveTo })
                      .HasDatabaseName("ix_mf_versions_period");

                entity.HasOne(x => x.ManufacturingFormula)
                      .WithMany(f => f.ManufacturingFormulaVersions)                 // ➜ cần ICollection<ManufacturingFormulaVersion> Versions trong ManufacturingFormula
                      .HasForeignKey(x => x.ManufacturingFormulaId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_mf_versions_formula");

                // Items
                entity.HasMany(x => x.Items)
                      .WithOne(i => i.Version)
                      .HasForeignKey(i => i.ManufacturingFormulaVersionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_mf_version_items_version");


            });

            modelBuilder.Entity<ManufacturingFormula>(entity =>
            {
                // ===== TABLE & PK =====
                entity.ToTable("manufacturing_formulas", "manufacturing");
                entity.HasKey(e => e.ManufacturingFormulaId)
                      .HasName("PK__ManufacturingFormulas__manufacturingFormulaId");

                entity.Property(e => e.ManufacturingFormulaId)
                      .HasDefaultValueSql("gen_random_uuid()").HasColumnName("manufacturingFormulaId");

                // ===== COLUMNS (snake_case) =====
                entity.Property(x => x.ExternalId).HasColumnName("external_id")
                      .HasColumnType("citext")       // hoặc "text" nếu không muốn case-insensitive
                      .IsRequired();

                entity.Property(x => x.Name)
                      .HasColumnName("name")
                      .HasColumnType("citext")       // hoặc "text"
                      .IsRequired();

                entity.Property(x => x.Status)
                      .HasColumnName("status")
                      .HasMaxLength(32)
                      .HasDefaultValue("New");       // ManufacturingProductOrderFormula.New.ToString()

                entity.Property(x => x.TotalPrice)
                      .HasColumnName("total_price")
                      .HasPrecision(18, 2);

                // Nguồn gốc (đa hình 1-trong-2 | cho phép NULL cả 2, nhưng cấm cả 2 cùng có)
                entity.Property(x => x.SourceManufacturingFormulaId)
                      .HasColumnName("source_manufacturing_formula_id");
                entity.Property(x => x.SourceManufacturingExternalIdSnapshot)
                      .HasColumnName("source_manufacturing_externalid_snapshot")
                      .HasColumnType("citext");
                entity.Property(x => x.SourceVUFormulaId)
                      .HasColumnName("source_vu_formula_id");
                entity.Property(x => x.SourceVUExternalIdSnapshot)
                      .HasColumnName("source_vu_externalid_snapshot")
                      .HasColumnType("citext");

                entity.Property(x => x.IsActive)
                      .HasColumnName("is_active")
                      .HasDefaultValue(true)
                      .IsRequired();

                entity.Property(x => x.Note)
                      .HasColumnName("note")
                      .HasColumnType("citext");

                entity.Property(x => x.CreatedDate)
                      .HasColumnName("created_date");
                entity.Property(x => x.CreatedBy)
                      .HasColumnName("created_by");

                entity.Property(x => x.UpdatedDate)
                      .HasColumnName("updated_date");
                entity.Property(x => x.UpdatedBy)
                      .HasColumnName("updated_by");

                entity.Property(x => x.CompanyId)
                      .HasColumnName("company_id");

                // ===== INDEXES =====
                // Duy nhất theo tenant + ExternalId
                entity.HasIndex(x => new { x.CompanyId, x.ExternalId })
                      .IsUnique()
                      .HasDatabaseName("ux_mfg_formulas_company_external_id");

                entity.HasIndex(x => x.CompanyId)
                      .HasDatabaseName("ix_mfg_formulas_company_id");
                entity.HasIndex(x => x.CreatedBy)
                      .HasDatabaseName("ix_mfg_formulas_created_by");
                entity.HasIndex(x => x.SourceVUFormulaId)
                      .HasDatabaseName("ix_mfg_formulas_source_vu_formula_id");
                entity.HasIndex(x => x.SourceManufacturingFormulaId)
                      .HasDatabaseName("ix_mfg_formulas_source_mfg_formula_id");

                // Paging trong tenant: CreatedDate DESC + tie-breaker theo PK
                entity.HasIndex(x => new { x.CompanyId, x.IsActive, x.CreatedDate, x.ManufacturingFormulaId })
                      .IsDescending(false, false, true, true)   // EF Core 8+
                      .HasDatabaseName("ix_mfg_formulas_company_active_created_desc");

                // ===== RELATIONSHIPS (FK names) =====

                entity.HasOne(d => d.SourceVUFormula).WithMany(p => p.ManufacturingFormulaSources)
                      .HasForeignKey(d => d.SourceVUFormulaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__SourceVUFormulaId");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ManufacturingFormulaCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__createdBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ManufacturingFormulaUpdatedByNavigations)
                      .HasForeignKey(d => d.UpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__updatedBy");

                entity.HasOne(d => d.Company).WithMany(p => p.ManufacturingFormulas)
                      .HasForeignKey(d => d.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__companyId");

                // self-referencing foreign key (khóa ngoại tự tham chiếu cùng bảng).
                entity.HasOne(x => x.SourceManufacturingFormula).WithMany()
                      .HasForeignKey(x => x.SourceManufacturingFormulaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__SourceManufacturingFormulaId");

            });

            modelBuilder.Entity<ManufacturingFormulaVersionItem>(entity =>
            {
                entity.ToTable("ManufacturingFormulaVersionItems", "manufacturing");

                // PK
                entity.HasKey(x => x.ManufacturingFormulaVersionItemId)
                      .HasName("pk_mf_version_items");

                entity.Property(x => x.ManufacturingFormulaVersionItemId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("manufacturingFormulaVersionItemId");

                // FKs / quan hệ
                entity.Property(x => x.ManufacturingFormulaVersionId)
                      .HasColumnName("manufacturingFormulaVersionId")
                      .IsRequired();

                entity.HasOne(x => x.Version)
                      .WithMany(v => v.Items)
                      .HasForeignKey(x => x.ManufacturingFormulaVersionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("fk_mf_version_items_version");

                // Cột dữ liệu
                entity.Property(x => x.MaterialId)
                      .HasColumnName("materialId")
                      .IsRequired();

                entity.Property(x => x.CategoryId).HasColumnName("category_id");

                entity.Property(x => x.Quantity)
                      .HasPrecision(18, 6)
                      .HasColumnName("quantity")
                      .IsRequired();

                entity.Property(x => x.UnitPrice)
                      .HasPrecision(16, 2)
                      .HasColumnName("unitPrice")
                      .IsRequired();

                entity.Property(x => x.TotalPrice)
                      .HasPrecision(16, 2)
                      .HasColumnName("totalPrice")
                      .IsRequired();

                entity.Property(x => x.Unit)
                      .HasColumnName("unit")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.MaterialNameSnapshot)
                      .HasColumnName("materialNameSnapshot")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.MaterialExternalIdSnapshot)
                      .HasColumnName("materialExternalIdSnapshot")
                      .HasColumnType("text")
                      .IsRequired();


                // ===== INDEXES =====
                entity.HasIndex(x => x.ManufacturingFormulaVersionId)
                      .HasDatabaseName("ix_mf_version_items_version");

                // Tra theo vật liệu trong một công thức (thường dùng khi hợp nhất dòng)
                entity.HasIndex(x => new { x.ManufacturingFormulaVersionId, x.MaterialId })
                      .HasDatabaseName("ix_mfm_version_items_material");

                // Một vật tư chỉ xuất hiện 1 lần trong 1 version (tránh trùng dòng)
                entity.HasIndex(x => new { x.ManufacturingFormulaVersionId, x.MaterialId })
                      .IsUnique()
                      .HasDatabaseName("ux_mfm_version_items_version_material");


                // ===== RELATIONSHIPS =====
                entity.HasOne(x => x.Version)
                      .WithMany(f => f.Items)
                      .HasForeignKey(x => x.ManufacturingFormulaVersionId)
                      .OnDelete(DeleteBehavior.Cascade) // xoá công thức => xoá vật liệu con
                      .HasConstraintName("FK__Mfm__manufacturingFormulaId");

                // ManufacturingFormulaMaterial mapping
                entity.HasOne(x => x.Category)
                      .WithMany(c => c.Items)   // <-- nếu CHƯA có collection, đổi .WithMany()
                      .HasForeignKey(x => x.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mfm__categoryId");

                entity.HasOne(x => x.Material)
                      .WithMany(m => m.Items)   // <-- nếu CHƯA có collection, đổi .WithMany()
                      .HasForeignKey(x => x.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mfm__materialId");


            });



            modelBuilder.Entity<ProductionSelectVersion>(entity =>
            {
                entity.ToTable("production_select_versions", "manufacturing");

                entity.HasKey(x => x.ProductionSelectVersionId)
                      .HasName("pk_production_select_versions");

                entity.Property(x => x.ProductionSelectVersionId)
                      .HasColumnName("production_select_version_id")
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.MfgProductionOrderId)
                      .HasColumnName("mfg_production_order_id")
                      .IsRequired();

                entity.Property(x => x.ManufacturingFormulaId)
                      .HasColumnName("manufacturing_formula_id")
                      .IsRequired();

                entity.Property(x => x.ValidFrom)
                      .HasColumnName("valid_from")
                      .IsRequired();

                entity.Property(x => x.ValidTo)
                      .HasColumnName("valid_to"); 

                entity.Property(x => x.CreatedBy).HasColumnName("created_by").IsRequired();
                entity.Property(x => x.ClosedBy).HasColumnName("closed_by");
                entity.Property(x => x.CompanyId).HasColumnName("company_id").IsRequired();

                // ===== Relationships =====
                entity.HasOne(x => x.MfgProductionOrder)
                      .WithMany(p => p.ProductionSelectVersions) 
                      .HasForeignKey(x => x.MfgProductionOrderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psv_mfg_production_order");

                entity.HasOne(x => x.ManufacturingFormula)
                      .WithMany(p => p.ProductionSelectVersions)
                      .HasForeignKey(x => x.ManufacturingFormulaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psv_manufacturing_formula");

                entity.HasOne(x => x.Company)
                      .WithMany(p => p.ProductionSelectVersions)
                      .HasForeignKey(x => x.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psv_company");

                entity.HasOne(x => x.CreatedByNavigation)
                      .WithMany(p => p.ProductionSelectVersionCreatedByNavigations)
                      .HasForeignKey(x => x.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psv_created_by");

                // Model đang có property `UpdatedByNavigation` nhưng không có `UpdatedBy`.
                // Nếu ý bạn là "đóng phiên" thì nên map ClosedBy:
                entity.HasOne(x => x.ClosedByNavigation) // hoặc thêm property ClosedByNavigation vào model
                      .WithMany(p => p.ProductionSelectVersionClosedByNavigations)
                      .HasForeignKey(x => x.ClosedBy)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_psv_closed_by");

                // ===== Indexes / ràng buộc nghiệp vụ =====
                entity.HasIndex(x => x.MfgProductionOrderId)
                      .HasDatabaseName("ix_psv_mpo");

                entity.HasIndex(x => x.ManufacturingFormulaId)
                      .HasDatabaseName("ix_psv_formula");

                // Paging theo đơn + mốc hiệu lực
                entity.HasIndex(x => new { x.MfgProductionOrderId, x.ValidFrom })
                      .IsDescending(false, true)
                      .HasDatabaseName("ix_psv_mpo_validfrom_desc");

                // Chỉ cho phép 1 bản “đang hiệu lực” (ValidTo IS NULL) cho mỗi đơn trong 1 company
                entity.HasIndex(x => new { x.CompanyId, x.MfgProductionOrderId })
                      .IsUnique()
                      .HasFilter("\"valid_to\" IS NULL")
                      .HasDatabaseName("ux_psv_current_per_order");

            });

            modelBuilder.Entity<ProductStandardFormula>(entity =>
            {
                // ===== Table & PK =====
                entity.ToTable("product_standard_formulas", "manufacturing");

                entity.HasKey(x => x.ProductStandardFormulaId)
                      .HasName("pk_product_standard_formulas");

                // ===== Columns =====
                entity.Property(x => x.ProductStandardFormulaId)
                      .HasColumnName("product_standard_formula_id")
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(x => x.ProductId)
                      .HasColumnName("product_id")
                      .IsRequired();

                entity.Property(x => x.ManufacturingFormulaId)
                      .HasColumnName("manufacturing_formula_id")
                      .IsRequired();

                // mốc hiệu lực: dùng timestamptz cho đồng bộ UTC
                entity.Property(x => x.ValidFrom)
                      .HasColumnName("valid_from")
                      .IsRequired();

                entity.Property(x => x.ValidTo)
                      .HasColumnName("valid_to"); // null = hiện hành

                entity.Property(x => x.CreatedBy)
                      .HasColumnName("created_by")
                      .IsRequired();

                entity.Property(x => x.ClosedBy)
                      .HasColumnName("closed_by");

                entity.Property(x => x.CompanyId)
                      .HasColumnName("company_id")
                      .IsRequired();

                entity.Property(x => x.Note)
                      .HasColumnName("note")
                      .HasColumnType("citext");

                // ===== Indexes =====

                // tra cứu theo product
                entity.HasIndex(x => x.ProductId)
                      .HasDatabaseName("ix_psf_product");

                entity.HasIndex(x => x.ManufacturingFormulaId)
                      .HasDatabaseName("ix_psf_formula");

                // tra cứu theo company
                entity.HasIndex(x => x.CompanyId)
                      .HasDatabaseName("ix_psf_company");

                // chỉ cho phép 1 công thức chuẩn ĐANG HIỆU LỰC cho 1 product trong 1 company
                entity.HasIndex(x => new { x.CompanyId, x.ProductId })
                      .IsUnique()
                      .HasFilter("\"valid_to\" IS NULL")
                      .HasDatabaseName("ux_psf_company_product_current");

                // nếu cần paging theo thời gian hiệu lực
                entity.HasIndex(x => new { x.ProductId, x.ValidFrom })
                      .IsDescending(false, true)
                      .HasDatabaseName("ix_psf_product_validfrom_desc");


                // ===== Relationships =====
                entity.HasOne(x => x.Product)
                      .WithMany(p => p.ProductStandardFormulas)       // nếu trong Product có ICollection<ProductStandardFormula>
                      .HasForeignKey(x => x.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psf_product");

                entity.HasOne(x => x.ManufacturingFormula)
                      .WithMany(f => f.ProductStandardFormulas)       // nếu bạn chưa có, có thể để .WithMany()
                      .HasForeignKey(x => x.ManufacturingFormulaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psf_manufacturing_formula");

                entity.HasOne(x => x.Company)
                      .WithMany(c => c.ProductStandardFormulas)       // hoặc .WithMany()
                      .HasForeignKey(x => x.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psf_company");

                entity.HasOne(x => x.CreatedByNavigation)
                      .WithMany(e => e.ProductStandardFormulaCreatedByNavigations)
                      .HasForeignKey(x => x.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_psf_created_by");

                entity.HasOne(x => x.ClosedByNavigation)
                      .WithMany(e => e.ProductStandardFormulaClosedByNavigations)
                      .HasForeignKey(x => x.ClosedBy)
                      .OnDelete(DeleteBehavior.SetNull)
                      .HasConstraintName("fk_psf_closed_by");

            });

            modelBuilder.Entity<MfgOrderPO>(entity =>
            {
                entity.ToTable("MfgOrderPOs", "manufacturing");
                entity.HasKey(x => new { x.MerchandiseOrderDetailId, x.MfgProductionOrderId })
                      .HasName("PK_MfgOrderPOs");

                // Columns (đặt tên y chang property)
                entity.Property(x => x.MerchandiseOrderDetailId).HasColumnName("MerchandiseOrderDetailId");
                entity.Property(x => x.MfgProductionOrderId).HasColumnName("MfgProductionOrderId");
                entity.Property(x => x.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

                // Indexes cho truy vấn
                entity.HasIndex(x => x.MerchandiseOrderDetailId)
                      .HasDatabaseName("IX_MfgOrderPOs_DetailId");
                entity.HasIndex(x => x.MfgProductionOrderId)
                      .HasDatabaseName("IX_MfgOrderPOs_MfgOrderId");

                // (TUỲ CHỌN) Muốn “mỗi detail chỉ có 1 mapping đang active”:
                // Bỏ nếu bạn cho phép tách 1 detail ra nhiều MFG cùng lúc.
                entity.HasIndex(x => new { x.MerchandiseOrderDetailId, x.IsActive })
                      .IsUnique()
                      .HasFilter("\"IsActive\" = TRUE")
                      .HasDatabaseName("UX_MfgOrderPOs_Detail_Active");

                // (TUỲ CHỌN) Mỗi MFG chỉ map 1 detail khi đang active:
                entity.HasIndex(x => new { x.MfgProductionOrderId, x.IsActive })
                      .IsUnique()
                      .HasFilter("\"IsActive\" = TRUE")
                      .HasDatabaseName("UX_MfgOrderPOs_MfgOrder_Active");

                // Relationships (không cần collection ở 2 phía thì dùng .WithMany())
                entity.HasOne(x => x.Detail)
                      .WithMany()
                      .HasForeignKey(x => x.MerchandiseOrderDetailId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_MfgOrderPOs_Detail");

                entity.HasOne(x => x.ProductionOrder)
                      .WithMany()
                      .HasForeignKey(x => x.MfgProductionOrderId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_MfgOrderPOs_MfgOrder");
            });

            modelBuilder.Entity<SchedualMfg>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Schedual__3214EC07A98DEC4E");

                entity.ToTable("SchedualMfg", "Schedual");

                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedNever();

                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.MfgProductionOrderId).HasColumnName("MfgProductionOrderId");

                //entity.Property(e => e.ExternalId).HasColumnName("ExternalId")
                //       .HasColumnType("citext");

                entity.Property(e => e.MachineId).HasColumnName("MachineId")
                       .HasColumnType("citext");

                entity.Property(e => e.ColorName)
                       .HasColumnName("ColorName")
                       .HasColumnType("citext");

                entity.Property(e => e.ColorCode)
                       .HasColumnName("ColorCode")
                       .HasColumnType("citext");

                entity.Property(e => e.VerifyBatches)
                       .HasColumnName("VerifyBatches")
                       .HasColumnType("citext");

                entity.Property(e => e.Note)
                       .HasColumnName("Note")
                       .HasColumnType("citext");

                entity.Property(e => e.requirement)
                       .HasColumnName("requirement")
                       .HasColumnType("citext");

                entity.Property(e => e.Status)
                       .HasColumnName("Status")
                       .HasColumnType("citext");

                entity.Property(e => e.Qcstatus)
                       .HasColumnName("QCStatus")
                       .HasColumnType("citext");

                // ===========================
                //  Numeric Types
                // ===========================
                entity.Property(e => e.Number)
                       .HasColumnName("Number");

                entity.Property(e => e.Quantity)
                       .HasColumnName("Quantity");

                entity.Property(e => e.Area)
                       .HasColumnName("Area");

                entity.Property(e => e.StepOfProduct)
                       .HasColumnName("StepOfProduct");

                entity.Property(e => e.Idpk)
                       .HasColumnName("Idpk");

                // Double fields
                entity.Property(e => e.ProductRecycleRate)
                       .HasColumnName("ProductRecycleRate");

                entity.Property(e => e.ProductWeight)
                       .HasColumnName("ProductWeight");

                entity.Property(e => e.ProductMaxTemp)
                       .HasColumnName("ProductMaxTemp");

                entity.Property(e => e.ProductAddRate)
                       .HasColumnName("ProductAddRate");

                // Bool fields
                entity.Property(e => e.ProductRohsStandard)
                       .HasColumnName("ProductRohsStandard");

                entity.Property(e => e.ReachStandard)
                       .HasColumnName("ReachStandard");

                // String fields (mặc định text)
                entity.Property(e => e.ProductExpiryType)
                       .HasColumnName("ProductExpiryType")
                       .HasColumnType("citext");

                entity.Property(e => e.BTPStatus)
                       .HasColumnName("BTPStatus")
                       .HasColumnType("citext");

                // ===========================
                entity.Property(e => e.ExpectedCompletionDate)
                       .HasColumnName("ExpectedCompletionDate");

                entity.Property(e => e.CreatedDate)
                       .HasColumnName("createdDate");

                entity.Property(e => e.PlanDate)
                       .HasColumnName("PlanDate");

                // Quan hệ OPTIONAL (cho phép null) + hạn chế xóa
                entity.HasOne(x => x.MfgProductionOrder)
                      .WithMany(a => a.SchedualMfgs) // Ensure MfgProductionOrder has ICollection<SchedualMfg> SchedualMfgs
                      .HasForeignKey(x => x.MfgProductionOrderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_SchedualMfg_MfgProductionOrder_id");

                entity.HasOne(x => x.Product)
                      .WithMany(a => a.SchedualMfgs) // Ensure MfgProductionOrder has ICollection<SchedualMfg> SchedualMfgs
                      .HasForeignKey(x => x.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_SchedualMfg_Product_id");

            });

            ///// ==================================== MRO Module ==================================== 
            //modelBuilder.Entity<AreaMRO>(entity =>
            //{
            //    entity.ToTable("areas", "mro");

            //    entity.HasKey(x => x.AreaId).HasName("pk_areas");

            //    entity.Property(x => x.AreaId)
            //          .UseIdentityByDefaultColumn().HasColumnName("area_id");
            //    entity.Property(x => x.AreaExternalId)
            //          .HasColumnName("area_externalid")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.AreaName)
            //          .HasColumnName("area_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.HasIndex(x => x.AreaExternalId)
            //          .HasDatabaseName("ux_areas_area_externalid")
            //          .IsUnique();
            //});

            //modelBuilder.Entity<WarehouseMRO>(entity =>
            //{
            //    entity.HasKey(e => e.WarehouseId).HasName("PK__Warehouses__3214EC07A98DEC4E");
            //    entity.Property(x => x.WarehouseId).UseIdentityAlwaysColumn();

            //    entity.ToTable("warehouses", "mro");


            //    entity.Property(x => x.WarehouseId).HasColumnName("warehouse_id");
            //    entity.Property(x => x.WarehouseExternalId)
            //          .HasColumnName("warehouse_external_id")
            //          .HasColumnType("text")
            //          .IsRequired();
            //    entity.Property(x => x.WarehouseName)
            //          .HasColumnName("warehouse_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.HasIndex(x => x.WarehouseExternalId)
            //          .HasDatabaseName("ix_warehouses_external_id")
            //          .IsUnique(); // nếu không muốn unique, bỏ dòng này
            //});

            //modelBuilder.Entity<ZoneMRO>(entity =>
            //{
            //    entity.HasKey(e => e.ZoneId).HasName("PK__Zone__3214EC07A98DEC4E");
            //    entity.Property(x => x.ZoneId).UseIdentityAlwaysColumn();

            //    entity.ToTable("zones", "mro");

            //    entity.Property(x => x.ZoneId).HasColumnName("zone_id");
            //    entity.Property(x => x.WarehouseId).HasColumnName("warehouse_id");
            //    entity.Property(x => x.ZoneExternalId)
            //          .HasColumnName("zone_external_id")
            //          .HasColumnType("text")
            //          .IsRequired();
            //    entity.Property(x => x.ZoneName)
            //          .HasColumnName("zone_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.HasIndex(x => x.WarehouseId)
            //          .HasDatabaseName("ix_zones_warehouse_id");

            //    // Một warehouse có nhiều zone
            //    entity.HasOne(z => z.Warehouse)
            //          .WithMany(w => w.Zones)
            //          .HasForeignKey(z => z.WarehouseId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_zones_warehouse_id");

            //    // ZoneExternalId duy nhất trong cùng 1 warehouse
            //    entity.HasIndex(x => new { x.WarehouseId, x.ZoneExternalId })
            //          .HasDatabaseName("ux_zones_warehouse_external")
            //          .IsUnique();
            //});

            //modelBuilder.Entity<RackMRO>(entity =>
            //{
            //    entity.HasKey(e => e.RackId).HasName("PK__Ranks__3214EC07A98DEC4E");
            //    entity.Property(x => x.RackId).UseIdentityAlwaysColumn();

            //    entity.ToTable("racks", "mro");
            //    entity.Property(x => x.RackId).HasColumnName("rack_id");
            //    entity.Property(x => x.ZoneId).HasColumnName("zone_id");
            //    entity.Property(x => x.RackExternalId)
            //          .HasColumnName("rack_external_id")
            //          .HasColumnType("text")
            //          .IsRequired();
            //    entity.Property(x => x.RackName)
            //          .HasColumnName("rack_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.HasIndex(x => x.ZoneId).HasDatabaseName("ix_racks_zone_id");

            //    // Một zone có nhiều rack
            //    entity.HasOne(r => r.Zone)
            //          .WithMany(z => z.Racks)
            //          .HasForeignKey(r => r.ZoneId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_racks_zone_id");

            //    // RackExternalId duy nhất trong cùng 1 zone
            //    entity.HasIndex(x => new { x.ZoneId, x.RackExternalId })
            //          .HasDatabaseName("ux_racks_zone_external")
            //          .IsUnique();
            //});

            //modelBuilder.Entity<SlotMRO>(entity =>
            //{
            //    entity.HasKey(e => e.SlotId).HasName("PK__Slots__3214EC07A98DEC4E");
            //    entity.Property(x => x.SlotId).UseIdentityAlwaysColumn();


            //    entity.ToTable("slots", "mro");
            //    entity.Property(x => x.SlotId).HasColumnName("slot_id");
            //    entity.Property(x => x.RackId).HasColumnName("rack_id");
            //    entity.Property(x => x.SlotExternalId)
            //          .HasColumnName("slot_external_id")
            //          .HasColumnType("text")
            //          .IsRequired();
            //    entity.Property(x => x.SlotName)
            //          .HasColumnName("slot_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.CapacityQty)
            //          .HasColumnName("capacity_qty")
            //          .HasColumnType("integer")
            //          .HasDefaultValue(0);

            //    entity.Property(x => x.CountToCapacity)
            //          .HasColumnName("count_to_capacity")
            //          .HasColumnType("boolean")
            //          .HasDefaultValue(true);

            //    entity.HasIndex(x => x.RackId).HasDatabaseName("ix_slots_rack_id");

            //    // Một rack có nhiều slot
            //    entity.HasOne(s => s.Rack)
            //          .WithMany(r => r.Slots)
            //          .HasForeignKey(s => s.RackId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_slots_rack_id");

            //    // SlotExternalId duy nhất trong cùng 1 rack
            //    entity.HasIndex(x => new { x.RackId, x.SlotExternalId })
            //          .HasDatabaseName("ux_slots_rack_external")
            //          .IsUnique();
            //});

            //modelBuilder.Entity<EquipmentMRO>(entity =>
            //{
            //    entity.ToTable("equipment", "mro");

            //    entity.HasKey(x => x.EquipmentId).HasName("pk_equipment");

            //    entity.Property(x => x.EquipmentId)
            //          .HasColumnName("equipment_id")
            //          .UseIdentityByDefaultColumn(); // Npgsql; nếu SQL Server thì bỏ dòng này

            //    entity.Property(x => x.EquipmentExternalId)
            //          .HasColumnName("equipment_externalid")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.EquipmentName)
            //          .HasColumnName("equipment_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    // soft refs (không FK)
            //    entity.Property(x => x.AreaExternalId)
            //          .HasColumnName("area_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.FactoryExternalId)
            //          .HasColumnName("factory_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.PartExternalId)
            //          .HasColumnName("part_externalid")
            //          .HasColumnType("citext"); // nếu không dùng PostgreSQL -> "text"

            //    // hard FK (tùy chọn)
            //    entity.Property(x => x.AreaId).HasColumnName("area_id");
            //    entity.Property(x => x.FactoryId).HasColumnName("factory_id");
            //    entity.Property(x => x.PartId).HasColumnName("part_id");

            //    // Index tra cứu theo external
            //    entity.HasIndex(x => x.AreaExternalId).HasDatabaseName("ix_equipment_area_externalid");
            //    entity.HasIndex(x => x.FactoryExternalId).HasDatabaseName("ix_equipment_factory_externalid");
            //    entity.HasIndex(x => x.PartExternalId).HasDatabaseName("ix_equipment_part_externalid");

            //    // Unique theo nhà máy + mã ngoài thiết bị (tuỳ nghiệp vụ)
            //    entity.HasIndex(x => new { x.FactoryId, x.EquipmentExternalId })
            //          .HasDatabaseName("ux_equipment_factory_extid")
            //          .IsUnique();

            //    // Quan hệ OPTIONAL (cho phép null) + hạn chế xóa
            //    entity.HasOne(x => x.Area)
            //          .WithMany(a => a.Equipments)
            //          .HasForeignKey(x => x.AreaId)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_equipment_area_id");

            //    entity.HasOne(x => x.Factory)
            //          .WithMany() // hoặc .WithMany(c => c.Equipments) nếu có
            //          .HasForeignKey(x => x.FactoryId)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_equipment_factory_id");

            //    entity.HasOne(x => x.Part)
            //          .WithMany()
            //          .HasForeignKey(x => x.PartId)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_equipment_part_id");
            //});


            //// mro.equipmenttype
            //modelBuilder.Entity<EquipmentTypeMRO>(entity =>
            //{
            //    entity.ToTable("equipmenttype", "mro");

            //    entity.HasKey(x => x.EquipmentTypeId).HasName("pk_equipmenttype");

            //    entity.Property(x => x.EquipmentTypeId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("equipmenttype_id");

            //    entity.Property(x => x.EquipmentTypeName)
            //          .HasColumnName("equipmenttype_name")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.HasIndex(x => x.EquipmentTypeName)
            //          .HasDatabaseName("ix_equipmenttype_name");
            //});

            //// mro.equipment_details
            //modelBuilder.Entity<EquipmentDetailMRO>(entity =>
            //{
            //    entity.ToTable("equipment_details", "mro");

            //    // PK = equipment_id (đồng thời là FK -> equipment)
            //    entity.HasKey(x => x.EquipmentId).HasName("pk_equipment_details");

            //    entity.Property(x => x.EquipmentId)
            //          .HasColumnName("equipment_id")
            //          .ValueGeneratedNever(); // vì lấy từ bảng equipment

            //    entity.Property(x => x.SerialNo).HasColumnName("serial_no").HasColumnType("text");
            //    entity.Property(x => x.Manufacturer).HasColumnName("manufacturer").HasColumnType("text");
            //    entity.Property(x => x.Model).HasColumnName("model").HasColumnType("text");

            //    entity.Property(x => x.PurchaseDate).HasColumnName("purchase_date").HasColumnType("date");
            //    entity.Property(x => x.CommissioningDate).HasColumnName("commissioning_date").HasColumnType("date");
            //    entity.Property(x => x.WarrantyUntil).HasColumnName("warranty_until").HasColumnType("date");

            //    entity.Property(x => x.Notes).HasColumnName("notes").HasColumnType("text");

            //    entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
            //    entity.Property(x => x.UpdatedBy).HasColumnName("updated_by");

            //    entity.Property(x => x.EquipmentTypeId).HasColumnName("equipmenttype_id");

            //    entity.HasIndex(x => x.EquipmentTypeId)
            //          .HasDatabaseName("ix_equipment_details_equipmenttype_id");

            //    // 1–1: Details ←→ Equipment (PK-as-FK), ON DELETE CASCADE
            //    entity.HasOne(d => d.Equipment)
            //          .WithMany(p => p.EquipmentDetails) // nếu không có nav ở Equipment, đổi .WithOne() thành .WithMany()
            //          .HasForeignKey(d => d.EquipmentId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_equipment_details_equipment");

            //    // n–1: Details → EquipmentType
            //    entity.HasOne(d => d.EquipmentType)
            //          .WithMany(t => t.EquipmentDetails)
            //          .HasForeignKey(d => d.EquipmentTypeId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_equipment_details_equipmenttype");
            //});

            //// mro.equipment_specs
            //modelBuilder.Entity<EquipmentSpecMRO>(entity =>
            //{
            //    entity.ToTable("equipment_specs", "mro");

            //    entity.HasKey(x => x.SpecId).HasName("pk_equipment_specs");

            //    entity.Property(x => x.SpecId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("spec_id");

            //    entity.Property(x => x.EquipmentId).HasColumnName("equipment_id");

            //    entity.Property(x => x.SpecKey).HasColumnName("spec_key").HasColumnType("text");
            //    entity.Property(x => x.SpecValue).HasColumnName("spec_value").HasColumnType("text");
            //    entity.Property(x => x.Unit).HasColumnName("unit").HasColumnType("text");
            //    entity.Property(x => x.Note).HasColumnName("note").HasColumnType("text");
            //    entity.Property(x => x.EnteredAt).HasColumnName("entered_at").HasColumnType("timestamp");
            //    entity.Property(x => x.EnteredBy).HasColumnName("entered_by");

            //    entity.HasIndex(x => x.EquipmentId)
            //          .HasDatabaseName("ix_equipment_specs_equipment_id");

            //    // nhiều–1: Specs → Equipment, ON DELETE CASCADE
            //    entity.HasOne(d => d.Equipment)
            //          .WithMany()
            //          .HasForeignKey(d => d.EquipmentId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_equipment_specs_equipment");
            //});


            //modelBuilder.Entity<IncidentHeaderMRO>(entity =>
            //{
            //    entity.ToTable("incident_hdr", "mro");

            //    entity.HasKey(x => x.IncidentId).HasName("pk_incident_hdr");

            //    entity.Property(x => x.IncidentId)
            //          .HasColumnName("incident_id")
            //          .UseIdentityByDefaultColumn(); // nếu SQL Server thì bỏ dòng này

            //    entity.Property(x => x.IncidentCode)
            //          .HasColumnName("incident_code")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Status)
            //          .HasColumnName("status")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Title)
            //          .HasColumnName("title")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Description)
            //          .HasColumnName("description")
            //          .HasColumnType("text");

            //    // soft refs + FK optional
            //    entity.Property(x => x.EquipmentId).HasColumnName("equipment_id");
            //    entity.Property(x => x.EquipmentCode)
            //          .HasColumnName("equipment_code")
            //          .HasColumnType("text"); // dùng "citext" nếu muốn case-insensitive

            //    entity.Property(x => x.AreaId).HasColumnName("area_id");
            //    entity.Property(x => x.AreaCode)
            //          .HasColumnName("area_code")
            //          .HasColumnType("text");

            //    // Company (FK bắt buộc)
            //    entity.Property(x => x.CompanyId).HasColumnName("company_id");

            //    entity.Property(x => x.RolePrefix)
            //          .HasColumnName("role_prefix")
            //          .HasColumnType("text");

            //    entity.Property(x => x.CreatedAt).HasColumnName("created_at");
            //    entity.Property(x => x.CreatedBy).HasColumnName("created_by");

            //    entity.Property(x => x.ExecAt).HasColumnName("exec_at");
            //    entity.Property(x => x.ExecBy).HasColumnName("exec_by");

            //    entity.Property(x => x.DoneAt).HasColumnName("done_at");
            //    entity.Property(x => x.DoneBy).HasColumnName("done_by");

            //    entity.Property(x => x.ClosedAt).HasColumnName("closed_at");
            //    entity.Property(x => x.ClosedBy).HasColumnName("closed_by");

            //    entity.Property(x => x.WaitMin).HasColumnName("wait_min");
            //    entity.Property(x => x.RepairMin).HasColumnName("repair_min");
            //    entity.Property(x => x.TotalMin).HasColumnName("total_min");

            //    // -------- Indexes / Unique --------
            //    entity.HasIndex(x => new { x.CompanyId, x.IncidentCode })
            //          .HasDatabaseName("ux_incident_hdr_company_code")
            //          .IsUnique();

            //    entity.HasIndex(x => x.Status).HasDatabaseName("ix_incident_hdr_status");
            //    entity.HasIndex(x => new { x.EquipmentId, x.EquipmentCode })
            //          .HasDatabaseName("ix_incident_hdr_equipment");
            //    entity.HasIndex(x => x.AreaId).HasDatabaseName("ix_incident_hdr_area");
            //    entity.HasIndex(x => x.CreatedAt).HasDatabaseName("ix_incident_hdr_created_at");

            //    // -------- Relationships --------
            //    entity.HasOne(x => x.Company)
            //          .WithMany()
            //          .HasForeignKey(x => x.CompanyId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_company_id");

            //    entity.HasOne(x => x.Area)
            //          .WithMany()
            //          .HasForeignKey(x => x.AreaId)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_area_id");

            //    entity.HasOne(x => x.Equipment)
            //          .WithMany()
            //          .HasForeignKey(x => x.EquipmentId)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_equipment_id");

            //    entity.HasOne(x => x.CreatedByEmployee)
            //          .WithMany()
            //          .HasForeignKey(x => x.CreatedBy)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_created_by");

            //    entity.HasOne(x => x.ExecByEmployee)
            //          .WithMany()
            //          .HasForeignKey(x => x.ExecBy)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_exec_by");

            //    entity.HasOne(x => x.DoneByEmployee)
            //          .WithMany()
            //          .HasForeignKey(x => x.DoneBy)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_done_by");

            //    entity.HasOne(x => x.ClosedByEmployee)
            //          .WithMany()
            //          .HasForeignKey(x => x.ClosedBy)
            //          .IsRequired(false)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incident_hdr_closed_by");
            //});

            //modelBuilder.Entity<IncidentLineMRO>(entity =>
            //{
            //    // ===== TABLE & PK =====
            //    entity.ToTable("incidenthistory", "mro");
            //    entity.HasKey(x => x.IncidentLineId).HasName("pk_incidenthistory");

            //    entity.Property(x => x.IncidentLineId)
            //          .HasColumnName("incidenthistory_id")
            //          .UseIdentityByDefaultColumn();      // bigint identity (bigserial)

            //    // ===== COLUMNS =====
            //    entity.Property(x => x.IncidentId).HasColumnName("incident_id").IsRequired();

            //    entity.Property(x => x.IncidentCode)
            //          .HasColumnName("incident_code")
            //          .HasColumnType("citext")            // tìm kiếm không phân biệt hoa/thường
            //          .IsRequired();

            //    entity.Property(x => x.Action).HasColumnName("action");
            //    entity.Property(x => x.Summary).HasColumnName("summary");

            //    entity.Property(x => x.StockOutId).HasColumnName("stock_out_id");
            //    entity.Property(x => x.WoRef).HasColumnName("wo_ref").HasColumnType("citext");

            //    entity.Property(x => x.PerformedBy).HasColumnName("performed_by");
            //    entity.Property(x => x.PerformedAt).HasColumnName("performed_at");

            //    entity.Property(x => x.RootCause).HasColumnName("root_cause");
            //    entity.Property(x => x.CorrectiveAction).HasColumnName("corrective_action");
            //    entity.Property(x => x.PreventiveAction).HasColumnName("preventive_action");

            //    entity.Property(x => x.DowntimeMinutes).HasColumnName("downtime_minutes");
            //    entity.Property(x => x.CostEstimate).HasColumnName("cost_estimate").HasPrecision(18, 2);

            //    // ===== INDEXES =====
            //    entity.HasIndex(x => x.IncidentId)
            //          .HasDatabaseName("ix_incidenthistory_incident");

            //    // Paging theo thời điểm thực hiện trong 1 incident (DESC + tie-breaker)
            //    entity.HasIndex(x => new { x.IncidentId, x.PerformedAt, x.IncidentLineId })
            //          .IsDescending(false, true, true)
            //          .HasDatabaseName("ix_incidenthistory_incident_performed_desc");

            //    entity.HasIndex(x => x.StockOutId).HasDatabaseName("ix_incidenthistory_stockout");
            //    entity.HasIndex(x => x.PerformedBy).HasDatabaseName("ix_incidenthistory_performed_by");

            //    // ===== RELATIONSHIPS =====
            //    entity.HasOne(x => x.Incident)
            //          .WithMany()             // cần collection ở IncidentHeaderMRO
            //          .HasForeignKey(x => x.IncidentId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_incidenthistory_incident");

            //    entity.HasOne(x => x.StockOut)
            //          .WithMany()                                  // không cần collection nếu chưa dùng
            //          .HasForeignKey(x => x.StockOutId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incidenthistory_stockout");

            //    entity.HasOne(x => x.PerformedByEmployee)
            //          .WithMany()
            //          .HasForeignKey(x => x.PerformedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_incidenthistory_performed_by");
            //});


            //modelBuilder.Entity<StockOutHeaderMRO>(entity =>
            //{
            //    entity.ToTable("stock_out_hdr", "mro");
            //    entity.HasKey(x => x.StockOutId).HasName("pk_stock_out_hdr");

            //    // ===== Columns =====
            //    entity.Property(x => x.StockOutId)
            //          .UseIdentityByDefaultColumn()                 // BIGINT identity (PostgreSQL)
            //          .HasColumnName("stock_out_id");

            //    entity.Property(x => x.StockOutCode)
            //          .HasColumnName("stock_out_code")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Status)
            //          .HasColumnName("status")
            //          .HasColumnType("text")
            //          .HasDefaultValue("Draft")
            //          .IsRequired();

            //    entity.Property(x => x.Reason)
            //          .HasColumnName("reason")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Note)
            //          .HasColumnName("note")
            //          .HasColumnType("text");

            //    entity.Property(x => x.FactoryId)
            //          .HasColumnName("factory_id")
            //          .IsRequired();

            //    entity.Property(x => x.FactoryCode)
            //          .HasColumnName("factory_code")
            //          .HasColumnType("text");

            //    entity.Property(x => x.SourceType)
            //          .HasColumnName("source_type")
            //          .HasColumnType("text");

            //    entity.Property(x => x.SourceId)
            //          .HasColumnName("source_id");

            //    entity.Property(x => x.SourceCode)
            //          .HasColumnName("source_code")
            //          .HasColumnType("text");

            //    entity.Property(x => x.CreatedBy)
            //          .HasColumnName("created_by")
            //          .IsRequired();

            //    entity.Property(x => x.PostedAt)
            //          .HasColumnName("posted_at");

            //    entity.Property(x => x.PostedBy)
            //          .HasColumnName("posted_by");

            //    // ===== Indexes / Uniques =====
            //    // Mã chứng từ là duy nhất theo kho (hoặc đổi thành company_id nếu bạn dùng tenant theo company)
            //    entity.HasIndex(x => new { x.FactoryId, x.StockOutCode })
            //          .IsUnique()
            //          .HasDatabaseName("ux_stock_out_code_per_factory");

            //    // Danh sách/paging: theo kho + trạng thái + ngày tạo desc (tie-break bằng id)
            //    entity.HasIndex(x => new { x.FactoryId, x.Status, x.CreatedAt, x.StockOutId })
            //          .IsDescending(false, false, true, true)
            //          .HasDatabaseName("ix_stock_out_hdr_factory_status_created_desc");


            //    // ===== Relationships (tùy schema thực tế) =====
            //    entity.HasOne(x => x.Factory).WithMany()
            //          .HasForeignKey(x => x.FactoryId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_stock_out_hdr_factory");

            //    entity.HasOne(x => x.CreatedByNav).WithMany()
            //          .HasForeignKey(x => x.CreatedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_stock_out_hdr_created_by");

            //    entity.HasOne(x => x.PostedByNav).WithMany()
            //          .HasForeignKey(x => x.PostedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_stock_out_hdr_posted_by");

            //    entity.HasOne(x => x.IncidentHeaderMRO).WithMany(a => a.StockOutHeaderMRO)
            //          .HasForeignKey(x => x.SourceId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_stock_out_hdr_incident ");
            //});

            //modelBuilder.Entity<StockOutLineMRO>(entity =>
            //{
            //    entity.ToTable("stock_out_line", "mro");
            //    entity.HasKey(x => x.LineId).HasName("pk_stock_out_line");

            //    // ===== Columns =====
            //    entity.Property(x => x.LineId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("line_id");

            //    entity.Property(x => x.StockOutId)
            //          .HasColumnName("stock_out_id")
            //          .IsRequired();

            //    entity.Property(x => x.MaterialId)
            //          .HasColumnName("material_id")
            //          .IsRequired();

            //    entity.Property(x => x.MaterialExternalId)
            //          .HasColumnName("material_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Qty)
            //          .HasColumnName("qty")
            //          .IsRequired();

            //    entity.Property(x => x.Uom)
            //          .HasColumnName("uom")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.SlotCode)
            //          .HasColumnName("slot_code")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Note)
            //          .HasColumnName("note")
            //          .HasColumnType("text");

            //    // ===== Indexes =====
            //    entity.HasIndex(x => x.StockOutId)
            //          .HasDatabaseName("ix_stock_out_line_stock_out_id");

            //    entity.HasIndex(x => x.MaterialId)
            //          .HasDatabaseName("ix_stock_out_line_material_id");

            //    // ===== Relationships =====
            //    entity.HasOne(x => x.StockOut)
            //          .WithMany() // hoặc .WithMany(h => h.Lines) nếu bạn thêm navigation ở header
            //          .HasForeignKey(x => x.StockOutId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_stock_out_line_stock_out");

            //    entity.HasOne(x => x.Material)
            //          .WithMany()
            //          .HasForeignKey(x => x.MaterialId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_stock_out_line_material");
            //});


            //modelBuilder.Entity<ImprovementHdrMRO>(entity =>
            //{
            //    entity.ToTable("improvement_hdr", "mro");
            //    entity.HasKey(x => x.ProposalId).HasName("pk_improvement_hdr");

            //    // ===== Columns =====
            //    entity.Property(x => x.ProposalId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("proposal_id");

            //    entity.Property(x => x.ProposalExternal)
            //          .HasColumnName("proposal_external")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Title)
            //          .HasColumnName("title")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Description)
            //          .HasColumnName("description")
            //          .HasColumnType("text");

            //    entity.Property(x => x.AreaExternalId)
            //          .HasColumnName("area_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.EquipmentExternalId)
            //          .HasColumnName("equipment_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.FactoryExternalId)
            //          .HasColumnName("factory_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Status)
            //          .HasColumnName("status")
            //          .HasColumnType("text")
            //          .HasDefaultValue("Draft")
            //          .IsRequired();

            //    entity.Property(x => x.CreatedAt)
            //          .HasColumnName("created_at")
            //          .HasDefaultValueSql("timezone('utc', now())");

            //    entity.Property(x => x.CreatedBy).HasColumnName("created_by");
            //    entity.Property(x => x.ApprovedAt).HasColumnName("approved_at");
            //    entity.Property(x => x.ApprovedBy).HasColumnName("approved_by");
            //    entity.Property(x => x.StartedAt).HasColumnName("started_at");
            //    entity.Property(x => x.StartedBy).HasColumnName("started_by");
            //    entity.Property(x => x.DoneAt).HasColumnName("done_at");
            //    entity.Property(x => x.DoneBy).HasColumnName("done_by");
            //    entity.Property(x => x.ClosedAt).HasColumnName("closed_at");
            //    entity.Property(x => x.ClosedBy).HasColumnName("closed_by");

            //    // ===== Indexes =====
            //    // Mã đề xuất duy nhất theo nhà máy (đổi theo nhu cầu: area/factory/company)
            //    entity.HasIndex(x => new { x.FactoryExternalId, x.ProposalExternal })
            //          .IsUnique()
            //          .HasDatabaseName("ux_improvement_hdr_factory_proposal_external");

            //    // List/paging theo nhà máy + trạng thái + thời gian tạo (mới nhất trước)
            //    entity.HasIndex(x => new { x.FactoryExternalId, x.Status, x.CreatedAt, x.ProposalId })
            //          .IsDescending(false, false, true, true)
            //          .HasDatabaseName("ix_improvement_hdr_factory_status_created_desc");

            //    // (Tuỳ) nếu hay lọc theo khu vực/thiết bị:
            //    entity.HasIndex(x => x.AreaExternalId).HasDatabaseName("ix_improvement_hdr_area_externalid");
            //    entity.HasIndex(x => x.EquipmentExternalId).HasDatabaseName("ix_improvement_hdr_equipment_externalid");


            //    // ===== Relationships (bạn có thể gỡ nếu không muốn FK) =====
            //    entity.HasOne(x => x.CreatedByNav).WithMany()
            //          .HasForeignKey(x => x.CreatedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_improvement_hdr_created_by");

            //    entity.HasOne(x => x.ApprovedByNav).WithMany()
            //          .HasForeignKey(x => x.ApprovedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_improvement_hdr_approved_by");

            //    entity.HasOne(x => x.StartedByNav).WithMany()
            //          .HasForeignKey(x => x.StartedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_improvement_hdr_started_by");

            //    entity.HasOne(x => x.DoneByNav).WithMany()
            //          .HasForeignKey(x => x.DoneBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_improvement_hdr_done_by");

            //    entity.HasOne(x => x.ClosedByNav).WithMany()
            //          .HasForeignKey(x => x.ClosedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_improvement_hdr_closed_by");
            //});


            //modelBuilder.Entity<ImprovementHistoryMRO>(entity =>
            //{
            //    entity.ToTable("improvementhistory", "mro");
            //    entity.HasKey(x => x.HistoryId).HasName("pk_improvementhistory");

            //    // ===== Columns =====
            //    entity.Property(x => x.HistoryId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("history_id");

            //    entity.Property(x => x.ProposalId)
            //          .HasColumnName("proposal_id")
            //          .IsRequired();

            //    entity.Property(x => x.ProposalExternal)
            //          .HasColumnName("proposal_external")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Action)
            //          .HasColumnName("action")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Summary)
            //          .HasColumnName("summary")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Minutes)
            //          .HasColumnName("minutes")
            //          .HasPrecision(10, 2);                // numeric(10,2) – tuỳ chỉnh

            //    entity.Property(x => x.Note)
            //          .HasColumnName("note")
            //          .HasColumnType("text");

            //    entity.Property(x => x.WoRef)
            //          .HasColumnName("wo_ref")
            //          .HasColumnType("text");

            //    entity.Property(x => x.PerformedAt)
            //          .HasColumnName("performed_at");

            //    entity.Property(x => x.PerformedBy)
            //          .HasColumnName("performed_by");

            //    // ===== Indexes (list/paging theo proposal) =====
            //    entity.HasIndex(x => new { x.ProposalId, x.PerformedAt, x.HistoryId })
            //          .IsDescending(false, true, true)
            //          .HasDatabaseName("ix_imprhist_proposal_performed_desc");

            //    entity.HasIndex(x => x.PerformedBy)
            //          .HasDatabaseName("ix_imprhist_performed_by");


            //    // ===== Relationships (gỡ nếu muốn UI tự lo) =====

            //    entity.HasOne(x => x.PerformedByNav)
            //          .WithMany()
            //          .HasForeignKey(x => x.PerformedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_imprhist_performed_by");
            //});


            //modelBuilder.Entity<PmPlanMRO>(entity =>
            //{
            //    entity.ToTable("pmplan", "mro");
            //    entity.HasKey(x => x.PlanId).HasName("pk_pmplan");

            //    // Columns
            //    entity.Property(x => x.PlanId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("plan_id");

            //    entity.Property(x => x.PlanExternalId)
            //          .HasColumnName("plan_externalid")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.EquipmentExternalId)
            //          .HasColumnName("equipment_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.DepartmentExternalId)
            //          .HasColumnName("department_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.FactoryExternalId)
            //          .HasColumnName("factory_externalid")
            //          .HasColumnType("text");

            //    entity.Property(x => x.Notes)
            //          .HasColumnName("notes")
            //          .HasColumnType("text");

            //    entity.Property(x => x.IntervalVal)
            //          .HasColumnName("interval_val")
            //          .IsRequired();

            //    entity.Property(x => x.IntervalUnit)
            //          .HasColumnName("interval_unit")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.AnchorDate)
            //          .HasColumnName("anchor_date")
            //          .IsRequired();

            //    entity.Property(x => x.Status)
            //          .HasColumnName("status")
            //          .HasColumnType("text")
            //          .HasDefaultValue("Draft")
            //          .IsRequired();

            //    entity.Property(x => x.CreatedAt)
            //          .HasColumnName("created_at");

            //    entity.Property(x => x.CreatedBy)
            //          .HasColumnName("created_by");

            //    // Indexes (duy nhất mã theo factory; và index cho list/paging)
            //    entity.HasIndex(x => new { x.FactoryExternalId, x.PlanExternalId })
            //          .IsUnique()
            //          .HasDatabaseName("ux_pmplan_factory_plan_external");

            //    entity.HasIndex(x => new { x.FactoryExternalId, x.Status, x.CreatedAt, x.PlanId })
            //          .IsDescending(false, false, true, true)
            //          .HasDatabaseName("ix_pmplan_factory_status_created_desc");

            //    entity.HasIndex(x => x.EquipmentExternalId)
            //          .HasDatabaseName("ix_pmplan_equipment_externalid");

            //});


            //modelBuilder.Entity<PmPlanHistoryMRO>(entity =>
            //{
            //    entity.ToTable("pmplanhistory", "mro");
            //    entity.HasKey(x => x.HistId).HasName("pk_pmplanhistory");

            //    // Columns
            //    entity.Property(x => x.HistId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("hist_id");

            //    entity.Property(x => x.PlanId).HasColumnName("plan_id").IsRequired();

            //    entity.Property(x => x.Action)
            //          .HasColumnName("action")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Details)
            //          .HasColumnName("details")
            //          .HasColumnType("jsonb");

            //    entity.Property(x => x.PerformedBy).HasColumnName("performed_by");
            //    entity.Property(x => x.PerformedAt).HasColumnName("performed_at");

            //    entity.Property(x => x.Note).HasColumnName("note").HasColumnType("text");
            //    entity.Property(x => x.WoRef).HasColumnName("wo_ref").HasColumnType("text");

            //    entity.Property(x => x.PlanExternalId)
            //          .HasColumnName("plan_externalid")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.MinutesSpent)
            //          .HasColumnName("minutesspent")
            //          .HasPrecision(10, 2);

            //    // Indexes: list theo plan, thời gian mới nhất trước
            //    entity.HasIndex(x => new { x.PlanId, x.PerformedAt, x.HistId })
            //          .IsDescending(false, true, true)
            //          .HasDatabaseName("ix_pmplanhist_plan_performed_desc");

            //    entity.HasIndex(x => x.PerformedBy)
            //          .HasDatabaseName("ix_pmplanhist_performed_by");

            //    // Relationships (bỏ nếu không muốn FK)
            //    entity.HasOne(x => x.Plan).WithMany()
            //          .HasForeignKey(x => x.PlanId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_pmplanhist_plan");

            //    //entity.HasOne(x => x.PerformedByNav).WithMany()
            //    //      .HasForeignKey(x => x.PerformedBy)
            //    //      .OnDelete(DeleteBehavior.Restrict)
            //    //      .HasConstraintName("fk_pmplanhist_performed_by");
            //});


            //modelBuilder.Entity<MovementMRO>(entity =>
            //{
            //    entity.ToTable("movements", "mro");
            //    entity.HasKey(x => x.MovementId).HasName("pk_movements");

            //    // Columns
            //    entity.Property(x => x.MovementId)
            //          .HasColumnName("movement_id")
            //          .HasDefaultValueSql("gen_random_uuid()");

            //    entity.Property(x => x.MaterialId).HasColumnName("material_id").IsRequired();
            //    entity.Property(x => x.FromSlotId).HasColumnName("fromslot_id");
            //    entity.Property(x => x.ToSlotId).HasColumnName("toslot_id");

            //    entity.Property(x => x.Qty).HasColumnName("qty").IsRequired();

            //    entity.Property(x => x.MovedAt)
            //          .HasColumnName("moved_at");

            //    entity.Property(x => x.Note).HasColumnName("note").HasColumnType("text");
            //    entity.Property(x => x.RequestExternal).HasColumnName("requestexternal").HasColumnType("text");
            //    entity.Property(x => x.MoveBy).HasColumnName("move_by").HasColumnType("text");

            //    // Indexes cho tra cứu & báo cáo
            //    entity.HasIndex(x => new { x.MaterialId, x.MovedAt, x.MovementId })
            //          .IsDescending(false, true, true)
            //          .HasDatabaseName("ix_movements_material_moved_desc");

            //    entity.HasIndex(x => x.FromSlotId).HasDatabaseName("ix_movements_fromslot");
            //    entity.HasIndex(x => x.ToSlotId).HasDatabaseName("ix_movements_toslot");


            //    entity.HasOne(x => x.Material)
            //          .WithMany()
            //          .HasForeignKey(x => x.MaterialId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_MovementMRO_material");


            //    entity.HasOne(x => x.SlotMROFrom)
            //          .WithMany()
            //          .HasForeignKey(x => x.FromSlotId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_MovementMRO_SlotMROFrom");


            //    entity.HasOne(x => x.SlotMROTo)
            //          .WithMany()
            //          .HasForeignKey(x => x.ToSlotId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_MovementMRO_SlotMROTo");
            //});

            //modelBuilder.Entity<TransferHeaderMRO>(entity =>
            //{
            //    entity.ToTable("transferheaders", "mro");
            //    entity.HasKey(x => x.TransferHeaderId).HasName("pk_transferheaders");

            //    // Columns
            //    entity.Property(x => x.TransferHeaderId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("transferheaders_id");

            //    entity.Property(x => x.TransferHeaderExternalId)
            //          .HasColumnName("transferheaders_externalid")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Note)
            //          .HasColumnName("note")
            //          .HasColumnType("text");

            //    entity.Property(x => x.CreatedAt)
            //          .HasColumnName("created_at");

            //    entity.Property(x => x.CreatedBy)
            //          .HasColumnName("created_by")
            //          .IsRequired();

            //    entity.Property(x => x.Posted)
            //          .HasColumnName("posted")
            //          .HasDefaultValue(false)
            //          .IsRequired();

            //    entity.Property(x => x.PostedAt)
            //          .HasColumnName("posted_at");

            //    entity.Property(x => x.PostedBy)
            //          .HasColumnName("posted_by");

            //    // Indexes
            //    entity.HasIndex(x => x.TransferHeaderExternalId)
            //          .IsUnique()
            //          .HasDatabaseName("ux_transferheaders_externalid");

            //    entity.HasIndex(x => new { x.Posted, x.CreatedAt, x.TransferHeaderId })
            //          .IsDescending(false, true, true)
            //          .HasDatabaseName("ix_transferheaders_posted_created_desc");

            //    entity.HasIndex(x => x.CreatedBy)
            //          .HasDatabaseName("ix_transferheaders_created_by");



            //    // FK mềm tới Employees (bỏ nếu không muốn)
            //    entity.HasOne(x => x.Creator)
            //          .WithMany()
            //          .HasForeignKey(x => x.CreatedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_transferheaders_created_by");
            //    entity.HasOne(x => x.Postor)
            //          .WithMany()
            //          .HasForeignKey(x => x.PostedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_transferheaders_posted_by");

            //});


            //// ======================= mro.transferdetails =======================
            //modelBuilder.Entity<TransferDetailMRO>(entity =>
            //{
            //    entity.ToTable("transferdetails", "mro");
            //    entity.HasKey(x => x.TransferDetailId).HasName("pk_transferdetails");

            //    // Columns
            //    entity.Property(x => x.TransferDetailId)
            //          .UseIdentityByDefaultColumn()
            //          .HasColumnName("transferdetail_id");

            //    entity.Property(x => x.TransferHeaderId)
            //          .HasColumnName("transferheaders_id")
            //          .IsRequired();

            //    entity.Property(x => x.MaterialId)
            //          .HasColumnName("material_id")
            //          .IsRequired();

            //    entity.Property(x => x.FromSlotId)
            //          .HasColumnName("fromslot_id");

            //    entity.Property(x => x.ToSlotId)
            //          .HasColumnName("toslot_id");

            //    entity.Property(x => x.Qty)
            //          .HasColumnName("qty")
            //          .HasPrecision(18, 3)        // numeric(18,3) – chỉnh theo nhu cầu
            //          .IsRequired();

            //    entity.Property(x => x.Note)
            //          .HasColumnName("note")
            //          .HasColumnType("text");

            //    // Indexes
            //    entity.HasIndex(x => x.TransferHeaderId)
            //          .HasDatabaseName("ix_transferdetails_header");

            //    entity.HasIndex(x => x.MaterialId)
            //          .HasDatabaseName("ix_transferdetails_material");

            //    entity.HasIndex(x => x.FromSlotId)
            //          .HasDatabaseName("ix_transferdetails_fromslot");

            //    entity.HasIndex(x => x.ToSlotId)
            //          .HasDatabaseName("ix_transferdetails_toslot");

     
            //    // Quan hệ: Header -> Details (Cascade)
            //    entity.HasOne(x => x.Header)
            //          .WithMany(h => h.Details)
            //          .HasForeignKey(x => x.TransferHeaderId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_transferdetails_header");

            //    // Nếu muốn FK cứng tới Material (đang có entity Material):
            //    // entity.HasOne<Material>()
            //    //       .WithMany()
            //    //       .HasForeignKey(x => x.MaterialId)
            //    //       .OnDelete(DeleteBehavior.Restrict)
            //    //       .HasConstraintName("fk_transferdetails_material");
            //});

            /// ==================================== Audit Module ==================================== 
            //modelBuilder.Entity<CodeCounter>(entity =>
            //{
            //    entity.ToTable("code_counters", "Audit");

            //    // PK tổng hợp (Prefix, Ymd)
            //    entity.HasKey(x => new { x.Prefix, x.Ymd })
            //          .HasName("pk_code_counters_prefix_ymd");

            //    entity.Property(x => x.Prefix).HasColumnName("prefix")
            //          .HasColumnType("text")
            //          .IsRequired();

            //    entity.Property(x => x.Ymd).HasColumnName("ymd")
            //          .HasColumnType("integer")
            //          .IsRequired();

            //    entity.Property(x => x.LastValue).HasColumnName("last_value")
            //          .HasColumnType("integer")
            //          .HasDefaultValue(0)
            //          .IsRequired();

            //    // Index phụ để tra cứu theo prefix nhanh
            //    entity.HasIndex(x => x.Prefix)
            //          .HasDatabaseName("ix_code_counters_prefix");
            //});

            ///// ==================================== Energy Module ==================================== 
            //modelBuilder.Entity<EnergyGroup>(entity =>
            //{
            //    entity.ToTable("groups", "energy");
            //    entity.HasKey(x => x.GroupId).HasName("pk_energy_groups");

            //    entity.Property(x => x.GroupId)
            //          .HasColumnName("group_id")
            //          .UseIdentityByDefaultColumn();               // smallserial

            //    entity.Property(x => x.Code)
            //          .HasColumnName("code")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    entity.Property(x => x.Name)
            //          .HasColumnName("name")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    // code duy nhất
            //    entity.HasIndex(x => x.Code)
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_groups_code");
            //});

            //modelBuilder.Entity<EnergyMeter>(entity =>
            //{
            //    entity.ToTable("meters", "energy");
            //    entity.HasKey(x => x.MeterId).HasName("pk_energy_meters");

            //    entity.Property(x => x.MeterId)
            //          .HasColumnName("meter_id")
            //          .UseIdentityByDefaultColumn();               // bigserial

            //    entity.Property(x => x.Code)
            //          .HasColumnName("code")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    entity.Property(x => x.Name)
            //          .HasColumnName("name")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    entity.Property(x => x.Multiplier)
            //          .HasColumnName("multiplier")
            //          .HasPrecision(10, 4)
            //          .HasDefaultValue(1.0000m);

            //    entity.Property(x => x.IsActive)
            //          .HasColumnName("is_active")
            //          .HasDefaultValue(true);

            //    entity.Property(x => x.GroupId)
            //          .HasColumnName("group_id")
            //          .IsRequired();

            //    // Indexes: tra cứu theo code, list theo group/active
            //    entity.HasIndex(x => x.Code)
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_meters_code");

            //    entity.HasIndex(x => new { x.GroupId, x.IsActive, x.MeterId })
            //          .IsDescending(false, false, true)
            //          .HasDatabaseName("ix_energy_meters_group_active");


            //    // FK -> groups
            //    entity.HasOne(x => x.Group)
            //          .WithMany(g => g.Meters)
            //          .HasForeignKey(x => x.GroupId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_meters_group");
            //});

            //modelBuilder.Entity<EnergyTariff>(entity =>
            //{
            //    entity.ToTable("tariffs", "energy");
            //    entity.HasKey(x => x.TariffId).HasName("pk_energy_tariffs");

            //    entity.Property(x => x.TariffId)
            //          .HasColumnName("tariff_id")
            //          .UseIdentityByDefaultColumn();               // serial

            //    entity.Property(x => x.Code)
            //          .HasColumnName("code")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    entity.Property(x => x.Name)
            //          .HasColumnName("name")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    entity.Property(x => x.Currency)
            //          .HasColumnName("currency")
            //          .HasColumnType("citext")
            //          .HasDefaultValue("VND");

            //    entity.Property(x => x.Utility)
            //          .HasColumnName("utility")
            //          .HasColumnType("citext");

            //    entity.Property(x => x.Note)
            //          .HasColumnName("note")
            //          .HasColumnType("citext");

            //    entity.HasIndex(x => x.Code)
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_tariffs_code");
            //});

            //modelBuilder.Entity<EnergyTariffVersion>(entity =>
            //{
            //    entity.ToTable("tariff_versions", "energy");
            //    entity.HasKey(x => x.VersionId).HasName("pk_energy_tariff_versions");

            //    entity.Property(x => x.VersionId)
            //          .HasColumnName("version_id")
            //          .UseIdentityByDefaultColumn();

            //    entity.Property(x => x.TariffId)
            //          .HasColumnName("tariff_id")
            //          .IsRequired();

            //    entity.Property(x => x.ValidFrom)
            //          .HasColumnName("valid_from")
            //          .HasColumnType("date")
            //          .HasDefaultValueSql("timezone('utc', now())")
            //          .IsRequired();

            //    entity.Property(x => x.ValidTo)
            //          .HasColumnName("valid_to")
            //          .HasColumnType("date")
            //          .HasDefaultValueSql("timezone('utc', now())");

            //    entity.Property(x => x.VatRate)
            //          .HasColumnName("vat_rate")
            //          .HasPrecision(6, 4)
            //          .HasDefaultValue(0m);

            //    entity.Property(x => x.FuelAdjVndPerKwh)
            //          .HasColumnName("fuel_adj_vnd_per_kwh")
            //          .HasPrecision(12, 4)
            //          .HasDefaultValue(0m);

            //    entity.Property(x => x.ServiceFixedVndPerMonth)
            //          .HasColumnName("service_fixed_vnd_per_month")
            //          .HasPrecision(14, 2)
            //          .HasDefaultValue(0m);

            //    entity.Property(x => x.DemandRateVndPerKw)
            //          .HasColumnName("demand_rate_vnd_per_kw")
            //          .HasPrecision(14, 2)
            //          .HasDefaultValue(0m);

            //    // Indexes
            //    entity.HasIndex(x => x.TariffId)
            //          .HasDatabaseName("ix_energy_tariff_versions_tariff_id");

            //    // 1 phiên bản bắt đầu từ một ngày cho mỗi tariff
            //    entity.HasIndex(x => new { x.TariffId, x.ValidFrom })
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_tariff_versions_tariff_validfrom");

            //    // Check: valid_to >= valid_from (hoặc NULL)

            //    // FK
            //    entity.HasOne(x => x.Tariff)
            //          .WithMany(t => t.Versions)
            //          .HasForeignKey(x => x.TariffId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_tariff_versions_tariff");
            //});


            //modelBuilder.Entity<EnergyTariffBandRate>(entity =>
            //{
            //    entity.ToTable("tariff_band_rates", "energy");
            //    entity.HasKey(x => new { x.VersionId, x.Band })
            //          .HasName("pk_energy_tariff_band_rates");

            //    entity.Property(x => x.VersionId).HasColumnName("version_id");

            //    entity.Property(x => x.Band)
            //          .HasColumnName("band")
            //          .HasColumnType("citext")
            //          .IsRequired();

            //    entity.Property(x => x.PriceVndPerKwh)
            //          .HasColumnName("price_vnd_per_kwh")
            //          .HasPrecision(12, 4)
            //          .IsRequired();

            //    entity.HasIndex(x => x.VersionId)
            //          .HasDatabaseName("ix_energy_tariff_band_rates_version");

            //    entity.HasOne(x => x.Version)
            //          .WithMany(v => v.BandRates)
            //          .HasForeignKey(x => x.VersionId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_energy_tariff_band_rates_version");
            //});

            //modelBuilder.Entity<EnergyRegisterSnapshot>(entity =>
            //{
            //    entity.ToTable("register_snapshots", "energy");
            //    entity.HasKey(x => new { x.MeterId, x.TsUtc })
            //          .HasName("pk_energy_register_snapshots");

            //    entity.Property(x => x.MeterId).HasColumnName("meter_id");

            //    entity.Property(x => x.TsUtc)
            //          .HasColumnName("ts_utc")
            //          ; // UTC

            //    entity.Property(x => x.KwhTotal)
            //          .HasColumnName("kwh_total")
            //          .HasPrecision(18, 4)
            //          .IsRequired();

            //    entity.Property(x => x.Source)
            //          .HasColumnName("source")
            //          .HasColumnType("citext");

            //    // Phục vụ list nhanh theo thời gian mới nhất
            //    entity.HasIndex(x => new { x.MeterId, x.TsUtc })
            //          .IsDescending(false, true)
            //          .HasDatabaseName("ix_energy_register_snapshots_meter_ts");

            //    entity.HasOne(x => x.Meter)
            //          .WithMany() // hoặc .WithMany(m => m.RegisterSnapshots) nếu bạn thêm navigation
            //          .HasForeignKey(x => x.MeterId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_energy_register_snapshots_meter");
            //});

            //modelBuilder.Entity<EnergyMeterGroupHistory>(entity =>
            //{
            //    entity.ToTable("meter_group_history", "energy");

            //    // PK tổng hợp: (meter_id, valid_from)
            //    entity.HasKey(x => new { x.MeterId, x.ValidFrom })
            //          .HasName("pk_energy_meter_group_history");

            //    // Columns
            //    entity.Property(x => x.MeterId).HasColumnName("meter_id");
            //    entity.Property(x => x.GroupId).HasColumnName("group_id");
            //    entity.Property(x => x.ValidFrom).HasColumnName("valid_from")
            //          .HasDefaultValueSql("timezone('utc', now())");
            //    entity.Property(x => x.ValidTo).HasColumnName("valid_to")
            //          .HasDefaultValueSql("timezone('utc', now())");

            //    // Indexes (tra cứu nhanh theo meter + thời gian)
            //    entity.HasIndex(x => x.MeterId)
            //          .HasDatabaseName("ix_energy_mgh_meter");
            //    entity.HasIndex(x => new { x.MeterId, x.ValidFrom })
            //          .IsDescending(false, true)
            //          .HasDatabaseName("ix_energy_mgh_meter_validfrom_desc");

            //    // Check: valid_to >= valid_from (hoặc NULL)

            //    // FK
            //    entity.HasOne(x => x.Meter)
            //          .WithMany() // không bắt buộc thêm collection ở EnergyMeter
            //          .HasForeignKey(x => x.MeterId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_energy_mgh_meter");

            //    entity.HasOne(x => x.Group)
            //          .WithMany()
            //          .HasForeignKey(x => x.GroupId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_mgh_group");
            //});

            ///* Nếu bạn muốn CHẶN trùng khoảng thời gian cho mỗi meter:
            //   Thêm ở migration:
            //   migrationBuilder.Sql(@"
            //     CREATE EXTENSION IF NOT EXISTS btree_gist;
            //     ALTER TABLE energy.meter_group_history
            //     ADD CONSTRAINT ex_energy_mgh_period
            //     EXCLUDE USING gist (
            //       meter_id WITH =,
            //       tstzrange(valid_from, COALESCE(valid_to, 'infinity'), '[]') WITH &&
            //     );
            //   ");
            //*/


            //modelBuilder.Entity<EnergyMeterCommConfig>(entity =>
            //{
            //    entity.ToTable("meter_comm_config", "energy");

            //    // PK = meter_id (1–1 theo meter)
            //    entity.HasKey(x => x.MeterId)
            //          .HasName("pk_energy_meter_comm_config");

            //    // Columns
            //    entity.Property(x => x.MeterId).HasColumnName("meter_id");
            //    entity.Property(x => x.Protocol).HasColumnName("protocol").HasColumnType("text");
            //    entity.Property(x => x.SerialPort).HasColumnName("serial_port").HasColumnType("text");
            //    entity.Property(x => x.BaudRate).HasColumnName("baud_rate");
            //    entity.Property(x => x.Parity).HasColumnName("parity").HasColumnType("text");
            //    entity.Property(x => x.DataBits).HasColumnName("data_bits");
            //    entity.Property(x => x.StopBits).HasColumnName("stop_bits");
            //    entity.Property(x => x.SlaveId).HasColumnName("slave_id");
            //    entity.Property(x => x.RegKwhAddr).HasColumnName("reg_kwh_addr");
            //    entity.Property(x => x.RegKwhLen).HasColumnName("reg_kwh_len");
            //    entity.Property(x => x.WordOrder).HasColumnName("word_order").HasColumnType("text");
            //    entity.Property(x => x.Scale).HasColumnName("scale").HasPrecision(12, 6);

            //    // FK 1–1
            //    entity.HasOne(x => x.Meter)
            //          .WithOne() // không cần navigation ở EnergyMeter
            //          .HasForeignKey<EnergyMeterCommConfig>(x => x.MeterId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_energy_mcc_meter");
            //});


            //modelBuilder.Entity<EnergyGroupTariffMap>(entity =>
            //{
            //    entity.ToTable("group_tariff_map", "energy");

            //    // PK tổng hợp (group_id, valid_from)
            //    entity.HasKey(x => new { x.GroupId, x.ValidFrom })
            //          .HasName("pk_energy_group_tariff_map");

            //    // Columns
            //    entity.Property(x => x.GroupId).HasColumnName("group_id");
            //    entity.Property(x => x.TariffId).HasColumnName("tariff_id");
            //    entity.Property(x => x.ValidFrom).HasColumnName("valid_from")
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnType("date");
            //    entity.Property(x => x.ValidTo).HasColumnName("valid_to")
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnType("date");

            //    // Indexes
            //    entity.HasIndex(x => x.TariffId)
            //          .HasDatabaseName("ix_energy_gtm_tariff");
            //    entity.HasIndex(x => new { x.GroupId, x.ValidFrom })
            //          .IsDescending(false, true)
            //          .HasDatabaseName("ix_energy_gtm_group_validfrom_desc");

            //    // FK
            //    // 👉 Chỉ rõ navigation
            //    entity.HasOne(x => x.Group)
            //          .WithMany(g => g.EnergyGroupTariffMaps)
            //          .HasForeignKey(x => x.GroupId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_gtm_group");

            //    entity.HasOne(x => x.Tariff)
            //          .WithMany(t => t.EnergyGroupTariffMaps)
            //          .HasForeignKey(x => x.TariffId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_gtm_tariff");
            //});

            ///* (Tuỳ chọn – chống overlap khoảng ngày trên từng group)
            //   Trong migration thêm:
            //   migrationBuilder.Sql(@"
            //     CREATE EXTENSION IF NOT EXISTS btree_gist;
            //     ALTER TABLE energy.group_tariff_map
            //     ADD CONSTRAINT ex_energy_gtm_period
            //     EXCLUDE USING gist (
            //       group_id WITH =,
            //       daterange(valid_from, COALESCE(valid_to, 'infinity'::date), '[]') WITH &&
            //     );
            //   ");
            //*/


            //modelBuilder.Entity<EnergyTouCalendar>(entity =>
            //{
            //    entity.ToTable("tou_calendar", "energy");

            //    entity.HasKey(x => x.CalendarId)
            //          .HasName("pk_energy_tou_calendar");

            //    entity.Property(x => x.CalendarId).HasColumnName("calendar_id");
            //    entity.Property(x => x.Code).HasColumnName("code").HasColumnType("text").IsRequired();
            //    entity.Property(x => x.Tz).HasColumnName("tz").HasColumnType("text");
            //    entity.Property(x => x.StartDate).HasColumnName("start_date")
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnType("date");
            //    entity.Property(x => x.EndDate).HasColumnName("end_date")
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnType("date");

            //    // Unique theo code
            //    entity.HasIndex(x => x.Code)
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_tou_calendar_code");

            //});


            //modelBuilder.Entity<EnergyTouException>(entity =>
            //{
            //    entity.ToTable("tou_exceptions", "energy");

            //    entity.HasKey(x => x.ExceptionId)
            //          .HasName("pk_energy_tou_exceptions");

            //    entity.Property(x => x.ExceptionId).HasColumnName("exception_id");
            //    entity.Property(x => x.CalendarId).HasColumnName("calendar_id");
            //    entity.Property(x => x.TheDate).HasColumnName("the_date").HasColumnType("date");
            //    entity.Property(x => x.StartTime)
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnName("start_time");
            //    entity.Property(x => x.EndTime)
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnName("end_time");
            //    entity.Property(x => x.Band).HasColumnName("band").HasColumnType("text");
            //    entity.Property(x => x.Note).HasColumnName("note").HasColumnType("text");

            //    // Tra cứu: theo calendar + ngày
            //    entity.HasIndex(x => new { x.CalendarId, x.TheDate })
            //          .HasDatabaseName("ix_energy_tou_exc_calendar_date");


            //    // (Tuỳ chọn) chặn trùng một slot trong cùng ngày:
            //    entity.HasIndex(x => new { x.CalendarId, x.TheDate, x.Band, x.StartTime })
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_tou_exc_unique_slot");

            //    // FK
            //    entity.HasOne(x => x.Calendar)
            //          .WithMany(c => c.Exceptions)
            //          .HasForeignKey(x => x.CalendarId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_energy_tou_exc_calendar");
            //});


            //modelBuilder.Entity<EnergyTouWindow>(entity =>
            //{
            //    entity.ToTable("tou_windows", "energy");
            //    entity.HasKey(x => x.WindowId).HasName("pk_energy_tou_windows");

            //    entity.Property(x => x.WindowId).HasColumnName("window_id")
            //          .UseIdentityByDefaultColumn();
            //    entity.Property(x => x.CalendarId).HasColumnName("calendar_id");
            //    entity.Property(x => x.Weekday).HasColumnName("weekday");
            //    entity.Property(x => x.StartTime)
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnName("start_time");
            //    entity.Property(x => x.EndTime)
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnName("end_time");
            //    entity.Property(x => x.Band).HasColumnName("band").HasColumnType("text");

            //    // FK
            //    entity.HasOne(w => w.Calendar)
            //          .WithMany() // hoặc .WithMany(c => c.Windows) nếu bạn thêm navigation
            //          .HasForeignKey(x => x.CalendarId)
            //          .OnDelete(DeleteBehavior.Cascade)
            //          .HasConstraintName("fk_energy_tou_windows_calendar");

            //    // Index & ràng buộc
            //    entity.HasIndex(x => new { x.CalendarId, x.Weekday, x.StartTime })
            //          .IsUnique()
            //          .HasDatabaseName("ux_energy_tou_windows_cal_wd_start");
            //});

            ///* Tuỳ chọn (chống overlap khung giờ trong cùng calendar + weekday):
            //   migrationBuilder.Sql(@"
            //     CREATE EXTENSION IF NOT EXISTS btree_gist;
            //     ALTER TABLE energy.tou_windows
            //     ADD CONSTRAINT ex_energy_tou_windows
            //     EXCLUDE USING gist (
            //       calendar_id WITH =,
            //       weekday WITH =,
            //       tstzrange(start_time, end_time, '[)') WITH &&
            //     );
            //   ");
            //*/

            //modelBuilder.Entity<EnergyReadingsHourly>(entity =>
            //{
            //    entity.ToTable("readings_hourly", "energy");
            //    entity.HasKey(x => new { x.MeterId, x.TsUtc })
            //          .HasName("pk_energy_readings_hourly"); // PK (meter_id, ts_utc)

            //    entity.Property(x => x.MeterId).HasColumnName("meter_id");
            //    entity.Property(x => x.TsUtc)
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnName("ts_utc");
            //    entity.Property(x => x.KwhImport).HasColumnName("kwh_import").HasPrecision(14, 5);
            //    entity.Property(x => x.Quality).HasColumnName("quality").HasColumnType("text");
            //    entity.Property(x => x.Source).HasColumnName("source").HasColumnType("text");


            //    // Tra cứu nhanh theo thời gian 1 công tơ (ORDER BY desc)
            //    entity.HasIndex(x => new { x.MeterId, x.TsUtc })
            //          .IsDescending(false, true)
            //          .HasDatabaseName("ix_energy_rh_meter_ts_desc");

            //    entity.HasOne(x => x.Meter)     // giữ nhẹ: không bắt buộc nav ở meter
            //          .WithMany()
            //          .HasForeignKey(x => x.MeterId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_rh_meter");
            //});

            //modelBuilder.Entity<EnergyReadingsHourlyVn>(entity =>
            //{
            //    entity.ToTable("readings_hourly_vn", "energy");
            //    entity.HasKey(x => new { x.MeterId, x.TsHourVn })
            //          .HasName("pk_energy_readings_hourly_vn");

            //    entity.Property(x => x.MeterId).HasColumnName("meter_id");
            //    entity.Property(x => x.TsHourVn)
            //    .HasDefaultValueSql("timezone('utc', now())").HasColumnName("ts_hour_vn");
            //    entity.Property(x => x.KwhImport).HasColumnName("kwh_import").HasPrecision(14, 5);
            //    entity.Property(x => x.Quality).HasColumnName("quality").HasColumnType("text");
            //    entity.Property(x => x.Source).HasColumnName("source").HasColumnType("text");


            //    // Theo note "tạo index cho cột này"
            //    entity.HasIndex(x => x.TsHourVn)
            //          .HasDatabaseName("ix_energy_rhvn_tshour");

            //    entity.HasIndex(x => new { x.MeterId, x.TsHourVn })
            //          .IsDescending(false, true)
            //          .HasDatabaseName("ix_energy_rhvn_meter_tshour_desc");

            //    entity.HasOne(x => x.Meter)     // giữ nhẹ: không bắt buộc nav ở meter
            //          .WithMany()
            //          .HasForeignKey(x => x.MeterId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("fk_energy_rhvn_meter");
            //});
        }
    }
}





