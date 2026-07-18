using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;
using VietausWebAPI.Core.Domain.Enums.WorkTaskEnums;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.WorkTaskSchema
{
    public class WorkPlanConfiguration : IEntityTypeConfiguration<WorkPlan>
    {
        public void Configure(EntityTypeBuilder<WorkPlan> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkPlans_Id");
            entity.ToTable("WorkPlans", "Work");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyId").IsRequired();
            entity.Property(e => e.PlanName).HasColumnName("PlanName").HasColumnType("citext").IsRequired();
            entity.Property(e => e.Objective).HasColumnName("Objective").HasColumnType("text");
            entity.Property(e => e.Strategy).HasColumnName("Strategy").HasColumnType("text");
            entity.Property(e => e.DiscussionSummary).HasColumnName("DiscussionSummary").HasColumnType("text");
            entity.Property(e => e.NextAction).HasColumnName("NextAction").HasColumnType("text");
            entity.Property(e => e.Status).HasColumnName("Status").HasConversion<int>().HasDefaultValue(WorkPlanStatus.Active);
            entity.Property(e => e.Priority).HasColumnName("Priority").HasConversion<int>().HasDefaultValue(WorkTaskPriority.Normal);
            entity.Property(e => e.StartDate).HasColumnName("StartDate");
            entity.Property(e => e.EndDate).HasColumnName("EndDate");
            entity.Property(e => e.NextFollowUpDate).HasColumnName("NextFollowUpDate");
            entity.Property(e => e.AssignedToEmployeeId).HasColumnName("AssignedToEmployeeId");
            entity.Property(e => e.CreatedDate).HasColumnName("CreatedDate");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
            entity.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate");
            entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);

            entity.HasIndex(e => new { e.CompanyId, e.AssignedToEmployeeId, e.Status, e.NextFollowUpDate })
                .HasDatabaseName("IX_WorkPlans_Assigned_Status_NextFollowUp");

            entity.HasIndex(e => new { e.CompanyId, e.Status, e.NextFollowUpDate })
                .HasDatabaseName("IX_WorkPlans_Company_Status_NextFollowUp");

            entity.HasOne(e => e.Company)
                .WithMany(c => c.WorkPlans)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkPlans_Company");

            entity.HasOne(e => e.AssignedToEmployee)
                .WithMany(e => e.WorkPlanAssignedToEmployeeNavigations)
                .HasForeignKey(e => e.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_WorkPlans_AssignedToEmployee");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.WorkPlanCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_WorkPlans_CreatedBy");

            entity.HasOne(e => e.UpdatedByNavigation)
                .WithMany(e => e.WorkPlanUpdatedByNavigations)
                .HasForeignKey(e => e.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_WorkPlans_UpdatedBy");
        }
    }
}
