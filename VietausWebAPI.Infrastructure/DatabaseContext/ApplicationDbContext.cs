
using VietausWebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Domain.Entities;
using VietausWebAPI.Core.Domain.Entities;

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
        public virtual DbSet<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; }

        public virtual DbSet<ApprovalLevelsCommonDatum> ApprovalLevelsCommonData { get; set; }

        public virtual DbSet<AssignTask> AssignTasks { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<EmployeesCommonDatum> EmployeesCommonData { get; set; }

        public virtual DbSet<EndOfShiftReport> EndOfShiftReports { get; set; }

        public virtual DbSet<EventHistoryQlsx> EventHistoryQlsxes { get; set; }

        public virtual DbSet<GroupsCommonDatum> GroupsCommonData { get; set; }

        public virtual DbSet<HandOverHistory> HandOverHistories { get; set; }

        public virtual DbSet<HistoryForProductionPlanPlpu> HistoryForProductionPlanPlpus { get; set; }

        public virtual DbSet<IncidentMaterial> IncidentMaterials { get; set; }

        public virtual DbSet<IncidentReport> IncidentReports { get; set; }

        public virtual DbSet<InfoBatForPlc> InfoBatForPlcs { get; set; }

        public virtual DbSet<InfoProForPlc> InfoProForPlcs { get; set; }

        public virtual DbSet<InformationShift> InformationShifts { get; set; }

        public virtual DbSet<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; }

        public virtual DbSet<ListProducedForQc> ListProducedForQcs { get; set; }

        public virtual DbSet<MachineHistoryMd> MachineHistoryMds { get; set; }

        public virtual DbSet<MachinesCommonDatum> MachinesCommonData { get; set; }

        public virtual DbSet<MaintenanceHistory> MaintenanceHistories { get; set; }

        public virtual DbSet<MaintenanceMaterial> MaintenanceMaterials { get; set; }

        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<MaterialGroup> MaterialGroups { get; set; }

        public virtual DbSet<MaterialGroupsMaterialDatum> MaterialGroupsMaterialData { get; set; }

        public virtual DbSet<MaterialsMaterialDatum> MaterialsMaterialData { get; set; }

        public virtual DbSet<MaterialsSuppliersMaterialDatum> MaterialsSuppliersMaterialData { get; set; }

        public virtual DbSet<NewMakingHistory> NewMakingHistories { get; set; }

        public virtual DbSet<NewMakingMaterial> NewMakingMaterials { get; set; }

        public virtual DbSet<NonCatalogHistory> NonCatalogHistories { get; set; }

        public virtual DbSet<OperationHistoryMd> OperationHistoryMds { get; set; }

        public virtual DbSet<OperatorForRecordToPlc> OperatorForRecordToPlcs { get; set; }

        public virtual DbSet<OtherMaintenanceHistory> OtherMaintenanceHistories { get; set; }

        public virtual DbSet<OtherMaintenanceMaterial> OtherMaintenanceMaterials { get; set; }

        public virtual DbSet<ParameterStandardMd> ParameterStandardMds { get; set; }

        public virtual DbSet<PartsCommonDatum> PartsCommonData { get; set; }

        public virtual DbSet<PassDetailHistoryLabqc> PassDetailHistoryLabqcs { get; set; }

        public virtual DbSet<PassDetailValuableLabqc> PassDetailValuableLabqcs { get; set; }

        public virtual DbSet<PriceHistoryMaterialDatum> PriceHistoryMaterialData { get; set; }

        public virtual DbSet<ProListToQcLabqc> ProListToQcLabqcs { get; set; }

        public virtual DbSet<ProductCodeHistoryMd> ProductCodeHistoryMds { get; set; }

        public virtual DbSet<ProductionOrderSummary> ProductionOrderSummaries { get; set; }

        public virtual DbSet<ProductionPlanChageHistoryQlsx> ProductionPlanChageHistoryQlsxes { get; set; }

        public virtual DbSet<ProductionPlanHistoryPlpu> ProductionPlanHistoryPlpus { get; set; }

        public virtual DbSet<ProductionPlanPlpu> ProductionPlanPlpus { get; set; }

        public virtual DbSet<ProductionStatus> ProductionStatuses { get; set; }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public virtual DbSet<PurchaseOrderDetailsMaterialDatum> PurchaseOrderDetailsMaterialData { get; set; }

        public virtual DbSet<PurchaseOrderStatusHistoryMaterialDatum> PurchaseOrderStatusHistoryMaterialData { get; set; }

        public virtual DbSet<PurchaseOrdersMaterialDatum> PurchaseOrdersMaterialData { get; set; }

        public virtual DbSet<QcresultLabqc> QcresultLabqcs { get; set; }

        public virtual DbSet<QlsxMachineEvent> QlsxMachineEvents { get; set; }

        public virtual DbSet<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; }

        public virtual DbSet<ShiftLeaderForRecordToPlc> ShiftLeaderForRecordToPlcs { get; set; }

        public virtual DbSet<ShiftScheduleHistory> ShiftScheduleHistories { get; set; }

        public virtual DbSet<SparePartsWarehouse> SparePartsWarehouses { get; set; }

        public virtual DbSet<SparePartsWarehouseHistory> SparePartsWarehouseHistories { get; set; }

        public virtual DbSet<SupplierAddressesMaterialDatum> SupplierAddressesMaterialData { get; set; }

        public virtual DbSet<SuppliersMaterialDatum> SuppliersMaterialData { get; set; }

        public virtual DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }

        public virtual DbSet<SystemGroup> SystemGroups { get; set; }

        public virtual DbSet<TempEmployeesImport> TempEmployeesImports { get; set; }

        public virtual DbSet<TempEndOfShiftReport> TempEndOfShiftReports { get; set; }

        public virtual DbSet<UsagePurpose> UsagePurposes { get; set; }

        //49
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApprovalHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC27ACA7D4C9");

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
                    .HasConstraintName("FK_ApprovalHistory_Employee");

                entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistoryMaterialData)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ApprovalHistory_Request");
            });

            modelBuilder.Entity<ApprovalLevelsCommonDatum>(entity =>
            {
                entity.HasKey(e => e.LevelId).HasName("PK__Approval__09F03C06EF2E5D5D");

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

                entity.HasOne(d => d.Part).WithMany(p => p.EmployeesCommonData)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Department");
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
                    .HasConstraintName("FK__HandOverH__Shift__32AB8735");
            });

            modelBuilder.Entity<HistoryForProductionPlanPlpu>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("HistoryForProductionPlan_Plpu");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Color).HasMaxLength(50);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note1).HasMaxLength(255);
                entity.Property(e => e.Note2).HasMaxLength(255);
                entity.Property(e => e.Note3).HasMaxLength(255);
                entity.Property(e => e.Note4).HasMaxLength(255);
                entity.Property(e => e.PlanRecordDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.PlannedDate).HasColumnType("datetime");
                entity.Property(e => e.ProducCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
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
                    .IsUnicode(false)
                    .HasColumnName("ApprovedID");
                entity.Property(e => e.CopletionDate).HasColumnType("datetime");
                entity.Property(e => e.IncidentDate).HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(50);
                entity.Property(e => e.Receiver).HasMaxLength(50);
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                entity.HasKey(e => e.ShiftId).HasName("PK__Informat__C0A838E198564FB8");

                entity.ToTable("InformationShift");

                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");
                entity.Property(e => e.ShiftName).HasMaxLength(100);
            });

            modelBuilder.Entity<InventoryReceiptsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C400E809090B");

                entity.ToTable("InventoryReceipts_Material_data");

                entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
                entity.Property(e => e.MaterialGroupId).HasColumnName("MaterialGroupID");
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
                    .HasConstraintName("FK_InventoryReceipts_MaterialGroupID");

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
                entity.Property(e => e.ApprovedId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ApprovedID");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(50);
                entity.Property(e => e.Receiver).HasMaxLength(50);
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Machine).WithMany(p => p.MaintenanceHistories)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_MaintenanceHistory_Machine");
            });

            modelBuilder.Entity<MaintenanceMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Maintena__3214EC27863646F9");

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
                entity.HasKey(e => e.MaterialId).HasName("PK__Material__C5061317643EBA3E");

                entity.Property(e => e.MaterialId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialID");
                entity.Property(e => e.MaterialName).HasMaxLength(100);
            });

            modelBuilder.Entity<MaterialGroup>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD59BE3B4D");

                entity.Property(e => e.MaterialGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialGroupName).HasMaxLength(100);
            });

            modelBuilder.Entity<MaterialGroupsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__E20265FD21A1D63C");

                entity.ToTable("MaterialGroups_Material_data");

                entity.Property(e => e.MaterialGroupId)
                    .HasDefaultValueSql("(newid())")
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
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("materialId");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EmployeeID");
                entity.Property(e => e.ExternalId)
                    .HasMaxLength(50)
                    .HasColumnName("externalId");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.Employee).WithMany(p => p.MaterialsMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Materials__Emplo__6DCC4D03");

                entity.HasOne(d => d.MaterialGroup).WithMany(p => p.MaterialsMaterialData)
                    .HasForeignKey(d => d.MaterialGroupId)
                    .HasConstraintName("FK__Materials__Mater__6CD828CA");
            });

            modelBuilder.Entity<MaterialsSuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.MaterialsSuppliersId).HasName("PK__Material__4F13EDBB9241ABE2");

                entity.ToTable("MaterialsSuppliers_material_data");

                entity.Property(e => e.MaterialsSuppliersId)
                    .HasDefaultValueSql("(newid())")
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
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.HasOne(d => d.Material).WithMany(p => p.MaterialsSuppliersMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__Materials__mater__7FEAFD3E");

                entity.HasOne(d => d.PriceHistory).WithMany(p => p.MaterialsSuppliersMaterialData)
                    .HasForeignKey(d => d.PriceHistoryId)
                    .HasConstraintName("FK__Materials__price__01D345B0");

                entity.HasOne(d => d.Supplier).WithMany(p => p.MaterialsSuppliersMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Materials__suppl__00DF2177");
            });

            modelBuilder.Entity<NewMakingHistory>(entity =>
            {
                entity.HasKey(e => e.NewMakingId).HasName("PK__NewMakin__24B8A803A3DA13A6");

                entity.ToTable("NewMakingHistory");

                entity.Property(e => e.NewMakingId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NewMakingID");
                entity.Property(e => e.ApprovedId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ApprovedID");
                entity.Property(e => e.CompletionDate).HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.NewMaintenanceDate).HasColumnType("datetime");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PerformedBy).HasMaxLength(50);
                entity.Property(e => e.Receiver).HasMaxLength(50);
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
                entity.HasKey(e => e.HistoryId).HasName("PK__NonCatal__4D7B4ADD6983B45C");

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
                entity.Property(e => e.ApprovedId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ApprovedID");
                entity.Property(e => e.CompletionDate).HasColumnType("datetime");
                entity.Property(e => e.MachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.OtherMaintenanceDate).HasColumnType("datetime");
                entity.Property(e => e.PerformedBy).HasMaxLength(50);
                entity.Property(e => e.Receiver).HasMaxLength(50);
                entity.Property(e => e.RelatedDocument)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Machine).WithMany(p => p.OtherMaintenanceHistories)
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_OtherMaintenanceHistory_Machine");
            });

            modelBuilder.Entity<OtherMaintenanceMaterial>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__OtherMai__3214EC2719A8E49D");

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
                entity.HasKey(e => e.Id).HasName("PK__Paramete__3214EC07CFF3A9B9");

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
                entity.Property(e => e.Qcround).HasColumnName("QCRound");
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

            modelBuilder.Entity<PassDetailValuableLabqc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("PassDetailValuable_LABQC");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);
                entity.Property(e => e.MachineId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.Qcdate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("QCDate");
                entity.Property(e => e.Qcround).HasColumnName("QCRound");
            });

            modelBuilder.Entity<PriceHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.PriceHistoryId).HasName("PK__PriceHis__77D1486CEBD19136");

                entity.ToTable("PriceHistory_material_data");

                entity.Property(e => e.PriceHistoryId)
                    .HasDefaultValueSql("(newid())")
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
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("updatedDate");

                entity.HasOne(d => d.Material).WithMany(p => p.PriceHistoryMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PriceHist__mater__793DFFAF");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PriceHistoryMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PriceHist__suppl__7A3223E8");

                entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PriceHistoryMaterialData)
                    .HasForeignKey(d => d.UpdatedBy)
                    .HasConstraintName("FK__PriceHist__updat__7B264821");
            });

            modelBuilder.Entity<ProListToQcLabqc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ProListToQC_LABQC");

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
                entity.Property(e => e.Qcround).HasColumnName("QCRound");
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
                entity.HasKey(e => e.BatchNo).HasName("PK__Producti__5D56EB97723AAC93");

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

            modelBuilder.Entity<ProductionPlanChageHistoryQlsx>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("ProductionPlanChageHistory_QLSX");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ChageDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.FromMachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("FromMachineID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.ProductionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ShiftId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ShiftID");
                entity.Property(e => e.ToMachineId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("ToMachineID");
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

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
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
                entity.Property(e => e.RequestDate)
                    .HasDefaultValue(new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Machine).WithMany()
                    .HasForeignKey(d => d.MachineId)
                    .HasConstraintName("FK_ProductionPlan_MachineID");
            });

            modelBuilder.Entity<ProductionStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC270A51BB82");

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
                entity.Property(e => e.RequestDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2F4524BBDA2");

                entity.Property(e => e.Poid)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("POID");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.Pocode)
                    .HasMaxLength(50)
                    .HasColumnName("POCode");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__PurchaseO__Emplo__0697FACD");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__PurchaseO__Suppl__05A3D694");
            });

            modelBuilder.Entity<PurchaseOrderDetailsMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.PodetailId).HasName("PK__Purchase__4EB47B3E41006D70");

                entity.ToTable("PurchaseOrderDetails_material_data");

                entity.Property(e => e.PodetailId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("PODetailId");
                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");
                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");
                entity.Property(e => e.Poid).HasColumnName("POID");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Material).WithMany(p => p.PurchaseOrderDetailsMaterialData)
                    .HasForeignKey(d => d.MaterialId)
                    .HasConstraintName("FK__PurchaseO__Mater__10216507");

                entity.HasOne(d => d.Po).WithMany(p => p.PurchaseOrderDetailsMaterialData)
                    .HasForeignKey(d => d.Poid)
                    .HasConstraintName("FK__PurchaseOr__POID__0F2D40CE");
            });

            modelBuilder.Entity<PurchaseOrderStatusHistoryMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.StatusHistoryId).HasName("PK__Purchase__DB973491D5943BE4");

                entity.ToTable("PurchaseOrderStatusHistory_material_data");

                entity.Property(e => e.StatusHistoryId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.ChangedDate).HasColumnType("datetime");
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
                    .HasConstraintName("FK__PurchaseOr__POID__13F1F5EB");
            });

            modelBuilder.Entity<PurchaseOrdersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2F422925495");

                entity.ToTable("PurchaseOrders_material_data");

                entity.Property(e => e.Poid)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("POID");
                entity.Property(e => e.EmployeeId)
                    .HasMaxLength(16)
                    .IsUnicode(false);
                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");
                entity.Property(e => e.OrderDate).HasColumnType("datetime");
                entity.Property(e => e.Pocode)
                    .HasMaxLength(50)
                    .HasColumnName("POCode");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.HasOne(d => d.Employee).WithMany(p => p.PurchaseOrdersMaterialData)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__PurchaseO__Emplo__0B5CAFEA");

                entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrdersMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__PurchaseO__Suppl__0A688BB1");
            });

            modelBuilder.Entity<QcresultLabqc>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("QCResult_LABQC");

                entity.Property(e => e.Qcdate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("QCDate");
                entity.Property(e => e.QcpassId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("QCPassID");
                entity.Property(e => e.StatusQc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("StatusQC");
            });

            modelBuilder.Entity<QlsxMachineEvent>(entity =>
            {
                entity.HasKey(e => e.EventId).HasName("PK__QlsxMach__7944C870293BFFCB");

                entity.ToTable("QlsxMachineEvent");

                entity.Property(e => e.EventId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("EventID");
                entity.Property(e => e.EventName).HasMaxLength(255);
            });

            modelBuilder.Entity<RequestDetailMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314D8E645DCC");

                entity.ToTable("RequestDetail_Material_data");

                entity.Property(e => e.DetailId).HasColumnName("DetailID");
                entity.Property(e => e.MaterialGroupId).HasColumnName("MaterialGroupID");
                entity.Property(e => e.MaterialName).HasMaxLength(50);
                entity.Property(e => e.RequestId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("RequestID");
                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.MaterialGroup).WithMany(p => p.RequestDetailMaterialData)
                    .HasForeignKey(d => d.MaterialGroupId)
                    .HasConstraintName("FK_RequestDetail_MaterialGroupID");

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
                    .HasConstraintName("FK__ShiftSche__Shift__46B27FE2");
            });

            modelBuilder.Entity<SparePartsWarehouse>(entity =>
            {
                entity.HasKey(e => e.SparePartId).HasName("PK__SparePar__F5BA41F2CDBCA8E5");

                entity.ToTable("SparePartsWarehouse");

                entity.Property(e => e.SparePartId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SparePartID");
                entity.Property(e => e.LocationMaterial).HasMaxLength(100);
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
                entity.HasKey(e => e.HistoryId).HasName("PK__SparePar__4D7B4ADD6B0DCDCC");

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

            modelBuilder.Entity<SupplierAddressesMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.AddressId).HasName("PK__Supplier__26A111AD8ED3633F");

                entity.ToTable("SupplierAddresses_material_data");

                entity.Property(e => e.AddressId)
                    .HasDefaultValueSql("(newid())")
                    .HasColumnName("addressId");
                entity.Property(e => e.AddressLine)
                    .HasMaxLength(255)
                    .HasColumnName("addressLine");
                entity.Property(e => e.SupplierId).HasColumnName("supplierId");

                entity.HasOne(d => d.Supplier).WithMany(p => p.SupplierAddressesMaterialData)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__SupplierA__suppl__74794A92");
            });

            modelBuilder.Entity<SuppliersMaterialDatum>(entity =>
            {
                entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__DB8E62ED6236CA8E");

                entity.ToTable("Suppliers_material_data");

                entity.Property(e => e.SupplierId)
                    .HasDefaultValueSql("(newid())")
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
                entity.HasKey(e => e.SystemGroupId).HasName("PK__SystemGr__485ED3E3654E0D8D");

                entity.Property(e => e.SystemGroupId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SystemGroupID");
                entity.Property(e => e.SystemGroupName).HasMaxLength(100);
            });

            modelBuilder.Entity<TempEmployeesImport>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToTable("Temp_Employees_Import");

                entity.Property(e => e.Address).HasMaxLength(255);
                entity.Property(e => e.DateHired).HasMaxLength(50);
                entity.Property(e => e.DateOfBirth).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
                entity.Property(e => e.EndDate).HasMaxLength(50);
                entity.Property(e => e.FullName).HasMaxLength(255);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.Identifier).HasMaxLength(50);
                entity.Property(e => e.LevelId)
                    .HasMaxLength(20)
                    .HasColumnName("LevelID");
                entity.Property(e => e.PartId)
                    .HasMaxLength(20)
                    .HasColumnName("PartID");
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(10);
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
                entity.HasKey(e => e.PurposeId).HasName("PK__UsagePur__79E6A1B4AAAD3DE9");

                entity.Property(e => e.PurposeId).HasColumnName("PurposeID");
                entity.Property(e => e.Note).HasMaxLength(255);
                entity.Property(e => e.PurposeName).HasMaxLength(100);
            });
        }
    }
}
