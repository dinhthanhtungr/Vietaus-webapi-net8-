//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace VietausWebAPI.Core.Entities;

//public partial class ApplicationDbContext : DbContext
//{
//    public ApplicationDbContext()
//    {
//    }

//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<ApprovalHistoryMaterialDatum> ApprovalHistoryMaterialData { get; set; }

//    public virtual DbSet<ApprovalLevelsCommonDatum> ApprovalLevelsCommonData { get; set; }

//    public virtual DbSet<City> Cities { get; set; }

//    public virtual DbSet<EmployeesCommonDatum> EmployeesCommonData { get; set; }

//    public virtual DbSet<EventHistoryQlsx> EventHistoryQlsxes { get; set; }

//    public virtual DbSet<GroupsCommonDatum> GroupsCommonData { get; set; }

//    public virtual DbSet<InventoryReceiptsMaterialDatum> InventoryReceiptsMaterialData { get; set; }

//    public virtual DbSet<MachineHistoryMd> MachineHistoryMds { get; set; }

//    public virtual DbSet<MachinesCommonDatum> MachinesCommonData { get; set; }

//    public virtual DbSet<MaterialsMaterialGroupsDatum> MaterialsMaterialGroupsData { get; set; }

//    public virtual DbSet<OperationHistoryMd> OperationHistoryMds { get; set; }

//    public virtual DbSet<PartsCommonDatum> PartsCommonData { get; set; }

//    public virtual DbSet<ProductCodeHistoryMd> ProductCodeHistoryMds { get; set; }

//    public virtual DbSet<QlsxMachineEvent> QlsxMachineEvents { get; set; }

//    public virtual DbSet<RequestDetailMaterialDatum> RequestDetailMaterialData { get; set; }

//    public virtual DbSet<SupplyRequestsMaterialDatum> SupplyRequestsMaterialData { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-8O1VNPK\\SQLEXPRESS;Database=VietausDb;Trusted_Connection=True;TrustServerCertificate=True;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<ApprovalHistoryMaterialDatum>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__Approval__3214EC27AC4B986D");

//            entity.ToTable("ApprovalHistory_Material_data");

//            entity.Property(e => e.Id)
//                .HasMaxLength(10)
//                .IsUnicode(false)
//                .HasColumnName("ID");
//            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.Note).HasMaxLength(255);
//            entity.Property(e => e.RequestId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("RequestID");

//            entity.HasOne(d => d.Employee).WithMany(p => p.ApprovalHistoryMaterialData)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__ApprovalH__Emplo__778AC167");

//            entity.HasOne(d => d.Request).WithMany(p => p.ApprovalHistoryMaterialData)
//                .HasForeignKey(d => d.RequestId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__ApprovalH__Reque__76969D2E");
//        });

//        modelBuilder.Entity<ApprovalLevelsCommonDatum>(entity =>
//        {
//            entity.HasKey(e => e.LevelId).HasName("PK__Approval__09F03C06899E86AB");

//            entity.ToTable("ApprovalLevels_Common_data");

//            entity.Property(e => e.LevelId)
//                .HasMaxLength(10)
//                .IsUnicode(false)
//                .HasColumnName("LevelID");
//            entity.Property(e => e.Description).HasMaxLength(255);
//            entity.Property(e => e.LevelName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//        });

//        modelBuilder.Entity<City>(entity =>
//        {
//            entity.Property(e => e.Id).ValueGeneratedNever();
//        });

//        modelBuilder.Entity<EmployeesCommonDatum>(entity =>
//        {
//            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF184E31508");

//            entity.ToTable("Employees_Common_data");

//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.Address).HasColumnType("text");
//            entity.Property(e => e.DateHired).HasColumnType("datetime");
//            entity.Property(e => e.DateOfBirth).HasColumnType("datetime");
//            entity.Property(e => e.Email)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.FullName)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.Gender)
//                .HasMaxLength(10)
//                .IsUnicode(false);
//            entity.Property(e => e.Identifier)
//                .HasMaxLength(20)
//                .IsUnicode(false);
//            entity.Property(e => e.LevelId)
//                .HasMaxLength(10)
//                .IsUnicode(false)
//                .HasColumnName("LevelID");
//            entity.Property(e => e.PartId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("PartID");
//            entity.Property(e => e.PhoneNumber)
//                .HasMaxLength(20)
//                .IsUnicode(false);
//            entity.Property(e => e.Status)
//                .HasMaxLength(50)
//                .IsUnicode(false);

