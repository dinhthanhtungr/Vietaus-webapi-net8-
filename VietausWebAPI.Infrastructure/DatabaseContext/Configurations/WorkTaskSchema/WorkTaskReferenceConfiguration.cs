using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.WorkTaskSchema
{
    public class WorkTaskReferenceConfiguration : IEntityTypeConfiguration<WorkTaskReference>
    {
        public void Configure(EntityTypeBuilder<WorkTaskReference> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkTaskReferences_Id");
            entity.ToTable("WorkTaskReferences", "Work");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.WorkTaskId).HasColumnName("WorkTaskId").IsRequired();
            entity.Property(e => e.ReferenceType).HasColumnName("ReferenceType").HasConversion<int>().IsRequired();
            entity.Property(e => e.ReferenceId).HasColumnName("ReferenceId").IsRequired();
            entity.Property(e => e.ReferenceCodeSnapshot).HasColumnName("ReferenceCodeSnapshot").HasColumnType("citext");
            entity.Property(e => e.ReferenceNameSnapshot).HasColumnName("ReferenceNameSnapshot").HasColumnType("citext");
            entity.Property(e => e.IsPrimary).HasColumnName("IsPrimary").HasDefaultValue(false);

            entity.HasIndex(e => new { e.ReferenceType, e.ReferenceId })
                .HasDatabaseName("IX_WorkTaskReferences_Reference");

            entity.HasIndex(e => new { e.WorkTaskId, e.ReferenceType, e.ReferenceId })
                .IsUnique()
                .HasDatabaseName("UX_WorkTaskReferences_Task_Reference");

            entity.HasIndex(e => e.WorkTaskId)
                .IsUnique()
                .HasDatabaseName("UX_WorkTaskReferences_Task_Primary")
                .HasFilter("\"IsPrimary\" = true");

            entity.HasOne(e => e.WorkTask)
                .WithMany(e => e.References)
                .HasForeignKey(e => e.WorkTaskId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkTaskReferences_WorkTask");
        }
    }
}
