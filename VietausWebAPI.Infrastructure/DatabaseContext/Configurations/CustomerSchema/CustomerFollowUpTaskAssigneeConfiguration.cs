using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.CustomerSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.CustomerSchema
{
    public class CustomerFollowUpTaskAssigneeConfiguration : IEntityTypeConfiguration<CustomerFollowUpTaskAssignee>
    {
        public void Configure(EntityTypeBuilder<CustomerFollowUpTaskAssignee> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_CustomerFollowUpTaskAssignees_Id");
            entity.ToTable("CustomerFollowUpTaskAssignees", "Customer");

            entity.Property(e => e.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.CustomerFollowUpTaskId)
                .HasColumnName("CustomerFollowUpTaskId")
                .IsRequired();

            entity.Property(e => e.EmployeeId)
                .HasColumnName("EmployeeId")
                .IsRequired();

            entity.Property(e => e.IsPrimary)
                .HasColumnName("IsPrimary")
                .HasDefaultValue(false);

            entity.Property(e => e.IsActive)
                .HasColumnName("IsActive")
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedDate)
                .HasColumnName("CreatedDate");

            entity.Property(e => e.CreatedBy)
                .HasColumnName("CreatedBy")
                .IsRequired();

            entity.HasIndex(e => new { e.CustomerFollowUpTaskId, e.EmployeeId })
                .HasDatabaseName("IX_CustomerFollowUpTaskAssignees_Task_Employee")
                .IsUnique();

            entity.HasIndex(e => new { e.EmployeeId, e.IsActive })
                .HasDatabaseName("IX_CustomerFollowUpTaskAssignees_Employee_IsActive");

            entity.HasOne(e => e.CustomerFollowUpTask)
                .WithMany(e => e.Assignees)
                .HasForeignKey(e => e.CustomerFollowUpTaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CustomerFollowUpTaskAssignees_Task");

            entity.HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CustomerFollowUpTaskAssignees_Employee");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CustomerFollowUpTaskAssignees_CreatedBy");
        }
    }
}
