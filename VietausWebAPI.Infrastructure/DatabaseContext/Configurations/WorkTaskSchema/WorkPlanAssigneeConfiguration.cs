using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.WorkTaskSchema
{
    public class WorkPlanAssigneeConfiguration : IEntityTypeConfiguration<WorkPlanAssignee>
    {
        public void Configure(EntityTypeBuilder<WorkPlanAssignee> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkPlanAssignees_Id");
            entity.ToTable("WorkPlanAssignees", "Work");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.WorkPlanId).HasColumnName("WorkPlanId").IsRequired();
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId").IsRequired();
            entity.Property(e => e.IsPrimary).HasColumnName("IsPrimary").HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");

            entity.HasIndex(e => new { e.WorkPlanId, e.EmployeeId })
                .IsUnique()
                .HasDatabaseName("UX_WorkPlanAssignees_Plan_Employee_Active")
                .HasFilter("\"IsActive\" = true");

            entity.HasIndex(e => new { e.EmployeeId, e.IsActive })
                .HasDatabaseName("IX_WorkPlanAssignees_Employee_IsActive");

            entity.HasOne(e => e.WorkPlan)
                .WithMany(e => e.Assignees)
                .HasForeignKey(e => e.WorkPlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkPlanAssignees_WorkPlan");

            entity.HasOne(e => e.Employee)
                .WithMany(e => e.WorkPlanAssigneeEmployees)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkPlanAssignees_Employee");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.WorkPlanAssigneeCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkPlanAssignees_CreatedBy");
        }
    }
}
