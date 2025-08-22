
using VietausWebAPI.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using QuestPDF.Infrastructure;
using System.Net;
//using System.Text.RegularExpressions;
using VietausWebAPI.Core.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace VietausWebAPI.WebAPI.DatabaseContext
{
    // Scaffold-DbContext "Server=DESKTOP-BL5L5IM;Database=VietausDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -context ApplicationDbContext


    //namespace VietausWebAPI.Core.Domain.Entities;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
        IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

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

        public virtual DbSet<PriceHistory1> PriceHistories1 { get; set; }

        public virtual DbSet<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductChangedHistory> ProductChangedHistories { get; set; }

        public virtual DbSet<ProductInspection> ProductInspections { get; set; }

        public virtual DbSet<ProductStandard> ProductStandards { get; set; }

        public virtual DbSet<ProductStandard1> ProductStandards1 { get; set; }

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

        public virtual DbSet<SchedualMfg> SchedualMfgs { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }

        public virtual DbSet<SupplierAddress> SupplierAddresses { get; set; }

        public virtual DbSet<SupplierContact> SupplierContacts { get; set; }

        public virtual DbSet<SuppliersMaterialDatum> SuppliersMaterialData { get; set; }

        public virtual DbSet<SupplyRequest> SupplyRequests { get; set; }

        public virtual DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }

        public virtual DbSet<Unit> Units { get; set; }

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

                entity.Property(e => e.AddressId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("AddressID");
                entity.Property(e => e.AddressLine).HasMaxLength(250);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.District).HasMaxLength(100);
                entity.Property(e => e.IsPrimary).HasDefaultValue(false);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.Province).HasMaxLength(100);

                entity.HasOne(d => d.Customer).WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Customer");
            });

            modelBuilder.Entity<ApprovalHistory>(entity =>
            {
                entity.HasKey(e => e.ApprovalId).HasName("PK__Approval__328477D4B3FEB4F1");

                entity.ToTable("ApprovalHistory", "SupplyRequest");

                entity.Property(e => e.ApprovalId)
                    .ValueGeneratedNever()
                    .HasColumnName("ApprovalID");
                entity.Property(e => e.ApprovalDate).HasColumnType("timestamptz");
                entity.Property(e => e.ApprovalStatus)
                    .HasMaxLength(16)
                    .IsUnicode(false);
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

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ApprovalDate).HasColumnType("timestamptz");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
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
                    .IsUnicode(false)
                    .HasColumnName("LevelID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.LevelName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FC5D195FBDD");

                entity.ToTable("Branches", "company");

                entity.Property(e => e.BranchId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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

                entity.Property(e => e.CategoryId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Types)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company).WithMany(p => p.Categories)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categories_Company");
            });


            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CAC11C45530");

                entity.ToTable("Companies", "company");

                entity.Property(e => e.CompanyId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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

                entity.Property(e => e.ContactId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ContactID");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(20);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.HasOne(d => d.Customer).WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contacts_Customer");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D875977EBB");

                entity.ToTable("Customer", "sales");

                entity.Property(e => e.CustomerId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ApplicationName).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.CustomerGroup).HasMaxLength(100);
                entity.Property(e => e.CustomerName).HasMaxLength(200);
                entity.Property(e => e.ExternalId).HasMaxLength(100);
                entity.Property(e => e.FaxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IssueDate).HasColumnType("timestamptz");
                entity.Property(e => e.IssuedPlace).HasMaxLength(255);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Product).HasMaxLength(255);
                entity.Property(e => e.RegistrationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TaxNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .IsUnicode(false);

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

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ID");
                entity.Property(e => e.CompanyId).HasColumnName("companyId");
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamptz")
                    .HasColumnName("createdDate");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.GroupId).HasColumnName("GroupID");
                entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
                entity.Property(e => e.UpdatedDate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamptz")
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

                entity.Property(e => e.Id)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("ID");
                entity.Property(e => e.CompanyId).HasColumnName("companyId");
                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("now()")
                    .HasColumnType("timestamptz")
                    .HasColumnName("createdDate");

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

                entity.Property(e => e.EmployeeId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.DateHired).HasColumnType("timestamptz");
                entity.Property(e => e.DateOfBirth).HasColumnType("timestamptz");
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(50);
                entity.Property(e => e.Identifier)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.LevelId).HasColumnName("LevelID");
                entity.Property(e => e.PartId).HasColumnName("PartID");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Part).WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PartId)
                    .HasConstraintName("FK_Employees_Parts");
            });


            modelBuilder.Entity<EmployeesCommonDatum>(entity =>
            {
                entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF184E31508");

                entity.ToTable("Employees_Common_data");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.DateHired).HasColumnType("timestamptz");
                entity.Property(e => e.DateOfBirth).HasColumnType("timestamptz");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FullName).HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(50);
                entity.Property(e => e.Identifier)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.LevelId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LevelID");
                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("PartID");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasMaxLength(50);

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

                entity.Property(e => e.FormulaId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.SentDate).HasColumnType("timestamptz");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(16, 2)");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.VerifiedDate).HasColumnType("timestamptz");

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

                entity.HasOne(d => d.SentByNavigation).WithMany(p => p.FormulaSentByNavigations)
                    .HasForeignKey(d => d.SentBy)
                    .HasConstraintName("FK_Formulas_SentBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.FormulaUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Formulas_UpdatedBy");

                entity.HasOne(d => d.VerifiedByNavigation).WithMany(p => p.FormulaVerifiedByNavigations)
                    .HasForeignKey(d => d.VerifiedBy)
                    .HasConstraintName("FK_Formulas_VerifiedBy");
            });

            modelBuilder.Entity<FormulaMaterial>(entity =>
            {
                entity.HasKey(e => e.FormulaMaterialId).HasName("PK__FormulaM__0315C60A1F19742A");

                entity.ToTable("FormulaMaterials", "labs");

                entity.Property(e => e.FormulaMaterialId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.LotNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MaterialType)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(16, 2)");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(16, 2)");

                entity.HasOne(d => d.Formula).WithMany(p => p.FormulaMaterials)
                    .HasForeignKey(d => d.FormulaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormulaMaterials_Formula");

                entity.HasOne(d => d.Material).WithMany(p => p.FormulaMaterials)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormulaMaterials_Material");

                entity.HasOne(d => d.SelectedSupplier).WithMany(p => p.FormulaMaterials)
                    .HasForeignKey(d => d.SelectedSupplierId)
                    .HasConstraintName("FK_FormulaMaterials_Supplier");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36A3DC9A844");

                entity.ToTable("Groups", "company");

                entity.Property(e => e.GroupId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.GroupType).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.GroupName).HasMaxLength(50);
            });


            modelBuilder.Entity<InventoryReceiptsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C400857F0DFA");

                entity.ToTable("InventoryReceipts_Material_data");

                entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.ReceiptDate).HasColumnType("timestamptz");
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("RequestID");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

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

                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Factory)
                    .HasMaxLength(100)
                    .HasDefaultValue("Tam Phước");
                entity.Property(e => e.GroupId)
                    .HasMaxLength(10)
                    .HasColumnName("GroupID");
                entity.Property(e => e.GroupMachine).HasMaxLength(50);
                entity.Property(e => e.MachineName).HasMaxLength(50);
                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
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

                entity.Property(e => e.MaterialId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Barcode)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.Comment).HasMaxLength(500);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.CustomCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ImagePath)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Package).HasMaxLength(100);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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

                entity.HasOne(d => d.Unit).WithMany(p => p.Materials)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Materials_Unit");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaterialUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Materials_UpdatedBy");
            });

            modelBuilder.Entity<MaterialGroup>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD68C7B626");

                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialGroupName).HasMaxLength(100);
            });

            modelBuilder.Entity<MaterialGroupsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD37B632C7");

                entity.ToTable("MaterialGroups_Material_data");

                entity.Property(e => e.MaterialGroupId)
                    .HasDefaultValueSql("gen_random_uuid()") 
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.Detail).HasMaxLength(255);
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("externalId");
                entity.Property(e => e.MaterialGroupName).HasMaxLength(255);
            });


            modelBuilder.Entity<MaterialsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialId).HasName("PK__Material__99B653FDEEB02A00");

                entity.ToTable("Materials_material_data");

                entity.Property(e => e.MaterialId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("materialId");
                entity.Property(e => e.CreateDate).HasColumnType("timestamptz");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .HasColumnName("externalId");
                entity.Property(e => e.Unit).HasMaxLength(50);

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

                entity.Property(e => e.MaterialsSuppliersId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("Materials_SuppliersId");
                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.CurrentPrice).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.IsPreferred).HasDefaultValue(false);
                entity.Property(e => e.LastPrice).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

                entity.HasOne(d => d.Material).WithMany(p => p.MaterialsSuppliers)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialsSuppliers_Material");

                entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialsSuppliers)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaterialsSuppliers_Supplier");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.MaterialsSuppliers)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_MaterialsSuppliers_UpdatedBy");
            });

            modelBuilder.Entity<MaterialsSuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB69530C3D");

                entity.ToTable("MaterialsSuppliers_material_data");

                entity.Property(e => e.MaterialsSuppliersId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("Materials_SuppliersId");
                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .HasColumnName("currency");
                entity.Property(e => e.CurrentPrice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("currentPrice");
                entity.Property(e => e.IsPreferred)
                    .HasDefaultValue(false)
                    .HasColumnName("isPreferred");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.MinDeliveryDays).HasColumnName("minDeliveryDays");
                entity.Property(e => e.PriceHistoryId).HasColumnName("priceHistoryId");
                entity.Property(e => e.SupplierId).HasColumnName("supplierId");
                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("timestamptz")
                    .HasColumnName("updatedDate");

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

                entity.Property(e => e.MemberId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.IsAdmin).HasDefaultValue(false);
                entity.Property(e => e.IsActive).HasDefaultValue(true);

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

                entity.Property(e => e.MerchandiseOrderId).ValueGeneratedNever();
                entity.Property(e => e.CreateDate).HasColumnType("timestamptz");
                entity.Property(e => e.DeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PaymentDate).HasColumnType("timestamptz");
                entity.Property(e => e.PaymentType).HasMaxLength(50);
                entity.Property(e => e.Receiver).HasMaxLength(255);
                entity.Property(e => e.ShippingMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");
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

                entity.Property(e => e.MerchandiseOrderDetailId).ValueGeneratedNever();
                entity.Property(e => e.BagType).HasMaxLength(50);
                entity.Property(e => e.DeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.FormulaExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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

                entity.Property(e => e.MerchandiseOrderScheduleId).ValueGeneratedNever();
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.DeliveryDate).HasColumnType("timestamptz");
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
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamptz")
                    .HasColumnName("createdDate");
                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("timestamptz")
                    .HasColumnName("expiryDate");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("externalId");
                entity.Property(e => e.ProductAddRate).HasColumnName("product_addRate");
                entity.Property(e => e.ProductCustomerExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_customerExternalId");
                entity.Property(e => e.ProductExpiryType)
                    .HasMaxLength(100)
                    .HasColumnName("product_expiryType");
                entity.Property(e => e.ProductExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_externalId");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.ProductMaxTemp).HasColumnName("product_maxTemp");
                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("product_name");
                entity.Property(e => e.ProductPackage)
                    .HasMaxLength(100)
                    .HasColumnName("product_package");
                entity.Property(e => e.ProductRecycleRate).HasColumnName("product_recycleRate");
                entity.Property(e => e.ProductRohsStandard).HasColumnName("product_rohsStandard");
                entity.Property(e => e.ProductWeight).HasColumnName("product_weight");
                entity.Property(e => e.Requirement).HasColumnName("requirement");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D30F786F0A7");

                entity.ToTable("Parts", "hr");

                entity.Property(e => e.PartId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PartID");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ExternalID");
                entity.Property(e => e.PartName).HasMaxLength(255);
            });


            modelBuilder.Entity<PartsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D3012CD1E19");

                entity.ToTable("Parts_Common_data");

                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("PartID");
                entity.Property(e => e.PartName).HasMaxLength(50);
            });


            modelBuilder.Entity<PriceHistory>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__A927CACB4B3A2EAC");

                entity.ToTable("PriceHistory", "inventory");

                entity.Property(e => e.PriceHistoryId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.CreateDate).HasColumnType("timestamptz");
                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.EndDate).HasColumnType("timestamptz");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.NewPrice).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.OldPrice).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PriceHistoryUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_PriceHistory_UpdatedBy");
            });

            modelBuilder.Entity<PriceHistory1>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__A927CACB8B375541");

                entity.ToTable("PriceHistory", "labs");

                entity.Property(e => e.PriceHistoryId).ValueGeneratedNever();
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.EndDate).HasColumnType("timestamptz");
                entity.Property(e => e.Price).HasColumnType("decimal(16, 2)");
                entity.Property(e => e.StartDate).HasColumnType("timestamptz");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PriceHistory1CreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_PriceHistory_CreatedBy");

                entity.HasOne(d => d.Product).WithMany(p => p.PriceHistory1s)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_PriceHistory_Product");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PriceHistory1UpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_PriceHistory_UpdatedBy");
            });

            modelBuilder.Entity<PriceHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__77D1486CB71DF1F1");

                entity.ToTable("PriceHistory_material_data");

                entity.Property(e => e.PriceHistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("priceHistoryId");
                entity.Property(e => e.Currency)
                    .HasMaxLength(10)
                    .HasColumnName("currency");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.NewPrice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("newPrice");
                entity.Property(e => e.OldPrice)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("oldPrice");
                entity.Property(e => e.Reason)
                    .HasMaxLength(255)
                    .HasColumnName("reason");
                entity.Property(e => e.SupplierId).HasColumnName("supplierId");
                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("updatedBy");
                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("timestamptz")
                    .HasColumnName("updatedDate");

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

                entity.Property(e => e.ProductId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Additive).HasMaxLength(200);
                entity.Property(e => e.Application).HasMaxLength(100);
                entity.Property(e => e.Code).HasMaxLength(100);
                entity.Property(e => e.ColourCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ColourName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.EndUser).HasMaxLength(100);
                entity.Property(e => e.ExpiryType)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.LabComment).HasMaxLength(500);
                entity.Property(e => e.LightCondition).HasMaxLength(100);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.OtherComment).HasColumnType("text");
                entity.Property(e => e.PolymerMatchedIn).HasMaxLength(100);
                entity.Property(e => e.Procedure).HasMaxLength(100);
                entity.Property(e => e.ProductType).HasMaxLength(100);
                entity.Property(e => e.ProductUsage).HasMaxLength(100);
                entity.Property(e => e.Requirement).HasMaxLength(500);
                entity.Property(e => e.StorageCondition)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.Unit).HasMaxLength(50);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.VisualTest).HasMaxLength(100);
                entity.Property(e => e.WeatherResistance)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Products_Category");

                entity.HasOne(d => d.Company).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Products_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductCreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Products_CreatedBy");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductUpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_Products_UpdatedBy");
            });

            modelBuilder.Entity<ProductChangedHistory>(entity =>
            {
                entity.HasKey(e => e.ProductChangedHistoryId).HasName("PK__ProductC__A793B6CA9FB36DED");

                entity.ToTable("ProductChangedHistory", "labs");

                entity.Property(e => e.ChangeNote).HasMaxLength(500);
                entity.Property(e => e.ChangeType).HasMaxLength(100);
                entity.Property(e => e.ChangedDate).HasColumnType("timestamptz");
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
                entity.Property(e => e.Antistatic)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.BatchId)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.BlackDots)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ColorDeltaE)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreateDate).HasColumnType("timestamptz");
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.DefectBlackDot).HasColumnName("Defect_BlackDot");
                entity.Property(e => e.DefectDusty).HasColumnName("Defect_Dusty");
                entity.Property(e => e.DefectImpurity).HasColumnName("Defect_Impurity");
                entity.Property(e => e.DefectMoist).HasColumnName("Defect_Moist");
                entity.Property(e => e.DefectShortFiber).HasColumnName("Defect_ShortFiber");
                entity.Property(e => e.DefectWrongColor).HasColumnName("Defect_WrongColor");
                entity.Property(e => e.Density)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Elongation)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ExpiryDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FlexuralModulus)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FlexuralStrength)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Hardness)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ImpactResistance)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IsColorDeltaEpass).HasColumnName("IsColorDeltaEPass");
                entity.Property(e => e.IsMfrpass).HasColumnName("IsMFRPass");
                entity.Property(e => e.ManufacturingDate).HasColumnType("timestamptz");
                entity.Property(e => e.MeshType).HasMaxLength(100);
                entity.Property(e => e.Mfr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MFR");
                entity.Property(e => e.Moisture)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PackingSpec)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ParticleSize)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ProductCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ProductName).HasMaxLength(254);
                entity.Property(e => e.Shape)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.StorageCondition)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.TensileStrength)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Types)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductStandard>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC0715B96228");

                entity.ToTable("ProductStandard");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.BlackDots)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ColourCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("colourCode");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.CustomerExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("customerExternalId");
                entity.Property(e => e.DeltaE)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Density)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.DwellTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ElongationAtBreak)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FlexuralModulus)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.FlexuralStrength)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Hardness)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IzodImpactStrength)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.MeltIndex)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.MigrationTest)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Moisture)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Package).HasMaxLength(255);
                entity.Property(e => e.PelletSize)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ProductExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Shape).HasMaxLength(100);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TensileStrength)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<ProductStandard1>(entity =>
            {
                entity.HasKey(e => e.ProductStandardId).HasName("PK__ProductS__A405C52429CBAE28");

                entity.ToTable("ProductStandards", "labs");

                entity.Property(e => e.ProductStandardId).ValueGeneratedNever();
                entity.Property(e => e.BlackDots)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ColourCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreateDate).HasColumnType("timestamptz");
                entity.Property(e => e.CustomerExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DeltaE)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Density)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.DwellTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ElongationAtBreak)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FlexuralModulus)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.FlexuralStrength)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Hardness)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.IzodImpactStrength)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.MeltIndex)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.MigrationTest)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Moisture)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Package).HasMaxLength(255);
                entity.Property(e => e.PelletSize)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Shape).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.TensileStrength)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

                entity.HasOne(d => d.Company).WithMany(p => p.ProductStandard1s)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_ProductStandards_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductStandard1CreatedByNavigations)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_ProductStandards_CreatedBy");

                entity.HasOne(d => d.Product).WithMany(p => p.ProductStandard1s)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ProductStandards_Product");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductStandard1UpdatedByNavigations)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK_ProductStandards_UpdatedBy");
            });

            modelBuilder.Entity<ProductTest>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__ProductT__3213E83F975320C1");

                entity.ToTable("ProductTest");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.ExpiryDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("externalId");
                entity.Property(e => e.ManufacturingDate).HasColumnType("timestamptz");
                entity.Property(e => e.ProductCustomerExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_customerExternalId");
                entity.Property(e => e.ProductExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("product_externalId");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("product_name");
                entity.Property(e => e.ProductPackage)
                    .HasMaxLength(100)
                    .HasColumnName("product_package");
                entity.Property(e => e.ProductWeight).HasColumnName("product_weight");
            });


            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__036BACA44B53DA7A");

                entity.ToTable("PurchaseOrders", "inventory");

                entity.Property(e => e.PurchaseOrderId).ValueGeneratedNever();
                entity.Property(e => e.CreateDate).HasColumnType("timestamptz");
                entity.Property(e => e.DeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.OrderType).HasMaxLength(50);
                entity.Property(e => e.PackageType).HasMaxLength(50);
                entity.Property(e => e.PaymentDays).HasMaxLength(50);
                entity.Property(e => e.RequestSourceType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(16, 2)");
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");
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

                entity.Property(e => e.DeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

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

                entity.Property(e => e.PodetailId)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("PODetailId");
                entity.Property(e => e.DeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");
                entity.Property(e => e.Poid).HasColumnName("POID");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

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

                entity.Property(e => e.ChangedDate).HasColumnType("timestamptz");
                entity.Property(e => e.StatusFrom)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.StatusTo)
                    .HasMaxLength(16)
                    .IsUnicode(false);

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

                entity.Property(e => e.StatusHistoryId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.ChangedDate).HasColumnType("timestamptz");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");
                entity.Property(e => e.Poid).HasColumnName("POID");
                entity.Property(e => e.StatusFrom).HasMaxLength(50);
                entity.Property(e => e.StatusTo).HasMaxLength(50);

                entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderStatusHistoryMaterialData)
                    .HasForeignKey(d => d.Poid)
                    .HasConstraintName("FK__PurchaseOr__POID__34E8D562");
            });

            modelBuilder.Entity<PurchaseOrdersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2F4EB754C0F");

                entity.ToTable("PurchaseOrders_material_data");

                entity.Property(e => e.Poid)
                    .HasDefaultValueSql("gen_random_uuid()")
                    .HasColumnName("POID");
                entity.Property(e => e.ContactName).HasMaxLength(100);
                entity.Property(e => e.DeliveryAddress).HasMaxLength(255);
                entity.Property(e => e.DeliveryContact).HasMaxLength(255);
                entity.Property(e => e.DeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.GrandTotal).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");
                entity.Property(e => e.OrderDate).HasColumnType("timestamptz");
                entity.Property(e => e.Packaging).HasMaxLength(100);
                entity.Property(e => e.Pocode)
                    .HasMaxLength(50)
                    .HasColumnName("POCode");
                entity.Property(e => e.RequiredDocumentsEng).HasColumnName("RequiredDocuments_Eng");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Vat).HasColumnName("VAT");
                entity.Property(e => e.VendorAddress).HasMaxLength(255);
                entity.Property(e => e.VendorPhone).HasMaxLength(50);

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

                entity.Property(e => e.PurchaseOrdersScheduleId).ValueGeneratedNever();
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.DeliveryDate).HasColumnType("timestamptz");
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

                entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.BatchExternalId)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.MachineExternalId)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                modelBuilder.Entity<Qcdetail>()
                    .HasOne(d => d.Batch)
                    .WithOne(p => p.Qcdetails) // ✅ Chỉ 1 đối tượng
                    .HasForeignKey<Qcdetail>(d => d.BatchId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_QCDetail_ProductInspection");

            });



            modelBuilder.Entity<RequestDetail>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D66D34B2A");

                entity.ToTable("RequestDetail", "SupplyRequest");

                entity.Property(e => e.DetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("DetailID");
                entity.Property(e => e.MaterialId).HasColumnName("MaterialID");
                entity.Property(e => e.RequestId).HasColumnName("RequestID");
                entity.Property(e => e.RequestStatus)
                    .HasMaxLength(16)
                    .IsUnicode(false);

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

                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.MaterialId).HasColumnName("materialId");
                entity.Property(e => e.PurchasedQuantity).HasDefaultValue(0);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
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

                entity.Property(e => e.SampleRequestId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.AdditionalComment).HasMaxLength(500);
                entity.Property(e => e.Comment).HasColumnType("text");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.RequestDeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExpectedPrice).HasColumnType("decimal(18, 4)");
                entity.Property(e => e.ExpectedPriceQuoteDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Image).HasMaxLength(255);
                entity.Property(e => e.InfoType).HasMaxLength(100);
                entity.Property(e => e.OtherComment).HasMaxLength(500);
                entity.Property(e => e.Package).HasMaxLength(100);
                entity.Property(e => e.RealDeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.RealPriceQuoteDate).HasColumnType("timestamptz");
                entity.Property(e => e.RequestType).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(100);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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

            modelBuilder.Entity<SchedualMfg>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Schedual__3214EC07A98DEC4E");

                entity.ToTable("SchedualMfg");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.BagType).HasMaxLength(50);
                entity.Property(e => e.ColorCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ColorName).HasMaxLength(100);
                entity.Property(e => e.CreatedDate)
                    .HasColumnType("timestamptz")
                    .HasColumnName("createdDate");
                entity.Property(e => e.CustomerExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CustomerRequiredDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExpectedCompletionDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExpectedDeliveryDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.PlanDate).HasColumnType("timestamptz");
                entity.Property(e => e.Qcstatus)
                    .HasMaxLength(50)
                    .HasColumnName("QCStatus");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.VerifyBatches)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666B4029CD1B8");

                entity.ToTable("Suppliers", "inventory");

                entity.Property(e => e.SupplierId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.Application).HasMaxLength(100);
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId).HasMaxLength(50);
                entity.Property(e => e.Group).HasMaxLength(100);
                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.RegNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.TaxNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Company).WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Suppliers_Company");

                entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Suppliers_CreatedBy");
            });

            modelBuilder.Entity<SupplierAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__Supplier__091C2AFB0B8862E7");

                entity.ToTable("SupplierAddresses", "inventory");

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
                    .HasMaxLength(50)
                    .HasColumnName("externalId");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");
                entity.Property(e => e.RegNo)
                    .HasMaxLength(50)
                    .HasColumnName("regNo");
                entity.Property(e => e.TaxNo)
                    .HasMaxLength(50)
                    .HasColumnName("taxNo");
                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<SupplyRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("PK__SupplyRe__33A8519A7A78FE1B");

                entity.ToTable("SupplyRequests", "SupplyRequest");

                entity.Property(e => e.RequestId)
                    .ValueGeneratedNever()
                    .HasColumnName("RequestID");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("ExternalID");
                entity.Property(e => e.RequestSourceType)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.RequestStatus)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedDate).HasColumnType("timestamptz");

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

                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("RequestID");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.RequestDate).HasColumnType("timestamptz");
                entity.Property(e => e.RequestStatus).HasMaxLength(50);

                entity.HasOne(d => d.Employee).WithMany(p => p.SupplyRequestsMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SupplyReq__Emplo__6C190EBB");
            });


            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasKey(e => e.UnitId).HasName("PK__Units__44F5ECB5E080D698");

                entity.ToTable("Units", "inventory");

                entity.Property(e => e.UnitId).HasDefaultValueSql("gen_random_uuid()");
                entity.Property(e => e.CreatedDate).HasColumnType("timestamptz");
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





