using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.Configurations.InternalMailSchema
{
    public class InternalConversationParticipantConfiguration : IEntityTypeConfiguration<InternalConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<InternalConversationParticipant> entity)
        {
            entity.HasKey(e => new { e.InternalConversationId, e.EmployeeId })
                .HasName("PK_InternalConversationParticipants_Conversation_Employee");

            entity.ToTable("InternalConversationParticipants", "InternalMail");

            entity.Property(e => e.InternalConversationId).HasColumnName("InternalConversationId");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeId");
            entity.Property(e => e.Role).HasColumnName("Role").HasConversion<int>().HasDefaultValue(Core.Domain.Enums.InternalMailEnums.InternalConversationParticipantRole.Member);
            entity.Property(e => e.IsArchived).HasColumnName("IsArchived").HasDefaultValue(false);
            entity.Property(e => e.ArchivedAt).HasColumnName("ArchivedAt");
            entity.Property(e => e.LastReadAt).HasColumnName("LastReadAt");
            entity.Property(e => e.JoinedAt).HasColumnName("JoinedAt");
            entity.Property(e => e.IsMuted).HasColumnName("IsMuted").HasDefaultValue(false);

            entity.HasIndex(e => new { e.EmployeeId, e.IsArchived, e.IsMuted })
                .HasDatabaseName("IX_InternalConversationParticipants_Employee_Archive_Mute");

            entity.HasIndex(e => new { e.EmployeeId, e.LastReadAt })
                .HasDatabaseName("IX_InternalConversationParticipants_Employee_LastReadAt");

            entity.HasOne(e => e.Conversation)
                .WithMany(e => e.Participants)
                .HasForeignKey(e => e.InternalConversationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_InternalConversationParticipants_Conversation");

            entity.HasOne(e => e.Employee)
                .WithMany(e => e.InternalConversationParticipants)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_InternalConversationParticipants_Employee");
        }
    }
}
