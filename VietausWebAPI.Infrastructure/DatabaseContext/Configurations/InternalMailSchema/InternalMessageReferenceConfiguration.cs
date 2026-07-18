using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.InternalMailSchema
{
    public class InternalMessageReferenceConfiguration : IEntityTypeConfiguration<InternalMessageReference>
    {
        public void Configure(EntityTypeBuilder<InternalMessageReference> entity)
        {
            entity.HasKey(e => e.InternalMessageReferenceId).HasName("PK_InternalMessageReferences_Id");
            entity.ToTable("InternalMessageReferences", "InternalMail");

            entity.Property(e => e.InternalMessageReferenceId)
                .HasColumnName("InternalMessageReferenceId")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.InternalMessageId).HasColumnName("InternalMessageId").IsRequired();
            entity.Property(e => e.RelatedType).HasColumnName("RelatedType").HasConversion<int>().IsRequired();
            entity.Property(e => e.RelatedId).HasColumnName("RelatedId").IsRequired();
            entity.Property(e => e.RelatedExternalId).HasColumnName("RelatedExternalId").HasColumnType("citext");
            entity.Property(e => e.RelatedNameSnapshot).HasColumnName("RelatedNameSnapshot").HasColumnType("citext");
            entity.Property(e => e.SnapshotJson).HasColumnName("SnapshotJson").HasColumnType("jsonb");
            entity.Property(e => e.IsPrimary).HasColumnName("IsPrimary").HasDefaultValue(false);

            entity.HasIndex(e => new { e.RelatedType, e.RelatedId })
                .HasDatabaseName("IX_InternalMessageReferences_Related");

            entity.HasIndex(e => new { e.InternalMessageId, e.RelatedType, e.RelatedId })
                .IsUnique()
                .HasDatabaseName("UX_InternalMessageReferences_Message_Related");

            entity.HasIndex(e => e.InternalMessageId)
                .IsUnique()
                .HasDatabaseName("UX_InternalMessageReferences_Message_Primary")
                .HasFilter("\"IsPrimary\" = true");

            entity.HasOne(e => e.Message)
                .WithMany(e => e.References)
                .HasForeignKey(e => e.InternalMessageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InternalMessageReferences_Message");
        }
    }
}
