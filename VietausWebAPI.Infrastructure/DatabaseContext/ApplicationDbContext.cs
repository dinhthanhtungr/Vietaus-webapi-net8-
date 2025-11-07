
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
using VietausWebAPI.Core.Domain.Entities.ManufacturingSchema;
using VietausWebAPI.Core.Domain.Entities.MROSchema;



//using System.Text.RegularExpressions;
using VietausWebAPI.Core.Identity;
using VietausWebAPI.Infrastructure.Helpers.IdCounter;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace VietausWebAPI.WebAPI.DatabaseContext
{
    // Scaffold-DbContext "Server=DESKTOP-BL5L5IM;Database=VietausDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -context ApplicationDbContext


    //Scaffold-DbContext "Host=Localhost;Port=5432;Database=VietausDb;Username=postgres;Password=qazwsxedc123@" 

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        //static ApplicationDbContext()
        //{
        //    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        //}

        public DbSet<IdCounter> IdCounters { get; set; } = default!;

        public virtual DbSet<AttachmentCollection> AttachmentCollections { get; set; }
        public virtual DbSet<AttachmentModel> AttachmentModels { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<ApprovalHistory> ApprovalHistories { get; set; }

        //public virtual DbSet<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; }

        //public virtual DbSet<ApprovalLevelsCommonDatum> ApprovalLevelsCommonData { get; set; }

        public virtual DbSet<Branch> Branches { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

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

        //public virtual DbSet<EmployeesCommonDatum> EmployeesCommonData { get; set; }
        public virtual DbSet<EventLog> EventLogs { get; set; }

        public virtual DbSet<Formula> Formulas { get; set; }

        public virtual DbSet<FormulaMaterial> FormulaMaterials { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        //public virtual DbSet<GroupsCommonDatum> GroupsCommonData { get; set; }

        //public virtual DbSet<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; }

        //public virtual DbSet<MachinesCommonDatum> MachinesCommonData { get; set; }

        public virtual DbSet<Material> Materials { get; set; }

        //public virtual DbSet<MaterialGroup> MaterialGroups { get; set; }

        //public virtual DbSet<MaterialGroupsMaterialDatum> MaterialGroupsMaterialData { get; set; }

        //public virtual DbSet<MaterialsMaterialDatum> MaterialsMaterialData { get; set; }

        public virtual DbSet<MaterialsSupplier> MaterialsSuppliers { get; set; }

        //public virtual DbSet<MaterialsSuppliersMaterialDatum> MaterialsSuppliersMaterialData { get; set; }

        public virtual DbSet<ManufacturingFormula> ManufacturingFormulas { get; set; }
        public virtual DbSet<ManufacturingFormulaMaterial> ManufacturingFormulaMaterials { get; set; }
        //public virtual DbSet<ManufacturingFormulaLog> ManufacturingFormulaLogs { get; set; }

        public virtual DbSet<MemberInGroup> MemberInGroups { get; set; }

        public virtual DbSet<MerchandiseOrder> MerchandiseOrders { get; set; }
        //public virtual DbSet<MerchandiseOrderLog> MerchandiseOrderLogs { get; set; }

        public virtual DbSet<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; }

        //public virtual DbSet<MerchandiseOrderSchedule> MerchandiseOrderSchedules { get; set; }
        //public virtual DbSet<OrderAttachment> OrderAttachments { get; set; }

        public virtual DbSet<MfgProductionOrdersPlan> MfgProductionOrdersPlans { get; set; }
        public virtual DbSet<MfgProductionOrder> MfgProductionOrders { get; set; }
        public virtual DbSet<MfgProductionOrderLog> MfgProductionOrderLogs { get; set; }

        public virtual DbSet<Part> Parts { get; set; }

        //public virtual DbSet<PartsCommonDatum> PartsCommonData { get; set; }

        public virtual DbSet<PriceHistory> PriceHistories { get; set; }

        //public virtual DbSet<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        //public virtual DbSet<ProductChangedHistory> ProductChangedHistories { get; set; }

        public virtual DbSet<ProductInspection> ProductInspections { get; set; }

        public virtual DbSet<ProductStandard> ProductStandards { get; set; }

        public virtual DbSet<ProductTest> ProductTests { get; set; }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public virtual DbSet<PurchaseOrderSnapshot> PurchaseOrderSnapshots { get; set; }

        //public virtual DbSet<PurchaseOrderDetailsMaterialDatum> PurchaseOrderDetailsMaterialData { get; set; }

        public virtual DbSet<PurchaseOrderStatusHistory> PurchaseOrderStatusHistories { get; set; }

        //public virtual DbSet<PurchaseOrderStatusHistoryMaterialDatum> PurchaseOrderStatusHistoryMaterialData { get; set; }

        //public virtual DbSet<PurchaseOrdersMaterialDatum> PurchaseOrdersMaterialData { get; set; }

        public virtual DbSet<PurchaseOrdersSchedule> PurchaseOrdersSchedules { get; set; }

        public virtual DbSet<Qcdetail> Qcdetails { get; set; }

        public virtual DbSet<RequestDetail> RequestDetails { get; set; }

        public virtual DbSet<PurchaseOrderLink> PurchaseOrderLinks { get; set; }
        //public virtual DbSet<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; }

        public virtual DbSet<SampleRequest> SampleRequests { get; set; }
        //public virtual DbSet<SampleRequestImage> SampleRequestImages { get; set; }

        public virtual DbSet<SchedualMfg> SchedualMfgs { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<SupplierAddress> SupplierAddresses { get; set; }

        public virtual DbSet<SupplierContact> SupplierContacts { get; set; }

        //public virtual DbSet<SuppliersMaterialDatum> SuppliersMaterialData { get; set; }

        public virtual DbSet<SupplyRequest> SupplyRequests { get; set; }

        //public virtual DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<WarehouseShelfStock> WarehouseShelfStocks { get; set; }
        public virtual DbSet<WarehouseTempStock> WarehouseTempStocks { get; set; }
        public virtual DbSet<WarehouseRequest> WarehouseRequests { get; set; }
        public virtual DbSet<WarehouseRequestDetail> WarehouseRequestDetails { get; set; }


        /// <summary>
        /// ==================================== MRO Module ==================================== 
        /// </summary>
        public virtual DbSet<AreaMRO> AreaMROs { get; set; }
        public virtual DbSet<WarehouseMRO> WarehouseMROs { get; set; }
        public virtual DbSet<ZoneMRO> ZoneMROs { get; set; }
        public virtual DbSet<RackMRO> RackMROs { get; set; }
        public virtual DbSet<SlotMRO> SlotMROs { get; set; }
        public virtual DbSet<EquipmentMRO> EquipmentMROs { get; set; }
        public virtual DbSet<IncidentHeaderMRO> IncidentHeaderMROs { get; set; }


        /// <summary>
        /// ==================================== Audit Module ==================================== 
        /// </summary>
        public virtual DbSet<CodeCounter> CodeCounters { get; set; }


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

            modelBuilder.Entity<ApprovalHistory>(entity =>
            {
                entity.HasKey(e => e.ApprovalId).HasName("PK__Approval__328477D4B3FEB4F1");

                entity.ToTable("ApprovalHistory", "SupplyRequest");

                entity.HasIndex(e => e.EmployeeId, "IX_ApprovalHistory_EmployeeID");

                entity.HasIndex(e => e.RequestId, "IX_ApprovalHistory_RequestID");

                entity.Property(e => e.ApprovalId)
                    .ValueGeneratedNever()
                    .HasColumnName("ApprovalID");
                entity.Property(e => e.ApprovalStatus).HasMaxLength(16);
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.HasOne(d => d.Employee).WithMany(p => p.ApprovalHistories)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_ApprovalHistory_EmployeeID");

                entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistories)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_ApprovalHistory_RequestID");
            });



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

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FC5D195FBDD");

                entity.ToTable("Branches", "company");

                entity.HasIndex(e => e.CompanyId, "IX_Branches_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Branches_CreatedBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_Branches_UpdatedBy");

                entity.Property(e => e.BranchId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Company).WithMany(p => p.Branches)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__Branches__Compan__0C70CFB4");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BranchCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Branches_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BranchUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Branches_UpdatedBy");
            });

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

                entity.Property(e => e.CompanyId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Code).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(200);

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

                entity.HasIndex(e => e.CompanyId, "IX_DeliveryOrders_CompanyId");
                entity.HasIndex(e => e.CreatedBy, "IX_DeliveryOrders_CreatedBy");
                entity.HasIndex(e => e.UpdatedBy, "IX_DeliveryOrders_UpdatedBy");
                entity.HasIndex(e => e.CustomerId, "IX_DeliveryOrders_CustomerId");
                //entity.HasIndex(e => e.MerchandiseOrderId, "IX_DeliveryOrders_MerchandiseOrderId");
                //entity.HasIndex(e => e.WarehouseRequestId, "IX_DeliveryOrders_WarehouseRequestId");

                entity.Property(x => x.ExternalId).HasColumnType("citext");
                //entity.Property(x => x.MerchandiseOrderExternalIdSnapShot).HasColumnType("citext");
                entity.Property(x => x.CustomerExternalIdSnapShot).HasColumnType("citext");
                entity.Property(x => x.IsActive).HasDefaultValue(true);

                //entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.DeliveryOrders)
                //    .HasForeignKey(d => d.MerchandiseOrderId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_DeliveryOrder_MerchandiseOrder");

                //entity.HasOne(d => d.WarehouseRequest).WithMany(p => p.DeliveryOrders)
                //    .HasForeignKey(d => d.WarehouseRequestId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_DeliveryOrder_WarehouseRequest");

                entity.HasOne(d => d.Customer).WithMany()
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_Customer");

                entity.HasOne(d => d.Company).WithMany()
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany()
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeliveryOrder_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany()
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
                entity.HasIndex(e => e.MerchandiseOrderId, "IX_DeliveryOrderDetails_ID");

                entity.Property(x => x.ProductExternalIdSnapShot).HasColumnType("citext");
                entity.Property(x => x.LotNoList).HasColumnType("citext");


                entity.Property(x => x.MerchandiseOrderExternalIdSnapShot).HasColumnType("citext");
                entity.Property(x => x.IsActive).HasDefaultValue(true);
                entity.Property(x => x.IsAttach).HasDefaultValue(true);


                // DECIMAL(18,3) cho số lượng
                entity.Property(x => x.Quantity).HasPrecision(18, 3);
                entity.Property(x => x.NumOfBags);

                entity.Property(x => x.PONo).HasColumnType("citext");


                // (1) Ràng buộc "detail phải đúng dòng của PO"
                entity.HasOne<MerchandiseOrderDetail>(d => d.MerchandiseOrderDetail)
                    .WithMany()
                    .HasPrincipalKey(mod => new { mod.MerchandiseOrderDetailId, mod.MerchandiseOrderId }) // <- generic OK
                    .HasForeignKey(d => new { d.MerchandiseOrderDetailId, d.MerchandiseOrderId })
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_DeliveryOrderDetail_MerchandiseOrder");

                // (2) Ràng buộc "PO của detail PHẢI thuộc tập PO đã gán cho DO"
                // (3) FK kép sang bảng bắc cầu DO–PO (PO này thuộc DO này)
                entity.HasOne<DeliveryOrderPO>()
                      .WithMany()
                      .HasForeignKey(x => new { x.DeliveryOrderId, x.MerchandiseOrderId })
                      .OnDelete(DeleteBehavior.Cascade);

                //entity.HasOne(d => d.DeliveryOrder).WithMany(p => p.Details)
                //    .HasForeignKey(d => d.DeliveryOrderId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_DeliveryOrderDetail_DeliveryOrder");

                entity.HasOne(d => d.Product).WithMany(p => p.DeliveryOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict)
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

                entity.HasOne(x => x.WarehouseRequest)
                    .WithMany(wr => wr.DeliveryOrderPOs)
                    .HasForeignKey(x => x.WarehouseRequestId)
                    .OnDelete(DeleteBehavior.SetNull);
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



            modelBuilder.Entity<EventLog>(entity =>
            {
                entity.HasKey(e => e.EventId).HasName("PK__EventLogs__227429A55C6F1195");

                entity.ToTable("EventLogs", "Audit");

                entity.Property(e => e.EventId)
                  .HasDefaultValueSql("gen_random_uuid()");

                entity.HasIndex(e => e.CompanyId, "IX_EventLogs_CompanyId");
                entity.HasIndex(e => e.EmployeeID, "IX_EventLogs_CreatedBy");

                entity.HasIndex(x => x.SourceId).HasDatabaseName("IX_EventLog_SourceId");
                entity.HasIndex(x => x.SourceCode).HasDatabaseName("IX_EventLog_SourceCode");
                entity.HasIndex(x => x.EventType).HasDatabaseName("IX_EventLog_EventType");
                entity.HasIndex(x => x.Status).HasDatabaseName("IX_EventLog_Status");


                // Composite cho các case lọc kết hợp
                entity.HasIndex(x => new { x.SourceId, x.EventType }).HasDatabaseName("IX_EventLog_SourceId_EventType");
                
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                //entity.Property(e => e.IsCustomerSelect).HasDefaultValue(false);
                entity.Property(e => e.Status)
                      .HasMaxLength(32)
                      .HasDefaultValue("Draft");


                entity.HasOne(d => d.Company).WithMany(p => p.EventLogs)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_EventLogs_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.EventLogs)
                    .HasForeignKey(d => d.EmployeeID)
                    .HasConstraintName("FK_EventLogs_CreatedBy");

                entity.HasOne(d => d.Part).WithMany(p => p.EventLogs)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_EventLogs_DepartmentId");


            });


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

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F7C355BA5C");

                entity.ToTable("Materials", "Material");

                entity.HasIndex(e => e.CategoryId, "IX_Materials_CategoryId");

                entity.HasIndex(e => e.CompanyId, "IX_Materials_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Materials_CreatedBy");

                //entity.HasIndex(e => e.UnitId, "IX_Materials_UnitId");

                entity.HasIndex(e => e.UpdatedBy, "IX_Materials_UpdatedBy");

                entity.Property(e => e.MaterialId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Barcode).HasMaxLength(16);
                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.Property(e => e.CustomCode).HasMaxLength(50);
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.ImagePath).HasMaxLength(300);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Package).HasMaxLength(100);

                entity.HasOne(d => d.Category).WithMany(p => p.Materials)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Materials_Category");

                entity.HasOne(d => d.Company).WithMany(p => p.Materials)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Materials_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaterialCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Materials_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaterialUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Materials_UpdatedBy");
            });

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

            modelBuilder.Entity<MaterialsSupplier>(entity =>
            {
                entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB73A34869");

                entity.ToTable("Materials_Suppliers", "Material");

                entity.HasIndex(e => e.MaterialId, "IX_Materials_Suppliers_MaterialId");

                entity.HasIndex(e => e.SupplierId, "IX_Materials_Suppliers_SupplierId");

                entity.HasIndex(e => e.UpdatedBy, "IX_Materials_Suppliers_UpdatedBy");

                entity.Property(e => e.MaterialsSuppliersId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("Materials_SuppliersId");
                entity.Property(e => e.Currency).HasMaxLength(10);
                entity.Property(e => e.CurrentPrice).HasPrecision(18, 4);
                entity.Property(e => e.IsPreferred).HasDefaultValue(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(d => d.Material).WithMany(p => p.MaterialsSuppliers)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialsSuppliers_Material");

                entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialsSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialsSuppliers_Supplier");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaterialsSupplierCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_MaterialsSuppliers_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaterialsSupplierUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_MaterialsSuppliers_UpdatedBy");
            });

            // 1 supplier preferred duy nhất cho mỗi material (PostgreSQL partial index)
            modelBuilder.Entity<MaterialsSupplier>()
                .HasIndex(x => new { x.MaterialId, x.IsPreferred })
                .HasFilter("\"IsPreferred\" = TRUE")
                .IsUnique();

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

            modelBuilder.Entity<MfgProductionOrder>(entity =>
            {
                entity.ToTable("MfgProductionOrders", "manufacturing");

                entity.HasKey(e => e.MfgProductionOrderId)
                      .HasName("PK__MfgProductionOrders__MfgProductionOrderId");

                entity.Property(e => e.MfgProductionOrderId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("mfgProductionOrderId");
                entity.HasIndex(x => new { x.CompanyId, x.ExternalId }).IsUnique();
                entity.HasIndex(e => e.CompanyId, "IX_MfgProductionOrders_companyId");
                entity.HasIndex(e => e.Status, "IX_MfgProductionOrders_status");

                entity.HasIndex(e => e.CustomerExternalIdSnapshot, "IX_MfgProductionOrders_customerExternalIdSnapshot");
                entity.HasIndex(e => e.ProductExternalIdSnapshot, "IX_MfgProductionOrders_productExternalIdSnapshot");
                entity.HasIndex(e => e.MerchandiseOrderExternalId, "IX_MfgProductionOrders_MerchandiseOrderExternalId");

                entity.HasIndex(e => new { e.Status, e.ExpectedDate }, "IX_MfgProductionOrders_status_expectedDate");
                entity.HasIndex(e => new { e.Status, e.requiredDate }, "IX_MfgProductionOrders_status_requiredDate");

                entity.Property(e => e.ExternalId).HasColumnName("externalId").HasColumnType("citext");
                entity.Property(e => e.MerchandiseOrderExternalId).HasColumnName("merchandiseOrderExternalId").HasColumnType("citext");
                entity.Property(e => e.ProductionType).HasColumnName("productionType").HasColumnType("citext");
                entity.Property(e => e.ProductExternalIdSnapshot).HasColumnName("productExternalIdSnapshot").HasColumnType("citext");
                entity.Property(e => e.ProductNameSnapshot).HasColumnName("productNameSnapshot").HasColumnType("citext");
                entity.Property(e => e.CustomerNameSnapshot).HasColumnName("customerNameSnapshot");
                entity.Property(e => e.CustomerExternalIdSnapshot).HasColumnName("customerExternalIdSnapshot");
                entity.Property(e => e.FormulaExternalIdSnapshot).HasColumnName("formulaExternalIdSnapshot");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CompanyId).HasColumnName("companyId");
                entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("isActive");
                entity.Property(e => e.ManufacturingDate).HasColumnName("manufacturingDate");
                entity.Property(e => e.ExpectedDate).HasColumnName("expectedDate");
                entity.Property(e => e.requiredDate).HasColumnName("requiredDate");
                entity.Property(e => e.TotalQuantity).HasColumnName("totalQuantity");
                entity.Property(e => e.TotalQuantityRequest).HasColumnName("totalQuantityRequest");
                entity.Property(e => e.NumOfBatches).HasColumnName("numOfBatches");
                entity.Property(e => e.LabNote).HasColumnName("labNote");
                entity.Property(e => e.Requirement).HasColumnName("requirement");
                entity.Property(e => e.PlpuNote).HasColumnName("plpuNote");
                entity.Property(e => e.BagType).HasColumnName("bagType");

                //entity.Property(e => e.QualifiedQuantity).HasPrecision(18, 2).HasColumnName("qualifiedQuantity");
                //entity.Property(e => e.RejectedQuantity).HasPrecision(18, 2).HasColumnName("rejectedQuantity");
                //entity.Property(e => e.WasteQuantity).HasPrecision(18, 2).HasColumnName("wasteQuantity");

  
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

                entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.MfgProductionOrders)
                      .HasForeignKey(d => d.MerchandiseOrderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mpo__merchandiseOrderId");

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

            modelBuilder.Entity<MfgProductionOrderLog>(entity =>
            {
                entity.ToTable("MfgProductionOrderLogs", "manufacturing");

                entity.HasKey(e => e.LogId)
                      .HasName("PK__MfgProductionOrderLogs__LogId");


                entity.Property(e => e.CreatedDate).HasDefaultValueSql("timezone('utc', now())").HasColumnName("createDate");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");


                entity.HasOne(d => d.MfgProductionOrder).WithMany(p => p.ManufacturingOrderLogs)
                      .HasForeignKey(d => d.MfgProductionOrderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__MfgProductionOrderLogs__MfgProductionOrderId");


                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MfgProductionOrderLogCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__MfgProductionOrderLogs__createdBy");
            });





            modelBuilder.Entity<ManufacturingFormula>(entity =>
            {
                // ===== TABLE & PK =====
                entity.ToTable("ManufacturingFormulas", "manufacturing");
                entity.HasKey(e => e.ManufacturingFormulaId)
                      .HasName("PK__ManufacturingFormulas__manufacturingFormulaId");

                entity.Property(e => e.ManufacturingFormulaId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("manufacturingFormulaId");

                // ===== INDEXES (tuỳ nhu cầu lọc) =====
                entity.HasIndex(e => e.MfgProductionOrderId, "IX_ManufacturingFormulas_mfgProductionOrderId");



                entity.Property(x => x.IsActive).HasDefaultValue(true).IsRequired().HasColumnName("isActive");

                entity.HasIndex(x => x.VUFormulaId)
                    .HasFilter(@"""isStandard"" = TRUE AND ""isActive"" = TRUE")
                    .IsUnique()
                    .HasDatabaseName("ux_mfg_formula_isstandard_one_per_vu");


                entity.HasIndex(x => x.MfgProductionOrderId)
                    .HasFilter(@"""isSelect"" = TRUE AND ""isActive"" = TRUE")
                    .IsUnique()
                    .HasDatabaseName("ux_mfg_formula_isselect_one_per_order");

                // ===== COLUMNS =====
                //entity.Property(e => e.MfgProductionOrderExternalId).HasColumnName("mfgProductionOrderExternalId");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.MfgProductionOrderId).HasColumnName("mfgProductionOrderId");
                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TotalPrice)
                      .HasPrecision(18, 2)
                      .HasColumnName("totalPrice");

                entity.Property(e => e.IsSelect)
                      .HasDefaultValue(false)
                      .HasColumnName("isSelect");

                entity.Property(e => e.IsStandard)
                      .HasDefaultValue(false)
                      .HasColumnName("isStandard");

                entity.Property(e => e.Note).HasColumnName("note");

                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
                entity.Property(e => e.CompanyId).HasColumnName("companyId");

                // ===== RELATIONSHIPS (FK names) =====
                entity.HasOne(d => d.MfgProductionOrder).WithMany(p => p.ManufacturingFormulas)
                      .HasForeignKey(d => d.MfgProductionOrderId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__mfgProductionOrderId");

                entity.HasOne(d => d.VUFormula).WithMany(p => p.ManufacturingFormulas)
                      .HasForeignKey(d => d.VUFormulaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mf__VUFormulaId");

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


            modelBuilder.Entity<ManufacturingFormulaMaterial>(entity =>
            {
                // ===== TABLE & PK =====
                entity.ToTable("ManufacturingFormulaMaterials", "manufacturing");

                entity.HasKey(e => e.ManufacturingFormulaMaterialId)
                      .HasName("PK__ManufacturingFormulaMaterials__manufacturingFormulaMaterialId");

                entity.Property(e => e.ManufacturingFormulaMaterialId)
                      .HasDefaultValueSql("gen_random_uuid()")
                      .HasColumnName("manufacturingFormulaMaterialId");

                // ===== INDEXES (tối thiểu & thực dụng) =====
                // Lấy danh sách nguyên liệu theo công thức


                // (Tuỳ) nếu tra cứu theo LotNo trong phạm vi một công thức/kho:
                // entity.HasIndex(e => new { e.ManufacturingFormulaId, e.LotNo }, "IX_Mfm_mf_lotNo");

                // ===== COLUMNS =====
                entity.Property(e => e.ManufacturingFormulaId).HasColumnName("manufacturingFormulaId");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Unit).HasColumnName("unit"); // có thể dùng citext nếu cần case-insensitive: .HasColumnType("citext")

                entity.Property(e => e.Quantity).HasPrecision(18, 6);
                entity.Property(e => e.UnitPrice).HasPrecision(16, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(16, 2);

                entity.Property(e => e.LotNo).HasColumnName("lotNo");
                entity.Property(e => e.StockId).HasColumnName("stockId");

                entity.Property(e => e.MaterialNameSnapshot).HasColumnName("materialNameSnapshot");
                entity.Property(e => e.MaterialExternalIdSnapshot).HasColumnName("materialExternalIdSnapshot"); // có thể citext nếu so sánh không phân biệt hoa/thường

                // ===== RELATIONSHIPS =====
                entity.HasOne(d => d.ManufacturingFormula).WithMany(p => p.ManufacturingFormulaMaterials)
                      .HasForeignKey(d => d.ManufacturingFormulaId)
                      .OnDelete(DeleteBehavior.Cascade) // thường xóa công thức thì xóa luôn vật liệu con
                      .HasConstraintName("FK__Mfm__manufacturingFormulaId");

                // ManufacturingFormulaMaterial mapping
                entity.HasOne(d => d.Category)
                      .WithMany(c => c.ManufacturingFormulaMaterials) // <-- CHỈ RÕ
                      .HasForeignKey(d => d.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mfm__categoryId");

                entity.HasOne(d => d.Material)
                      .WithMany(m => m.ManufacturingFormulaMaterials) // cần có collection này trong Material
                      .HasForeignKey(d => d.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK__Mfm__materialId");
            });



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

            modelBuilder.Entity<MerchandiseOrder>(entity =>
            {
                // Primary key
                entity.HasKey(e => e.MerchandiseOrderId).HasName("PK__Merchand__D0AB7E7AFDA62167");

                // Tên bảng và schema
                entity.ToTable("MerchandiseOrders", "Orders");

                // ===== Columns =====
                entity.Property(e => e.MerchandiseOrderId).HasColumnName("MerchandiseOrderId")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ExternalId).HasColumnName("ExternalId")
                      .HasColumnType("citext");

                entity.Property(e => e.DeliveryAddress).HasColumnName("DeliveryAddress")
                      .HasMaxLength(255);

                entity.Property(e => e.PaymentType).HasColumnName("PaymentType")
                      .HasColumnType("citext");

                entity.Property(e => e.Receiver).HasColumnName("Receiver")
                      .HasColumnType("citext");

                entity.Property(e => e.ShippingMethod).HasColumnName("ShippingMethod")
                      .HasColumnType("citext");

                entity.Property(e => e.Status)
                      .HasColumnName("Status")
                      .HasColumnType("citext");

                entity.Property(e => e.CustomerExternalIdSnapshot)
                      .HasColumnName("CustomerExternalIdSnapshot")
                      .HasColumnType("citext");

                entity.Property(e => e.CustomerNameSnapshot)
                      .HasColumnName("CustomerNameSnapshot")
                      .HasColumnType("citext");

                entity.Property(e => e.TotalPrice)
                      .HasColumnName("TotalPrice")
                      .HasPrecision(18, 2);

                entity.Property(e => e.Vat)
                      .HasColumnName("VAT")
                      .HasPrecision(5, 2);

                entity.Property(e => e.Currency).HasColumnName("Currency")
                      .HasMaxLength(10)
                      .IsUnicode(false);

                entity.Property(e => e.IsPaid).HasColumnName("IsPaid")
                      .HasDefaultValue(false);

                entity.Property(e => e.IsActive).HasColumnName("IsActive")
                      .HasDefaultValue(true);

                entity.Property(e => e.PONo).HasColumnName("PONo")
                      .HasColumnType("citext");

                entity.Property(e => e.CreateDate).HasColumnName("CreateDate");

                entity.Property(e => e.UpdatedDate)
                      .HasColumnName("UpdatedDate");

                // ===== Indexes =====

                // Đảm bảo không trùng ExternalId trong 1 company
                entity.HasIndex(e => new { e.CompanyId, e.ExternalId })
                      .IsUnique()
                      .HasDatabaseName("UX_MO_Company_ExternalId");


                entity.HasIndex(e => e.CompanyId).HasDatabaseName("IX_MerchandiseOrders_CompanyId");
                entity.HasIndex(e => e.CustomerId).HasDatabaseName("IX_MerchandiseOrders_CustomerId");
                entity.HasIndex(e => e.ManagerById).HasDatabaseName("IX_MerchandiseOrders_ManagerById");
                entity.HasIndex(e => e.AttachmentCollectionId).HasDatabaseName("IX_Order_AttachmentCollection");


                // List/paging trong 1 tenant: IsActive luôn TRUE, sắp xếp CreateDate DESC, tie-break bằng PK
                entity.HasIndex(e => new { e.CompanyId, e.CreateDate, e.MerchandiseOrderId })
                      .IsDescending(false, true, true)
                      .HasFilter("\"IsActive\" = TRUE")
                      .HasDatabaseName("IX_MO_Tenant_Active_CreateDateDesc");

                // Lọc theo CustomerId + Active, lấy mới nhất
                entity.HasIndex(e => new { e.CustomerId, e.CreateDate, e.MerchandiseOrderId })
                      .IsDescending(false, true, true)
                      .HasFilter("\"IsActive\" = TRUE")
                      .HasDatabaseName("IX_MO_Customer_Active_CreateDateDesc");

                // Cho bulk update/soft delete theo đơn
                entity.HasIndex(e => new { e.MerchandiseOrderId, e.IsActive })
                      .HasDatabaseName("IX_MO_Detail_Order_Active");

                // ===== Relationships =====
                entity.HasOne(d => d.AttachmentCollection)
                      .WithMany()
                      .HasForeignKey(d => d.AttachmentCollectionId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_MerchandiseOrders_AttachmentCollection");

                entity.HasOne(d => d.Company)
                      .WithMany(p => p.MerchandiseOrders)
                      .HasForeignKey(d => d.CompanyId)
                      .HasConstraintName("FK_MerchandiseOrders_Company");

                entity.HasOne(d => d.CreatedByNavigation)
                      .WithMany(p => p.MerchandiseOrderCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .HasConstraintName("FK_MerchandiseOrders_CreatedBy");

                entity.HasOne(d => d.Customer)
                      .WithMany(p => p.MerchandiseOrders)
                      .HasForeignKey(d => d.CustomerId)
                      .HasConstraintName("FK_MerchandiseOrders_Customer");

                entity.HasOne(d => d.ManagerBy)
                      .WithMany(p => p.MerchandiseOrderManagerBies)
                      .HasForeignKey(d => d.ManagerById)
                      .HasConstraintName("FK_MerchandiseOrders_ManagerById");

                entity.HasOne(d => d.UpdatedByNavigation)
                      .WithMany(p => p.MerchandiseOrderUpdatedByNavigations)
                      .HasForeignKey(d => d.UpdatedBy)
                      .HasConstraintName("FK_MerchandiseOrders_UpdatedBy");
            });


            //modelBuilder.Entity<MerchandiseOrderLog>(entity =>
            //{
            //    entity.ToTable("MerchandiseOrderLogs", "Orders");

            //    entity.HasKey(e => e.LogId)
            //          .HasName("PK__MerchandiseOrderLogs__LogId");


            //    entity.Property(e => e.CreatedDate).HasDefaultValueSql("timezone('utc', now())").HasColumnName("createDate");

            //    entity.Property(e => e.CreatedBy).HasColumnName("createdBy");


            //    entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.MerchandiseOrderLogs)
            //          .HasForeignKey(d => d.MerchandiseOrderId)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK__MerchandiseOrderLogs__MerchandiseOrderId");


            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MerchandiseOrderLogCreatedByNavigations)
            //          .HasForeignKey(d => d.CreatedBy)
            //          .OnDelete(DeleteBehavior.Restrict)
            //          .HasConstraintName("FK__MerchandiseOrderLogs__createdBy");
            //});

            modelBuilder.Entity<MerchandiseOrderDetail>(entity =>
            {
                entity.ToTable("MerchandiseOrderDetails", "Orders");
                entity.HasKey(e => e.MerchandiseOrderDetailId).HasName("PK__Merchand__FE0FB3FF67BDE750");

                // ===== Columns (HasColumnName cho tất cả) =====
                entity.Property(e => e.MerchandiseOrderDetailId)
                      .HasColumnName("MerchandiseOrderDetailId")
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.MerchandiseOrderId).HasColumnName("MerchandiseOrderId");
                entity.Property(e => e.ProductId).HasColumnName("ProductId");
                entity.Property(e => e.FormulaId).HasColumnName("FormulaId");

                entity.Property(e => e.ProductExternalIdSnapshot)
                      .HasColumnName("ProductExternalIdSnapshot")
                      .HasColumnType("citext");
                entity.Property(e => e.ProductNameSnapshot)
                      .HasColumnName("ProductNameSnapshot")
                      .HasColumnType("citext");
                entity.Property(e => e.FormulaExternalIdSnapshot)
                      .HasColumnName("FormulaExternalIdSnapshot")
                      .HasColumnType("citext");

                entity.Property(e => e.ExpectedQuantity)
                      .HasColumnName("ExpectedQuantity")
                      .HasPrecision(18, 3);
                entity.Property(e => e.RealQuantity)
                      .HasColumnName("RealQuantity")
                      .HasPrecision(18, 3);

                entity.Property(e => e.BagType).HasColumnName("BagType").HasMaxLength(50);
                entity.Property(e => e.PackageWeight).HasColumnName("PackageWeight").HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("Status").HasMaxLength(50);
                entity.Property(e => e.Comment).HasColumnName("Comment").HasColumnType("citext");

                entity.Property(e => e.DeliveryRequestDate).HasColumnName("DeliveryRequestDate");
                entity.Property(e => e.DeliveryActualDate).HasColumnName("DeliveryActualDate");
                entity.Property(e => e.ExpectedDeliveryDate).HasColumnName("ExpectedDeliveryDate");

                entity.Property(e => e.BaseCostSnapshot)
                      .HasColumnName("BaseCostSnapshot").HasPrecision(18, 2);
                entity.Property(e => e.RecommendedUnitPrice)
                      .HasColumnName("RecommendedUnitPrice").HasPrecision(18, 2);
                entity.Property(e => e.UnitPriceAgreed)
                      .HasColumnName("UnitPriceAgreed").HasPrecision(18, 2);
                entity.Property(e => e.TotalPriceAgreed)
                      .HasColumnName("TotalPriceAgreed").HasPrecision(18, 2);

                entity.Property(e => e.IsActive)
                      .HasColumnName("IsActive")
                      .HasDefaultValue(true);

                // ===== Indexes =====
                entity.HasIndex(e => e.MerchandiseOrderId)
                      .HasDatabaseName("IX_MO_Details_OrderId");

                entity.HasIndex(e => new { e.MerchandiseOrderId, e.ProductId })
                      .HasDatabaseName("IX_MO_Details_Order_Product");

                entity.HasIndex(e => new { e.MerchandiseOrderId, e.FormulaId })
                      .HasDatabaseName("IX_MO_Details_Order_Formula");

                // paging/lọc theo trạng thái & ngày giao dự kiến (DESC), ổn định theo DetailId
                entity.HasIndex(e => new { e.MerchandiseOrderId, e.IsActive, e.Status, e.ExpectedDeliveryDate, e.MerchandiseOrderDetailId })
                      .IsDescending(false, false, false, true, true)
                      .HasDatabaseName("IX_MO_Details_Order_Status_DateDesc");

                // (TUỲ CHỌN) Chống trùng item: mỗi (Order, Product, Formula) chỉ 1 dòng active
                // entity.HasIndex(e => new { e.MerchandiseOrderId, e.ProductId, e.FormulaId })
                //       .IsUnique()
                //       .HasFilter("\"IsActive\" = TRUE")
                //       .HasDatabaseName("UX_MO_Detail_Order_Product_Formula_Active");

                // ===== Relationships =====
                // Xoá Order -> xoá luôn Detail (chuẩn đơn hàng)
                entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.MerchandiseOrderDetails)
                      .HasForeignKey(d => d.MerchandiseOrderId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_MerchandiseOrderDetails_MerchandiseOrderId");

                // Product/Formula không nullable => dùng Restrict (không SetNull)
                entity.HasOne(d => d.Product).WithMany(p => p.MerchandiseOrderDetails)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_MerchandiseOrderDetails_Product");

                entity.HasOne(d => d.Formula).WithMany(p => p.MerchandiseOrderDetails)
                      .HasForeignKey(d => d.FormulaId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_MerchandiseOrderDetails_FormulaId");
            });


            //modelBuilder.Entity<MerchandiseOrderSchedule>(entity =>
            //{
            //    entity.HasKey(e => e.MerchandiseOrderScheduleId).HasName("PK__Merchand__B49D545E8C296524");

            //    entity.ToTable("MerchandiseOrderSchedules", "Orders");

            //    entity.HasIndex(e => e.MerchandiseOrderId, "IX_MerchandiseOrderSchedules_MerchandiseOrderId");

            //    entity.Property(e => e.MerchandiseOrderScheduleId).ValueGeneratedNever();
            //    entity.Property(e => e.Status).HasMaxLength(50);

            //    entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.MerchandiseOrderSchedules)
            //        .HasForeignKey(d => d.MerchandiseOrderId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_MerchandiseOrderSchedules_MerchandiseOrderId");
            //});

            //modelBuilder.Entity<OrderAttachment>(entity =>
            //{
            //    entity.HasKey(e => e.AttachmentId).HasName("PK__OrderAttachment__B49D545E8C296524");


            //    entity.ToTable("OrderAttachments", "Orders");
            //    entity.Property(e => e.AttachmentId).HasDefaultValueSql("gen_random_uuid()");

            //    entity.HasOne(x => x.MerchandiseOrder).WithMany(o => o.Attachments).HasForeignKey(x => x.MerchandiseOrderId);
            //    entity.Property(x => x.FileName).HasMaxLength(255);
            //    entity.Property(x => x.StoragePath).HasMaxLength(500);

            //    entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.Attachments)
            //        .HasForeignKey(d => d.MerchandiseOrderId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_OrderAttachment_MerchandiseOrderId");

            //    entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.OrderAttachmentCreatedByNavigations)
            //        .HasForeignKey(d => d.CreateBy)
            //        .HasConstraintName("FK_OrderAttachment_CreatedBy");
            //});




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

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__A927CACB4B3A2EAC");

                entity.ToTable("PriceHistory", "Material");

                entity.HasIndex(e => e.CreatedBy, "IX_PriceHistory_CreatedBy");

                entity.HasIndex(e => e.MaterialId, "IX_PriceHistory_MaterialId");

                entity.HasIndex(e => e.SupplierId, "IX_PriceHistory_SupplierId");

                entity.Property(e => e.PriceHistoryId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Currency).HasMaxLength(10);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.NewPrice).HasPrecision(18, 4);
                entity.Property(e => e.OldPrice).HasPrecision(18, 4);

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PriceHistoryCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_PriceHistory_CreatedBy");

                entity.HasOne(d => d.Material).WithMany(p => p.PriceHistories)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PriceHistory_Material");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PriceHistories)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PriceHistory_Supplier");

            });

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

            modelBuilder.Entity<ProductInspection>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC072F6AD3E1");

                entity.ToTable("ProductInspection");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Antistatic).HasColumnType("citext");
                entity.Property(e => e.BatchId).HasColumnType("citext");
                entity.Property(e => e.BlackDots).HasColumnType("citext");
                entity.Property(e => e.ColorDeltaE).HasColumnType("citext");
                entity.Property(e => e.CreatedBy).HasColumnType("citext");
                entity.Property(e => e.DefectBlackDot).HasColumnName("Defect_BlackDot");
                entity.Property(e => e.DefectDusty).HasColumnName("Defect_Dusty");
                entity.Property(e => e.DefectImpurity).HasColumnName("Defect_Impurity");
                entity.Property(e => e.DefectMoist).HasColumnName("Defect_Moist");
                entity.Property(e => e.DefectShortFiber).HasColumnName("Defect_ShortFiber");
                entity.Property(e => e.DefectWrongColor).HasColumnName("Defect_WrongColor");
                entity.Property(e => e.Density).HasColumnType("citext");
                entity.Property(e => e.Elongation).HasColumnType("citext");
                entity.Property(e => e.ExternalId).HasColumnType("citext");
                entity.Property(e => e.FlexuralModulus).HasColumnType("citext");
                entity.Property(e => e.FlexuralStrength).HasColumnType("citext");
                entity.Property(e => e.Hardness).HasColumnType("citext");
                entity.Property(e => e.ImpactResistance).HasColumnType("citext");
                entity.Property(e => e.IsColorDeltaEpass).HasColumnName("IsColorDeltaEPass");
                entity.Property(e => e.IsMfrpass).HasColumnName("IsMFRPass");
                entity.Property(e => e.MeshType).HasColumnType("citext");
                entity.Property(e => e.Mfr)
                    .HasColumnType("citext")
                    .HasColumnName("MFR");
                entity.Property(e => e.Moisture).HasColumnType("citext");
                entity.Property(e => e.Notes).HasColumnType("citext");
                entity.Property(e => e.PackingSpec).HasColumnType("citext");
                entity.Property(e => e.ParticleSize).HasColumnType("citext");
                entity.Property(e => e.ProductCode).HasColumnType("citext");
                entity.Property(e => e.ProductName).HasColumnType("citext");
                entity.Property(e => e.Shape).HasColumnType("citext");
                entity.Property(e => e.StorageCondition).HasColumnType("citext");
                entity.Property(e => e.TensileStrength).HasColumnType("citext");
                entity.Property(e => e.Types).HasColumnType("citext");
            });

            modelBuilder.Entity<ProductStandard>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC0715B96228");

                entity.ToTable("ProductStandard");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.BlackDots).HasColumnType("citext");
                entity.Property(e => e.ColourCode)
                    .HasColumnType("citext")
                    .HasColumnName("colourCode");
                entity.Property(e => e.CustomerExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("customerExternalId");
                entity.Property(e => e.DeltaE).HasColumnType("citext");
                entity.Property(e => e.Density).HasColumnType("citext");
                entity.Property(e => e.DwellTime).HasColumnType("citext");
                entity.Property(e => e.ElongationAtBreak).HasColumnType("citext");
                entity.Property(e => e.ExternalId).HasColumnType("citext");
                entity.Property(e => e.FlexuralModulus).HasColumnType("citext");
                entity.Property(e => e.FlexuralStrength).HasColumnType("citext");
                entity.Property(e => e.Hardness).HasColumnType("citext");
                entity.Property(e => e.IzodImpactStrength).HasColumnType("citext");
                entity.Property(e => e.MeltIndex).HasColumnType("citext");
                entity.Property(e => e.MigrationTest).HasColumnType("citext");
                entity.Property(e => e.Moisture).HasColumnType("citext");
                entity.Property(e => e.Package).HasColumnType("citext");
                entity.Property(e => e.PelletSize).HasColumnType("citext");
                entity.Property(e => e.ProductExternalId).HasColumnType("citext");
                entity.Property(e => e.Shape).HasColumnType("citext");
                entity.Property(e => e.Status).HasColumnType("citext");
                entity.Property(e => e.TensileStrength).HasColumnType("citext");
                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<ProductTest>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ProductT__3213E83F975320C1");

                entity.ToTable("ProductTest");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("externalId");
                entity.Property(e => e.ProductCustomerExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("product_customerExternalId");
                entity.Property(e => e.ProductExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("product_externalId");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.ProductName)
                    .HasColumnType("citext")
                    .HasColumnName("product_name");
                entity.Property(e => e.ProductPackage)
                    .HasColumnType("citext")
                    .HasColumnName("product_package");
                entity.Property(e => e.ProductWeight).HasColumnName("product_weight");
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__036BACA44B53DA7A");

                entity.ToTable("PurchaseOrders", "Orders");

                entity.HasIndex(e => e.CompanyId, "IX_PurchaseOrders_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_PurchaseOrders_CreatedBy");

                entity.HasIndex(e => e.SupplierId, "IX_PurchaseOrders_SupplierId");

                entity.HasIndex(e => e.UpdatedBy, "IX_PurchaseOrders_UpdatedBy");

                entity.Property(e => e.PurchaseOrderId).ValueGeneratedNever();
                //entity.Property(e => e.DeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.OrderType).HasMaxLength(50);
                //entity.Property(e => e.PackageType).HasMaxLength(50);
                //entity.Property(e => e.PaymentDays).HasMaxLength(50);
                //entity.Property(e => e.RequestSourceType).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.IsActive).HasDefaultValue(true);
                //entity.Property(e => e.TotalPrice).HasPrecision(16, 2);
                //entity.Property(e => e.Vat).HasColumnName("VAT");

                entity.HasOne(d => d.PurchaseOrderSnapshot).WithMany()
                    .HasForeignKey(d => d.PurchaseOrderSnapshotId)
                    .HasConstraintName("FK_PurchaseOrders_PurchaseOrderSnapshot");

                entity.HasOne(d => d.Company).WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_PurchaseOrders_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PurchaseOrderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_PurchaseOrders_CreatedBy");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_PurchaseOrders_SupplierId");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PurchaseOrderUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_PurchaseOrders_UpdatedBy");
            });

            modelBuilder.Entity<PurchaseOrderDetail>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderDetailId).HasName("PK__Purchase__5026B698A94EFE60");


                entity.Property(e => e.PurchaseOrderDetailId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PurchaseOrderDetailId");

                entity.ToTable("PurchaseOrderDetails", "Orders");

                entity.HasIndex(e => e.MaterialId, "IX_PurchaseOrderDetails_MaterialId");

                entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseOrderDetails_PurchaseOrderId");

                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.TotalPriceAgreed).HasPrecision(18, 2);
                entity.Property(e => e.BaseCostSnapshot).HasPrecision(18, 2);
                entity.Property(e => e.UnitPriceAgreed).HasPrecision(18, 2);
                entity.Property(e => e.RealQuantity).HasPrecision(18, 2);
                entity.Property(e => e.RequestQuantity).HasPrecision(18, 2);

                //entity.Property(e => e.ExternalId).HasColumnType("citext");
                entity.Property(e => e.MaterialExternalIDSnapshot).HasColumnType("citext");
                entity.Property(e => e.MaterialNameSnapshot).HasColumnType("citext");


                entity.HasOne(d => d.Material).WithMany(p => p.PurchaseOrderDetails)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderDetails_MaterialId");

                entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderDetails_PurchaseOrderId");
            });

            modelBuilder.Entity<PurchaseOrderSnapshot>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderSnapshotId).HasName("PK__PurchaseOrderSnapshot__5026B698A94EFE60");


                entity.Property(e => e.PurchaseOrderSnapshotId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PurchaseOrderSnapshotId");

                entity.ToTable("PurchaseOrderSnapshot", "Orders");

                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);

            });





            modelBuilder.Entity<PurchaseOrderLink>(entity =>
            {
                entity.ToTable("PurchaseOrderLink", "Orders");
                entity.HasKey(e => e.PurchaseOrderLinkId).HasName("PK__PurchaseOrderLink__5026B698A94EFE60");


                entity.Property(e => e.PurchaseOrderLinkId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PurchaseOrderLinkId");

                entity.HasOne(x => x.PurchaseOrder)
                    .WithMany(o => o.PurchaseOrderLinks)
                    .HasForeignKey(x => x.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.MerchandiseOrder)
                    .WithMany(mo => mo.PurchaseOrderLinks)
                    .HasForeignKey(x => x.MerchandiseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            //modelBuilder.Entity<PurchaseOrderDetailsMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.PodetailId).HasName("PK__Purchase__4EB47B3EB8D4CA48");

            //    entity.ToTable("PurchaseOrderDetails_material_data");

            //    entity.HasIndex(e => e.DetailId, "IX_PurchaseOrderDetails_material_data_DetailID");

            //    entity.HasIndex(e => e.MaterialId, "IX_PurchaseOrderDetails_material_data_MaterialId");

            //    entity.HasIndex(e => e.Poid, "IX_PurchaseOrderDetails_material_data_POID");

            //    entity.Property(e => e.PodetailId)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("PODetailId");
            //    entity.Property(e => e.DetailId).HasColumnName("DetailID");
            //    entity.Property(e => e.Note)
            //        .HasColumnType("citext")
            //        .HasColumnName("note");
            //    entity.Property(e => e.Poid).HasColumnName("POID");
            //    entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            //    entity.HasOne(d => d.Detail).WithMany(p => p.PurchaseOrderDetailsMaterialData)
            //        .HasForeignKey(d => d.DetailId)
            //        .HasConstraintName("FK_PO_Detail_RequestDetail");

            //    entity.HasOne(d => d.Material).WithMany(p => p.PurchaseOrderDetailsMaterialData)
            //        .HasForeignKey(d => d.MaterialId)
            //        .HasConstraintName("FK__PurchaseO__Mater__10216507");

            //    entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderDetailsMaterialData)
            //        .HasForeignKey(d => d.Poid)
            //        .HasConstraintName("FK__PurchaseOr__POID__30242045");
            //});

            modelBuilder.Entity<PurchaseOrderStatusHistory>(entity =>
            {
                entity.HasKey(e => e.StatusHistoryId).HasName("PK__Purchase__DB973491370CD24F");

                entity.ToTable("PurchaseOrderStatusHistory", "Orders");

                entity.HasIndex(e => e.EmployeeId, "IX_PurchaseOrderStatusHistory_EmployeeId");

                entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseOrderStatusHistory_PurchaseOrderId");

                entity.Property(e => e.StatusFrom).HasMaxLength(16);
                entity.Property(e => e.StatusTo).HasMaxLength(16);

                entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrderStatusHistories)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_PurchaseOrderStatusHistory_EmployeeId");

                entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderStatusHistories)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderStatusHistory_PurchaseOrderId");
            });

            //modelBuilder.Entity<PurchaseOrderStatusHistoryMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.StatusHistoryId).HasName("PK__Purchase__DB9734917B73E8D7");

            //    entity.ToTable("PurchaseOrderStatusHistory_material_data");

            //    entity.HasIndex(e => e.Poid, "IX_PurchaseOrderStatusHistory_material_data_POID");

            //    entity.Property(e => e.StatusHistoryId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.EmployeeId).HasColumnType("citext");
            //    entity.Property(e => e.Note)
            //        .HasColumnType("citext")
            //        .HasColumnName("note");
            //    entity.Property(e => e.Poid).HasColumnName("POID");
            //    entity.Property(e => e.StatusFrom).HasColumnType("citext");
            //    entity.Property(e => e.StatusTo).HasColumnType("citext");

            //    entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderStatusHistoryMaterialData)
            //        .HasForeignKey(d => d.Poid)
            //        .HasConstraintName("FK__PurchaseOr__POID__34E8D562");
            //});

            //modelBuilder.Entity<PurchaseOrdersMaterialDatum>(entity =>
            //{
            //    entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2F4EB754C0F");

            //    entity.ToTable("PurchaseOrders_material_data");

            //    entity.HasIndex(e => e.EmployeeId, "IX_PurchaseOrders_material_data_EmployeeId");

            //    entity.HasIndex(e => e.SupplierId, "IX_PurchaseOrders_material_data_SupplierId");

            //    entity.Property(e => e.Poid)
            //        .HasDefaultValueSql("gen_random_uuid()")
            //        .HasColumnName("POID");
            //    entity.Property(e => e.ContactName).HasColumnType("citext");
            //    entity.Property(e => e.DeliveryAddress).HasColumnType("citext");
            //    entity.Property(e => e.DeliveryContact).HasColumnType("citext");
            //    entity.Property(e => e.EmployeeId).HasMaxLength(16);
            //    entity.Property(e => e.GrandTotal).HasPrecision(18, 2);
            //    entity.Property(e => e.InvoiceNote).HasColumnType("citext");
            //    entity.Property(e => e.Note)
            //        .HasColumnType("citext")
            //        .HasColumnName("note");
            //    entity.Property(e => e.Packaging).HasColumnType("citext");
            //    entity.Property(e => e.PaymentTerm).HasColumnType("citext");
            //    entity.Property(e => e.Pocode)
            //        .HasColumnType("citext")
            //        .HasColumnName("POCode");
            //    entity.Property(e => e.RequiredDocuments).HasColumnType("citext");
            //    entity.Property(e => e.RequiredDocumentsEng)
            //        .HasColumnType("citext")
            //        .HasColumnName("RequiredDocuments_Eng");
            //    entity.Property(e => e.Status)
            //        .HasColumnType("citext")
            //        .HasColumnName("status");
            //    entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            //    entity.Property(e => e.Vat).HasColumnName("VAT");
            //    entity.Property(e => e.VendorAddress).HasColumnType("citext");
            //    entity.Property(e => e.VendorPhone).HasColumnType("citext");

            //    entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrdersMaterialData)
            //        .HasForeignKey(d => d.EmployeeId)
            //        .HasConstraintName("FK__PurchaseO__Emplo__320C68B7");

            //    entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrdersMaterialData)
            //        .HasForeignKey(d => d.SupplierId)
            //        .HasConstraintName("FK__PurchaseO__Suppl__33008CF0");
            //});

            modelBuilder.Entity<PurchaseOrdersSchedule>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrdersScheduleId).HasName("PK__Purchase__658C1532BFBEF664");

                entity.ToTable("PurchaseOrdersSchedules", "Orders");

                entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseOrdersSchedules_PurchaseOrderId");

                entity.Property(e => e.PurchaseOrdersScheduleId).ValueGeneratedNever();
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrdersSchedules)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrdersSchedules_PurchaseOrderId");
            });

            modelBuilder.Entity<Qcdetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__QCDetail__3214EC07937EA2C5");

                entity.ToTable("QCDetail");

                entity.HasIndex(e => e.BatchId, "IX_QCDetail_BatchId").IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.BatchExternalId).HasColumnType("citext");
                entity.Property(e => e.MachineExternalId).HasColumnType("citext");

                entity.HasOne(d => d.Batch).WithOne(p => p.Qcdetails)
                    .HasForeignKey<Qcdetail>(d => d.BatchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_QCDetail_ProductInspection");
            });

            modelBuilder.Entity<RequestDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D66D34B2A");

                entity.ToTable("RequestDetail", "SupplyRequest");

                entity.HasIndex(e => e.MaterialId, "IX_RequestDetail_MaterialID");

                entity.HasIndex(e => e.RequestId, "IX_RequestDetail_RequestID");

                entity.Property(e => e.DetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("DetailID");
                entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
                entity.Property(e => e.RequestId).HasColumnName("RequestID");
                entity.Property(e => e.RequestStatus).HasMaxLength(16);

                entity.HasOne(d => d.Material).WithMany(p => p.RequestDetails)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK_SupplyRequest_MaterialId");

                entity.HasOne(d => d.Request).WithMany(p => p.RequestDetails)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_SupplyRequest_RequestID");
            });



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
                entity.Property(e => e.BranchId).HasColumnName("BranchId");
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

                entity.HasOne(d => d.Branch).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_SampleRequests_Branch");

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



            //modelBuilder.Entity<SampleRequestImage>(entity =>
            //{
            //    entity.ToTable("SampleRequestImages", "SampleRequests");
            //    entity.HasKey(e => e.SampleRequestImageId).HasName("PK__SampleRequestImage__3214EC07A98DEC4E");

            //    entity.Property(e => e.SampleRequestImageId).HasDefaultValueSql("gen_random_uuid()");
            //    entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
            //    entity.Property(e => e.FileType).HasMaxLength(100).IsRequired();
            //    entity.Property(e => e.FileUrl).IsRequired();

            //    entity.HasOne(img => img.SampleRequest)
            //          .WithMany(sr => sr.SampleRequestImages)
            //          .HasForeignKey(img => img.SampleRequestId)
            //          .OnDelete(DeleteBehavior.Cascade);
            //});

            modelBuilder.Entity<SchedualMfg>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Schedual__3214EC07A98DEC4E");

                entity.ToTable("SchedualMfg", "Schedual");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.BagType).HasColumnType("citext");
                entity.Property(e => e.ColorCode).HasColumnType("citext");
                entity.Property(e => e.ColorName).HasColumnType("citext");
                entity.Property(e => e.CreatedDate).HasColumnName("createdDate");
                entity.Property(e => e.CustomerExternalId).HasColumnType("citext");
                entity.Property(e => e.ExternalId).HasColumnType("citext");
                entity.Property(e => e.MachineId).HasColumnType("citext");
                entity.Property(e => e.Note).HasColumnType("citext");
                entity.Property(e => e.Qcstatus)
                    .HasColumnType("citext")
                    .HasColumnName("QCStatus");
                entity.Property(e => e.Status).HasColumnType("citext");
                entity.Property(e => e.VerifyBatches).HasColumnType("citext");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4029CD1B8");

                entity.ToTable("Suppliers", "Material");

                entity.HasIndex(e => e.CompanyId, "IX_Suppliers_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Suppliers_CreatedBy");

                entity.Property(e => e.SupplierId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.IssuedPlace).HasColumnType("citext");
                entity.Property(e => e.SupplierName).HasMaxLength(200);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.RegistrationNumber).HasMaxLength(50);
                entity.Property(e => e.Website).HasMaxLength(200);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

                entity.HasOne(d => d.Company).WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Suppliers_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplierCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Suppliers_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SupplierUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Supplier_UpdatedBy");
            });


            modelBuilder.Entity<SupplierAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__Supplier__091C2AFB0B8862E7");

                entity.ToTable("SupplierAddresses", "Material");

                entity.HasIndex(e => e.SupplierId, "IX_SupplierAddresses_SupplierId");

                entity.Property(e => e.AddressId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AddressLine).HasMaxLength(250);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.District).HasMaxLength(100);
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.Province).HasMaxLength(100);

                entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAddresses)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierAddress_Supplier");
            });

            modelBuilder.Entity<SupplierContact>(entity =>
            {
                entity.HasKey(e => e.ContactId).HasName("PK__Supplier__5C66259B7A3571DD");

                entity.ToTable("SupplierContacts", "Material");

                entity.HasIndex(e => e.SupplierId, "IX_SupplierContacts_SupplierId");

                entity.Property(e => e.ContactId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(20);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierContacts)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupplierContact_Supplier");
            });

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

            modelBuilder.Entity<SupplyRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("PK__SupplyRe__33A8519A7A78FE1B");

                entity.ToTable("SupplyRequests", "SupplyRequest");

                entity.HasIndex(e => e.CompanyId, "IX_SupplyRequests_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_SupplyRequests_CreatedBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_SupplyRequests_UpdatedBy");

                entity.Property(e => e.RequestId)
                    .ValueGeneratedNever()
                    .HasColumnName("RequestID");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(16)
                    .HasColumnName("ExternalID");
                entity.Property(e => e.RequestSourceType).HasMaxLength(16);
                entity.Property(e => e.RequestStatus).HasMaxLength(16);

                entity.HasOne(d => d.Company).WithMany(p => p.SupplyRequests)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_SupplyRequest_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupplyRequestCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_SupplyRequest_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SupplyRequestUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_SupplyRequest_UpdatedBy");
            });

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

            modelBuilder.Entity<WarehouseShelfStock>(entity =>
            {
                entity.HasKey(e => e.SlotId).HasName("PK__WarehouseShelfStock__3214EC07A98DEC4E");
                entity.Property(x => x.SlotId).UseIdentityAlwaysColumn();

                entity.ToTable("WarehouseShelfStock", "Warehouse");


                entity.Property(x => x.SlotCode).HasMaxLength(50).IsRequired();
                entity.Property(x => x.Code).HasColumnType("citext").IsRequired();
                entity.Property(x => x.BatchNo).HasMaxLength(50);
                entity.Property(x => x.LotKey).HasColumnType("citext");
                entity.Property(x => x.StockType).HasConversion<int>();
                entity.Property(x => x.QtyKg).HasPrecision(18, 3);

                entity.HasOne(d => d.Company).WithMany(p => p.WarehouseShelfStocks)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_WarehouseShelfStock_Company");


                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WarehouseShelfStockUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_WarehouseShelfStock_UpdatedBy");

                entity.HasIndex(x => new { x.CompanyId, x.Code });
                entity.HasIndex(x => new { x.CompanyId, x.Code, x.LotKey });

            });

            modelBuilder.Entity<WarehouseRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("PK__WareHouseRequest__3214EC07A98DEC4E");
               
                entity.Property(x => x.RequestId).UseIdentityAlwaysColumn();


                entity.ToTable("WarehouseRequest", "Warehouse");


                entity.Property(x => x.RequestName).IsRequired();
                entity.Property(x => x.CreatedBy).IsRequired();
                entity.Property(x => x.ReqStatus);
                
                entity.Property(x => x.ReqType);

                entity.HasOne(d => d.Company).WithMany(p => p.WarehouseRequests)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_WarehouseRequest_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WarehouseRequestCreatedByNavigations)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_WarehouseRequest_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WarehouseRequestUpdatedByNavigations)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_WarehouseRequest_UpdatedBy");

            });

            modelBuilder.Entity<WarehouseRequestDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__WareHouseRequestDetail__3214EC07A98DEC4E");
                entity.Property(x => x.DetailId).UseIdentityAlwaysColumn();

                entity.HasIndex(e => e.RequestId, "IX_WarehouseRequestDetail_RequestCode");
                entity.ToTable("WarehouseRequestDetail", "Warehouse");


                entity.Property(x => x.WeightKg).HasPrecision(18, 3);

                entity.HasOne(d => d.WarehouseRequest)
                    .WithMany(p => p.WarehouseRequestDetails)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasForeignKey(d => d.RequestId)
                    .HasConstraintName("FK_WarehouseRequestDetail_RequestId");

            });

           
            modelBuilder.Entity<WarehouseTempStock>(entity =>
            {
                entity.HasKey(e => e.TempId).HasName("PK__WarehouseTempStock__3214EC07A98DEC4E");

                entity.ToTable("WarehouseTempStock", "Warehouse");
                entity.Property(x => x.TempId).UseIdentityAlwaysColumn(); // Postgres identity;


                entity.Property(x => x.Code).HasColumnType("citext").IsRequired();
                entity.Property(x => x.VaCode).HasColumnType("citext").IsRequired();
                //entity.Property(x => x.VaLineCode).HasColumnType("citext");
                entity.Property(x => x.LotKey).HasColumnType("citext");

                //entity.Property(x => x.QtyStock).HasPrecision(18, 3);
                entity.Property(x => x.QtyRequest).HasPrecision(18, 3);


                // Enum = int (mặc định của EF) -> KHÔNG dùng HasConversion<string>()
                //entity.Property(x => x.TempType).IsRequired();
                entity.Property(x => x.ReserveStatus);


                entity.HasOne(x => x.CreatedByNavigation)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WarehouseTempStockCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_WarehouseTempStock_CreatedBy");

                //entity.HasIndex(x => new { x.CompanyId, x.TempType, x.Code, x.LotKey });
                entity.HasIndex(x => new { x.CompanyId, x.VaCode });
                //entity.HasIndex(x => new { x.CompanyId, x.SnapshotSetId });
            });




            /// ==================================== MRO Module ==================================== 
            modelBuilder.Entity<AreaMRO>(entity =>
            {
                entity.ToTable("areas", "mro");

                entity.HasKey(x => x.AreaId).HasName("pk_areas");

                entity.Property(x => x.AreaId)
                      .UseIdentityByDefaultColumn().HasColumnName("area_id");
                entity.Property(x => x.AreaExternalId)
                      .HasColumnName("area_externalid")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.AreaName)
                      .HasColumnName("area_name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.HasIndex(x => x.AreaExternalId)
                      .HasDatabaseName("ux_areas_area_externalid")
                      .IsUnique();
            });

            modelBuilder.Entity<WarehouseMRO>(entity =>
            {
                entity.HasKey(e => e.WarehouseId).HasName("PK__Warehouses__3214EC07A98DEC4E");
                entity.Property(x => x.WarehouseId).UseIdentityAlwaysColumn();

                entity.ToTable("warehouses", "mro");


                entity.Property(x => x.WarehouseId).HasColumnName("warehouse_id");
                entity.Property(x => x.WarehouseExternalId)
                      .HasColumnName("warehouse_external_id")
                      .HasColumnType("text")
                      .IsRequired();
                entity.Property(x => x.WarehouseName)
                      .HasColumnName("warehouse_name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.HasIndex(x => x.WarehouseExternalId)
                      .HasDatabaseName("ix_warehouses_external_id")
                      .IsUnique(); // nếu không muốn unique, bỏ dòng này
            });

            modelBuilder.Entity<ZoneMRO>(entity =>
            {
                entity.HasKey(e => e.ZoneId).HasName("PK__Zone__3214EC07A98DEC4E");
                entity.Property(x => x.ZoneId).UseIdentityAlwaysColumn();

                entity.ToTable("zones", "mro");

                entity.Property(x => x.ZoneId).HasColumnName("zone_id");
                entity.Property(x => x.WarehouseId).HasColumnName("warehouse_id");
                entity.Property(x => x.ZoneExternalId)
                      .HasColumnName("zone_external_id")
                      .HasColumnType("text")
                      .IsRequired();
                entity.Property(x => x.ZoneName)
                      .HasColumnName("zone_name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.HasIndex(x => x.WarehouseId)
                      .HasDatabaseName("ix_zones_warehouse_id");

                // Một warehouse có nhiều zone
                entity.HasOne(z => z.Warehouse)
                      .WithMany(w => w.Zones)
                      .HasForeignKey(z => z.WarehouseId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_zones_warehouse_id");

                // ZoneExternalId duy nhất trong cùng 1 warehouse
                entity.HasIndex(x => new { x.WarehouseId, x.ZoneExternalId })
                      .HasDatabaseName("ux_zones_warehouse_external")
                      .IsUnique();
            });

            modelBuilder.Entity<RackMRO>(entity =>
            {
                entity.HasKey(e => e.RackId).HasName("PK__Ranks__3214EC07A98DEC4E");
                entity.Property(x => x.RackId).UseIdentityAlwaysColumn();

                entity.ToTable("racks", "mro");
                entity.Property(x => x.RackId).HasColumnName("rack_id");
                entity.Property(x => x.ZoneId).HasColumnName("zone_id");
                entity.Property(x => x.RackExternalId)
                      .HasColumnName("rack_external_id")
                      .HasColumnType("text")
                      .IsRequired();
                entity.Property(x => x.RackName)
                      .HasColumnName("rack_name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.HasIndex(x => x.ZoneId).HasDatabaseName("ix_racks_zone_id");

                // Một zone có nhiều rack
                entity.HasOne(r => r.Zone)
                      .WithMany(z => z.Racks)
                      .HasForeignKey(r => r.ZoneId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_racks_zone_id");

                // RackExternalId duy nhất trong cùng 1 zone
                entity.HasIndex(x => new { x.ZoneId, x.RackExternalId })
                      .HasDatabaseName("ux_racks_zone_external")
                      .IsUnique();
            });

            modelBuilder.Entity<SlotMRO>(entity =>
            {
                entity.HasKey(e => e.SlotId).HasName("PK__Slots__3214EC07A98DEC4E");
                entity.Property(x => x.SlotId).UseIdentityAlwaysColumn();


                entity.ToTable("slots", "mro");
                entity.Property(x => x.SlotId).HasColumnName("slot_id");
                entity.Property(x => x.RackId).HasColumnName("rack_id");
                entity.Property(x => x.SlotExternalId)
                      .HasColumnName("slot_external_id")
                      .HasColumnType("text")
                      .IsRequired();
                entity.Property(x => x.SlotName)
                      .HasColumnName("slot_name")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.CapacityQty)
                      .HasColumnName("capacity_qty")
                      .HasColumnType("integer")
                      .HasDefaultValue(0);

                entity.Property(x => x.CountToCapacity)
                      .HasColumnName("count_to_capacity")
                      .HasColumnType("boolean")
                      .HasDefaultValue(true);

                entity.HasIndex(x => x.RackId).HasDatabaseName("ix_slots_rack_id");

                // Một rack có nhiều slot
                entity.HasOne(s => s.Rack)
                      .WithMany(r => r.Slots)
                      .HasForeignKey(s => s.RackId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_slots_rack_id");

                // SlotExternalId duy nhất trong cùng 1 rack
                entity.HasIndex(x => new { x.RackId, x.SlotExternalId })
                      .HasDatabaseName("ux_slots_rack_external")
                      .IsUnique();
            });

            modelBuilder.Entity<EquipmentMRO>(entity =>
            {
                entity.ToTable("equipment", "mro");

                entity.HasKey(x => x.EquipmentId).HasName("pk_equipment");

                entity.Property(x => x.EquipmentId)
                      .HasColumnName("equipment_id")
                      .UseIdentityByDefaultColumn(); // Npgsql; nếu SQL Server thì bỏ dòng này

                entity.Property(x => x.EquipmentExternalId)
                      .HasColumnName("equipment_externalid")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.EquipmentName)
                      .HasColumnName("equipment_name")
                      .HasColumnType("text")
                      .IsRequired();

                // soft refs (không FK)
                entity.Property(x => x.AreaExternalId)
                      .HasColumnName("area_externalid")
                      .HasColumnType("text");

                entity.Property(x => x.FactoryExternalId)
                      .HasColumnName("factory_externalid")
                      .HasColumnType("text");

                entity.Property(x => x.PartExternalId)
                      .HasColumnName("part_externalid")
                      .HasColumnType("citext"); // nếu không dùng PostgreSQL -> "text"

                // hard FK (tùy chọn)
                entity.Property(x => x.AreaId).HasColumnName("area_id");
                entity.Property(x => x.FactoryId).HasColumnName("factory_id");
                entity.Property(x => x.PartId).HasColumnName("part_id");

                // Index tra cứu theo external
                entity.HasIndex(x => x.AreaExternalId).HasDatabaseName("ix_equipment_area_externalid");
                entity.HasIndex(x => x.FactoryExternalId).HasDatabaseName("ix_equipment_factory_externalid");
                entity.HasIndex(x => x.PartExternalId).HasDatabaseName("ix_equipment_part_externalid");

                // Unique theo nhà máy + mã ngoài thiết bị (tuỳ nghiệp vụ)
                entity.HasIndex(x => new { x.FactoryId, x.EquipmentExternalId })
                      .HasDatabaseName("ux_equipment_factory_extid")
                      .IsUnique();

                // Quan hệ OPTIONAL (cho phép null) + hạn chế xóa
                entity.HasOne(x => x.Area)
                      .WithMany(a => a.Equipments)
                      .HasForeignKey(x => x.AreaId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_equipment_area_id");

                entity.HasOne(x => x.Factory)
                      .WithMany() // hoặc .WithMany(c => c.Equipments) nếu có
                      .HasForeignKey(x => x.FactoryId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_equipment_factory_id");

                entity.HasOne(x => x.Part)
                      .WithMany()
                      .HasForeignKey(x => x.PartId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_equipment_part_id");
            });

            modelBuilder.Entity<IncidentHeaderMRO>(entity =>
            {
                entity.ToTable("incident_hdr", "mro");

                entity.HasKey(x => x.IncidentId).HasName("pk_incident_hdr");

                entity.Property(x => x.IncidentId)
                      .HasColumnName("incident_id")
                      .UseIdentityByDefaultColumn(); // nếu SQL Server thì bỏ dòng này

                entity.Property(x => x.IncidentCode)
                      .HasColumnName("incident_code")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.Status)
                      .HasColumnName("status")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.Title)
                      .HasColumnName("title")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.Description)
                      .HasColumnName("description")
                      .HasColumnType("text");

                // soft refs + FK optional
                entity.Property(x => x.EquipmentId).HasColumnName("equipment_id");
                entity.Property(x => x.EquipmentCode)
                      .HasColumnName("equipment_code")
                      .HasColumnType("text"); // dùng "citext" nếu muốn case-insensitive

                entity.Property(x => x.AreaId).HasColumnName("area_id");
                entity.Property(x => x.AreaCode)
                      .HasColumnName("area_code")
                      .HasColumnType("text");

                // Company (FK bắt buộc)
                entity.Property(x => x.CompanyId).HasColumnName("company_id");

                entity.Property(x => x.RolePrefix)
                      .HasColumnName("role_prefix")
                      .HasColumnType("text");

                entity.Property(x => x.CreatedAt).HasColumnName("created_at");
                entity.Property(x => x.CreatedBy).HasColumnName("created_by");

                entity.Property(x => x.ExecAt).HasColumnName("exec_at");
                entity.Property(x => x.ExecBy).HasColumnName("exec_by");

                entity.Property(x => x.DoneAt).HasColumnName("done_at");
                entity.Property(x => x.DoneBy).HasColumnName("done_by");

                entity.Property(x => x.ClosedAt).HasColumnName("closed_at");
                entity.Property(x => x.ClosedBy).HasColumnName("closed_by");

                entity.Property(x => x.WaitMin).HasColumnName("wait_min");
                entity.Property(x => x.RepairMin).HasColumnName("repair_min");
                entity.Property(x => x.TotalMin).HasColumnName("total_min");

                // -------- Indexes / Unique --------
                entity.HasIndex(x => new { x.CompanyId, x.IncidentCode })
                      .HasDatabaseName("ux_incident_hdr_company_code")
                      .IsUnique();

                entity.HasIndex(x => x.Status).HasDatabaseName("ix_incident_hdr_status");
                entity.HasIndex(x => new { x.EquipmentId, x.EquipmentCode })
                      .HasDatabaseName("ix_incident_hdr_equipment");
                entity.HasIndex(x => x.AreaId).HasDatabaseName("ix_incident_hdr_area");
                entity.HasIndex(x => x.CreatedAt).HasDatabaseName("ix_incident_hdr_created_at");

                // -------- Relationships --------
                entity.HasOne(x => x.Company)
                      .WithMany()
                      .HasForeignKey(x => x.CompanyId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_company_id");

                entity.HasOne(x => x.Area)
                      .WithMany()
                      .HasForeignKey(x => x.AreaId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_area_id");

                entity.HasOne(x => x.Equipment)
                      .WithMany()
                      .HasForeignKey(x => x.EquipmentId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_equipment_id");

                entity.HasOne(x => x.CreatedByEmployee)
                      .WithMany()
                      .HasForeignKey(x => x.CreatedBy)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_created_by");

                entity.HasOne(x => x.ExecByEmployee)
                      .WithMany()
                      .HasForeignKey(x => x.ExecBy)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_exec_by");

                entity.HasOne(x => x.DoneByEmployee)
                      .WithMany()
                      .HasForeignKey(x => x.DoneBy)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_done_by");

                entity.HasOne(x => x.ClosedByEmployee)
                      .WithMany()
                      .HasForeignKey(x => x.ClosedBy)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("fk_incident_hdr_closed_by");
            });


            /// ==================================== Audit Module ==================================== 
            modelBuilder.Entity<CodeCounter>(entity =>
            {
                entity.ToTable("code_counters", "Audit");

                // PK tổng hợp (Prefix, Ymd)
                entity.HasKey(x => new { x.Prefix, x.Ymd })
                      .HasName("pk_code_counters_prefix_ymd");

                entity.Property(x => x.Prefix)
                      .HasColumnName("prefix")
                      .HasColumnType("text")
                      .IsRequired();

                entity.Property(x => x.Ymd)
                      .HasColumnName("ymd")
                      .HasColumnType("integer")
                      .IsRequired();

                entity.Property(x => x.LastValue)
                      .HasColumnName("last_value")
                      .HasColumnType("integer")
                      .HasDefaultValue(0)
                      .IsRequired();

                // Index phụ để tra cứu theo prefix nhanh
                entity.HasIndex(x => x.Prefix)
                      .HasDatabaseName("ix_code_counters_prefix");
            });

        }
    }
}





