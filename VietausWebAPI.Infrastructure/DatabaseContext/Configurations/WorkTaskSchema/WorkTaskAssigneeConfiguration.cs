using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.WorkTaskSchema
{
    public class WorkTaskAssigneeConfiguration : IEntityTypeConfiguration<WorkTaskAssignee>
    {
        public void Configure(EntityTypeBuilder<WorkTaskAssignee> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkTaskAssignees_Id");
            entity.ToTable("WorkTaskAssignees", "Work");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.WorkTaskId).HasColumnName("WorkTaskId").IsRequired();
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId").IsRequired();
            entity.Property(e => e.IsPrimary).HasColumnName("IsPrimary").HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

            entity.HasIndex(e => new { e.WorkTaskId, e.EmployeeId })
                .IsUnique()
                .HasDatabaseName("UX_WorkTaskAssignees_Task_Employee_Active")
                .HasFilter("\"IsActive\" = true");

            entity.HasIndex(e => new { e.EmployeeId, e.IsActive })
                .HasDatabaseName("IX_WorkTaskAssignees_Employee_IsActive");

            entity.HasOne(e => e.WorkTask)
                .WithMany(e => e.Assignees)
                .HasForeignKey(e => e.WorkTaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkTaskAssignees_WorkTask");

            entity.HasOne(e => e.Employee)
                .WithMany(e => e.WorkTaskAssigneeEmployees)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkTaskAssignees_Employee");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.WorkTaskAssigneeCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkTaskAssignees_CreatedBy");
        }
    }
}
