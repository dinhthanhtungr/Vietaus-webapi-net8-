using VietausWebAPI.Core.Models;
using VietausWebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Entities;

namespace VietausWebAPI.WebAPI.DatabaseContext
{
    // Scaffold-DbContext "Server=DESKTOP-BL5L5IM;Database=VietausDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -context ApplicationDbContext


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid,
    IdentityUserClaim<Guid>, ApplicationUserRole, IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> // Cái này khá hay là khiu mình kế thừa
    // IdentityDbContext thì nó sẽ tự tạo ra các bản database về User cũng như về Role
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { } 
        public ApplicationDbContext() { }
        public DbSet<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; }

        public DbSet<ApprovalLevelsCommonDatum> ApprovalLevelsCommonData { get; set; }

        public DbSet<EmployeesCommonDatum> EmployeesCommonData { get; set; }

        public DbSet<EventHistoryQlsx> EventHistoryQlsxes { get; set; }

        public DbSet<GroupsCommonDatum> GroupsCommonData { get; set; }

        public DbSet<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; }

        public DbSet<MachineHistoryMd> MachineHistoryMds { get; set; }

        public DbSet<MachinesCommonDatum> MachinesCommonData { get; set; }

        public DbSet<MaterialsMaterialGroupsDatum> MaterialsMaterialGroupsData { get; set; }

        public DbSet<MaterialSuppliersMaterialDatum> MaterialSuppliersMaterialData { get; set; }

        public DbSet<OperationHistoryMd> OperationHistoryMds { get; set; }

        public DbSet<PartsCommonDatum> PartsCommonData { get; set; }

        public DbSet<ProductCodeHistoryMd> ProductCodeHistoryMds { get; set; }

        public DbSet<QlsxMachineEvent> QlsxMachineEvents { get; set; }

        public DbSet<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; }

        public DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }
        public DbSet<Cities> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cities>().HasData(new Cities { Id = Guid.Parse("E9A77912-953B-4E0F-A650-6D8FA72DAE54"), Name = "Rem" });

            modelBuilder.Entity<SupplyRequestsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.Property(e => e.RequestId)
                      .HasColumnType("NVARCHAR(50)")
                      .HasMaxLength(50);
                entity.Property(e => e.EmployeeId)
                      .HasColumnType("NVARCHAR(50)")
                      .HasMaxLength(50);
                entity.Property(e => e.RequestStatus)
                      .HasColumnType("NVARCHAR(50)")
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<ApplicationUserRole>(userRole =>
            {
                // Định nghĩa khóa chính
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                // Quan hệ với ApplicationUser
                userRole.HasOne(ur => ur.User)
                        .WithMany(u => u.UserRoles)
                        .HasForeignKey(ur => ur.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                // Quan hệ với ApplicationRole
                userRole.HasOne(ur => ur.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(ur => ur.RoleId)
                        .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApprovalHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC279BFACB25");

                entity.ToTable("ApprovalHistory_Material_data");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
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
                    .HasConstraintName("FK_EmployeeID_Common");

                entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistoryMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHistory_Request");
            });


            modelBuilder.Entity<MaterialSuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Material__4BE66694441AFF7D");

                entity.ToTable("MaterialSuppliers_Material_data");

                entity.Property(e => e.SupplierId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("SupplierID");
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.SupplierName).HasMaxLength(255);
            });

            modelBuilder.Entity<ApprovalLevelsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__Approval__09F03C069F3BF8EB");

                entity.ToTable("ApprovalLevels_Common_data");

                entity.Property(e => e.LevelId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("LevelID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.LevelName).HasMaxLength(50);
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
                entity.Property(e => e.DateHired).HasColumnType("datetime");
                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false);
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
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Level).WithMany(p => p.EmployeesCommonData)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__Level__693CA210");

                entity.HasOne(d => d.Part).WithMany(p => p.EmployeesCommonData)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__PartI__68487DD7");
            });

            modelBuilder.Entity<EventHistoryQlsx>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC27229A5B38");

                entity.ToTable("EventHistory_Qlsx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.EventId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EventID");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");

                entity.HasOne(d => d.Event).WithMany(p => p.EventHistoryQlsxes)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_Event");
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
                entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C4005E3983E1");

                entity.ToTable("InventoryReceipts_Material_data");

                entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialName).HasMaxLength(100);
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.ReceiptDate).HasColumnType("datetime");
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("RequestID");
                //entity.Property(e => e.SupplierId)
                //    .HasMaxLength(16)
                //    .IsUnicode(false)
                //    .HasColumnName("SupplierID");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Unit).HasMaxLength(50);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.MaterialGroup).WithMany(p => p.InventoryReceiptsMaterialData)
                    .HasForeignKey(d => d.MaterialGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Mater__72C60C4A");

                entity.HasOne(d => d.Request).WithMany(p => p.InventoryReceiptsMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Reque__73BA3083");

                entity.HasOne(d => d.Supplier).WithMany(p => p.InventoryReceiptsMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_Materials_Suppliers");
            });

            modelBuilder.Entity<MachineHistoryMd>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("MachineHistory_MD");

                entity.Property(e => e.EnergyTotalOfDay).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.MachineCleaningEnergyOfDay).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.MachineCleaningTimeOfDay).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.ProducingEnergyOfDay).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.ProducingTimeOfDay).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.Time).HasColumnType("datetime");
                entity.Property(e => e.WaitingEnergyOfDay).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.WaitingTimeOfDay).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Machine).WithMany()
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MachineHi__Waiti__05D8E0BE");
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
                entity.Property(e => e.GroupId)
                    .HasMaxLength(10)
                    .HasColumnName("GroupID");
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

            modelBuilder.Entity<MaterialSuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Material__4BE66694441AFF7D");

                entity.ToTable("MaterialSuppliers_Material_data");

                entity.Property(e => e.SupplierId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("SupplierID");
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Note).HasMaxLength(500);
                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.SupplierName).HasMaxLength(255);
            });

            modelBuilder.Entity<MaterialsMaterialGroupsDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__C506131752C1EB3B");

                entity.ToTable("Materials_MaterialGroups_data");

                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialGroupName).HasMaxLength(50);
                entity.Property(e => e.PartId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("PartID");

                entity.HasOne(d => d.Part).WithMany(p => p.MaterialsMaterialGroupsData)
                    .HasForeignKey(d => d.PartId)
                    .HasConstraintName("FK_Materials_PartID");
            });

            modelBuilder.Entity<OperationHistoryMd>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("OperationHistory_MD");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.Machine).WithMany()
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OperationHistory_MachinesCommon");
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

            modelBuilder.Entity<ProductCodeHistoryMd>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ProductCodeHistory_MD");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.EndTime).HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.ProducingTime).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.StartTime).HasColumnType("datetime");
                entity.Property(e => e.TotalTime).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.WaitingTime).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Machine).WithMany()
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductCodeHistory_MachinesCommon");
            });

            modelBuilder.Entity<QlsxMachineEvent>(entity =>
            {
                entity.HasKey(e => e.EventId).HasName("PK__QlsxMach__7944C870DBF57559");

                entity.ToTable("QlsxMachineEvent");

                entity.Property(e => e.EventId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EventID");
                entity.Property(e => e.EventName).HasMaxLength(255);
            });

            modelBuilder.Entity<RequestDetailMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314DBB24E9A3");

                entity.ToTable("RequestDetail_Material_data");

                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialName).HasMaxLength(50);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("RequestID");
                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.MaterialGroup).WithMany(p => p.RequestDetailMaterialData)
                    .HasForeignKey(d => d.MaterialGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RequestDe__Mater__70DDC3D8");

                entity.HasOne(d => d.Request).WithMany(p => p.RequestDetailMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RequestDe__Reque__71D1E811");
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
                entity.Property(e => e.RequestDate).HasColumnType("datetime");
                entity.Property(e => e.RequestStatus)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee).WithMany(p => p.SupplyRequestsMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SupplyReq__Emplo__6C190EBB");
            });
        }
    }
}
