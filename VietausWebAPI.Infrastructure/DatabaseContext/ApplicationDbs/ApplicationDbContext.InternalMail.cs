using Microsoft.EntityFrameworkCore;
using VietausWebAPI.Core.Domain.Entities.InternalMailSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public DbSet<InternalConversation> InternalConversations => Set<InternalConversation>();
        public DbSet<InternalConversationParticipant> InternalConversationParticipants => Set<InternalConversationParticipant>();
        public DbSet<InternalMessage> InternalMessages => Set<InternalMessage>();
        public DbSet<InternalMessageAttachment> InternalMessageAttachments => Set<InternalMessageAttachment>();
        public DbSet<InternalMessageReference> InternalMessageReferences => Set<InternalMessageReference>();
        public DbSet<InternalMessageReadState> InternalMessageReadStates => Set<InternalMessageReadState>();
    }
}
