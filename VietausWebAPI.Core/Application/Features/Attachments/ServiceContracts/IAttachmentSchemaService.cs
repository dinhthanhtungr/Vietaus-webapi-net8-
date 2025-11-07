using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Attachments.DTOs;
using VietausWebAPI.Core.Domain.Enums.Attachment;

namespace VietausWebAPI.Core.Application.Features.Attachments.ServiceContracts
{
    public interface IAttachmentSchemaService
    {
        Task<List<AttachmentDTO>> ListAsync(Guid collectionId, AttachmentSlot? slot, CancellationToken ct);
        Task<AttachmentDTO> UploadAsync(Guid collectionId, AttachmentSlot slot, IFormFile file, Guid userId, CancellationToken ct);
        Task<List<AttachmentDTO>> UploadListAsync(Guid collectionId, AttachmentSlot slot, List<IFormFile> file, Guid userId, CancellationToken ct);
        Task<StreamResult> GetContentAsync(Guid attachmentId, CancellationToken ct);
        Task DeleteAsync(Guid attachmentId, CancellationToken ct); // soft-delete
        Task HardDeleteAsync(Guid attachmentId, CancellationToken ct); // soft-delete
    }
}
