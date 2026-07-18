using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.WorkTaskSchema
{
    public class WorkTaskConfiguration : IEntityTypeConfiguration<WorkTask>
    {
        public void Configure(EntityTypeBuilder<WorkTask> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkTasks_Id");
            entity.ToTable("WorkTasks", "Work");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Title).HasColumnName("Title").HasColumnType("citext").IsRequired();
            entity.Property(e => e.Description).HasColumnName("Description").HasColumnType("text");
            entity.Property(e => e.NextAction).HasColumnName("NextAction").HasColumnType("text");
            entity.Property(e => e.Status).HasColumnName("Status").HasConversion<int>().HasDefaultValue(WorkTaskStatus.Pending);
            entity.Property(e => e.Priority).HasColumnName("Priority").HasConversion<int>().HasDefaultValue(WorkTaskPriority.Normal);
            entity.Property(e => e.DueDate).HasColumnName("DueDate");
            entity.Property(e => e.DueReminderSentAt).HasColumnName("DueReminderSentAt");
            entity.Property(e => e.CompletedDate).HasColumnName("CompletedDate");
            entity.Property(e => e.CompletedBy).HasColumnName("CompletedBy");
            entity.Property(e => e.CompletionNote).HasColumnName("CompletionNote").HasColumnType("text");
            entity.Property(e => e.AssignedToEmployeeId).HasColumnName("AssignedToEmployeeId");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId").IsRequired();
            entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
            entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

            entity.HasIndex(e => new { e.CompanyId, e.AssignedToEmployeeId, e.Status, e.DueDate })
                .HasDatabaseName("IX_WorkTasks_Assigned_Status_DueDate");

            entity.HasIndex(e => new { e.CompanyId, e.Status, e.DueDate })
                .HasDatabaseName("IX_WorkTasks_Company_Status_DueDate");

            entity.HasIndex(e => new { e.Status, e.DueDate })
                .HasDatabaseName("IX_WorkTasks_DueReminder")
                .HasFilter("\"IsActive\" = true AND \"DueReminderSentAt\" IS NULL");

            entity.HasOne(e => e.Company)
                .WithMany(c => c.WorkTasks)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkTasks_Company");

            entity.HasOne(e => e.CompletedByNavigation)
                .WithMany(e => e.WorkTaskCompletedByNavigations)
                .HasForeignKey(e => e.CompletedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_WorkTasks_CompletedBy");

            entity.HasOne(e => e.AssignedToEmployee)
                .WithMany(e => e.WorkTaskAssignedToEmployeeNavigations)
                .HasForeignKey(e => e.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_WorkTasks_AssignedToEmployee");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.WorkTaskCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkTasks_CreatedBy");

            entity.HasOne(e => e.UpdatedByNavigation)
                .WithMany(e => e.WorkTaskUpdatedByNavigations)
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_WorkTasks_UpdatedBy");
        }
    }
}
