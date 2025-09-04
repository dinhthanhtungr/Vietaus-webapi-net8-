
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
//using QuestPDF.Infrastructure;
using System.Net;
using System.Reflection.Metadata;
using VietausWebAPI.Core.Domain.Entities;
//using System.Text.RegularExpressions;
using VietausWebAPI.Core.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace VietausWebAPI.WebAPI.DatabaseContext
{
    // Scaffold-DbContext "Server=DESKTOP-BL5L5IM;Database=VietausDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -context ApplicationDbContext


    //Scaffold-DbContext "Host=Localhost;Port=5432;Database=VietausDb;Username=postgres;Password=qazwsxedc123@" 


    //namespace VietausWebAPI.Core.Domain.Entities;



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

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<ApprovalHistory> ApprovalHistories { get; set; }

        public virtual DbSet<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; }

        public virtual DbSet<ApprovalLevelsCommonDatum> ApprovalLevelsCommonData { get; set; }

        public virtual DbSet<Branch> Branches { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<CustomerAssignment> CustomerAssignments { get; set; }

        public virtual DbSet<CustomerTransferLog> CustomerTransferLogs { get; set; }

        public virtual DbSet<DetailCustomerTransfer> DetailCustomerTransfers { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<EmployeesCommonDatum> EmployeesCommonData { get; set; }

        public virtual DbSet<Formula> Formulas { get; set; }

        public virtual DbSet<FormulaMaterial> FormulaMaterials { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<GroupsCommonDatum> GroupsCommonData { get; set; }

        public virtual DbSet<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; }

        public virtual DbSet<MachinesCommonDatum> MachinesCommonData { get; set; }

        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<MaterialGroup> MaterialGroups { get; set; }

        public virtual DbSet<MaterialGroupsMaterialDatum> MaterialGroupsMaterialData { get; set; }

        public virtual DbSet<MaterialsMaterialDatum> MaterialsMaterialData { get; set; }

        public virtual DbSet<MaterialsSupplier> MaterialsSuppliers { get; set; }

        public virtual DbSet<MaterialsSuppliersMaterialDatum> MaterialsSuppliersMaterialData { get; set; }

        public virtual DbSet<MemberInGroup> MemberInGroups { get; set; }

        public virtual DbSet<MerchandiseOrder> MerchandiseOrders { get; set; }

        public virtual DbSet<MerchandiseOrderDetail> MerchandiseOrderDetails { get; set; }

        public virtual DbSet<MerchandiseOrderSchedule> MerchandiseOrderSchedules { get; set; }

        public virtual DbSet<MfgProductionOrdersPlan> MfgProductionOrdersPlans { get; set; }

        public virtual DbSet<Part> Parts { get; set; }

        public virtual DbSet<PartsCommonDatum> PartsCommonData { get; set; }

        public virtual DbSet<PriceHistory> PriceHistories { get; set; }

        public virtual DbSet<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductChangedHistory> ProductChangedHistories { get; set; }

        public virtual DbSet<ProductInspection> ProductInspections { get; set; }

        public virtual DbSet<ProductStandard> ProductStandards { get; set; }

        public virtual DbSet<ProductTest> ProductTests { get; set; }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public virtual DbSet<PurchaseOrderDetailsMaterialDatum> PurchaseOrderDetailsMaterialData { get; set; }

        public virtual DbSet<PurchaseOrderStatusHistory> PurchaseOrderStatusHistories { get; set; }

        public virtual DbSet<PurchaseOrderStatusHistoryMaterialDatum> PurchaseOrderStatusHistoryMaterialData { get; set; }

        public virtual DbSet<PurchaseOrdersMaterialDatum> PurchaseOrdersMaterialData { get; set; }

        public virtual DbSet<PurchaseOrdersSchedule> PurchaseOrdersSchedules { get; set; }

        public virtual DbSet<Qcdetail> Qcdetails { get; set; }

        public virtual DbSet<RequestDetail> RequestDetails { get; set; }

        public virtual DbSet<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; }

        public virtual DbSet<SampleRequest> SampleRequests { get; set; }
        public virtual DbSet<SampleRequestImage> SampleRequestImages { get; set; }

        public virtual DbSet<SchedualMfg> SchedualMfgs { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<SupplierAddress> SupplierAddresses { get; set; }

        public virtual DbSet<SupplierContact> SupplierContacts { get; set; }

        public virtual DbSet<SuppliersMaterialDatum> SuppliersMaterialData { get; set; }

        public virtual DbSet<SupplyRequest> SupplyRequests { get; set; }

        public virtual DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }

        public virtual DbSet<Unit> Units { get; set; }

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


            // Thiết lập khóa chính
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__Address__091C2A1BCCA306B0");

                entity.ToTable("Address", "sales");

                entity.HasIndex(e => e.CustomerId, "IX_Address_CustomerID");

                entity.Property(e => e.AddressId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("AddressID");
                entity.Property(e => e.AddressLine).HasColumnType("citext");
                entity.Property(e => e.City).HasColumnType("citext");
                entity.Property(e => e.Country).HasColumnType("citext");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.District).HasColumnType("citext");
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);
                entity.Property(e => e.PostalCode).HasColumnType("citext");
                entity.Property(e => e.Province).HasColumnType("citext");

                entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

            modelBuilder.Entity<ApprovalHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC27EC140479");

                entity.ToTable("ApprovalHistory_Material_data");

                entity.HasIndex(e => e.EmployeeId, "IX_ApprovalHistory_Material_data_EmployeeID");

                entity.HasIndex(e => e.RequestId, "IX_ApprovalHistory_Material_data_RequestID");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Note).HasColumnType("citext");
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .HasColumnName("RequestID");

                entity.HasOne(d => d.Employee).WithMany(p => p.ApprovalHistoryMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHistory_Employee");

                entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistoryMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHistory_Request");
            });

            modelBuilder.Entity<ApprovalLevelsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__Approval__09F03C061CA120B7");

                entity.ToTable("ApprovalLevels_Common_data");

                entity.Property(e => e.LevelId)
                    .HasMaxLength(10)
                    .HasColumnName("LevelID");
                entity.Property(e => e.Description).HasColumnType("citext");
                entity.Property(e => e.LevelName).HasColumnType("citext");
            });

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

                entity.ToTable("Categories", "inventory");

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
                entity.HasKey(e => e.ContactId).HasName("PK__Contacts__5C6625BB570D4F62");

                entity.ToTable("Contacts", "sales");

                entity.HasIndex(e => e.CustomerId, "IX_Contacts_CustomerID");

                entity.Property(e => e.ContactId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ContactID");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.Email).HasColumnType("citext");
                entity.Property(e => e.FirstName).HasColumnType("citext");
                entity.Property(e => e.Gender).HasColumnType("citext");
                entity.Property(e => e.LastName).HasColumnType("citext");
                entity.Property(e => e.Phone).HasColumnType("citext");

                entity.HasOne(d => d.Customer).WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contacts_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D875977EBB");

                entity.ToTable("Customer", "sales");

                entity.HasIndex(e => e.CompanyId, "IX_Customer_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Customer_CreatedBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_Customer_UpdatedBy");

                entity.Property(e => e.CustomerId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ApplicationName).HasColumnType("citext");
                entity.Property(e => e.CustomerGroup).HasColumnType("citext");
                entity.Property(e => e.CustomerName).HasColumnType("citext");
                entity.Property(e => e.ExternalId).HasColumnType("citext");
                entity.Property(e => e.FaxNumber).HasColumnType("citext");
                entity.Property(e => e.IssuedPlace).HasColumnType("citext");
                entity.Property(e => e.Phone).HasColumnType("citext");
                entity.Property(e => e.Product).HasColumnType("citext");
                entity.Property(e => e.RegistrationNumber).HasColumnType("citext");
                entity.Property(e => e.TaxNumber).HasColumnType("citext");
                entity.Property(e => e.Website).HasColumnType("citext");

                entity.HasOne(d => d.Company).WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__Customer__Compan__1E8F7FEF");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Groups_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CustomerUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Groups_UpdatedBy");
            });

            modelBuilder.Entity<CustomerAssignment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC279397A842");

                entity.ToTable("CustomerAssignment", "sales");

                entity.HasIndex(e => e.CustomerId, "IX_CustomerAssignment_CustomerID");

                entity.HasIndex(e => e.EmployeeId, "IX_CustomerAssignment_EmployeeID");

                entity.HasIndex(e => e.CompanyId, "IX_CustomerAssignment_companyId");

                entity.HasIndex(e => e.CreatedBy, "IX_CustomerAssignment_createdBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_CustomerAssignment_updatedBy");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ID");
                entity.Property(e => e.CompanyId).HasColumnName("companyId");
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdDate");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
                entity.Property(e => e.UpdatedDate)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("updatedDate");

                entity.HasOne(d => d.Company).WithMany(p => p.CustomerAssignments)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAssignment_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerAssignmentCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAssignment_CreatedBy");

                entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAssignments)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAssignment_Customer");

                entity.HasOne(d => d.Employee).WithMany(p => p.CustomerAssignmentEmployees)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAssignment_EmployeeID");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.CustomerAssignmentUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerAssignment_updatedBy");
            });

            modelBuilder.Entity<CustomerTransferLog>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC276977D605");

                entity.ToTable("CustomerTransferLog", "sales");

                entity.HasIndex(e => e.FromEmployeeId, "IX_CustomerTransferLog_FromEmployeeId");

                entity.HasIndex(e => e.FromGroupId, "IX_CustomerTransferLog_FromGroupId");

                entity.HasIndex(e => e.ToEmployeeId, "IX_CustomerTransferLog_ToEmployeeId");

                entity.HasIndex(e => e.ToGroupId, "IX_CustomerTransferLog_ToGroupId");

                entity.HasIndex(e => e.CompanyId, "IX_CustomerTransferLog_companyId");

                entity.HasIndex(e => e.CreatedBy, "IX_CustomerTransferLog_createdBy");

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ID");
                entity.Property(e => e.CompanyId).HasColumnName("companyId");
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("now()")
                    .HasColumnName("createdDate");
                entity.Property(e => e.Note).HasColumnType("citext");

                entity.HasOne(d => d.Company).WithMany(p => p.CustomerTransferLogs)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransferLog_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CustomerTransferLogCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransferLog_CreatedBy");

                entity.HasOne(d => d.FromEmployee).WithMany(p => p.CustomerTransferLogFromEmployees)
                    .HasForeignKey(d => d.FromEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransferLog_FromEmployeeId");

                entity.HasOne(d => d.FromGroup).WithMany(p => p.CustomerTransferLogFromGroups)
                    .HasForeignKey(d => d.FromGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransferLog_FromGroupId");

                entity.HasOne(d => d.ToEmployee).WithMany(p => p.CustomerTransferLogToEmployees)
                    .HasForeignKey(d => d.ToEmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransferLog_ToEmployeeId");

                entity.HasOne(d => d.ToGroup).WithMany(p => p.CustomerTransferLogToGroups)
                    .HasForeignKey(d => d.ToGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerTransferLog_ToGroupId");
            });

            modelBuilder.Entity<DetailCustomerTransfer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__DetailCu__3214EC273B3F47A0");

                entity.ToTable("DetailCustomerTransfer", "sales");

                entity.HasIndex(e => e.CustomerId, "IX_DetailCustomerTransfer_CustomerID");

                entity.HasIndex(e => e.LogId, "IX_DetailCustomerTransfer_LogID");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.HasOne(d => d.Customer).WithMany(p => p.DetailCustomerTransfers)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailCustomerTransfer_CustomerID");

                entity.HasOne(d => d.Log).WithMany(p => p.DetailCustomerTransfers)
                    .HasForeignKey(d => d.LogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetailCustomerTransfer_LogID");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1C1895B9F");

                entity.ToTable("Employees", "hr");

                entity.HasIndex(e => e.PartId, "IX_Employees_PartID");

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
            });

            modelBuilder.Entity<EmployeesCommonDatum>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF184E31508");

                entity.ToTable("Employees_Common_data");

                entity.HasIndex(e => e.LevelId, "IX_Employees_Common_data_LevelID");

                entity.HasIndex(e => e.PartId, "IX_Employees_Common_data_PartID");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Address).HasColumnType("citext");
                entity.Property(e => e.Email).HasColumnType("citext");
                entity.Property(e => e.FullName).HasColumnType("citext");
                entity.Property(e => e.Gender).HasColumnType("citext");
                entity.Property(e => e.Identifier).HasColumnType("citext");
                entity.Property(e => e.LevelId)
                    .HasMaxLength(10)
                    .HasColumnName("LevelID");
                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .HasColumnName("PartID");
                entity.Property(e => e.PhoneNumber).HasColumnType("citext");
                entity.Property(e => e.Status).HasColumnType("citext");

                entity.HasOne(d => d.Level).WithMany(p => p.EmployeesCommonData)
                    .HasForeignKey(d => d.LevelId)
                    .HasConstraintName("FK__Employees__Level__693CA210");

                entity.HasOne(d => d.Part).WithMany(p => p.EmployeesCommonData)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__PartI__68487DD7");
            });

            modelBuilder.Entity<Formula>(entity =>
            {
                entity.HasKey(e => e.FormulaId).HasName("PK__Formulas__227429A55C6F1195");

                entity.ToTable("Formulas", "labs");

                entity.HasIndex(e => e.CompanyId, "IX_Formulas_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Formulas_CreatedBy");

                entity.HasIndex(e => e.ProductId, "IX_Formulas_ProductId");

                entity.HasIndex(e => e.UpdatedBy, "IX_Formulas_UpdatedBy");

                entity.Property(e => e.FormulaId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.TotalPrice).HasPrecision(16, 2);
                entity.Property(e => e.Status)
                      .HasMaxLength(32)
                      .HasDefaultValue("Draft");

                entity.HasOne(d => d.Company).WithMany(p => p.Formulas)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Formulas_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FormulaCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Formulas_CreatedBy");

                entity.HasOne(d => d.Product).WithMany(p => p.Formulas)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Formulas_Product");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FormulaUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Formulas_UpdatedBy");

            });


            modelBuilder.Entity<FormulaStatusLog>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK_FormulaStatusLog");
                entity.ToTable("FormulaStatusLog", "labs");

                // Index như yêu cầu
                entity.HasIndex(e => e.FormulaId, "IX_FormulaStatusLog_FormulaId");
                entity.HasIndex(e => e.CreatedDate, "IX_FormulaStatusLog_CreatedAt");

                entity.Property(e => e.LogId)
                      .HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.OldStatus).HasMaxLength(32);
                entity.Property(e => e.NewStatus).HasMaxLength(32);
                entity.Property(e => e.CreateNameSnapShot).HasMaxLength(200);
                entity.Property(e => e.CreatedDate)
                      .HasDefaultValueSql("now()"); // mặc định thời điểm ghi log

                entity.HasOne(d => d.Formula).WithMany(p => p.StatusLogs)
                      .HasForeignKey(d => d.FormulaId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_FormulaStatusLog_Formula");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.FormulaStatusLogCreatedByNavigations)
                      .HasForeignKey(d => d.CreatedBy)
                      .HasConstraintName("FK_FormulaStatusLog_CreatedBy");

                // (Khuyến nghị) hợp lệ hoá trạng thái
                //entity.ToTable(tb => tb.HasCheckConstraint(
                //    "CK_FormulaStatusLog_Status",
                //    "(OldStatus IS NULL OR OldStatus IN ('Draft','Sent','Verified','Rejected','Archived')) " +
                //    "AND (NewStatus IS NULL OR NewStatus IN ('Draft','Sent','Verified','Rejected','Archived'))"
                //));
            });


            modelBuilder.Entity<FormulaMaterial>(entity =>
            {
                entity.HasKey(e => e.FormulaMaterialId).HasName("PK__FormulaM__0315C60A1F19742A");

                entity.ToTable("FormulaMaterials", "labs");

                entity.HasIndex(e => e.FormulaId, "IX_FormulaMaterials_FormulaId");

                entity.HasIndex(e => e.MaterialId, "IX_FormulaMaterials_MaterialId");


                entity.Property(e => e.FormulaMaterialId).HasDefaultValueSql("gen_random_uuid()");


                entity.Property(e => e.Quantity).HasPrecision(18, 6);
                entity.Property(e => e.UnitPrice).HasPrecision(16, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(16, 2);

                entity.Property(e => e.MaterialNameSnapshot).HasMaxLength(200);
                entity.Property(e => e.MaterialCodeSnapshot).HasMaxLength(50);
                entity.Property(e => e.Unit).HasMaxLength(32);

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

                entity.HasIndex(e => e.CompanyId, "IX_Groups_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Groups_CreatedBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_Groups_UpdatedBy");

                entity.Property(e => e.GroupId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.GroupType).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(200);

                entity.HasOne(d => d.Company).WithMany(p => p.Groups)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK__Groups__CompanyI__131DCD43");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GroupCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Groups_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.GroupUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Groups_UpdatedBy");
            });

            modelBuilder.Entity<GroupsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF30A6FD59030");

                entity.ToTable("Groups_Common_data");

                entity.Property(e => e.GroupId)
                    .HasMaxLength(10)
                    .HasColumnName("GroupID");
                entity.Property(e => e.Description).HasColumnType("citext");
                entity.Property(e => e.GroupName).HasColumnType("citext");
            });

            modelBuilder.Entity<InventoryReceiptsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C400857F0DFA");

                entity.ToTable("InventoryReceipts_Material_data");

                entity.HasIndex(e => e.DetailId, "IX_InventoryReceipts_Material_data_DetailID");

                entity.HasIndex(e => e.RequestId, "IX_InventoryReceipts_Material_data_RequestID");

                entity.HasIndex(e => e.MaterialId, "IX_InventoryReceipts_Material_data_materialId");

                entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.Note).HasColumnType("citext");
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .HasColumnName("RequestID");
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

                entity.HasOne(d => d.Detail).WithMany(p => p.InventoryReceiptsMaterialData)
                    .HasForeignKey(d => d.DetailId)
                    .HasConstraintName("FK_PO_Detail_InventoryReceipt");

                entity.HasOne(d => d.Material).WithMany(p => p.InventoryReceiptsMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryReceipts_Material");

                entity.HasOne(d => d.Request).WithMany(p => p.InventoryReceiptsMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryReceipts_Request");
            });

            modelBuilder.Entity<MachinesCommonDatum>(entity =>
            {
                entity.HasKey(e => e.MachineId).HasName("PK__Machines__5A97603FADC7D2ED");

                entity.ToTable("Machines_Common_data");

                entity.HasIndex(e => e.GroupId, "IX_Machines_Common_data_GroupID");

                entity.HasIndex(e => e.PartId, "IX_Machines_Common_data_PartID");

                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Description).HasColumnType("citext");
                entity.Property(e => e.Factory)
                    .HasDefaultValueSql("'Tam Phước'::character varying")
                    .HasColumnType("citext");
                entity.Property(e => e.GroupId)
                    .HasMaxLength(10)
                    .HasColumnName("GroupID");
                entity.Property(e => e.GroupMachine).HasColumnType("citext");
                entity.Property(e => e.MachineName).HasColumnType("citext");
                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .HasColumnName("PartID");

                entity.HasOne(d => d.Group).WithMany(p => p.MachinesCommonData)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Machines__GroupI__6477ECF3");

                entity.HasOne(d => d.Part).WithMany(p => p.MachinesCommonData)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Machines__PartID__656C112C");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.MaterialId).HasName("PK__Material__C50610F7C355BA5C");

                entity.ToTable("Materials", "inventory");

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

            modelBuilder.Entity<MaterialGroup>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD68C7B626");

                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(50)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialGroupName).HasColumnType("citext");
            });

            modelBuilder.Entity<MaterialGroupsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD37B632C7");

                entity.ToTable("MaterialGroups_Material_data");

                entity.Property(e => e.MaterialGroupId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.Detail).HasColumnType("citext");
                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("externalId");
                entity.Property(e => e.MaterialGroupName).HasColumnType("citext");
            });

            modelBuilder.Entity<MaterialsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialId).HasName("PK__Material__99B653FDEEB02A00");

                entity.ToTable("Materials_material_data");

                entity.HasIndex(e => e.EmployeeId, "IX_Materials_material_data_EmployeeID");

                entity.HasIndex(e => e.MaterialGroupId, "IX_Materials_material_data_MaterialGroupId");

                entity.Property(e => e.MaterialId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("materialId");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("externalId");
                entity.Property(e => e.Name).HasColumnType("citext");
                entity.Property(e => e.Unit).HasColumnType("citext");

                entity.HasOne(d => d.Employee).WithMany(p => p.MaterialsMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Materials__Emplo__6DCC4D03");

                entity.HasOne(d => d.MaterialGroup).WithMany(p => p.MaterialsMaterialData)
                    .HasForeignKey(d => d.MaterialGroupId)
                    .HasConstraintName("FK__Materials__Mater__6CD828CA");
            });

            modelBuilder.Entity<MaterialsSupplier>(entity =>
            {
                entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB73A34869");

                entity.ToTable("Materials_Suppliers", "inventory");

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

            modelBuilder.Entity<MaterialsSuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB69530C3D");

                entity.ToTable("MaterialsSuppliers_material_data");

                entity.HasIndex(e => e.MaterialId, "IX_MaterialsSuppliers_material_data_materialId");

                entity.HasIndex(e => e.PriceHistoryId, "IX_MaterialsSuppliers_material_data_priceHistoryId");

                entity.HasIndex(e => e.SupplierId, "IX_MaterialsSuppliers_material_data_supplierId");

                entity.Property(e => e.MaterialsSuppliersId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("Materials_SuppliersId");
                entity.Property(e => e.Currency)
                    .HasColumnType("citext")
                    .HasColumnName("currency");
                entity.Property(e => e.CurrentPrice)
                    .HasPrecision(18, 2)
                    .HasColumnName("currentPrice");
                entity.Property(e => e.IsPreferred)
                    .HasDefaultValue(false)
                    .HasColumnName("isPreferred");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.MinDeliveryDays).HasColumnName("minDeliveryDays");
                entity.Property(e => e.PriceHistoryId).HasColumnName("priceHistoryId");
                entity.Property(e => e.SupplierId).HasColumnName("supplierId");
                entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");

                entity.HasOne(d => d.Material).WithMany(p => p.MaterialsSuppliersMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__Materials__mater__7FEAFD3E");

                entity.HasOne(d => d.PriceHistory).WithMany(p => p.MaterialsSuppliersMaterialData)
                    .HasForeignKey(d => d.PriceHistoryId)
                    .HasConstraintName("FK__Materials__price__2A6B46EF");

                entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialsSuppliersMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Materials__suppl__2B5F6B28");
            });

            modelBuilder.Entity<MemberInGroup>(entity =>
            {
                entity.HasKey(e => e.MemberId).HasName("PK__MemberIn__0CF04B189ED315D5");

                entity.ToTable("MemberInGroup", "company");

                entity.HasIndex(e => e.GroupId, "IX_MemberInGroup_GroupId");

                entity.HasIndex(e => e.Profile, "IX_MemberInGroup_Profile");

                entity.Property(e => e.MemberId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.IsAdmin).HasDefaultValue(false);

                entity.HasOne(d => d.Group).WithMany(p => p.MemberInGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberInGroup_Groups");

                entity.HasOne(d => d.ProfileNavigation).WithMany(p => p.MemberInGroups)
                    .HasForeignKey(d => d.Profile)
                    .HasConstraintName("FK_MemberInGroup_Profile");
            });

            modelBuilder.Entity<MerchandiseOrder>(entity =>
            {
                entity.HasKey(e => e.MerchandiseOrderId).HasName("PK__Merchand__D0AB7E7AFDA62167");

                entity.ToTable("MerchandiseOrders", "Orders");

                entity.HasIndex(e => e.CompanyId, "IX_MerchandiseOrders_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_MerchandiseOrders_CreatedBy");

                entity.HasIndex(e => e.CustomerId, "IX_MerchandiseOrders_CustomerId");

                entity.HasIndex(e => e.ManagerById, "IX_MerchandiseOrders_ManagerById");

                entity.HasIndex(e => e.UpdatedBy, "IX_MerchandiseOrders_UpdatedBy");

                entity.Property(e => e.MerchandiseOrderId).ValueGeneratedNever();
                entity.Property(e => e.DeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.PaymentType).HasMaxLength(50);
                entity.Property(e => e.Receiver).HasMaxLength(255);
                entity.Property(e => e.ShippingMethod).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
                entity.Property(e => e.Vat).HasColumnName("VAT");

                entity.HasOne(d => d.Company).WithMany(p => p.MerchandiseOrders)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_MerchandiseOrderst_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MerchandiseOrderCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_MerchandiseOrderst_CreatedBy");

                entity.HasOne(d => d.Customer).WithMany(p => p.MerchandiseOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_MerchandiseOrders_Customer");

                entity.HasOne(d => d.ManagerBy).WithMany(p => p.MerchandiseOrderManagerBies)
                    .HasForeignKey(d => d.ManagerById)
                    .HasConstraintName("FK_MerchandiseOrderst_ManagerById");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MerchandiseOrderUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_MerchandiseOrderst_UpdatedBy");
            });

            modelBuilder.Entity<MerchandiseOrderDetail>(entity =>
            {
                entity.HasKey(e => e.MerchandiseOrderDetailId).HasName("PK__Merchand__FE0FB3FF67BDE750");

                entity.ToTable("MerchandiseOrderDetails", "Orders");

                entity.HasIndex(e => e.MerchandiseOrderId, "IX_MerchandiseOrderDetails_MerchandiseOrderId");

                entity.HasIndex(e => e.ProductId, "IX_MerchandiseOrderDetails_ProductId");

                entity.Property(e => e.MerchandiseOrderDetailId).ValueGeneratedNever();
                entity.Property(e => e.BagType).HasMaxLength(50);
                entity.Property(e => e.FormulaExternalId).HasMaxLength(50);
                entity.Property(e => e.Price).HasPrecision(18, 2);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.MerchandiseOrderDetails)
                    .HasForeignKey(d => d.MerchandiseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchandiseOrderDetails_MerchandiseOrderId");

                entity.HasOne(d => d.Product).WithMany(p => p.MerchandiseOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchandiseOrderDetails_ProductId");
            });

            modelBuilder.Entity<MerchandiseOrderSchedule>(entity =>
            {
                entity.HasKey(e => e.MerchandiseOrderScheduleId).HasName("PK__Merchand__B49D545E8C296524");

                entity.ToTable("MerchandiseOrderSchedules", "Orders");

                entity.HasIndex(e => e.MerchandiseOrderId, "IX_MerchandiseOrderSchedules_MerchandiseOrderId");

                entity.Property(e => e.MerchandiseOrderScheduleId).ValueGeneratedNever();
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.MerchandiseOrder).WithMany(p => p.MerchandiseOrderSchedules)
                    .HasForeignKey(d => d.MerchandiseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MerchandiseOrderSchedules_MerchandiseOrderId");
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

            modelBuilder.Entity<PartsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D3012CD1E19");

                entity.ToTable("Parts_Common_data");

                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .HasColumnName("PartID");
                entity.Property(e => e.PartName).HasColumnType("citext");
            });

            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__A927CACB4B3A2EAC");

                entity.ToTable("PriceHistory", "inventory");

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

            modelBuilder.Entity<PriceHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__77D1486CB71DF1F1");

                entity.ToTable("PriceHistory_material_data");

                entity.HasIndex(e => e.MaterialId, "IX_PriceHistory_material_data_materialId");

                entity.HasIndex(e => e.SupplierId, "IX_PriceHistory_material_data_supplierId");

                entity.HasIndex(e => e.UpdatedBy, "IX_PriceHistory_material_data_updatedBy");

                entity.Property(e => e.PriceHistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("priceHistoryId");
                entity.Property(e => e.Currency)
                    .HasColumnType("citext")
                    .HasColumnName("currency");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.NewPrice)
                    .HasPrecision(18, 2)
                    .HasColumnName("newPrice");
                entity.Property(e => e.OldPrice)
                    .HasPrecision(18, 2)
                    .HasColumnName("oldPrice");
                entity.Property(e => e.Reason)
                    .HasColumnType("citext")
                    .HasColumnName("reason");
                entity.Property(e => e.SupplierId).HasColumnName("supplierId");
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(16)
                    .HasColumnName("updatedBy");
                entity.Property(e => e.UpdatedDate).HasColumnName("updatedDate");

                entity.HasOne(d => d.Material).WithMany(p => p.PriceHistoryMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PriceHist__mater__793DFFAF");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PriceHistoryMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PriceHist__suppl__2D47B39A");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PriceHistoryMaterialData)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__PriceHist__updat__2E3BD7D3");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD344F4294");

                entity.ToTable("Products", "labs");

                entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

                entity.HasIndex(e => e.CompanyId, "IX_Products_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_Products_CreatedBy");

                entity.HasIndex(e => e.UpdatedBy, "IX_Products_UpdatedBy");

                entity.Property(e => e.ProductId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Additive).HasMaxLength(200);
                entity.Property(e => e.Application).HasMaxLength(100);
                entity.Property(e => e.Code).HasMaxLength(100);
                entity.Property(e => e.ColourCode).HasMaxLength(50);
                entity.Property(e => e.ColourName).HasMaxLength(100);
                entity.Property(e => e.EndUser).HasMaxLength(100);
                entity.Property(e => e.ExpiryType).HasMaxLength(100);
                entity.Property(e => e.LabComment).HasMaxLength(500);
                entity.Property(e => e.LightCondition).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.PolymerMatchedIn).HasMaxLength(100);
                entity.Property(e => e.Procedure).HasMaxLength(100);
                entity.Property(e => e.ProductType).HasMaxLength(100);
                entity.Property(e => e.ProductUsage).HasMaxLength(100);
                entity.Property(e => e.Requirement).HasMaxLength(500);
                //entity.Property(e => e.StorageCondition).HasMaxLength(200);
                entity.Property(e => e.Unit).HasMaxLength(50);
                entity.Property(e => e.VisualTest).HasMaxLength(100);
                entity.Property(e => e.WeatherResistance).HasMaxLength(100);

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Category");

                entity.HasOne(d => d.Company).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Products_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Products_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Products_UpdatedBy");
            });

            modelBuilder.Entity<ProductChangedHistory>(entity =>
            {
                entity.HasKey(e => e.ProductChangedHistoryId).HasName("PK__ProductC__A793B6CA9FB36DED");

                entity.ToTable("ProductChangedHistory", "labs");

                entity.HasIndex(e => e.ChangedBy, "IX_ProductChangedHistory_ChangedBy");

                entity.HasIndex(e => e.ProductId, "IX_ProductChangedHistory_ProductId");

                entity.Property(e => e.ChangeNote).HasMaxLength(500);
                entity.Property(e => e.ChangeType).HasMaxLength(100);
                entity.Property(e => e.FieldChanged).HasMaxLength(100);
                entity.Property(e => e.NewValue).HasMaxLength(500);
                entity.Property(e => e.OldValue).HasMaxLength(500);

                entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.ProductChangedHistories)
                    .HasForeignKey(d => d.ChangedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductHistory_ChangedBy");

                entity.HasOne(d => d.Product).WithMany(p => p.ProductChangedHistories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductHistory_Product");
            });

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

                entity.ToTable("PurchaseOrders", "inventory");

                entity.HasIndex(e => e.CompanyId, "IX_PurchaseOrders_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_PurchaseOrders_CreatedBy");

                entity.HasIndex(e => e.SupplierId, "IX_PurchaseOrders_SupplierId");

                entity.HasIndex(e => e.UpdatedBy, "IX_PurchaseOrders_UpdatedBy");

                entity.Property(e => e.PurchaseOrderId).ValueGeneratedNever();
                entity.Property(e => e.DeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.OrderType).HasMaxLength(50);
                entity.Property(e => e.PackageType).HasMaxLength(50);
                entity.Property(e => e.PaymentDays).HasMaxLength(50);
                entity.Property(e => e.RequestSourceType).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.TotalPrice).HasPrecision(16, 2);
                entity.Property(e => e.Vat).HasColumnName("VAT");

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
                entity.HasKey(e => e.PurchaseOrderDetailId).HasName("PK__Purchase__5026B698A94EFE68");

                entity.ToTable("PurchaseOrderDetails", "inventory");

                entity.HasIndex(e => e.MaterialId, "IX_PurchaseOrderDetails_MaterialId");

                entity.HasIndex(e => e.PurchaseOrderId, "IX_PurchaseOrderDetails_PurchaseOrderId");

                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Price).HasPrecision(18, 2);

                entity.HasOne(d => d.Material).WithMany(p => p.PurchaseOrderDetails)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderDetails_MaterialId");

                entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderDetails)
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseOrderDetails_PurchaseOrderId");
            });

            modelBuilder.Entity<PurchaseOrderDetailsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.PodetailId).HasName("PK__Purchase__4EB47B3EB8D4CA48");

                entity.ToTable("PurchaseOrderDetails_material_data");

                entity.HasIndex(e => e.DetailId, "IX_PurchaseOrderDetails_material_data_DetailID");

                entity.HasIndex(e => e.MaterialId, "IX_PurchaseOrderDetails_material_data_MaterialId");

                entity.HasIndex(e => e.Poid, "IX_PurchaseOrderDetails_material_data_POID");

                entity.Property(e => e.PodetailId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PODetailId");
                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.Note)
                    .HasColumnType("citext")
                    .HasColumnName("note");
                entity.Property(e => e.Poid).HasColumnName("POID");
                entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

                entity.HasOne(d => d.Detail).WithMany(p => p.PurchaseOrderDetailsMaterialData)
                    .HasForeignKey(d => d.DetailId)
                    .HasConstraintName("FK_PO_Detail_RequestDetail");

                entity.HasOne(d => d.Material).WithMany(p => p.PurchaseOrderDetailsMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__PurchaseO__Mater__10216507");

                entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderDetailsMaterialData)
                    .HasForeignKey(d => d.Poid)
                    .HasConstraintName("FK__PurchaseOr__POID__30242045");
            });

            modelBuilder.Entity<PurchaseOrderStatusHistory>(entity =>
            {
                entity.HasKey(e => e.StatusHistoryId).HasName("PK__Purchase__DB973491370CD24F");

                entity.ToTable("PurchaseOrderStatusHistory", "inventory");

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

            modelBuilder.Entity<PurchaseOrderStatusHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.StatusHistoryId).HasName("PK__Purchase__DB9734917B73E8D7");

                entity.ToTable("PurchaseOrderStatusHistory_material_data");

                entity.HasIndex(e => e.Poid, "IX_PurchaseOrderStatusHistory_material_data_POID");

                entity.Property(e => e.StatusHistoryId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.EmployeeId).HasColumnType("citext");
                entity.Property(e => e.Note)
                    .HasColumnType("citext")
                    .HasColumnName("note");
                entity.Property(e => e.Poid).HasColumnName("POID");
                entity.Property(e => e.StatusFrom).HasColumnType("citext");
                entity.Property(e => e.StatusTo).HasColumnType("citext");

                entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderStatusHistoryMaterialData)
                    .HasForeignKey(d => d.Poid)
                    .HasConstraintName("FK__PurchaseOr__POID__34E8D562");
            });

            modelBuilder.Entity<PurchaseOrdersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2F4EB754C0F");

                entity.ToTable("PurchaseOrders_material_data");

                entity.HasIndex(e => e.EmployeeId, "IX_PurchaseOrders_material_data_EmployeeId");

                entity.HasIndex(e => e.SupplierId, "IX_PurchaseOrders_material_data_SupplierId");

                entity.Property(e => e.Poid)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("POID");
                entity.Property(e => e.ContactName).HasColumnType("citext");
                entity.Property(e => e.DeliveryAddress).HasColumnType("citext");
                entity.Property(e => e.DeliveryContact).HasColumnType("citext");
                entity.Property(e => e.EmployeeId).HasMaxLength(16);
                entity.Property(e => e.GrandTotal).HasPrecision(18, 2);
                entity.Property(e => e.InvoiceNote).HasColumnType("citext");
                entity.Property(e => e.Note)
                    .HasColumnType("citext")
                    .HasColumnName("note");
                entity.Property(e => e.Packaging).HasColumnType("citext");
                entity.Property(e => e.PaymentTerm).HasColumnType("citext");
                entity.Property(e => e.Pocode)
                    .HasColumnType("citext")
                    .HasColumnName("POCode");
                entity.Property(e => e.RequiredDocuments).HasColumnType("citext");
                entity.Property(e => e.RequiredDocumentsEng)
                    .HasColumnType("citext")
                    .HasColumnName("RequiredDocuments_Eng");
                entity.Property(e => e.Status)
                    .HasColumnType("citext")
                    .HasColumnName("status");
                entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
                entity.Property(e => e.Vat).HasColumnName("VAT");
                entity.Property(e => e.VendorAddress).HasColumnType("citext");
                entity.Property(e => e.VendorPhone).HasColumnType("citext");

                entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrdersMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__PurchaseO__Emplo__320C68B7");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrdersMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__PurchaseO__Suppl__33008CF0");
            });

            modelBuilder.Entity<PurchaseOrdersSchedule>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrdersScheduleId).HasName("PK__Purchase__658C1532BFBEF664");

                entity.ToTable("PurchaseOrdersSchedules", "inventory");

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

            modelBuilder.Entity<RequestDetailMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D44B7445B");

                entity.ToTable("RequestDetail_Material_data");

                entity.HasIndex(e => e.RequestId, "IX_RequestDetail_Material_data_RequestID");

                entity.HasIndex(e => e.MaterialId, "IX_RequestDetail_Material_data_materialId");

                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.Note).HasColumnType("citext");
                entity.Property(e => e.PurchasedQuantity).HasDefaultValue(0);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .HasColumnName("RequestID");

                entity.HasOne(d => d.Material).WithMany(p => p.RequestDetailMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestDetail_Material");

                entity.HasOne(d => d.Request).WithMany(p => p.RequestDetailMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestDetail_Request");
            });

            modelBuilder.Entity<SampleRequest>(entity =>
            {
                entity.HasKey(e => e.SampleRequestId).HasName("PK__SampleRe__6F83B553A1A0D2A8");

                entity.ToTable("SampleRequests", "labs");

                entity.HasIndex(e => e.CompanyId, "IX_SampleRequests_CompanyId");

                entity.HasIndex(e => e.CreatedBy, "IX_SampleRequests_CreatedBy");

                entity.HasIndex(e => e.CustomerId, "IX_SampleRequests_CustomerId");

                entity.HasIndex(e => e.FormulaId, "IX_SampleRequests_FormulaId");

                entity.HasIndex(e => e.ManagerBy, "IX_SampleRequests_ManagerBy");

                entity.HasIndex(e => e.ProductId, "IX_SampleRequests_ProductId");

                entity.HasIndex(e => e.UpdatedBy, "IX_SampleRequests_UpdatedBy");

                entity.Property(e => e.SampleRequestId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AdditionalComment).HasMaxLength(500);
                entity.Property(e => e.ExpectedPrice).HasPrecision(18, 4);
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.Image).HasMaxLength(255);
                entity.Property(e => e.InfoType).HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.OtherComment).HasMaxLength(500);
                entity.Property(e => e.Package).HasMaxLength(100);
                entity.Property(e => e.CustomerProductCode).HasMaxLength(100);
                entity.Property(e => e.RequestType).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(100);

                entity.HasOne(d => d.Company).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_SampleRequests_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SampleRequestCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_SampleRequests_CreatedBy");

                entity.HasOne(d => d.Customer).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SampleRequests_Customer");

                entity.HasOne(d => d.Formula).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.FormulaId)
                    .HasConstraintName("FK_SampleRequests_Formula");

                entity.HasOne(d => d.ManagerByNavigation).WithMany(p => p.SampleRequestManagerByNavigations)
                    .HasForeignKey(d => d.ManagerBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SampleRequests_Manager");

                entity.HasOne(d => d.Product).WithMany(p => p.SampleRequests)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SampleRequests_Product");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SampleRequestUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_SampleRequests_UpdatedBy");
            });

            modelBuilder.Entity<SampleRequestImage>(entity =>
            {
                entity.ToTable("SampleRequestImages", "labs");
                entity.HasKey(e => e.SampleRequestImageId).HasName("PK__SampleRequestImage__3214EC07A98DEC4E");

                entity.Property(e => e.SampleRequestImageId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.FileName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FileType).HasMaxLength(100).IsRequired();
                entity.Property(e => e.FileUrl).IsRequired();

                entity.HasOne(img => img.SampleRequest)
                      .WithMany(sr => sr.SampleRequestImages)
                      .HasForeignKey(img => img.SampleRequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SchedualMfg>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Schedual__3214EC07A98DEC4E");

                entity.ToTable("SchedualMfg");

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

                entity.ToTable("Suppliers", "inventory");

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

                entity.ToTable("SupplierAddresses", "inventory");

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

                entity.ToTable("SupplierContacts", "inventory");

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

            modelBuilder.Entity<SuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__DB8E62ED69C94CF3");

                entity.ToTable("Suppliers_material_data");

                entity.Property(e => e.SupplierId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("supplierId");
                entity.Property(e => e.ExternalId)
                    .HasColumnType("citext")
                    .HasColumnName("externalId");
                entity.Property(e => e.Name).HasColumnType("citext");
                entity.Property(e => e.Phone)
                    .HasColumnType("citext")
                    .HasColumnName("phone");
                entity.Property(e => e.RegNo)
                    .HasColumnType("citext")
                    .HasColumnName("regNo");
                entity.Property(e => e.TaxNo)
                    .HasColumnType("citext")
                    .HasColumnName("taxNo");
                entity.Property(e => e.Website)
                    .HasColumnType("citext")
                    .HasColumnName("website");
            });

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

            modelBuilder.Entity<SupplyRequestsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("PK__SupplyRe__33A8519AEED4B83E");

                entity.ToTable("SupplyRequests_Material_data");

                entity.HasIndex(e => e.EmployeeId, "IX_SupplyRequests_Material_data_EmployeeID");

                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .HasColumnName("RequestID");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Note).HasColumnType("citext");
                entity.Property(e => e.NoteCancel).HasColumnType("citext");
                entity.Property(e => e.RequestStatus).HasColumnType("citext");

                entity.HasOne(d => d.Employee).WithMany(p => p.SupplyRequestsMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SupplyReq__Emplo__6C190EBB");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.UnitId).HasName("PK__Units__44F5ECB5E080D698");

                entity.ToTable("Units", "inventory");

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
        }
    }
}





