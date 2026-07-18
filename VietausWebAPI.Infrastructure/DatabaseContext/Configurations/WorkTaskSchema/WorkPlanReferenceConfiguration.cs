using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.WorkTaskSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.WorkTaskSchema
{
    public class WorkPlanReferenceConfiguration : IEntityTypeConfiguration<WorkPlanReference>
    {
        public void Configure(EntityTypeBuilder<WorkPlanReference> entity)
        {
            entity.HasKey(e => e.Id).HasName("PK_WorkPlanReferences_Id");
            entity.ToTable("WorkPlanReferences", "Work");

            entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd().HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.WorkPlanId).HasColumnName("WorkPlanId").IsRequired();
            entity.Property(e => e.ReferenceType).HasColumnName("ReferenceType").HasConversion<int>().IsRequired();
            entity.Property(e => e.ReferenceId).HasColumnName("ReferenceId").IsRequired();
            entity.Property(e => e.ReferenceCodeSnapshot).HasColumnName("ReferenceCodeSnapshot").HasColumnType("citext");
            entity.Property(e => e.ReferenceNameSnapshot).HasColumnName("ReferenceNameSnapshot").HasColumnType("citext");
            entity.Property(e => e.IsPrimary).HasColumnName("IsPrimary").HasDefaultValue(false);

            entity.HasIndex(e => new { e.ReferenceType, e.ReferenceId })
                .HasDatabaseName("IX_WorkPlanReferences_Reference");

            entity.HasIndex(e => new { e.WorkPlanId, e.ReferenceType, e.ReferenceId })
                .IsUnique()
                .HasDatabaseName("UX_WorkPlanReferences_Plan_Reference");

            entity.HasIndex(e => e.WorkPlanId)
                .IsUnique()
                .HasDatabaseName("UX_WorkPlanReferences_Plan_Primary")
                .HasFilter("\"IsPrimary\" = true");

            entity.HasOne(e => e.WorkPlan)
                .WithMany(e => e.References)
                .HasForeignKey(e => e.WorkPlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_WorkPlanReferences_WorkPlan");
        }
    }
}