//            entity.HasOne(d => d.Level).WithMany(p => p.EmployeesCommonData)
//                .HasForeignKey(d => d.LevelId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Employees__Level__693CA210");

//            entity.HasOne(d => d.Part).WithMany(p => p.EmployeesCommonData)
//                .HasForeignKey(d => d.PartId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Employees__PartI__68487DD7");
//        });

//        modelBuilder.Entity<EventHistoryQlsx>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK__EventHis__3214EC27A12BECC3");

//            entity.ToTable("EventHistory_Qlsx");

//            entity.Property(e => e.Id).HasColumnName("ID");
//            entity.Property(e => e.EventId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("EventID");
//            entity.Property(e => e.MachineId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MachineID");

//            entity.HasOne(d => d.Event).WithMany(p => p.EventHistoryQlsxes)
//                .HasForeignKey(d => d.EventId)
//                .HasConstraintName("FK_Event");
//        });

//        modelBuilder.Entity<GroupsCommonDatum>(entity =>
//        {
//            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF30A6FD59030");

//            entity.ToTable("Groups_Common_data");

//            entity.Property(e => e.GroupId)
//                .HasMaxLength(10)
//                .HasColumnName("GroupID");
//            entity.Property(e => e.Description).HasMaxLength(255);
//            entity.Property(e => e.GroupName).HasMaxLength(50);
//        });

//        modelBuilder.Entity<InventoryReceiptsMaterialDatum>(entity =>
//        {
//            entity.HasKey(e => e.ReceiptId).HasName("PK__Inventor__CC08C4004A60C119");

//            entity.ToTable("InventoryReceipts_Material_data");

//            entity.Property(e => e.ReceiptId).HasColumnName("ReceiptID");
//            entity.Property(e => e.MaterialGroupId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MaterialGroupID");
//            entity.Property(e => e.MaterialName).HasMaxLength(100);
//            entity.Property(e => e.Note).HasMaxLength(255);
//            entity.Property(e => e.ReceiptDate).HasColumnType("datetime");
//            entity.Property(e => e.RequestId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("RequestID");
//            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
//            entity.Property(e => e.Unit).HasMaxLength(50);
//            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

//            entity.HasOne(d => d.MaterialGroup).WithMany(p => p.InventoryReceiptsMaterialData)
//                .HasForeignKey(d => d.MaterialGroupId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Inventory__Mater__72C60C4A");

//            entity.HasOne(d => d.Request).WithMany(p => p.InventoryReceiptsMaterialData)
//                .HasForeignKey(d => d.RequestId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Inventory__Reque__73BA3083");
//        });

//        modelBuilder.Entity<MachineHistoryMd>(entity =>
//        {
//            entity
//                .HasNoKey()
//                .ToTable("MachineHistory_MD");

//            entity.Property(e => e.EnergyTotalOfDay).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.MachineCleaningEnergyOfDay).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.MachineCleaningTimeOfDay).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.MachineId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MachineID");
//            entity.Property(e => e.ProducingEnergyOfDay).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.ProducingTimeOfDay).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.Time).HasColumnType("datetime");
//            entity.Property(e => e.WaitingEnergyOfDay).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.WaitingTimeOfDay).HasColumnType("decimal(10, 2)");

//            entity.HasOne(d => d.Machine).WithMany()
//                .HasForeignKey(d => d.MachineId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__MachineHi__Waiti__05D8E0BE");
//        });

//        modelBuilder.Entity<MachinesCommonDatum>(entity =>
//        {
//            entity.HasKey(e => e.MachineId).HasName("PK__Machines__5A97603FADC7D2ED");

//            entity.ToTable("Machines_Common_data");

//            entity.Property(e => e.MachineId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MachineID");
//            entity.Property(e => e.Description).HasMaxLength(255);
//            entity.Property(e => e.GroupId)
//                .HasMaxLength(10)
//                .HasColumnName("GroupID");
//            entity.Property(e => e.MachineName).HasMaxLength(50);
//            entity.Property(e => e.PartId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("PartID");

//            entity.HasOne(d => d.Group).WithMany(p => p.MachinesCommonData)
//                .HasForeignKey(d => d.GroupId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Machines__GroupI__6477ECF3");

//            entity.HasOne(d => d.Part).WithMany(p => p.MachinesCommonData)
//                .HasForeignKey(d => d.PartId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__Machines__PartID__656C112C");
//        });

