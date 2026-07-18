using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.InternalMailSchema
{
    public class InternalMessageReadStateConfiguration : IEntityTypeConfiguration<InternalMessageReadState>
    {
        public void Configure(EntityTypeBuilder<InternalMessageReadState> entity)
        {
            entity.HasKey(e => new { e.InternalMessageId, e.EmployeeId })
                .HasName("PK_InternalMessageReadStates_Message_Employee");

            entity.ToTable("InternalMessageReadStates", "InternalMail");

            entity.Property(e => e.InternalMessageId).HasColumnName("InternalMessageId");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId");
            entity.Property(e => e.IsRead).HasColumnName("IsRead").HasDefaultValue(false);
            entity.Property(e => e.ReadAt).HasColumnName("ReadAt");

            entity.HasIndex(e => new { e.EmployeeId, e.IsRead })
                .HasDatabaseName("IX_InternalMessageReadStates_Employee_IsRead");

            entity.HasOne(e => e.Message)
                .WithMany(e => e.ReadStates)
                .HasForeignKey(e => e.InternalMessageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InternalMessageReadStates_Message");

            entity.HasOne(e => e.Employee)
                .WithMany(e => e.InternalMessageReadStates)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalMessageReadStates_Employee");
        }
    }
}
