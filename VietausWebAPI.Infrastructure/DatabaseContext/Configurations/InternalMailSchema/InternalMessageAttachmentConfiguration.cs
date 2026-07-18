using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.InternalMailSchema
{
    public class InternalMessageAttachmentConfiguration : IEntityTypeConfiguration<InternalMessageAttachment>
    {
        public void Configure(EntityTypeBuilder<InternalMessageAttachment> entity)
        {
            entity.HasKey(e => e.InternalMessageAttachmentId).HasName("PK_InternalMessageAttachments_Id");
            entity.ToTable("InternalMessageAttachments", "InternalMail");

            entity.Property(e => e.InternalMessageAttachmentId)
                .HasColumnName("InternalMessageAttachmentId")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.InternalMessageId).HasColumnName("InternalMessageId").IsRequired();
            entity.Property(e => e.AttachmentId).HasColumnName("AttachmentId").IsRequired();
            entity.Property(e => e.AttachedAt).HasColumnName("AttachedAt");

            entity.HasIndex(e => e.InternalMessageId)
                .HasDatabaseName("IX_InternalMessageAttachments_Message");

            entity.HasIndex(e => e.AttachmentId)
                .HasDatabaseName("IX_InternalMessageAttachments_Attachment");

            entity.HasOne(e => e.Message)
                .WithMany(e => e.Attachments)
                .HasForeignKey(e => e.InternalMessageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InternalMessageAttachments_Message");

            entity.HasOne(e => e.Attachment)
                .WithMany(e => e.InternalMessageAttachments)
                .HasForeignKey(e => e.AttachmentId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalMessageAttachments_Attachment");
        }
    }
}