//        modelBuilder.Entity<MaterialsMaterialGroupsDatum>(entity =>
//        {
//            entity.HasKey(e => e.MaterialGroupId).HasName("PK__Material__C506131752C1EB3B");

//            entity.ToTable("Materials_MaterialGroups_data");

//            entity.Property(e => e.MaterialGroupId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MaterialGroupID");
//            entity.Property(e => e.MaterialGroupName).HasMaxLength(50);
//        });

//        modelBuilder.Entity<OperationHistoryMd>(entity =>
//        {
//            entity
//                .HasNoKey()
//                .ToTable("OperationHistory_MD");

//            entity.Property(e => e.BatchNo)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.MachineId)
//                .HasMaxLength(10)
//                .IsUnicode(false)
//                .HasColumnName("MachineID");
//            entity.Property(e => e.ProductionCode)
//                .HasMaxLength(50)
//                .IsUnicode(false);
//            entity.Property(e => e.Time).HasColumnType("datetime");
//        });

//        modelBuilder.Entity<PartsCommonDatum>(entity =>
//        {
//            entity.HasKey(e => e.PartId).HasName("PK__Parts__7C3F0D3012CD1E19");

//            entity.ToTable("Parts_Common_data");

//            entity.Property(e => e.PartId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("PartID");
//            entity.Property(e => e.PartName).HasMaxLength(50);
//        });

//        modelBuilder.Entity<ProductCodeHistoryMd>(entity =>
//        {
//            entity
//                .HasNoKey()
//                .ToTable("ProductCodeHistory_MD");

//            entity.Property(e => e.BatchNo)
//                .HasMaxLength(10)
//                .IsUnicode(false);
//            entity.Property(e => e.EndTime).HasColumnType("datetime");
//            entity.Property(e => e.MachineId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MachineID");
//            entity.Property(e => e.ProducingTime).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.ProductionCode)
//                .HasMaxLength(16)
//                .IsUnicode(false);
//            entity.Property(e => e.StartTime).HasColumnType("datetime");
//            entity.Property(e => e.TotalTime).HasColumnType("decimal(10, 2)");
//            entity.Property(e => e.WaitingTime).HasColumnType("decimal(10, 2)");
//        });

//        modelBuilder.Entity<QlsxMachineEvent>(entity =>
//        {
//            entity.HasKey(e => e.EventId).HasName("PK__QlsxMach__7944C870FF2700F3");

//            entity.ToTable("QlsxMachineEvent");

//            entity.Property(e => e.EventId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("EventID");
//            entity.Property(e => e.EventName).HasMaxLength(255);
//        });

//        modelBuilder.Entity<RequestDetailMaterialDatum>(entity =>
//        {
//            entity.HasKey(e => e.DetailId).HasName("PK__RequestD__135C314DE18E63EF");

//            entity.ToTable("RequestDetail_Material_data");

//            entity.Property(e => e.DetailId).HasColumnName("DetailID");
//            entity.Property(e => e.MaterialGroupId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("MaterialGroupID");
//            entity.Property(e => e.MaterialName).HasMaxLength(50);
//            entity.Property(e => e.RequestId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("RequestID");
//            entity.Property(e => e.Unit).HasMaxLength(50);

//            entity.HasOne(d => d.MaterialGroup).WithMany(p => p.RequestDetailMaterialData)
//                .HasForeignKey(d => d.MaterialGroupId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__RequestDe__Mater__1F63A897");

//            entity.HasOne(d => d.Request).WithMany(p => p.RequestDetailMaterialData)
//                .HasForeignKey(d => d.RequestId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__RequestDet__Unit__1E6F845E");
//        });

//        modelBuilder.Entity<SupplyRequestsMaterialDatum>(entity =>
//        {
//            entity.HasKey(e => e.RequestId).HasName("PK__SupplyRe__33A8519AEED4B83E");

//            entity.ToTable("SupplyRequests_Material_data");

//            entity.Property(e => e.RequestId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("RequestID");
//            entity.Property(e => e.EmployeeId)
//                .HasMaxLength(16)
//                .IsUnicode(false)
//                .HasColumnName("EmployeeID");
//            entity.Property(e => e.RequestDate).HasColumnType("datetime");
//            entity.Property(e => e.RequestStatus)
//                .HasMaxLength(10)
//                .IsUnicode(false);

//            entity.HasOne(d => d.Employee).WithMany(p => p.SupplyRequestsMaterialData)
//                .HasForeignKey(d => d.EmployeeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK__SupplyReq__Emplo__6C190EBB");
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
