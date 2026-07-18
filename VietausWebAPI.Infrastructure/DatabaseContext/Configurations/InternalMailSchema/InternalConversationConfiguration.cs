using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.InternalMailSchema
{
    public class InternalConversationConfiguration : IEntityTypeConfiguration<InternalConversation>
    {
        public void Configure(EntityTypeBuilder<InternalConversation> entity)
        {
            entity.HasKey(e => e.InternalConversationId).HasName("PK_InternalConversations_Id");
            entity.ToTable("InternalConversations", "InternalMail");

            entity.Property(e => e.InternalConversationId)
                .HasColumnName("InternalConversationId")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.CompanyId).HasColumnName("CompanyId").IsRequired();
            entity.Property(e => e.Subject).HasColumnName("Subject").HasColumnType("citext").IsRequired();
            entity.Property(e => e.RelatedType).HasColumnName("RelatedType").HasConversion<int>();
            entity.Property(e => e.RelatedId).HasColumnName("RelatedId");
            entity.Property(e => e.RelatedExternalId).HasColumnName("RelatedExternalId").HasColumnType("citext");
            entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(e => e.LastMessageAt).HasColumnName("LastMessageAt");
            entity.Property(e => e.LastMessageId).HasColumnName("LastMessageId");
            entity.Property(e => e.IsActive).HasColumnName("IsActive").HasDefaultValue(true);
            entity.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
            entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeId");

            entity.HasIndex(e => new { e.CompanyId, e.IsActive, e.LastMessageAt })
                .HasDatabaseName("IX_InternalConversations_Company_Active_LastMessageAt");

            entity.HasIndex(e => new { e.CompanyId, e.RelatedType, e.RelatedId })
                .HasDatabaseName("IX_InternalConversations_Company_Related");

            entity.HasIndex(e => e.LastMessageId)
                .HasDatabaseName("IX_InternalConversations_LastMessage");

            entity.HasOne(e => e.Company)
                .WithMany(e => e.InternalConversations)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalConversations_Company");

            entity.HasOne(e => e.CreatedByNavigation)
                .WithMany(e => e.InternalConversationCreatedByNavigations)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalConversations_CreatedBy");

            entity.HasOne(e => e.DeletedByEmployeeNavigation)
                .WithMany(e => e.InternalConversationDeletedByNavigations)
                .HasForeignKey(e => e.DeletedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalConversations_DeletedByEmployee");

            entity.HasOne(e => e.LastMessage)
                .WithMany()
                .HasForeignKey(e => e.LastMessageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_InternalConversations_LastMessage");
        }
    }
}
