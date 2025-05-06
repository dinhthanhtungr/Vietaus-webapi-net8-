using VietausWebAPI.Core.Models;
using VietausWebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Entities;
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

        public DbSet<AssignTask> AssignTasks { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<EmployeesCommonDatum> EmployeesCommonData { get; set; }

        public DbSet<EndOfShiftReport> EndOfShiftReports { get; set; }

        public DbSet<EventHistoryQlsx> EventHistoryQlsxes { get; set; }

        public DbSet<GroupsCommonDatum> GroupsCommonData { get; set; }

        public DbSet<HandOverHistory> HandOverHistories { get; set; }

        public DbSet<IncidentMaterial> IncidentMaterials { get; set; }

        public DbSet<IncidentReport> IncidentReports { get; set; }

        public DbSet<InfoBatForPlc> InfoBatForPlcs { get; set; }

        public DbSet<InfoProForPlc> InfoProForPlcs { get; set; }

        public DbSet<InformationShift> InformationShifts { get; set; }

        public DbSet<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; }

        public DbSet<ListProducedForQc> ListProducedForQcs { get; set; }

        public DbSet<MachineHistoryMd> MachineHistoryMds { get; set; }

        public DbSet<MachinesCommonDatum> MachinesCommonData { get; set; }

        public DbSet<MaintenanceHistory> MaintenanceHistories { get; set; }

        public DbSet<MaintenanceMaterial> MaintenanceMaterials { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<MaterialGroup> MaterialGroups { get; set; }

        public DbSet<MaterialSuppliersMaterialDatum> MaterialSuppliersMaterialData { get; set; }

        public DbSet<MaterialsMaterialGroupsDatum> MaterialsMaterialGroupsData { get; set; }

        public DbSet<NewMakingHistory> NewMakingHistories { get; set; }

        public DbSet<NewMakingMaterial> NewMakingMaterials { get; set; }

        public DbSet<NonCatalogHistory> NonCatalogHistories { get; set; }

        public DbSet<OperationHistoryMd> OperationHistoryMds { get; set; }

        public DbSet<OperatorForRecordToPlc> OperatorForRecordToPlcs { get; set; }

        public DbSet<OtherMaintenanceHistory> OtherMaintenanceHistories { get; set; }

        public DbSet<OtherMaintenanceMaterial> OtherMaintenanceMaterials { get; set; }

        public DbSet<ParameterStandardMd> ParameterStandardMds { get; set; }

        public DbSet<PartsCommonDatum> PartsCommonData { get; set; }

        public DbSet<PassDetailHistoryLabqc> PassDetailHistoryLabqcs { get; set; }

        public DbSet<ProductCodeHistoryMd> ProductCodeHistoryMds { get; set; }

        public DbSet<ProductionOrderSummary> ProductionOrderSummaries { get; set; }

        public DbSet<ProductionPlanHistoryPlpu> ProductionPlanHistoryPlpus { get; set; }

        public DbSet<ProductionPlanPlpu> ProductionPlanPlpus { get; set; }

        public DbSet<ProductionStatus> ProductionStatuses { get; set; }

        public DbSet<QlsxMachineEvent> QlsxMachineEvents { get; set; }

        public DbSet<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; }

        public DbSet<ShiftLeaderForRecordToPlc> ShiftLeaderForRecordToPlcs { get; set; }

        public DbSet<ShiftScheduleHistory> ShiftScheduleHistories { get; set; }

        public DbSet<SparePartsWarehouse> SparePartsWarehouses { get; set; }

        public DbSet<SparePartsWarehouseHistory> SparePartsWarehouseHistories { get; set; }

        public DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }

        public DbSet<SystemGroup> SystemGroups { get; set; }

        public DbSet<TempEndOfShiftReport> TempEndOfShiftReports { get; set; }

        public DbSet<UsagePurpose> UsagePurposes { get; set; }

        //49
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApprovalHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC27427D13D4");

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
            });

            modelBuilder.Entity<ApprovalLevelsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__Approval__09F03C0601A6AFB1");

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

            modelBuilder.Entity<AssignTask>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__AssignTa__3214EC2724CFB7BB");

                entity.ToTable("AssignTask");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Operator).HasMaxLength(100);
                entity.Property(e => e.RecordDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");

                entity.HasOne(d => d.Shift).WithMany(p => p.AssignTasks)
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK__AssignTas__Shift__7E02B4CC");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__Level__693CA210");

                entity.HasOne(d => d.Part).WithMany(p => p.EmployeesCommonData)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__PartI__68487DD7");
            });

            modelBuilder.Entity<EndOfShiftReport>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("EndOfShiftReport");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.BtpKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("BTP_kg");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Operator).HasMaxLength(100);
                entity.Property(e => e.ProducingErrKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("Producing_Err_kg");
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");
                entity.Property(e => e.TpKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("TP_kg");
                entity.Property(e => e.TpWaitingForQcKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("TP_WaitingForQC_kg");
                entity.Property(e => e.UnfinishedProductKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("UnfinishedProduct_kg");
            });

            modelBuilder.Entity<EventHistoryQlsx>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC279CE737FF");

                entity.ToTable("EventHistory_Qlsx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.EventDate).HasColumnType("datetime");
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

            modelBuilder.Entity<HandOverHistory>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("HandOverHistory");

                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.RecordTime)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");

                entity.HasOne(d => d.Shift).WithMany()
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK__HandOverH__Shift__2A164134");
            });

            modelBuilder.Entity<IncidentMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Incident__3214EC27770B23BA");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.IncidentId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IncidentID");
                entity.Property(e => e.MaterialName).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(255);

                entity.HasOne(d => d.Incident).WithMany(p => p.IncidentMaterials)
                    .HasForeignKey(d => d.IncidentId)
                    .HasConstraintName("FK_IncidentMaterials_Incident");
            });

            modelBuilder.Entity<IncidentReport>(entity =>
            {
                entity.HasKey(e => e.IncidentId).HasName("PK__Incident__3D805392DC455CAC");

                entity.Property(e => e.IncidentId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IncidentID");
                entity.Property(e => e.ApprovedId)
                    .HasMaxLength(50)
                    .HasColumnName("ApprovedID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.EndTime).HasColumnType("datetime");
                entity.Property(e => e.IncidentDate).HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.StartTime).HasColumnType("datetime");
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.Machine).WithMany(p => p.IncidentReports)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_IncidentReports_Machine");
            });

            modelBuilder.Entity<InfoBatForPlc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("InfoBatFor_PLC");

                entity.Property(e => e.D0).HasColumnName("d0");
                entity.Property(e => e.D1).HasColumnName("d1");
                entity.Property(e => e.D2).HasColumnName("d2");
                entity.Property(e => e.D3).HasColumnName("d3");
                entity.Property(e => e.D4).HasColumnName("d4");
                entity.Property(e => e.D5).HasColumnName("d5");
                entity.Property(e => e.D6).HasColumnName("d6");
                entity.Property(e => e.D7).HasColumnName("d7");
                entity.Property(e => e.D8).HasColumnName("d8");
                entity.Property(e => e.D9).HasColumnName("d9");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
            });

            modelBuilder.Entity<InfoProForPlc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("InfoProFor_PLC");

                entity.Property(e => e.D0).HasColumnName("d0");
                entity.Property(e => e.D1).HasColumnName("d1");
                entity.Property(e => e.D2).HasColumnName("d2");
                entity.Property(e => e.D3).HasColumnName("d3");
                entity.Property(e => e.D4).HasColumnName("d4");
                entity.Property(e => e.D5).HasColumnName("d5");
                entity.Property(e => e.D6).HasColumnName("d6");
                entity.Property(e => e.D7).HasColumnName("d7");
                entity.Property(e => e.D8).HasColumnName("d8");
                entity.Property(e => e.D9).HasColumnName("d9");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
            });

            modelBuilder.Entity<InformationShift>(entity =>
            {
                entity.HasKey(e => e.ShiftId).HasName("PK__Informat__C0A838E17FD7FFB8");

                entity.ToTable("InformationShift");

                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");
                entity.Property(e => e.ShiftName).HasMaxLength(100);
            });

            modelBuilder.Entity<InventoryReceiptsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C400D77F9BDE");

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
                entity.Property(e => e.SupplierId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("SupplierID");
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
            });

            modelBuilder.Entity<ListProducedForQc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ListProducedForQC");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.QcpassId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("QCPassID");
                entity.Property(e => e.StartDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
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

            modelBuilder.Entity<MaintenanceHistory>(entity =>
            {
                entity.HasKey(e => e.MaintenanceId).HasName("PK__Maintena__E60542B55AF9F288");

                entity.ToTable("MaintenanceHistory");

                entity.Property(e => e.MaintenanceId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaintenanceID");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(100);
                entity.Property(e => e.Receiver).HasMaxLength(255);
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Machine).WithMany(p => p.MaintenanceHistories)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_MaintenanceHistory_Machine");
            });

            modelBuilder.Entity<MaintenanceMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Maintena__3214EC27892EF135");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.MaintenanceId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaintenanceID");
                entity.Property(e => e.MaterialName).HasMaxLength(255);
                entity.Property(e => e.Note).HasMaxLength(255);

                entity.HasOne(d => d.Maintenance).WithMany(p => p.MaintenanceMaterials)
                    .HasForeignKey(d => d.MaintenanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MaintenanceMaterials_Maintenance");
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.MaterialId).HasName("PK__Material__C5061317706EA7D7");

                entity.Property(e => e.MaterialId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialID");
                entity.Property(e => e.MaterialName).HasMaxLength(100);
            });

            modelBuilder.Entity<MaterialGroup>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD50195A54");

                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialGroupName).HasMaxLength(100);
            });

            modelBuilder.Entity<MaterialSuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Material__4BE6669436FEAB6C");

                entity.ToTable("MaterialSuppliers_Material_data");

                entity.Property(e => e.SupplierId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("SupplierID");
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.SupplierName).HasMaxLength(100);
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
            });

            modelBuilder.Entity<NewMakingHistory>(entity =>
            {
                entity.HasKey(e => e.NewMakingId).HasName("PK__NewMakin__24B8A803A3DA13A6");

                entity.ToTable("NewMakingHistory");

                entity.Property(e => e.NewMakingId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NewMakingID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.NewMaintenanceDate).HasColumnType("datetime");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(100);
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Machine).WithMany(p => p.NewMakingHistories)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_NewMakingHistory_Machine");
            });

            modelBuilder.Entity<NewMakingMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__NewMakin__3214EC27BFB43E24");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.MaterialName).HasMaxLength(50);
                entity.Property(e => e.NewMakingId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NewMakingID");
                entity.Property(e => e.Note).HasMaxLength(255);

                entity.HasOne(d => d.NewMaking).WithMany(p => p.NewMakingMaterials)
                    .HasForeignKey(d => d.NewMakingId)
                    .HasConstraintName("FK_NewMakingMaterials_NewMaking");
            });

            modelBuilder.Entity<NonCatalogHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId).HasName("PK__NonCatal__4D7B4ADD322138F1");

                entity.ToTable("NonCatalogHistory");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
                entity.Property(e => e.MaterialName).HasMaxLength(255);
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(100);
                entity.Property(e => e.PurposeId).HasColumnName("PurposeID");
                entity.Property(e => e.RelatedDocument).HasMaxLength(100);
                entity.Property(e => e.TransactionDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.TransactionType).HasMaxLength(10);
                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.Purpose).WithMany(p => p.NonCatalogHistories)
                    .HasForeignKey(d => d.PurposeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NonCatalogHistory_Purpose");
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
                    .HasConstraintName("FK_Operator_Common");
            });

            modelBuilder.Entity<OperatorForRecordToPlc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("OperatorForRecordTo_PLC");

                entity.Property(e => e.D0).HasColumnName("d0");
                entity.Property(e => e.D1).HasColumnName("d1");
                entity.Property(e => e.D10).HasColumnName("d10");
                entity.Property(e => e.D11).HasColumnName("d11");
                entity.Property(e => e.D2).HasColumnName("d2");
                entity.Property(e => e.D3).HasColumnName("d3");
                entity.Property(e => e.D4).HasColumnName("d4");
                entity.Property(e => e.D5).HasColumnName("d5");
                entity.Property(e => e.D6).HasColumnName("d6");
                entity.Property(e => e.D7).HasColumnName("d7");
                entity.Property(e => e.D8).HasColumnName("d8");
                entity.Property(e => e.D9).HasColumnName("d9");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("machineID");
            });

            modelBuilder.Entity<OtherMaintenanceHistory>(entity =>
            {
                entity.HasKey(e => e.OtherMaintenanceId).HasName("PK__OtherMai__AE216D9F9B949274");

                entity.ToTable("OtherMaintenanceHistory");

                entity.Property(e => e.OtherMaintenanceId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OtherMaintenanceID");
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.NextOtherMaintenanceDate).HasColumnType("datetime");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.OtherMaintenanceDate).HasColumnType("datetime");
                entity.Property(e => e.PerformedBy).HasMaxLength(100);
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Machine).WithMany(p => p.OtherMaintenanceHistories)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_OtherMaintenanceHistory_Machine");
            });

            modelBuilder.Entity<OtherMaintenanceMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__OtherMai__3214EC27797D4C8D");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.MaterialName).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(255);

                entity.HasOne(d => d.Machine).WithMany(p => p.OtherMaintenanceMaterials)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_OtherMaintenanceMaterials_Machine");
            });

            modelBuilder.Entity<ParameterStandardMd>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Paramete__3214EC07CB1C8BFE");

                entity.ToTable("ParameterStandard_MD");

                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.FeederSpeedStandard).HasColumnName("FeederSpeed_Standard");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ScrewCurrentStandard).HasColumnName("ScrewCurrent_Standard");
                entity.Property(e => e.ScrewSpeedStandard).HasColumnName("ScrewSpeed_Standard");
                entity.Property(e => e.Set10Standard).HasColumnName("Set10_Standard");
                entity.Property(e => e.Set11Standard).HasColumnName("Set11_Standard");
                entity.Property(e => e.Set12Standard).HasColumnName("Set12_Standard");
                entity.Property(e => e.Set13Standard).HasColumnName("Set13_Standard");
                entity.Property(e => e.Set1Standard).HasColumnName("Set1_Standard");
                entity.Property(e => e.Set2Standard).HasColumnName("Set2_Standard");
                entity.Property(e => e.Set3Standard).HasColumnName("Set3_Standard");
                entity.Property(e => e.Set4Standard).HasColumnName("Set4_Standard");
                entity.Property(e => e.Set5Standard).HasColumnName("Set5_Standard");
                entity.Property(e => e.Set6Standard).HasColumnName("Set6_Standard");
                entity.Property(e => e.Set7Standard).HasColumnName("Set7_Standard");
                entity.Property(e => e.Set8Standard).HasColumnName("Set8_Standard");
                entity.Property(e => e.Set9Standard).HasColumnName("Set9_Standard");

                entity.HasOne(d => d.Employee).WithMany(p => p.ParameterStandardMds)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineAssignments_Employees");

                entity.HasOne(d => d.Machine).WithMany(p => p.ParameterStandardMds)
                    .HasForeignKey(d => d.MachineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MachineAssignments_Machines");
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

            modelBuilder.Entity<PassDetailHistoryLabqc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("PassDetailHistory_LABQC");

                entity.Property(e => e.Appearance).HasMaxLength(100);
                entity.Property(e => e.BatchNo)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.BlackSpot).HasMaxLength(100);
                entity.Property(e => e.ChipPressing).HasMaxLength(100);
                entity.Property(e => e.Dispersion).HasMaxLength(100);
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.InspectionNo).HasMaxLength(50);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Physical).HasMaxLength(100);
                entity.Property(e => e.Qcdate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("QCDate");
                entity.Property(e => e.QcpassId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("QCPassID");
                entity.Property(e => e.SizeMoisture)
                    .HasMaxLength(100)
                    .HasColumnName("Size_moisture");
                entity.Property(e => e.StatusQc)
                    .HasMaxLength(20)
                    .HasColumnName("StatusQC");
                entity.Property(e => e.TempSmell)
                    .HasMaxLength(100)
                    .HasColumnName("Temp_smell");
            });

            modelBuilder.Entity<ProductCodeHistoryMd>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ProductCodeHistory_MD");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.EndTime).HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.StartTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductionOrderSummary>(entity =>
            {
                entity.HasKey(e => e.BatchNo).HasName("PK__Producti__5D56EB97F590F1F2");

                entity.ToTable("ProductionOrderSummary");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.BtpKg).HasColumnName("BTP_kg");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.TpKg).HasColumnName("TP_kg");
                entity.Property(e => e.TperrKg).HasColumnName("TPErr_kg");
            });

            modelBuilder.Entity<ProductionPlanHistoryPlpu>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ProductionPlanHistory_Plpu");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.Color)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note1).HasMaxLength(255);
                entity.Property(e => e.Note2).HasMaxLength(255);
                entity.Property(e => e.Note3).HasMaxLength(255);
                entity.Property(e => e.Note4).HasMaxLength(255);
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProductionPlanPlpu>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ProductionPlan_Plpu");

                entity.Property(e => e.BatchNo).HasMaxLength(255);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note1).HasMaxLength(255);
                entity.Property(e => e.Note2).HasMaxLength(255);
                entity.Property(e => e.Note3).HasMaxLength(255);
                entity.Property(e => e.Note4).HasMaxLength(255);
                entity.Property(e => e.ProducCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.RequestDate).HasDefaultValue(new DateOnly(2000, 1, 1));

                entity.HasOne(d => d.Machine).WithMany()
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_ProductionPlan_MachineID");
            });

            modelBuilder.Entity<ProductionStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC27F938111E");

                entity.ToTable("ProductionStatus");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.BatchNo).HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note1).HasMaxLength(50);
                entity.Property(e => e.Note2).HasMaxLength(50);
                entity.Property(e => e.Note3).HasMaxLength(50);
                entity.Property(e => e.Note4).HasMaxLength(50);
                entity.Property(e => e.ProductionCode).HasMaxLength(50);
            });

            modelBuilder.Entity<QlsxMachineEvent>(entity =>
            {
                entity.HasKey(e => e.EventId).HasName("PK__QlsxMach__7944C87098A5F404");

                entity.ToTable("QlsxMachineEvent");

                entity.Property(e => e.EventId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EventID");
                entity.Property(e => e.EventName).HasMaxLength(255);
            });

            modelBuilder.Entity<RequestDetailMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D298B6310");

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
                    .HasConstraintName("FK__RequestDe__Mater__3C34F16F");

                entity.HasOne(d => d.Request).WithMany(p => p.RequestDetailMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RequestDet__Unit__778AC167");
            });

            modelBuilder.Entity<ShiftLeaderForRecordToPlc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ShiftLeaderForRecordTo_PLC");

                entity.Property(e => e.D0).HasColumnName("d0");
                entity.Property(e => e.D1).HasColumnName("d1");
                entity.Property(e => e.D10).HasColumnName("d10");
                entity.Property(e => e.D11).HasColumnName("d11");
                entity.Property(e => e.D2).HasColumnName("d2");
                entity.Property(e => e.D3).HasColumnName("d3");
                entity.Property(e => e.D4).HasColumnName("d4");
                entity.Property(e => e.D5).HasColumnName("d5");
                entity.Property(e => e.D6).HasColumnName("d6");
                entity.Property(e => e.D7).HasColumnName("d7");
                entity.Property(e => e.D8).HasColumnName("d8");
                entity.Property(e => e.D9).HasColumnName("d9");
            });

            modelBuilder.Entity<ShiftScheduleHistory>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ShiftScheduleHistory");

                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.ProductionShift)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");

                entity.HasOne(d => d.Shift).WithMany()
                    .HasForeignKey(d => d.ShiftId)
                    .HasConstraintName("FK__ShiftSche__Shift__3E1D39E1");
            });

            modelBuilder.Entity<SparePartsWarehouse>(entity =>
            {
                entity.HasKey(e => e.SparePartId).HasName("PK__SparePar__F5BA41F25032381C");

                entity.ToTable("SparePartsWarehouse");

                entity.Property(e => e.SparePartId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SparePartID");
                entity.Property(e => e.Location).HasMaxLength(100);
                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialID");
                entity.Property(e => e.MaterialName).HasMaxLength(255);
                entity.Property(e => e.MaterialParameter).HasMaxLength(255);
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.SystemGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SystemGroupID");
                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.MaterialGroup).WithMany(p => p.SparePartsWarehouses)
                    .HasForeignKey(d => d.MaterialGroupId)
                    .HasConstraintName("FK_SpareParts_MaterialGroup");

                entity.HasOne(d => d.Material).WithMany(p => p.SparePartsWarehouses)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK_SpareParts_Material");

                entity.HasOne(d => d.SystemGroup).WithMany(p => p.SparePartsWarehouses)
                    .HasForeignKey(d => d.SystemGroupId)
                    .HasConstraintName("FK_SpareParts_SystemGroup");
            });

            modelBuilder.Entity<SparePartsWarehouseHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId).HasName("PK__SparePar__4D7B4ADDFA72954B");

                entity.ToTable("SparePartsWarehouseHistory");

                entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
                entity.Property(e => e.Department).HasMaxLength(100);
                entity.Property(e => e.IsPlanned).HasDefaultValue(true);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(50)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(100);
                entity.Property(e => e.PurposeId).HasColumnName("PurposeID");
                entity.Property(e => e.RelatedDocument).HasMaxLength(100);
                entity.Property(e => e.SparePartId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SparePartID");
                entity.Property(e => e.TransactionDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.TransactionType).HasMaxLength(10);
                entity.Property(e => e.WorkOrderId)
                    .HasMaxLength(100)
                    .HasColumnName("WorkOrderID");

                entity.HasOne(d => d.Purpose).WithMany(p => p.SparePartsWarehouseHistories)
                    .HasForeignKey(d => d.PurposeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_Purpose");

                entity.HasOne(d => d.SparePart).WithMany(p => p.SparePartsWarehouseHistories)
                    .HasForeignKey(d => d.SparePartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_History_SparePart");
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
                entity.Property(e => e.RequestStatus).HasMaxLength(50);

                entity.HasOne(d => d.Employee).WithMany(p => p.SupplyRequestsMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SupplyReq__Emplo__6C190EBB");
            });

            modelBuilder.Entity<SystemGroup>(entity =>
            {
                entity.HasKey(e => e.SystemGroupId).HasName("PK__SystemGr__485ED3E3B94E23DD");

                entity.Property(e => e.SystemGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SystemGroupID");
                entity.Property(e => e.SystemGroupName).HasMaxLength(100);
            });

            modelBuilder.Entity<TempEndOfShiftReport>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("TempEndOfShiftReport");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.BtpKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("BTP_kg");
                entity.Property(e => e.Date)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Operator).HasMaxLength(100);
                entity.Property(e => e.ProducingErrKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("Producing_Err_kg");
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");
                entity.Property(e => e.TpKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("TP_kg");
                entity.Property(e => e.TpWaitingForQcKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("TP_WaitingForQC_kg");
                entity.Property(e => e.UnfinishedProductKg)
                    .HasColumnType("decimal(10, 1)")
                    .HasColumnName("UnfinishedProduct_kg");
            });

            modelBuilder.Entity<UsagePurpose>(entity =>
            {
                entity.HasKey(e => e.PurposeId).HasName("PK__UsagePur__79E6A1B40E992E4C");

                entity.Property(e => e.PurposeId).HasColumnName("PurposeID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PurposeName).HasMaxLength(100);
            });
        }
    }
}
