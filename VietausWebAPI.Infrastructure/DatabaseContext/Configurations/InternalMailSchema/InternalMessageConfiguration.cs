using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;
using VietausWebAPI.Core.Domain.Enums.InternalMailEnums;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.InternalMailSchema
{
    public class InternalMessageConfiguration : IEntityTypeConfiguration<InternalMessage>
    {
        public void Configure(EntityTypeBuilder<InternalMessage> entity)
        {
            entity.HasKey(e => e.InternalMessageId).HasName("PK_InternalMessages_Id");
            entity.ToTable("InternalMessages", "InternalMail");

            entity.Property(e => e.InternalMessageId)
                .HasColumnName("InternalMessageId")
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.InternalConversationId).HasColumnName("InternalConversationId").IsRequired();
            entity.Property(e => e.SenderEmployeeId).HasColumnName("SenderEmployeeId").IsRequired();
            entity.Property(e => e.MessageType).HasColumnName("MessageType").HasConversion<int>().HasDefaultValue(InternalMessageType.Text);
            entity.Property(e => e.Body).HasColumnName("Body").HasColumnType("text").IsRequired();
            entity.Property(e => e.PayloadJson).HasColumnName("PayloadJson").HasColumnType("jsonb");
            entity.Property(e => e.ReplyToMessageId).HasColumnName("ReplyToMessageId");
            entity.Property(e => e.IsUrgent).HasColumnName("IsUrgent").HasDefaultValue(false);
            entity.Property(e => e.SentAt).HasColumnName("SentAt");
            entity.Property(e => e.IsEdited).HasColumnName("IsEdited").HasDefaultValue(false);
            entity.Property(e => e.EditedAt).HasColumnName("EditedAt");
            entity.Property(e => e.EditedByEmployeeId).HasColumnName("EditedByEmployeeId");
            entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(false);
            entity.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
            entity.Property(e => e.DeletedByEmployeeId).HasColumnName("DeletedByEmployeeId");

            entity.HasIndex(e => new { e.InternalConversationId, e.SentAt })
                .HasDatabaseName("IX_InternalMessages_Conversation_SentAt");

            entity.HasIndex(e => new { e.SenderEmployeeId, e.SentAt })
                .HasDatabaseName("IX_InternalMessages_Sender_SentAt");

            entity.HasIndex(e => e.ReplyToMessageId)
                .HasDatabaseName("IX_InternalMessages_ReplyToMessage");

            entity.HasIndex(e => e.IsDeleted)
                .HasDatabaseName("IX_InternalMessages_IsDeleted");

            entity.HasOne(e => e.Conversation)
                .WithMany(e => e.Messages)
                .HasForeignKey(e => e.InternalConversationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InternalMessages_Conversation");

            entity.HasOne(e => e.SenderEmployee)
                .WithMany(e => e.InternalMessageSenderNavigations)
                .HasForeignKey(e => e.SenderEmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalMessages_SenderEmployee");

            entity.HasOne(e => e.EditedByEmployeeNavigation)
                .WithMany(e => e.InternalMessageEditedByNavigations)
                .HasForeignKey(e => e.EditedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalMessages_EditedByEmployee");

            entity.HasOne(e => e.DeletedByEmployeeNavigation)
                .WithMany(e => e.InternalMessageDeletedByNavigations)
                .HasForeignKey(e => e.DeletedByEmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalMessages_DeletedByEmployee");

            entity.HasOne(e => e.ReplyToMessage)
                .WithMany(e => e.Replies)
                .HasForeignKey(e => e.ReplyToMessageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalMessages_ReplyToMessage");
        }
    }
}
