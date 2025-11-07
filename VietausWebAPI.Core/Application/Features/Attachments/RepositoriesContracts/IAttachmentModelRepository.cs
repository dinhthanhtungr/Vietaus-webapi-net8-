using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Shared.Helper.Repository;
using VietausWebAPI.Core.Domain.Entities.AttachmentSchema;

namespace VietausWebAPI.Core.Application.Features.Attachments.RepositoriesContracts
{
    public interface IAttachmentModelRepository : IRepository<AttachmentModel>
    {
        Task AddRangeAsync (IEnumerable<AttachmentModel> attachments, CancellationToken ct);
    }
}
