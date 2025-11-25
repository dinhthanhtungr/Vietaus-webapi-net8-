using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;
using VietausWebAPI.Infrastructure.Helpers.Repositories;
using VietausWebAPI.Infrastructure.ApplicationDbs.DatabaseContext;

namespace VietausWebAPI.Infrastructure.Repositories.Attachments
{
    public class AttachmentModelRepository : Repository<AttachmentModel>, IAttachmentModelRepository
    {
        private readonly ApplicationDbContext _context;
        public AttachmentModelRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<AttachmentModel> attachments, CancellationToken ct)
        {
            await _context.AttachmentModels.AddRangeAsync(attachments, ct);
        }
    }
}
