using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.WebAPI.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Attachments
{
    public class AttachmentCollectionRepository : Repository<AttachmentCollection>, IAttachmentCollectionRepository
    {
        private readonly ApplicationDbContext _context;
        public AttachmentCollectionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }

}
